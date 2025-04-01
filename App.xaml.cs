using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Windows.Media.SpeechRecognition;
using Microsoft.Windows.AppLifecycle;

namespace Key_Wizard
{
    public partial class App : Application
    {
        private Window? m_window;
        private AppWindow? m_appWindow;
        private IntPtr m_hookHandle = IntPtr.Zero;
        private HookProc m_hookProc; // Strong reference to prevent garbage collection
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYUP = 0x0105;
        private const int VK_K = 0x4B;
        private const int VK_CONTROL = 0x11;
        private const int VK_MENU = 0x12; // Alt key

        private const int SW_HIDE = 0;
        private const int SW_NORMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_MINIMIZE = 6;
        private const int SW_SHOWNA = 8;
        private const int HWND_TOP = 0;
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_SHOWWINDOW = 0x0040;

        private bool m_ctrlPressed = false;
        private bool m_altPressed = false;
        private bool m_keepRunning = true;
        private SpeechRecognizer? _speechRecognizer;
        private bool _isListening = false;
        private bool _windowVisible = true;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]

        private static extern bool IsWindowVisible(IntPtr hWnd);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        private struct KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        private int m_originalWidth;
        private int m_originalHeight;
        private int m_originalX;
        private int m_originalY;

        public App()
        {
            this.InitializeComponent();
            this.UnhandledException += OnUnhandledException;
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;

            // Initialize the hook procedure to prevent garbage collection
            m_hookProc = new HookProc(KeyboardHookProc);
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            Debug.WriteLine("OnLaunched called");
            m_window = new MainWindow();
            m_appWindow = GetAppWindow(m_window);
            m_window.Closed += OnWindowClosed;
            m_window.Activate();
            Debug.WriteLine("Window activated");

            // Setup keyboard hook first
            SetupKeyboardHook();

            // Then save window position
            SaveWindowPosition();

            // Hide the window
            HideWindow();

            // Keep a reference to the window to prevent garbage collection
            GC.KeepAlive(m_window);
        }

        private async void StartSpeechRecognitionAsync()
        {
            try
            {
                // If we already have a speech recognizer, clean it up first
                if (_speechRecognizer != null)
                {
                    await StopSpeechRecognitionAsync();
                }

                _speechRecognizer = new SpeechRecognizer();

                // Add constraints BEFORE compiling
                var listConstraint = new SpeechRecognitionListConstraint(
                    new List<string> {
                        "Key Wizard",
                        "Open Key Wizard",
                        "Show Key Wizard",
                        "Launch Key Wizard",
                        "Open Wizard",
                        "Show Wizard",
                        "Launch Wizard",
                        "Wizard"
                    }, "activationPhrases");

                _speechRecognizer.Constraints.Add(listConstraint);

                // Compile constraints
                var compilationResult = await _speechRecognizer.CompileConstraintsAsync();
                if (compilationResult.Status != SpeechRecognitionResultStatus.Success)
                {
                    Debug.WriteLine($"Constraint compilation failed: {compilationResult.Status}");
                    return;
                }

                // Start continuous recognition
                _speechRecognizer.ContinuousRecognitionSession.ResultGenerated +=
                    ContinuousRecognitionSession_ResultGenerated;

                await _speechRecognizer.ContinuousRecognitionSession.StartAsync();
                _isListening = true;
                Debug.WriteLine("Continuous speech recognition started");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Speech Recognition Error: {ex.Message}");
            }
        }

        private async Task StopSpeechRecognitionAsync()
        {
            if (_speechRecognizer != null && _isListening)
            {
                try
                {
                    _speechRecognizer.ContinuousRecognitionSession.ResultGenerated -=
                        ContinuousRecognitionSession_ResultGenerated;
                    await _speechRecognizer.ContinuousRecognitionSession.StopAsync();
                    _isListening = false;
                    _speechRecognizer.Dispose();
                    _speechRecognizer = null;
                    Debug.WriteLine("Speech recognition stopped and cleaned up");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error stopping speech recognition: {ex.Message}");
                }
            }
        }

        private void ContinuousRecognitionSession_ResultGenerated(
            SpeechContinuousRecognitionSession sender,
            SpeechContinuousRecognitionResultGeneratedEventArgs args)
        {
            if (args.Result.Status == SpeechRecognitionResultStatus.Success)
            {
                string recognizedText = args.Result.Text;
                Debug.WriteLine($"Recognized: {recognizedText}");

                if ((recognizedText.Contains("Key") || recognizedText.Contains("Wizard")) &&
                    (recognizedText.Contains("Open") || recognizedText.Contains("Launch") || recognizedText.Contains("Show")))
                {
                    // Run on UI thread
                    _ = m_window?.DispatcherQueue.TryEnqueue(() =>
                    {
                        ShowAndActivateWindow();
                    });
                }
            }
        }

        private void SetupKeyboardHook()
        {
            try
            {
                string moduleName = Process.GetCurrentProcess().MainModule?.ModuleName ?? "";
                IntPtr hInstance = GetModuleHandle(moduleName);
                Debug.WriteLine($"🔧 Setting up keyboard hook with module: {moduleName}");

                m_hookHandle = SetWindowsHookEx(WH_KEYBOARD_LL, m_hookProc, hInstance, 0);
                if (m_hookHandle == IntPtr.Zero)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    Debug.WriteLine($"❌ Failed to set keyboard hook! Error code: {errorCode}");
                }
                else
                {
                    Debug.WriteLine("✅ Keyboard hook set successfully.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Exception setting up keyboard hook: {ex.Message}");
            }
        }



        private void SaveWindowPosition()
        {
            try
            {
                IntPtr hwnd = GetWindowHandle();
                if (GetWindowRect(hwnd, out RECT rect))
                {
                    m_originalX = rect.Left;
                    m_originalY = rect.Top;
                    m_originalWidth = rect.Right - rect.Left;
                    m_originalHeight = rect.Bottom - rect.Top;
                    Debug.WriteLine($"Saved window position: X={m_originalX}, Y={m_originalY}, Width={m_originalWidth}, Height={m_originalHeight}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving window position: {ex.Message}");
            }
        }

        private void OnUnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            Debug.WriteLine($"Unhandled exception: {e.Message}");
            e.Handled = true;
        }

        private void HideWindow()
        {
            try
            {
                if (m_window != null)
                {
                    IntPtr hwnd = GetWindowHandle();
                    ShowWindow(hwnd, SW_HIDE);
                    Debug.WriteLine("Window hidden.");
                    _windowVisible = false;

                    // Restart speech recognition when window is hidden
                    StartSpeechRecognitionAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error hiding window: {ex.Message}");
            }
        }

        private bool IsKeyPressed(int key)
        {
            return (GetAsyncKeyState(key) & 0x8000) != 0;
        }

        private IntPtr KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                KBDLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam);
                int wParamInt = wParam.ToInt32();

                Debug.WriteLine($"Key: {hookStruct.vkCode}, wParam: {wParamInt}");

                // Check if Ctrl + Alt + K is pressed
                if (hookStruct.vkCode == VK_K &&
                    (GetAsyncKeyState(VK_CONTROL) & 0x8000) != 0 &&
                    (GetAsyncKeyState(VK_MENU) & 0x8000) != 0 &&
                    (wParamInt == WM_KEYDOWN || wParamInt == WM_SYSKEYDOWN))
                {
                    Debug.WriteLine("🔥 Hotkey Ctrl+Alt+K detected!");
                    ShowAndActivateWindow(); // Call your hotkey action
                    return (IntPtr)1; // Block the key event (optional)
                }
            }

            // Call the next hook in the chain
            return CallNextHookEx(m_hookHandle, nCode, wParam, lParam);
        }




        private bool _isActivating = false; // Prevent duplicate activations

        private void ShowAndActivateWindow()
        {
            if (_isActivating) return;
            _isActivating = true;

            try
            {
                Debug.WriteLine("Activating window...");

                if (m_window != null)
                {
                    _ = StopSpeechRecognitionAsync(); // Stop voice recognition

                    IntPtr hwnd = GetWindowHandle();
                    if (!IsWindowVisible(hwnd))
                    {
                        ShowWindow(hwnd, SW_SHOWNA);
                        SetWindowPos(hwnd, (IntPtr)HWND_TOP, m_originalX, m_originalY, m_originalWidth, m_originalHeight, SWP_SHOWWINDOW);
                    }

                    ShowWindow(hwnd, SW_NORMAL);
                    m_window.Activate();
                    BringWindowToTop(hwnd);
                    SetForegroundWindow(hwnd);

                    _windowVisible = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in ShowAndActivateWindow: {ex.Message}");
            }
            finally
            {
                _isActivating = false;
            }
        }


        private IntPtr GetWindowHandle()
        {
            if (m_window == null)
            {
                throw new InvalidOperationException("Window is not initialized.");
            }
            return WinRT.Interop.WindowNative.GetWindowHandle(m_window);
        }

        private AppWindow GetAppWindow(Window window)
        {
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
            return AppWindow.GetFromWindowId(windowId);
        }

        private void OnWindowClosed(object sender, WindowEventArgs args)
        {
            Debug.WriteLine("Window closed event");
            _ = StopSpeechRecognitionAsync();

            // We don't want to clean up the hook here, as the app is still running
            // Just hide the window instead
            HideWindow();

            // Mark the event as handled to prevent the default behavior
            args.Handled = true;
        }

        private void OnProcessExit(object sender, EventArgs e)
        {
            Debug.WriteLine("Process exit event");
            _ = StopSpeechRecognitionAsync();
            CleanupHook();
        }

        private void CleanupHook()
        {
            if (m_hookHandle != IntPtr.Zero)
            {
                try
                {
                    UnhookWindowsHookEx(m_hookHandle);
                    Debug.WriteLine("Keyboard hook unregistered.");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error unregistering hook: {ex.Message}");
                }
                finally
                {
                    m_hookHandle = IntPtr.Zero;
                }
            }
        }
    }
}
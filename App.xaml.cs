using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel.Activation;

namespace Key_Wizard
{
    public partial class App : Application
    {
        private Window? m_window;
        private AppWindow? m_appWindow;
        private IntPtr m_hookHandle = IntPtr.Zero;
        private HookProc? m_hookProc;
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYUP = 0x0105;
        private const int VK_K = 0x4B;
        private const int VK_CONTROL = 0x11;
        private const int VK_MENU = 0x12; // Alt key

        // ShowWindow command constants
        private const int SW_HIDE = 0;
        private const int SW_NORMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_MINIMIZE = 6;
        private const int SW_SHOWNA = 8;
        private const int HWND_TOP = 0;
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;

        // Track the state of modifier keys
        private bool m_ctrlPressed = false;
        private bool m_altPressed = false;

        // Add a flag to keep the application running
        private bool m_keepRunning = true;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool BringWindowToTop(IntPtr hWnd);

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

        // Store the original window position and size
        private int m_originalWidth;
        private int m_originalHeight;
        private int m_originalX;
        private int m_originalY;
        private bool m_isFirstLaunch = true;

        public App()
        {
            this.InitializeComponent();
            this.UnhandledException += OnUnhandledException;

            // Add exit handler to clean up resources properly
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            Debug.WriteLine("OnLaunched called");
            m_window = new MainWindow();
            m_appWindow = GetAppWindow(m_window);

            // Subscribe to the Closed event
            m_window.Closed += OnWindowClosed;

            // Show window initially to get its dimensions
            m_window.Activate();
            Debug.WriteLine("Window activated");

            // Set up the keyboard hook
            SetupKeyboardHook();

            // Store the original window position and size before minimizing
            SaveWindowPosition();

            // Hide the window after initialization
            HideWindow();

            // Important: Keep a reference to this window to prevent GC
            GC.KeepAlive(m_window);
            GC.KeepAlive(m_hookProc);
        }

        private void SetupKeyboardHook()
        {
            try
            {
                m_hookProc = new HookProc(KeyboardHookProc);
                string moduleName = Process.GetCurrentProcess().MainModule?.ModuleName ?? "";
                IntPtr hInstance = GetModuleHandle(moduleName);

                Debug.WriteLine($"Setting up keyboard hook with module: {moduleName}");

                m_hookHandle = SetWindowsHookEx(WH_KEYBOARD_LL, m_hookProc, hInstance, 0);

                if (m_hookHandle == IntPtr.Zero)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    Debug.WriteLine($"Failed to set keyboard hook. Error code: {errorCode}");
                }
                else
                {
                    Debug.WriteLine("Keyboard hook set successfully.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception setting up keyboard hook: {ex.Message}");
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
            e.Handled = true; // Prevent the app from crashing
        }

        private void HideWindow()
        {
            try
            {
                if (m_window != null)
                {
                    IntPtr hwnd = GetWindowHandle();

                    // Completely hide the window instead of minimizing
                    ShowWindow(hwnd, SW_HIDE);

                    Debug.WriteLine("Window hidden.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error hiding window: {ex.Message}");
            }
        }

        private IntPtr KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (nCode >= 0)
                {
                    int wParamInt = wParam.ToInt32();
                    KBDLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam);

                    // Track modifier keys
                    if (hookStruct.vkCode == VK_CONTROL)
                    {
                        m_ctrlPressed = wParamInt == WM_KEYDOWN || wParamInt == WM_SYSKEYDOWN;
                        Debug.WriteLine($"Ctrl key {(m_ctrlPressed ? "pressed" : "released")}");
                    }
                    else if (hookStruct.vkCode == VK_MENU)
                    {
                        m_altPressed = wParamInt == WM_KEYDOWN || wParamInt == WM_SYSKEYDOWN;
                        Debug.WriteLine($"Alt key {(m_altPressed ? "pressed" : "released")}");
                    }

                    // Check for Ctrl+Alt+K hotkey
                    if (m_ctrlPressed && m_altPressed && hookStruct.vkCode == VK_K &&
                        (wParamInt == WM_KEYDOWN || wParamInt == WM_SYSKEYDOWN))
                    {
                        Debug.WriteLine("Hotkey Ctrl+Alt+K detected!");

                        if (m_window != null && m_window.DispatcherQueue != null)
                        {
                            bool enqueued = m_window.DispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.High, () =>
                            {
                                ShowAndActivateWindow();
                            });

                            Debug.WriteLine($"Enqueue result: {enqueued}");
                        }
                        else
                        {
                            Debug.WriteLine("Window or dispatcher is null!");
                        }

                        // Don't process further to avoid K being typed
                        return new IntPtr(1);
                    }

                    // Also check using GetAsyncKeyState as a fallback
                    bool ctrlDown = (GetAsyncKeyState(VK_CONTROL) & 0x8000) != 0;
                    bool altDown = (GetAsyncKeyState(VK_MENU) & 0x8000) != 0;
                    bool kDown = (GetAsyncKeyState(VK_K) & 0x8000) != 0;

                    if (ctrlDown && altDown && kDown && hookStruct.vkCode == VK_K)
                    {
                        Debug.WriteLine("Hotkey Ctrl+Alt+K detected via GetAsyncKeyState!");

                        if (m_window != null && m_window.DispatcherQueue != null)
                        {
                            bool enqueued = m_window.DispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.High, () =>
                            {
                                ShowAndActivateWindow();
                            });

                            Debug.WriteLine($"Fallback enqueue result: {enqueued}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in keyboard hook: {ex.Message}");
            }

            // Call the next hook in the chain
            return CallNextHookEx(m_hookHandle, nCode, wParam, lParam);
        }

        private void ShowAndActivateWindow()
        {
            try
            {
                Debug.WriteLine("ShowAndActivateWindow called.");
                if (m_window != null)
                {
                    IntPtr hwnd = GetWindowHandle();

                    // Check if window is visible
                    bool isVisible = IsWindowVisible(hwnd);
                    Debug.WriteLine($"Window is currently {(isVisible ? "visible" : "hidden")}");

                    if (!isVisible)
                    {
                        // Show the window with its original size and position
                        ShowWindow(hwnd, SW_SHOWNA);

                        // Restore the original window position and size if we saved it
                        if (m_originalWidth > 0 && m_originalHeight > 0)
                        {
                            SetWindowPos(
                                hwnd,
                                (IntPtr)HWND_TOP,
                                m_originalX,
                                m_originalY,
                                m_originalWidth,
                                m_originalHeight,
                                SWP_SHOWWINDOW
                            );
                            Debug.WriteLine($"Restored window to: X={m_originalX}, Y={m_originalY}, Width={m_originalWidth}, Height={m_originalHeight}");
                        }
                    }

                    // Ensure the window is in normal state
                    ShowWindow(hwnd, SW_NORMAL);

                    // Activate the window to bring it to the foreground
                    m_window.Activate();
                    BringWindowToTop(hwnd);
                    SetForegroundWindow(hwnd);

                    Debug.WriteLine("Window shown and activated.");
                }
                else
                {
                    Debug.WriteLine("m_window is null.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in ShowAndActivateWindow: {ex.Message}");
            }
        }

        private const uint SWP_SHOWWINDOW = 0x0040;

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
            Debug.WriteLine("Window closed event - NOT unregistering hook");

            // Important: Don't unregister the hook when the window is closed
            // We want the hook to remain active so the hotkey still works

            // Just hide the window instead
            HideWindow();

            // Mark the event as handled to prevent default window closing behavior
            args.Handled = true;
        }

        // Clean up the hook when the app is actually exiting
        private void OnProcessExit(object sender, EventArgs e)
        {
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
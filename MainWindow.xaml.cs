using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;

namespace Key_Wizard
{
    /// <summary>
    /// Main Window of Key Wizard
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            ExtendsContentIntoTitleBar = true;

            ResourceDictionary gridResources = MainGrid.Resources;
            if (gridResources.TryGetValue("WindowWidth", out var windowWidth) &&
                gridResources.TryGetValue("WindowHeight", out var windowHeight))
            {
                this.AppWindow.Resize(new SizeInt32((int)windowWidth, (int)windowHeight));
            }
        }
        
        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            shortcutsList.Visibility = Visibility.Visible;
            myButton.Content = "Clicked";
        }
    }
}

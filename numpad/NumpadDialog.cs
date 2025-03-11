using Microsoft.UI.Windowing;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Graphics;
using WinRT.Interop;

namespace Key_Wizard
{
    public class NumberPadWindow : Window
    {
        private NumPadDialog numPadDialog;
        public static int SelectedNumber { get; private set; }

        public NumberPadWindow()
        {
            // Create an instance of the NumPadDialog (your keypad)
            numPadDialog = new NumPadDialog();

            // Subscribe to the event to get the selected number
            numPadDialog.NumberSelected += (number) =>
            {
                SelectedNumber = number;
                Console.WriteLine($"Number Pressed: {number}");

                // Close once number selected
                this.Close();
            };

            this.Content = numPadDialog;
            this.Activate();
        }

        // Method to show the number pad window when needed
        public void ShowNumberPad()
        {
            this.Activate(); // Activate the window
        }

    }
}

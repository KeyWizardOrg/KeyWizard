//using Microsoft.UI.Xaml;
//using Microsoft.UI.Xaml.Controls;
//using Microsoft.UI.Xaml.Media;
//using System;

//namespace Key_Wizard
//{
//    public sealed class NumPadDialog : UserControl
//    {
//        public int SelectedNumber { get; private set; }
//        public event Action<int> NumberSelected;

//        public NumPadDialog()
//        {
//            this.Content = CreateNumberGrid();
//        }

//        private Grid CreateNumberGrid()
//        {
//            Grid grid = new Grid
//            {
//                Margin = new Thickness(10),
//                HorizontalAlignment = HorizontalAlignment.Stretch,
//                VerticalAlignment = VerticalAlignment.Stretch
//            };

//            for (int i = 0; i < 5; i++)
//                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

//            for (int i = 0; i < 2; i++)
//                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

//            int[] numbers = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
//            int col = 0, row = 0;

//            foreach (int num in numbers)
//            {
//                Button btn = new Button
//                {
//                    Content = num.ToString(),
//                    VerticalAlignment = VerticalAlignment.Stretch,
//                    HorizontalAlignment = HorizontalAlignment.Stretch,
//                    Margin = new Thickness(5)
//                };

//                // Event to send selected number
//                btn.Click += (s, e) =>
//                {
//                    SelectedNumber = num;
//                    NumberSelected?.Invoke(num);
//                };

//                Grid.SetColumn(btn, col);
//                Grid.SetRow(btn, row);
//                grid.Children.Add(btn);

//                col++;
//                if (col > 4)
//                {
//                    col = 0;
//                    row++;
//                }
//            }

//            return grid;
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SeeManHua
{
    class SearchList
    {
        double width, height;
        public SearchList(Grid mGrid, TextBox textBox, Image img, Grid grid_List)
        {
            width = mGrid.ActualWidth * 0.25;
            height = 25;
            
            textBox.HorizontalAlignment = HorizontalAlignment.Left;
            textBox.VerticalAlignment = VerticalAlignment.Top;
            textBox.Background = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));
            textBox.Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 200, 200));
            textBox.Opacity = 30;
            textBox.Margin = new Thickness(2);
            textBox.Width = width - height;
            textBox.Height = height;
            img.HorizontalAlignment = HorizontalAlignment.Left;
            img.VerticalAlignment = VerticalAlignment.Top;
            img.Margin = new Thickness(textBox.Margin.Left + textBox.Width, textBox.Margin.Top, 0, 0);
            img.Width = height;
            img.Height = height;
            grid_List.HorizontalAlignment = HorizontalAlignment.Left;
            grid_List.VerticalAlignment = VerticalAlignment.Top;
            grid_List.Margin = new Thickness(0);
            grid_List.Width = width;
            grid_List.Height = mGrid.Height;
            grid_List.Background = new SolidColorBrush(Color.FromArgb(100, 43, 43, 43));
        }
    }
}

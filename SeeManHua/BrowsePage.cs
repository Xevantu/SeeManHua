using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Converters;

namespace SeeManHua
{
    class BrowsePage
    {
        //CoverH= NameH+ StatusH+ SortH
        double CoverH = 200;
        double CoverW = 100;
        double NameH = 50;
        double NameW = 500;
        double NameFS = 40;
        double StatusH = 25;
        double StatusW = 100;
        double StatusFS = 20;
        double SortH = 25;
        double SortW = 100;
        double SortFS = 20;
        double IntroH = 50;
        double IntroW = 500;
        double IntroFS = 18;
        double StartReadH = 50;
        double StartReadW = 100;
        double StartReadFS = 40;
        double ChapterItemH = 30;
        double ChapterItemW = 40;
        double ChpaterItemFS = 25;

        public BrowsePage() { }

        public BrowsePage(Image imgCover, Label lbName, Label lbStatus, Label lbSort, TextBox tbIntro, Button btnStartRead, Button btnChpater)
        {
            imgCover.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            imgCover.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            imgCover.Margin = new System.Windows.Thickness(2);
            imgCover.Height = CoverH;
            imgCover.Width = CoverW;

            lbName.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            lbName.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            lbName.Margin = new System.Windows.Thickness(imgCover.Margin.Left + imgCover.Width, imgCover.Margin.Top, 0, 0);
            lbName.Height = NameH;
            lbName.Width = NameW;
            lbName.FontWeight = FontWeights.Bold;
            lbName.FontSize = NameFS;

            lbStatus.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            lbStatus.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            lbStatus.Margin = new System.Windows.Thickness(lbName.Margin.Left, lbName.Margin.Top + lbName.Height + 1, 0, 0);
            lbStatus.Height = StatusH;
            lbStatus.Width = StatusW;
            lbStatus.FontSize = StatusFS;

            lbSort.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            lbSort.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            lbSort.Margin = new System.Windows.Thickness(lbName.Margin.Left, lbStatus.Margin.Top + lbStatus.Height + 1, 0, 0);
            lbSort.Height = SortH;
            lbSort.Width = SortW;
            lbSort.FontSize = SortFS;

            tbIntro.TextWrapping = TextWrapping.WrapWithOverflow;
            tbIntro.HorizontalAlignment = HorizontalAlignment.Left;
            tbIntro.VerticalAlignment = VerticalAlignment.Top;
            tbIntro.HorizontalContentAlignment = HorizontalAlignment.Left;
            tbIntro.VerticalContentAlignment = VerticalAlignment.Top;
            tbIntro.Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255));
            tbIntro.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            tbIntro.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            tbIntro.Margin = new Thickness(imgCover.Margin.Left, imgCover.Margin.Top + imgCover.Height, 0, 0);
            tbIntro.BorderThickness = new Thickness(0);
            tbIntro.Height = IntroH;
            tbIntro.Width = IntroW;
            tbIntro.IsReadOnly = true;
            tbIntro.FontSize = IntroFS;


        }
    }
}

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
        double CoverH = 400;
        double CoverW = 200;
        double NameH = 200;
        double NameW = 500;
        double NameFS = 50;
        double StatusH = 100;
        double StatusW = 500;
        double StatusFS = 25;
        double SortH = 100;
        double SortW = 500;
        double SortFS = 25;
        double IntroH = 100;
        double IntroW = 1000;
        double IntroFS = 18;
        double StartReadH = 50;
        double StartReadW = 100;
        double StartReadFS = 30;
        double ChapterItemFS = 20;

        public BrowsePage() { }

        public BrowsePage(Image imgCover, Label lbName, Label lbStatus, Label lbSort, TextBox tbIntro, Button btnStartRead, object btnChpater, int chpaterCount)
        {
            imgCover.HorizontalAlignment = HorizontalAlignment.Left;
            imgCover.VerticalAlignment = VerticalAlignment.Top;
            imgCover.Margin = new Thickness(2);
            imgCover.Height = CoverW * imgCover.Source.Height / imgCover.Source.Width;
            imgCover.Width = CoverW;

            lbName.HorizontalAlignment = HorizontalAlignment.Left;
            lbName.VerticalAlignment = VerticalAlignment.Top;
            lbName.Margin = new Thickness(imgCover.Margin.Left + imgCover.Width, imgCover.Margin.Top, 0, 0);
            lbName.Height = imgCover.Height * 0.4;
            lbName.Width = NameW;
            lbName.FontWeight = FontWeights.Bold;
            lbName.FontSize = lbName.Height * 0.5;

            lbStatus.HorizontalAlignment = HorizontalAlignment.Left;
            lbStatus.VerticalAlignment = VerticalAlignment.Top;
            lbStatus.Margin = new Thickness(lbName.Margin.Left, lbName.Margin.Top + lbName.Height, 0, 0);
            lbStatus.Height = imgCover.Height * 0.3;
            lbStatus.Width = StatusW;
            lbStatus.FontSize = lbStatus.Height * 0.5;

            lbSort.HorizontalAlignment = HorizontalAlignment.Left;
            lbSort.VerticalAlignment = VerticalAlignment.Top;
            lbSort.Margin = new Thickness(lbName.Margin.Left, lbStatus.Margin.Top + lbStatus.Height + 1, 0, 0);
            lbSort.Height = imgCover.Height * 0.3;
            lbSort.Width = SortW;
            lbSort.FontSize = lbSort.Height * 0.5;

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

            btnStartRead.HorizontalAlignment = HorizontalAlignment.Left;
            btnStartRead.VerticalAlignment = VerticalAlignment.Top;
            btnStartRead.HorizontalContentAlignment = HorizontalAlignment.Left;
            btnStartRead.VerticalContentAlignment = VerticalAlignment.Top;
            btnStartRead.Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255));
            btnStartRead.Background = new SolidColorBrush(Color.FromArgb(60, 255, 255, 0));
            btnStartRead.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            btnStartRead.Margin = new Thickness(imgCover.Margin.Left, tbIntro.Margin.Top + tbIntro.Height, 0, 0);
            btnStartRead.BorderThickness = new Thickness(0);
            btnStartRead.Height = StartReadH;
            btnStartRead.Width = StartReadW;
            btnStartRead.FontSize = StartReadFS;

            /* 5個1列
             * row |
             * col -
             */
            List<Button> list_chapters = (List<Button>)btnChpater;
            int row, col;
            double ChapterItemH = ChapterItemFS * 2;
            double ChapterItemW = ChapterItemFS * 5;
            for (int i = 0; i < chpaterCount; i++)
            {
                row = i % 5;
                col = i / 5;
                Button button = new Button()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Foreground = new SolidColorBrush(Color.FromArgb(100, 0, 0, 0)),
                    Background = new SolidColorBrush(Color.FromArgb(80, 200, 200, 200)),
                    BorderBrush = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255)),
                    Margin = new Thickness(imgCover.Margin.Left + ChapterItemW * row + 2, btnStartRead.Margin.Top + btnStartRead.Height + ChapterItemH * col + 2, 0, 0),
                    BorderThickness = new Thickness(0),
                    Height = ChapterItemH,
                    Width = ChapterItemW,
                    FontSize = ChapterItemFS
                };
                list_chapters.Add(button);
            }
        }
    }
}

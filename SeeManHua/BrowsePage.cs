using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Media;
using System.Windows.Media.Converters;
using System.Windows.Media.Imaging;

namespace SeeManHua
{
    class BrowsePage
    {
        //CoverH= NameH+ StatusH+ SortH
        double CoverW = 200;
        double IntroH = 100;
        double IntroFS = 18;
        double ChapterItemFS = 20;
        /// <summary>
        /// 字元與寬度之間的係數
        /// </summary>
        double widthCoefficient = 1.2;
        /// <summary>
        /// 外框高度與字元大小的係數
        /// </summary>
        double fsCoefficient = 0.6;
        public BrowsePage() { }

        public BrowsePage(Grid tGrid, BrowsePageSetting brPageSetting)
        {
            tGrid.Children.Clear();
            Image imgCover = new Image
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(2),
                Height = CoverW * brPageSetting.Bimg.Height / brPageSetting.Bimg.Width,
                Width = CoverW,
                Source = brPageSetting.Bimg
            };
            tGrid.Children.Add(imgCover);

            Label lbName = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(imgCover.Margin.Left + imgCover.Width, imgCover.Margin.Top, 0, 0),
                Height = imgCover.Height * 0.3,
                FontWeight = FontWeights.Bold
            };
            lbName.FontSize = lbName.Height * fsCoefficient;
            lbName.Width = brPageSetting.Name.Length * lbName.FontSize * widthCoefficient;
            lbName.Content = brPageSetting.Name;
            tGrid.Children.Add(lbName);

            Label lbAuthor = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(lbName.Margin.Left, lbName.Margin.Top + lbName.Height, 0, 0),
                Height = imgCover.Height * 0.2
            };
            lbAuthor.FontSize = lbAuthor.Height * fsCoefficient;
            lbAuthor.Width = brPageSetting.Author.Length * lbAuthor.FontSize * widthCoefficient;
            lbAuthor.Content = brPageSetting.Author;
            tGrid.Children.Add(lbAuthor);

            Label lbSort = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(lbName.Margin.Left, lbAuthor.Margin.Top + lbAuthor.Height, 0, 0),
                Height = imgCover.Height * 0.2
            };
            lbSort.FontSize = lbSort.Height * fsCoefficient;
            lbSort.Width = brPageSetting.Sort.Length * lbSort.FontSize * widthCoefficient;
            lbSort.Content = brPageSetting.Sort;
            tGrid.Children.Add(lbSort);

            TextBox tbIntro = new TextBox
            {
                TextWrapping = TextWrapping.WrapWithOverflow,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Top,
                Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255)),
                Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255)),
                BorderBrush = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255)),
                Margin = new Thickness(imgCover.Margin.Left, imgCover.Margin.Top + imgCover.Height + 5, 0, 0),
                BorderThickness = new Thickness(0),
                Height = IntroH,
                Width = tGrid.Width,
                IsReadOnly = true,
                FontSize = IntroFS,
                Text = brPageSetting.Intro
            };
            tGrid.Children.Add(tbIntro);

            Button btnStartRead = new Button
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255)),
                Background = new SolidColorBrush(Color.FromArgb(60, 255, 255, 0)),
                BorderBrush = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255)),
                Margin = new Thickness(tGrid.Width * 0.06, tbIntro.Margin.Top + tbIntro.Height + 5, 0, 5),
                BorderThickness = new Thickness(0),
                Height = imgCover.Height * 0.1,
                Content = "開始閱讀",
                Uid = brPageSetting.StartRead
            };
            btnStartRead.FontSize = btnStartRead.Height * fsCoefficient;
            //btnStartRead.Width = btnStartRead.FontSize * btnStartRead.Content.ToString().Length * widthCoefficient;
            btnStartRead.Width = tGrid.Width * 0.87;
            tGrid.Children.Add(btnStartRead);

            /* 5個1列
             * row |
             * col -
             */
            HtmlNodeCollection chapterItem = (HtmlNodeCollection)brPageSetting.BtnChapter;
            int row, col;
            double ChapterItemH = ChapterItemFS * 2;
            double ChapterItemW = tGrid.Width * 0.25;
            double ChapterItemDistance = tGrid.Width * 0.06;
            for (int i = 0; i < chapterItem.Count; i++)
            {
                row = i % 3;
                col = i / 3;
                Button button = new Button()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Foreground = new SolidColorBrush(Color.FromArgb(100, 0, 0, 0)),
                    Background = new SolidColorBrush(Color.FromArgb(80, 200, 200, 200)),
                    BorderBrush = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255)),
                    Margin = new Thickness((ChapterItemW + ChapterItemDistance) * row + ChapterItemDistance, btnStartRead.Margin.Top + btnStartRead.Height + (ChapterItemH + 5) * col + 10, 0, 0),
                    BorderThickness = new Thickness(0),
                    
                    Height = ChapterItemH,
                    Width = ChapterItemW,
                    FontSize = ChapterItemFS,
                    Content = chapterItem[i].SelectSingleNode("a").InnerText,
                    Uid = chapterItem[i].SelectSingleNode("a").Attributes["href"].Value
                };
                button.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(BtnChapter_MouseLeftButtonDown), true);
                tGrid.Children.Add(button);
            }
        }
        private void BtnChapter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Button btn = (Button)sender;
            Console.WriteLine(btn.Uid);
        }
    }
}

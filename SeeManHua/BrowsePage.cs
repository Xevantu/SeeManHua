using HtmlAgilityPack;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
        double fsCoefficient = 0.6, fsOffset = 5;
        /// <summary> CoverPropHW = CoverProportionHW
        /// <para>圖片長寬比，因部分圖片下載後長寬不一，造成排版變形，所以統一比例尺(Proportion)。</para>
        /// </summary>
        double CoverPropHW = 1.333;
        TextBox tb_SearchBar = new TextBox();

        public BrowsePage(object sender, Grid tGrid, BrowsePageSetting brPageSetting)
        {
            tb_SearchBar = (TextBox)sender;
            tGrid.Children.Clear();
            Image imgCover = new Image
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                //填滿圖片框
                Stretch = Stretch.UniformToFill,
                Margin = new Thickness(2),
                Height = CoverW * CoverPropHW,
                Width = CoverW,
                Source = brPageSetting.Bimg
            };
            tGrid.Children.Add(imgCover);

            Label lbName = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(imgCover.Margin.Left + imgCover.Width, imgCover.Margin.Top, 0, 0),
                Height = imgCover.Height * 0.3,
                FontWeight = FontWeights.Bold
            };
            lbName.FontSize = lbName.Height * fsCoefficient;
            lbName.Width = brPageSetting.Name.Length * lbName.FontSize * widthCoefficient;
            lbName.Content = brPageSetting.Name;
            tGrid.Children.Add(lbName);

            HtmlNodeCollection list_author = (HtmlNodeCollection)brPageSetting.Author;
            Label lbAuthor = new Label();
            if (list_author != null)
            {
                double AuthorW = 0;
                for (int i = 0; i < list_author.Count; i++)
                {
                    lbAuthor = new Label
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalContentAlignment = HorizontalAlignment.Left,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255)),
                        //BorderThickness = new Thickness(0.6),
                        //BorderBrush = new SolidColorBrush(Color.FromArgb(100, 150, 0, 0)),
                        Height = imgCover.Height * 0.15,
                        Uid = list_author[i].Attributes["href"].Value
                    };
                    lbAuthor.FontSize = lbAuthor.Height * fsCoefficient;
                    lbAuthor.Width = list_author[i].InnerText.Length * lbAuthor.FontSize * widthCoefficient + fsOffset;
                    lbAuthor.Margin = new Thickness(lbName.Margin.Left + AuthorW + 5, lbName.Margin.Top + lbName.Height + 5, 0, 0);
                    AuthorW += lbAuthor.Width + 5;
                    lbAuthor.Content = list_author[i].InnerText;
                    lbAuthor.AddHandler(UIElement.MouseEnterEvent, new MouseEventHandler(Label_MouseEnter), true);
                    lbAuthor.AddHandler(UIElement.MouseLeaveEvent, new MouseEventHandler(Label_MouseLeave), true);
                    lbAuthor.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(LabelAuthor_MouseLeftButtonDown), true);
                    tGrid.Children.Add(lbAuthor);
                }
            }

            HtmlNodeCollection list_sort = (HtmlNodeCollection)brPageSetting.Sort;
            Label lbSort = new Label();
            if (list_sort != null)
            {
                double SortW = 0;
                for (int i = 0; i < list_sort.Count; i++)
                {
                    lbSort = new Label
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalContentAlignment = HorizontalAlignment.Left,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255)),
                        //BorderThickness = new Thickness(0.6),
                        //BorderBrush = new SolidColorBrush(Color.FromArgb(100, 150, 0, 0)),
                        Height = imgCover.Height * 0.15,
                        Uid = list_sort[i].Attributes["href"].Value
                    };
                    lbSort.FontSize = lbSort.Height * fsCoefficient;
                    lbSort.Width = list_sort[i].InnerText.Length * lbSort.FontSize * widthCoefficient + fsOffset;
                    lbSort.Margin = new Thickness(lbName.Margin.Left + SortW + 5, lbAuthor.Margin.Top + lbAuthor.Height + 5, 0, 0);
                    SortW += lbSort.Width + 5;
                    lbSort.Content = list_sort[i].InnerText;
                    lbSort.AddHandler(UIElement.MouseEnterEvent, new MouseEventHandler(Label_MouseEnter), true);
                    lbSort.AddHandler(UIElement.MouseLeaveEvent, new MouseEventHandler(Label_MouseLeave), true);
                    lbSort.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(LabelSort_MouseLeftButtonDown), true);
                    tGrid.Children.Add(lbSort);
                }
            }

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
            btnStartRead.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(BtnStartRead_MouseLeftButtonDown), true);
            tGrid.Children.Add(btnStartRead);

            /* 5個1列
             * row |
             * col -
             */
            ScrollViewer svChapter = new ScrollViewer
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, btnStartRead.Margin.Top + btnStartRead.Height, 0, 0),
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                Width = tGrid.Width,
                Height = tGrid.Height - btnStartRead.Margin.Top,
            };
            Grid chGrid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = tGrid.Width
            };
            HtmlNodeCollection chapterItem = (HtmlNodeCollection)brPageSetting.BtnChapter;
            int row = 0, col = 0;
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
                    Margin = new Thickness((ChapterItemW + ChapterItemDistance) * row + ChapterItemDistance, (ChapterItemH + 5) * col + 10, 0, 0),
                    BorderThickness = new Thickness(0),
                    Height = ChapterItemH,
                    Width = ChapterItemW,
                    FontSize = ChapterItemFS,
                    Content = chapterItem[i].SelectSingleNode("a").InnerText,
                    Uid = chapterItem[i].SelectSingleNode("a").Attributes["href"].Value
                };
                button.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(BtnChapter_MouseLeftButtonDown), true);
                chGrid.Children.Add(button);
            }
            chGrid.Height = (ChapterItemH + 5) * (col + 2) + 10;
            svChapter.Content = chGrid;
            tGrid.Children.Add(svChapter);
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            Label label = (Label)sender;
            label.Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 200, 0));
        }
        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            Label label = (Label)sender;
            label.Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255));
        }
        private void LabelAuthor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Label label = (Label)sender;
            tb_SearchBar.Text = label.Content.ToString();
            Console.WriteLine(label.Uid);
        }
        private void LabelSort_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Label label = (Label)sender;
            tb_SearchBar.Text = label.Content.ToString();
            Console.WriteLine(label.Uid);
        }
        private void BtnStartRead_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Button btn = (Button)sender;
            Console.WriteLine(btn.Uid);
        }
        private void BtnChapter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Button btn = (Button)sender;
            Console.WriteLine(btn.Uid);
        }
    }
}

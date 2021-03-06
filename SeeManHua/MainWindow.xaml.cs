﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CustomExtensions;
using System.Configuration;

namespace SeeManHua
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public struct Size_m
        {
            public double Height { get; set; }
            public double Width { get; set; }
            public double Length { get; set; }
            public double SerialNember { get; set; }
        }
        readonly static Size_m CoverSize = new Size_m
        {
            Height = 150,
            Width = 100
        };
        readonly static Size_m IntroSize = new Size_m
        {
            Height = 100,
            Width = 300
        };
        readonly static Size_m SearchBarSize = new Size_m
        {
            Height = 25,
            Width = 375
        };
        readonly static Size_m SearchIconSize = new Size_m
        {
            Height = 25,
            Width = 25
        };
        #region 搜尋結果清單
        //建立htmlweb
        HtmlWeb webClient = new HtmlWeb();
        List<Grid> List_manhuaGrid = new List<Grid> { };
        List<string> List_manhuaLink = new List<string> { };
        string strManHuaName, strManHuaIntro, searchContext, strManHuaLink;
        int pressEnter = 0;
        string searchHint = "搜些什麼吧...";
        ManhuarenHtml manhuarenHtml = new ManhuarenHtml();
        #endregion
        /// <summary>
        /// 網頁抽取
        /// </summary>
        class ManhuarenHtml
        {
            readonly string Title = "https://www.manhuaren.com";
            readonly string SearchHead = "/search?title=";
            readonly string SearchEnd = "&language=1";
            public ManhuarenHtml()
            {

            }
            /// <summary>
            /// 透過關鍵字尋找漫畫。
            /// </summary>
            /// <param name="target">要搜尋的漫畫名稱。</param>
            /// <returns>搜尋html格式字串。</returns>
            public string Search(string target)
            {
                return (Title + SearchHead + target + SearchEnd);
            }
            /// <summary>
            /// 鏈結到該漫畫的分頁。
            /// </summary>
            /// <param name="target">擷取的分頁名稱。</param>
            /// <returns>分頁格式字串。</returns>
            public string Link(string target)
            {
                return (Title + target);
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            //處理C# 連線 HTTPS 網站發生驗證失敗導致基礎連接已關閉
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
            //SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            #region 搜尋動作及結果
            //搜尋頁佈局
            LayoutSetting(LayoutMode.RL, Image_searchIcon, TextBox_search, "", false);
            Grid_manhua_menu.Margin = new Thickness(0);
            Grid_manhua_menu.Width = SearchBarSize.Width + SearchIconSize.Width;
            ListBox_manhua.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(ListBox_manhua_MouseLeftButtonDown), true);
            #endregion
        }
        private void Main_szChange(object sender, SizeChangedEventArgs e)
        {
            Grid_manhua_menu.Height = this.Window_BG.ActualHeight;
        }

        
        #region 佈局設定
        /// <summary>搜尋列。
        /// <para>僅負責設定排版位置，參數引進之前須設定好大小、顏色等等所需內容。</para>
        /// </summary>
        /// <param name="mode">排版模式</param>
        /// <param name="img">圖片</param>
        /// <param name="str">文字框提示文字</param>
        /// <param name="addToReturn">是否需要將項目放進回傳的Grid中</param>
        /// <returns>回傳放入參數的Grid.</returns>
        public Grid LayoutSetting(LayoutMode mode, Image img, TextBox textBox, string str, bool addToReturn)
        {
            Grid grid = new Grid();
            switch (mode)
            {
                case LayoutMode.LR:
                    img.HorizontalAlignment = HorizontalAlignment.Left;
                    img.VerticalAlignment = VerticalAlignment.Top;
                    img.Margin = new Thickness(2);
                    img.Width = SearchIconSize.Width;
                    img.Height = SearchIconSize.Height;
                    textBox.HorizontalAlignment = HorizontalAlignment.Left;
                    textBox.VerticalAlignment = VerticalAlignment.Top;
                    textBox.Background = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));
                    textBox.Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 200, 200));
                    textBox.Opacity = 30;
                    textBox.Margin = new Thickness(img.Margin.Left + img.Width, img.Margin.Top, 0, 0);
                    textBox.Width = SearchBarSize.Width;
                    textBox.Height = SearchBarSize.Height;
                    grid.HorizontalAlignment = HorizontalAlignment.Left;
                    grid.VerticalAlignment = VerticalAlignment.Top;
                    grid.Margin = new Thickness(0);
                    grid.Width = img.Width + textBox.Width;
                    grid.Height = img.Height;
                    grid.Background = new SolidColorBrush(Color.FromArgb(100, 43, 43, 43));
                    break;
                case LayoutMode.RL:
                    textBox.HorizontalAlignment = HorizontalAlignment.Left;
                    textBox.VerticalAlignment = VerticalAlignment.Top;
                    textBox.Background = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));
                    textBox.Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 200, 200));
                    textBox.Opacity = 30;
                    textBox.Margin = new Thickness(2);
                    textBox.Width = SearchBarSize.Width;
                    textBox.Height = SearchBarSize.Height;
                    img.HorizontalAlignment = HorizontalAlignment.Left;
                    img.VerticalAlignment = VerticalAlignment.Top;
                    img.Margin = new Thickness(textBox.Margin.Left + textBox.Width, textBox.Margin.Top, 0, 0);
                    img.Width = SearchIconSize.Width;
                    img.Height = SearchIconSize.Height;
                    grid.HorizontalAlignment = HorizontalAlignment.Left;
                    grid.VerticalAlignment = VerticalAlignment.Top;
                    grid.Margin = new Thickness(0);
                    grid.Width = img.Width + textBox.Width;
                    grid.Height = img.Height;
                    grid.Background = new SolidColorBrush(Color.FromArgb(100, 43, 43, 43));
                    break;
                default:

                    break;
            }
            if (addToReturn)
            {
                grid.Children.Add(textBox);
                grid.Children.Add(img);
            }
            return grid;
        }

        /// <summary>圖片、描述
        /// <para>僅負責設定排版位置，參數引進之前須設定好大小、顏色等等所需內容。</para>
        /// </summary>
        /// <param name="mode">排版模式</param>
        /// <param name="img">圖片</param>
        /// <param name="str">描述/ 內容</param>
        /// <returns>包裝好排版的Grid.</returns>
        public Grid LayoutSetting(LayoutMode mode, Image img, string str)
        {
            Grid grid = new Grid();
            Label label = new Label();
            switch (mode)
            {
                case LayoutMode.LR:
                    img.HorizontalAlignment = HorizontalAlignment.Left;
                    img.VerticalAlignment = VerticalAlignment.Center;
                    img.Margin = new Thickness(2);
                    img.Width = CoverSize.Width;
                    img.Height = CoverSize.Height;
                    label.Content = str;
                    label.HorizontalAlignment = HorizontalAlignment.Left;
                    label.VerticalAlignment = VerticalAlignment.Center;
                    label.HorizontalContentAlignment = HorizontalAlignment.Left;
                    label.VerticalContentAlignment = VerticalAlignment.Center;
                    label.Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255));
                    label.Margin = new Thickness(img.Margin.Left + img.Width, img.Margin.Top, 0, 0);
                    label.Width = IntroSize.Width;
                    label.Height = IntroSize.Height;
                    grid.HorizontalAlignment = HorizontalAlignment.Right;
                    grid.VerticalAlignment = VerticalAlignment.Top;
                    grid.Margin = new Thickness(-5);
                    grid.Width = img.Width + label.Width;
                    grid.Height = img.Height;
                    grid.Background = new SolidColorBrush(Color.FromArgb(100, 43, 43, 43));
                    grid.Children.Add(img);
                    grid.Children.Add(label);
                    break;
                default:

                    break;
            }
            return grid;
        }
        /// <summary> 單一圖片輸出
        /// <para>置中</para>
        /// </summary>
        /// <param name="img">要輸出的圖片</param>
        /// <returns></returns>
        public Grid LayoutSetting(Image img)
        {
            Grid grid = new Grid();
            img.HorizontalAlignment = HorizontalAlignment.Center;
            img.VerticalAlignment = VerticalAlignment.Center;
            img.Margin = new Thickness(0);
            img.Width = SearchBarSize.Width + SearchIconSize.Width;
            img.Height = SearchBarSize.Width + SearchIconSize.Width;
            grid.Children.Add(img);
            return grid;
        }
        #endregion

        /// <summary>
        /// 搜尋列的搜尋功能
        /// </summary>
        /// <param name="sender">TextBox_search</param>
        /// <param name="e"></param>
        private void SearchBar_key_pup(object sender, KeyEventArgs e)
        {
            try
            {
                TextBox textBox = (TextBox)sender;
                //只要按到鍵盤就會觸發..
                switch (e.Key)
                {
                    //這邊以新注音的做法來做判斷，因為要選字的關係，所以要確認選完之後按下enter才開始搜尋。
                    //日後應加上輸入法判斷。
                    case Key.Enter:
                        if (pressEnter == 2)
                        {
                            Console.WriteLine("Click PreviewKeyUp= Enter" + searchContext);
                            //執行前先清空所有內容
                            List_manhuaLink.Clear();
                            List_manhuaGrid.Clear();
                            ListBox_manhua.ItemsSource = null;
                            //載入網址資料
                            searchContext = textBox.Text;
                            HtmlDocument doc = webClient.Load(manhuarenHtml.Search(searchContext));
                            Console.WriteLine(doc.Text);
                            //lb_html.Content = doc.Text;   //確認是否有抓到html代碼
                            try
                            {
                                /* SelectNodes: 切換到指定的層
                             * Return: 該層內的所有內容，標籤相同的會成為list
                             * 註: 回傳為list之後起始位置=0, 尚未回傳要直接指向的時候起始位置=1
                             */
                                HtmlNodeCollection list = doc.DocumentNode.SelectNodes("/html/body/ul/li");
                                //將掃到的內容全部排列後放到清單中
                                for (int cnt = 0; cnt < list.Count; cnt++)
                                {
                                    //標籤img內有需要的網址，透過Attribute取出
                                    string Url = list[cnt].SelectSingleNode("div[1]/a/img").Attributes["src"].Value;
                                    //抽出分頁鏈結，並加入List
                                    strManHuaLink= list[cnt].SelectSingleNode("div[1]/a").Attributes["href"].Value;
                                    List_manhuaLink.Add(strManHuaLink);
                                    //將網址字串轉為Uri格式
                                    Uri uri = new Uri(Url);
                                    //透過網址下載圖片轉為BitmapImage格式
                                    BitmapImage image = new BitmapImage(uri);
                                    /* 使用Image儲存該BitmapImage
                                     * 註: 透過引數傳送指向的Image會是同一個位址，無法使用同一個Image來複寫傳送圖像資料，必須將每一筆圖像都分開建立成獨立檔案。
                                    */
                                    Image img = new Image
                                    {
                                        Source = image
                                    };
                                    Thread.Sleep(5);
                                    strManHuaName = list[cnt].SelectSingleNode("div[2]/a/p[1]").InnerText;
                                    strManHuaIntro = list[cnt].SelectSingleNode("div[2]/a/p[2]").InnerText;
                                    List_manhuaGrid.Add(LayoutSetting(LayoutMode.LR, img, strManHuaName + "\n" + strManHuaIntro));
                                }
                            }
                            catch (Exception)
                            {
                                HtmlNodeCollection list = doc.DocumentNode.SelectNodes("/html/body");
                                //標籤img內有需要的網址，透過Attribute取出
                                string Url = list[0].SelectSingleNode("div[1]/img").Attributes["src"].Value;
                                //將網址字串轉為Uri格式
                                Uri uri = new Uri(Url);
                                //透過網址下載圖片轉為BitmapImage格式
                                BitmapImage image = new BitmapImage(uri);
                                /* 使用Image儲存該BitmapImage
                                     * 註: 透過引數傳送指向的Image會是同一個位址，無法使用同一個Image來複寫傳送圖像資料，必須將每一筆圖像都分開建立成獨立檔案。
                                    */
                                Image img = new Image
                                {
                                    Source = image
                                };
                                List_manhuaLink.Add("搜尋不到結果，請更換關鍵字。");
                                List_manhuaGrid.Add(LayoutSetting(img));
                            }
                            //設定列表排版
                            ListBox_manhua.HorizontalAlignment = HorizontalAlignment.Left;
                            ListBox_manhua.Margin = new Thickness(0, SearchBarSize.Height, 0, 0);
                            ListBox_manhua.Width = CoverSize.Width + IntroSize.Width;

                            //加入抓到的清單
                            ListBox_manhua.ItemsSource = List_manhuaGrid;
                            pressEnter = 0;
                        }
                        else
                        {
                            pressEnter += 1;
                        }
                        break;
                    case Key.Escape:
                        textBox.Text = "";
                        pressEnter = 0;
                        break;
                    default:
                        pressEnter = 0;
                        break;
                }
                Console.WriteLine("pressEnter= " + pressEnter);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        /// <summary>
        /// 搜尋結果的左鍵功能
        /// </summary>
        /// <param name="sender">ListBox_manhua</param>
        /// <param name="e"></param>
        private void ListBox_manhua_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //不使用webbrowser, 自己做介面來輸出內容。
                ListBox listBox = (ListBox)sender;
                //當內容為空SelectedIndex= -1
                if (listBox.SelectedIndex != -1)
                {
                    Console.WriteLine(manhuarenHtml.Link(List_manhuaLink[listBox.SelectedIndex]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

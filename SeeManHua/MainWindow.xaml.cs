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
using System.Windows.Documents;

namespace SeeManHua
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        MyFormat myFormat = new MyFormat();
        #region 搜尋結果清單
        SearchList searchList;
        //建立htmlweb
        HtmlWeb webClient = new HtmlWeb();
        List<Grid> List_manhuaGrid = new List<Grid> { };
        List<string> List_manhuaLink = new List<string> { };
        string strManHuaName, strManHuaIntro, strManHuaSort, strManHuaStatus, searchContext, strManHuaLink;
        int pressEnter = 0;
        ManhuarenHtml manhuarenHtml = new ManhuarenHtml();
        #endregion
        #region BrowsePage
        BrowsePage browsePage;
        #endregion
        /// <summary>
        /// 網頁抽取
        /// </summary>
        class ManhuarenHtml
        {
            readonly string Title = "https://www.manhuaren.com";
            readonly string SearchHead = "/search?title=";
            readonly string SearchEnd = "&language=1";
            public ManhuarenHtml() { }
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
        /// <summary>
        /// 主要動作都在這邊做，布局、宣告、排列等等。
        /// </summary>
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
            Grid_manhua_menu.Width = myFormat.SearchBarWidth + myFormat.SearchIconWidth;
            ListBox_manhua.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(ListBox_manhua_MouseLeftButtonDown), true);
            #endregion

            #region 觀賞區
            Grid_browse.Margin = new Thickness(myFormat.SearchBarWidth + myFormat.SearchIconWidth, 0, 0, 0);
            Grid_browse.Background = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255));
            #endregion
        }
        /// <summary>
        /// 畫面改變大小得時候觸發。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_szChange(object sender, SizeChangedEventArgs e)
        {
            //抓取畫面變化的數值
            double winW = this.Window_BG.ActualWidth, winH = this.Window_BG.ActualHeight;
            //列表布置
            Grid_manhua_menu.Height = winH;
            //觀賞區布置
            Grid_browse.Width = winW - myFormat.SearchBarWidth - myFormat.SearchIconWidth;
            Grid_browse.Height = winH;
            //searchList = new SearchList(main_bg, TextBox_search, Image_searchIcon, Grid_manhua_menu);
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
                    img.Width = myFormat.SearchIconWidth;
                    img.Height = myFormat.SearchIconHeight;
                    textBox.HorizontalAlignment = HorizontalAlignment.Left;
                    textBox.VerticalAlignment = VerticalAlignment.Top;
                    textBox.Background = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));
                    textBox.Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 200, 200));
                    textBox.Opacity = 30;
                    textBox.Margin = new Thickness(img.Margin.Left + img.Width, img.Margin.Top, 0, 0);
                    textBox.Width = myFormat.SearchBarWidth;
                    textBox.Height = myFormat.SearchBarHeight;
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
                    textBox.Width = myFormat.SearchBarWidth;
                    textBox.Height = myFormat.SearchBarHeight;
                    img.HorizontalAlignment = HorizontalAlignment.Left;
                    img.VerticalAlignment = VerticalAlignment.Top;
                    img.Margin = new Thickness(textBox.Margin.Left + textBox.Width, textBox.Margin.Top, 0, 0);
                    img.Width = myFormat.SearchIconWidth;
                    img.Height = myFormat.SearchIconHeight;
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
        public Grid LayoutSetting(LayoutMode mode, Image img, string sName, string sIntro, string sSort, string sStatus)
        {
            Grid grid = new Grid();
            TextBox textBox_Intro = new TextBox();
            TextBox textBox_Name = new TextBox();
            TextBox textBox_Sort = new TextBox();
            TextBox textBox_Status = new TextBox();
            switch (mode)
            {
                case LayoutMode.LR:
                    img.HorizontalAlignment = HorizontalAlignment.Left;
                    img.VerticalAlignment = VerticalAlignment.Center;
                    img.Margin = new Thickness(2);
                    img.Width = myFormat.ResultCoverWidth;
                    img.Height = myFormat.ResultCoverHeight;
                    textBox_Name.TextWrapping = TextWrapping.WrapWithOverflow;
                    textBox_Name.HorizontalAlignment = HorizontalAlignment.Left;
                    textBox_Name.VerticalAlignment = VerticalAlignment.Top;
                    textBox_Name.HorizontalContentAlignment = HorizontalAlignment.Left;
                    textBox_Name.VerticalContentAlignment = VerticalAlignment.Top;
                    textBox_Name.Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255));
                    textBox_Name.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
                    textBox_Name.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
                    textBox_Name.Margin = new Thickness(img.Margin.Left + img.Width, img.Margin.Top, 0, 0);
                    textBox_Name.BorderThickness = new Thickness(0);
                    textBox_Name.Width = myFormat.ResultNameWidth;
                    textBox_Name.Height = myFormat.ResultNameHeight;
                    textBox_Name.IsReadOnly = true;
                    textBox_Name.FontWeight = FontWeights.Bold;
                    textBox_Name.FontSize = myFormat.ResultNameFontSize;
                    textBox_Name.Text = sName;
                    textBox_Intro.TextWrapping = TextWrapping.WrapWithOverflow;
                    textBox_Intro.HorizontalAlignment = HorizontalAlignment.Left;
                    textBox_Intro.VerticalAlignment = VerticalAlignment.Top;
                    textBox_Intro.HorizontalContentAlignment = HorizontalAlignment.Left;
                    textBox_Intro.VerticalContentAlignment = VerticalAlignment.Top;
                    textBox_Intro.Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255));
                    textBox_Intro.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
                    textBox_Intro.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
                    textBox_Intro.Margin = new Thickness(img.Margin.Left + img.Width, img.Margin.Top + textBox_Name.Height, 0, 0);
                    textBox_Intro.BorderThickness = new Thickness(0);
                    textBox_Intro.Width = myFormat.ResultIntroWidth;
                    textBox_Intro.Height = myFormat.ResultIntroHeight;
                    textBox_Intro.IsReadOnly = true;
                    textBox_Intro.FontWeight = FontWeights.Normal;
                    textBox_Intro.FontSize = myFormat.ResultIntroFontSize;
                    textBox_Intro.Text = sIntro;
                    textBox_Sort.TextWrapping = TextWrapping.WrapWithOverflow;
                    textBox_Sort.HorizontalAlignment = HorizontalAlignment.Left;
                    textBox_Sort.VerticalAlignment = VerticalAlignment.Top;
                    textBox_Sort.HorizontalContentAlignment = HorizontalAlignment.Left;
                    textBox_Sort.VerticalContentAlignment = VerticalAlignment.Top;
                    textBox_Sort.Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 200, 0));
                    textBox_Sort.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
                    textBox_Sort.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
                    textBox_Sort.Margin = new Thickness(img.Margin.Left + img.Width, textBox_Intro.Margin.Top + textBox_Intro.Height, 0, 0);
                    textBox_Sort.BorderThickness = new Thickness(0);
                    textBox_Sort.Width = myFormat.ResultSortWidth;
                    textBox_Sort.Height = myFormat.ResultSortHeight;
                    textBox_Sort.IsReadOnly = true;
                    textBox_Sort.FontWeight = FontWeights.Normal;
                    textBox_Sort.FontSize = myFormat.ResultSortFontSize;
                    textBox_Sort.Text = sSort;
                    textBox_Status.TextWrapping = TextWrapping.WrapWithOverflow;
                    textBox_Status.HorizontalAlignment = HorizontalAlignment.Left;
                    textBox_Status.VerticalAlignment = VerticalAlignment.Top;
                    textBox_Status.HorizontalContentAlignment = HorizontalAlignment.Right;
                    textBox_Status.VerticalContentAlignment = VerticalAlignment.Top;
                    textBox_Status.Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 200, 0));
                    textBox_Status.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
                    textBox_Status.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
                    textBox_Status.Margin = new Thickness(textBox_Sort.Margin.Left + textBox_Sort.Width, textBox_Intro.Margin.Top + textBox_Intro.Height, 0, 0);
                    textBox_Status.BorderThickness = new Thickness(0);
                    textBox_Status.Width = myFormat.ResultStatusWidth;
                    textBox_Status.Height = myFormat.ResultStatusHeight;
                    textBox_Status.IsReadOnly = true;
                    textBox_Status.FontWeight = FontWeights.Normal;
                    textBox_Status.FontSize = myFormat.ResultStatusFontSize;
                    textBox_Status.Text = sStatus;
                    grid.HorizontalAlignment = HorizontalAlignment.Right;
                    grid.VerticalAlignment = VerticalAlignment.Top;
                    grid.Margin = new Thickness(-5);
                    grid.Width = img.Width + textBox_Intro.Width;
                    grid.Height = img.Height;
                    grid.Background = new SolidColorBrush(Color.FromArgb(100, 43, 43, 43));
                    grid.Children.Add(img);
                    grid.Children.Add(textBox_Name);
                    grid.Children.Add(textBox_Intro);
                    grid.Children.Add(textBox_Sort);
                    grid.Children.Add(textBox_Status);
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
            img.Width = myFormat.SearchBarWidth + myFormat.SearchIconWidth;
            img.Height = myFormat.SearchBarHeight + myFormat.SearchIconHeight;
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
                            //Console.WriteLine(doc.Text);
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
                                    strManHuaSort = list[cnt].SelectSingleNode("div[2]/p/span[1]").InnerText;
                                    strManHuaStatus = list[cnt].SelectSingleNode("div[2]/p/span").InnerText;
                                    List_manhuaGrid.Add(LayoutSetting(LayoutMode.LR, img, strManHuaName, strManHuaIntro, strManHuaSort, strManHuaStatus));
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
                            ListBox_manhua.Margin = new Thickness(0, myFormat.SearchBarHeight, 0, 0);
                            ListBox_manhua.Width = myFormat.ResultCoverWidth + myFormat.ResultIntroWidth;

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
                List<Button> list_chapters=new List<Button> { };
                Image image = new Image();
                Label lbName = new Label();
                Label lbAuthor = new Label();
                Label lbSort = new Label();
                TextBox tbIntro = new TextBox();
                Button btnStartRead = new Button();

                //當內容為空SelectedIndex= -1
                if (listBox.SelectedIndex != -1)
                {
                    Console.WriteLine(manhuarenHtml.Link(List_manhuaLink[listBox.SelectedIndex]));
                    //進行畫面的排版
                    
                    HtmlDocument doc = webClient.Load(manhuarenHtml.Link(List_manhuaLink[listBox.SelectedIndex]));
                    //Image
                    string coverLink = doc.DocumentNode.SelectSingleNode("/html/body/div[3]/div[1]/img").Attributes["src"].Value;
                    //將網址字串轉為Uri格式
                    Uri uri = new Uri(coverLink);
                    //透過網址下載圖片轉為BitmapImage格式
                    BitmapImage bmg = new BitmapImage(uri);
                    image.Source = bmg;
                    Console.WriteLine("CoverLink= " + coverLink);
                    Grid_browse.Children.Add(image);
                    //Label
                    string Name = doc.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/p[1]").InnerText;
                    lbName.Content = Name;
                    Console.WriteLine("Name= " + Name);
                    Grid_browse.Children.Add(lbName);
                    //Label
                    string Author = doc.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/p[3]/a").InnerText;
                    lbAuthor.Content = Author;
                    Console.WriteLine("Author= " + Author);
                    Grid_browse.Children.Add(lbAuthor);
                    //Label
                    string sSort = doc.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/p[4]/span/a").InnerText;
                    lbSort.Content = sSort;
                    Console.WriteLine("Sort= " + sSort);
                    Grid_browse.Children.Add(lbSort);
                    //TextBox
                    string sIntro = doc.DocumentNode.SelectSingleNode("/html/body/p").InnerText;
                    tbIntro.Text = sIntro;
                    Console.WriteLine("Intro= " + sIntro);
                    Grid_browse.Children.Add(tbIntro);
                    //Button
                    string StartRead = doc.DocumentNode.SelectSingleNode("/html/body/div[7]/a[4]").Attributes["href"].Value;
                    btnStartRead.Content = StartRead;
                    Console.WriteLine("StartRead Link= " + StartRead);
                    Grid_browse.Children.Add(btnStartRead);
                    //List<Button>
                    HtmlNodeCollection list_chapter = doc.DocumentNode.SelectNodes("/html/body/div[5]/ul[1]/li");
                    browsePage = new BrowsePage(image, lbName, lbAuthor, lbSort, tbIntro, btnStartRead, list_chapters, list_chapter.Count);
                    for (int i = 0; i < list_chapters.Count; i++)
                    {
                        list_chapters[i].Content = list_chapter[i].SelectSingleNode("a").InnerText;
                        Grid_browse.Children.Add(list_chapters[i]);
                    }
                    Console.WriteLine("Chapter Count= " + list_chapter.Count);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

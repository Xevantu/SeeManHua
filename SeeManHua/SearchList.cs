using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SeeManHua
{
    class SearchList
    {
        double width, height, hDw, menuWidth;
        //建立htmlweb
        HtmlWeb webClient = new HtmlWeb();
        List<Grid> List_manhuaGrid = new List<Grid> { };
        List<string> List_manhuaLink = new List<string> { };
        string strManHuaName, strManHuaIntro, strManHuaSort, strManHuaStatus, searchContext, strManHuaLink;
        int pressEnter = 0;
        /// <summary>
        /// 字元與寬度之間的係數
        /// </summary>
        double widthCoefficient = 1.2;
        /// <summary>
        /// 外框高度與字元大小的係數
        /// </summary>
        double fsCoefficient = 0.6;
        ListBox ListBox_manhua;
        ManHuaRenHtml manhuarenHtml = new ManHuaRenHtml();
        Grid Grid_browse, Grid_menu;
        #region BrowsePage
        BrowsePage browsePage;
        BrowsePageSetting brPageSetting;
        #endregion
        public SearchList(Grid mGrid, Grid bGrid, SearchListSetting listSetting)
        {
            try
            {
                width = 400;
                menuWidth = width + 2;
                height = 30;
                Grid_browse = bGrid;
                Grid_menu = mGrid;
                TextBox textBox = new TextBox
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    VerticalContentAlignment = VerticalAlignment.Bottom,
                    Background = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0)),
                    Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 200, 200)),
                    BorderThickness = new Thickness(0, 0, 0, 2),
                    BorderBrush = new SolidColorBrush(Color.FromArgb(80, 255, 200, 20)),
                    Opacity = 30,
                    Margin = new Thickness(2, 2, 0, 0),
                    Width = width,
                    Height = height,
                    FontSize = height * fsCoefficient
                };
                ListBox_manhua = new ListBox
                {
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(1, textBox.Margin.Top + textBox.Height + 10, 0, 0),
                    Background = new SolidColorBrush(Color.FromArgb(0, 0, 43, 43)),
                    BorderThickness = new Thickness(0),
                    Width = width
                };
                ListBox_manhua.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
                ListBox_manhua.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);

                mGrid.HorizontalAlignment = HorizontalAlignment.Left;
                mGrid.VerticalAlignment = VerticalAlignment.Top;
                mGrid.Margin = new Thickness(0);
                mGrid.Width = menuWidth;
                mGrid.Background = new SolidColorBrush(Color.FromArgb(0, 0, 43, 43));
                
                mGrid.Children.Add(textBox);
                mGrid.Children.Add(ListBox_manhua);
                ListBox_manhua.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(ListBox_manhua_MouseLeftButtonDown), true);
                textBox.PreviewKeyDown += SearchBar_key_pup;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

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
            img.Width = 300;
            img.Height = 50;
            grid.Children.Add(img);
            return grid;
        }

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
                        if (pressEnter == 0)
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
                                    strManHuaLink = list[cnt].SelectSingleNode("div[1]/a").Attributes["href"].Value;
                                    List_manhuaLink.Add(strManHuaLink);
                                    //將網址字串轉為Uri格式
                                    Uri uri = new Uri(Url);
                                    //透過網址下載圖片轉為BitmapImage格式
                                    BitmapImage image = new BitmapImage(uri);
                                    hDw = image.Height / image.Width;
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
                                    List_manhuaGrid.Add(LayoutSetting(img, strManHuaName, strManHuaIntro, strManHuaSort, strManHuaStatus));
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
        #region 佈局設定
        /// <summary>圖片、描述
        /// <para>僅負責設定排版位置，參數引進之前須設定好大小、顏色等等所需內容。</para>
        /// </summary>
        /// <param name="mode">排版模式</param>
        /// <param name="img">圖片</param>
        /// <param name="str">描述/ 內容</param>
        /// <returns>包裝好排版的Grid.</returns>
        public Grid LayoutSetting(Image img, string sName, string sIntro, string sSort, string sStatus)
        {
            Grid grid = new Grid();
            TextBox textBox_Intro = new TextBox();
            TextBox textBox_Name = new TextBox();
            TextBox textBox_Sort = new TextBox();
            TextBox textBox_Status = new TextBox();
            img.HorizontalAlignment = HorizontalAlignment.Left;
            img.VerticalAlignment = VerticalAlignment.Center;
            img.Margin = new Thickness(2);
            img.Width = width * 0.3;
            img.Height = img.Width * hDw;
            textBox_Name.TextWrapping = TextWrapping.WrapWithOverflow;
            textBox_Name.HorizontalAlignment = HorizontalAlignment.Left;
            textBox_Name.VerticalAlignment = VerticalAlignment.Top;
            textBox_Name.HorizontalContentAlignment = HorizontalAlignment.Left;
            textBox_Name.VerticalContentAlignment = VerticalAlignment.Top;
            textBox_Name.Foreground = new SolidColorBrush(Color.FromArgb(100, 200, 255, 255));
            textBox_Name.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            textBox_Name.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            textBox_Name.Margin = new Thickness(img.Margin.Left + img.Width, img.Margin.Top, 0, 0);
            textBox_Name.BorderThickness = new Thickness(0);
            textBox_Name.Width = width - img.Width;
            textBox_Name.Height = img.Height * 0.2;
            textBox_Name.IsReadOnly = true;
            textBox_Name.FontWeight = FontWeights.Bold;
            textBox_Name.FontSize = textBox_Name.Height * fsCoefficient;
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
            textBox_Intro.Width = width - img.Width;
            textBox_Intro.Height = img.Height * 0.6;
            textBox_Intro.IsReadOnly = true;
            textBox_Intro.FontWeight = FontWeights.Normal;
            textBox_Intro.FontSize = 14;
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
            textBox_Sort.Width = width - img.Width;
            textBox_Sort.Height = img.Height * 0.15;
            textBox_Sort.IsReadOnly = true;
            textBox_Sort.FontWeight = FontWeights.Normal;
            textBox_Sort.FontSize = textBox_Sort.Height * fsCoefficient;
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
            textBox_Status.Width = width - img.Width;
            textBox_Status.Height = img.Height * 0.15;
            textBox_Status.IsReadOnly = true;
            textBox_Status.FontWeight = FontWeights.Normal;
            textBox_Status.FontSize = textBox_Status.Height * fsCoefficient;
            textBox_Status.Text = sStatus;
            grid.HorizontalAlignment = HorizontalAlignment.Right;
            grid.VerticalAlignment = VerticalAlignment.Top;
            grid.Margin = new Thickness(0, 2, 0, 2);
            grid.Width = img.Width + textBox_Intro.Width;
            grid.Height = img.Height;
            grid.Background = new SolidColorBrush(Color.FromArgb(100, 43, 43, 43));
            grid.Children.Add(img);
            grid.Children.Add(textBox_Name);
            grid.Children.Add(textBox_Intro);
            grid.Children.Add(textBox_Sort);
            grid.Children.Add(textBox_Status);
            return grid;
        }

        #endregion
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
                brPageSetting = new BrowsePageSetting();
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
                    brPageSetting.Bimg = bmg;
                    Console.WriteLine("CoverLink= " + coverLink);
                    //Grid_browse.Children.Add(image);
                    //Label
                    string Name = doc.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/p[1]").InnerText;
                    brPageSetting.Name = Name;
                    Console.WriteLine("Name= " + Name);
                    //Grid_browse.Children.Add(lbName);
                    //Label
                    string Author = doc.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/p[3]/a").InnerText;
                    brPageSetting.Author = Author;
                    Console.WriteLine("Author= " + Author);
                    //Grid_browse.Children.Add(lbAuthor);
                    //Label
                    string sSort = doc.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/p[4]/span/a").InnerText;
                    brPageSetting.Sort = sSort;
                    Console.WriteLine("Sort= " + sSort);
                    //Grid_browse.Children.Add(lbSort);
                    //TextBox
                    string Intro = doc.DocumentNode.SelectSingleNode("/html/body/p").InnerText;
                    brPageSetting.Intro = Intro;
                    Console.WriteLine("Intro= " + Intro);
                    //Grid_browse.Children.Add(tbIntro);
                    //Button
                    string StartRead = doc.DocumentNode.SelectSingleNode("/html/body/div[7]/a[4]").Attributes["href"].Value;
                    brPageSetting.StartRead = StartRead;
                    Console.WriteLine("StartRead Link= " + StartRead);
                    //Grid_browse.Children.Add(btnStartRead);
                    //List<Button>
                    HtmlNodeCollection list_chapter = doc.DocumentNode.SelectNodes("/html/body/div[5]/ul[1]/li");
                    brPageSetting.BtnChapter = list_chapter;
                    browsePage = new BrowsePage(Grid_browse, brPageSetting);
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

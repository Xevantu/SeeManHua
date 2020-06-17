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
        double width, height, hDw = 1.333, menuWidth;
        //建立htmlweb
        HtmlWeb webClient = new HtmlWeb();
        List<Grid> List_manhuaGrid = new List<Grid> { };
        TextBox tb_SearchBar = new TextBox();
        //List<string> List_manhuaLink = new List<string> { };
        //string strManHuaName, strManHuaIntro, strManHuaSort, strManHuaStatus, searchContext, strManHuaLink;
        List<SearchResult> List_SearchResult = new List<SearchResult>();
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
        /// <summary>
        /// 搜尋列的類。
        /// </summary>
        public class SearchResult
        {
            public Image image;
            public string link, name, intro, sort, status;
        };
        /// <summary>
        /// Refresh SearchList to search target.
        /// </summary>
        public SearchList() { }
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
                tb_SearchBar = new TextBox
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
                    Margin = new Thickness(1, tb_SearchBar.Margin.Top + tb_SearchBar.Height + 10, 0, 0),
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
                
                mGrid.Children.Add(tb_SearchBar);
                mGrid.Children.Add(ListBox_manhua);
                ListBox_manhua.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(ListBox_manhua_MouseLeftButtonDown), true);
                tb_SearchBar.TextChanged += SearchBar_TextChanged;
                tb_SearchBar.PreviewKeyDown += SearchBar_key_pup;
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
        /// 當文字發生改變時觸發。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            //當鍵盤的目標位置不在搜尋列上，才可觸發該項目。
            if (!box.IsKeyboardFocusWithin)
            {
                Search(box.Text);
            }
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
                    case Key.Enter:
                        if (pressEnter == 0)
                        {
                            Search(textBox.Text);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void Search(string target)
        {
            Console.WriteLine("Click PreviewKeyUp= Enter " + target);
            //執行前先清空所有內容
            List_SearchResult.Clear();
            List_manhuaGrid.Clear();
            ListBox_manhua.ItemsSource = null;
            //載入網址資料
            //searchContext = textBox.Text;
            HtmlDocument doc = webClient.Load(manhuarenHtml.Search(target));
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
                    SearchResult result = new SearchResult();
                    //標籤img內有需要的網址，透過Attribute取出
                    string Url = list[cnt].SelectSingleNode("div[1]/a/img").Attributes["src"].Value;
                    //抽出分頁鏈結，並加入List
                    //strManHuaLink = list[cnt].SelectSingleNode("div[1]/a").Attributes["href"].Value;
                    result.link = list[cnt].SelectSingleNode("div[1]/a").Attributes["href"].Value;

                    //List_manhuaLink.Add(strManHuaLink);
                    //將網址字串轉為Uri格式
                    Uri uri = new Uri(Url);
                    //透過網址下載圖片轉為BitmapImage格式
                    BitmapImage image = new BitmapImage(uri);
                    //hDw = image.Height / image.Width;
                    //Console.WriteLine("HDW= " + hDw*100);
                    /* 使用Image儲存該BitmapImage
                     * 註: 透過引數傳送指向的Image會是同一個位址，無法使用同一個Image來複寫傳送圖像資料，必須將每一筆圖像都分開建立成獨立檔案。
                    */
                    Image img = new Image
                    {
                        //填滿圖片框
                        Stretch = Stretch.UniformToFill,
                        Source = image
                    };
                    Thread.Sleep(5);
                    result.image = img;
                    //strManHuaName = list[cnt].SelectSingleNode("div[2]/a/p[1]").InnerText;
                    result.name = list[cnt].SelectSingleNode("div[2]/a/p[1]").InnerText;
                    //strManHuaIntro = list[cnt].SelectSingleNode("div[2]/a/p[2]").InnerText;
                    result.intro = list[cnt].SelectSingleNode("div[2]/a/p[2]").InnerText;
                    //strManHuaSort = list[cnt].SelectSingleNode("div[2]/p/span[1]").InnerText;
                    HtmlNodeCollection list_sort = list[cnt].SelectNodes("div[2]/p/a");
                    if (list_sort != null)
                    {
                        for (int i = 0; i < list_sort.Count; i++)
                        {
                            result.sort += list_sort[i].SelectSingleNode("span").InnerText;
                            if (i + 1 != list_sort.Count) result.sort += ", ";
                        }
                    }
                    //strManHuaStatus = list[cnt].SelectSingleNode("div[2]/p/span").InnerText;
                    result.status = list[cnt].SelectSingleNode("div[2]/p/span").InnerText;
                    List_SearchResult.Add(result);
                    List_manhuaGrid.Add(LayoutSetting(result));
                }
            }
            catch (Exception)
            {
                SearchResult result = new SearchResult();
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
                result.image = img;
                result.link = "搜尋不到結果，請更換關鍵字。";
                List_SearchResult.Add(result);
                List_manhuaGrid.Add(LayoutSetting(img));
            }
            //加入抓到的清單
            ListBox_manhua.ItemsSource = List_manhuaGrid;
            //更新後至頂
            ListBox_manhua.ScrollIntoView(ListBox_manhua.Items[0]);
        }
        #region 佈局設定
        /// <summary>圖片、描述
        /// <para>僅負責設定排版位置，參數引進之前須設定好大小、顏色等等所需內容。</para>
        /// </summary>
        /// <param name="result"></param>
        /// <returns>包裝好的排版Grid</returns>
        public Grid LayoutSetting(SearchResult result)
        {
            Grid grid = new Grid();
            TextBox textBox_Intro = new TextBox();
            TextBox textBox_Name = new TextBox();
            TextBox textBox_Sort = new TextBox();
            TextBox textBox_Status = new TextBox();
            result.image.HorizontalAlignment = HorizontalAlignment.Left;
            result.image.VerticalAlignment = VerticalAlignment.Center;
            result.image.Margin = new Thickness(2);
            result.image.Width = width * 0.3;
            result.image.Height = result.image.Width * hDw;
            textBox_Name.TextWrapping = TextWrapping.WrapWithOverflow;
            textBox_Name.HorizontalAlignment = HorizontalAlignment.Left;
            textBox_Name.VerticalAlignment = VerticalAlignment.Top;
            textBox_Name.HorizontalContentAlignment = HorizontalAlignment.Left;
            textBox_Name.VerticalContentAlignment = VerticalAlignment.Top;
            textBox_Name.Foreground = new SolidColorBrush(Color.FromArgb(100, 200, 255, 255));
            textBox_Name.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            textBox_Name.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            textBox_Name.Margin = new Thickness(result.image.Margin.Left + result.image.Width, result.image.Margin.Top, 0, 0);
            textBox_Name.BorderThickness = new Thickness(0);
            textBox_Name.Width = width - result.image.Width;
            textBox_Name.Height = result.image.Height * 0.2;
            textBox_Name.IsReadOnly = true;
            textBox_Name.FontWeight = FontWeights.Bold;
            textBox_Name.FontSize = textBox_Name.Height * fsCoefficient;
            textBox_Name.Text = result.name;
            textBox_Intro.TextWrapping = TextWrapping.WrapWithOverflow;
            textBox_Intro.HorizontalAlignment = HorizontalAlignment.Left;
            textBox_Intro.VerticalAlignment = VerticalAlignment.Top;
            textBox_Intro.HorizontalContentAlignment = HorizontalAlignment.Left;
            textBox_Intro.VerticalContentAlignment = VerticalAlignment.Top;
            textBox_Intro.Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255));
            textBox_Intro.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            textBox_Intro.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            textBox_Intro.Margin = new Thickness(textBox_Name.Margin.Left, textBox_Name.Margin.Top + textBox_Name.Height, 0, 0);
            textBox_Intro.BorderThickness = new Thickness(0);
            textBox_Intro.Width = textBox_Name.Width;
            textBox_Intro.Height = result.image.Height * 0.6;
            textBox_Intro.IsReadOnly = true;
            textBox_Intro.FontWeight = FontWeights.Normal;
            textBox_Intro.FontSize = 14;
            textBox_Intro.Text = result.intro;
            textBox_Status.TextWrapping = TextWrapping.WrapWithOverflow;
            textBox_Status.HorizontalAlignment = HorizontalAlignment.Right;
            textBox_Status.VerticalAlignment = VerticalAlignment.Top;
            textBox_Status.HorizontalContentAlignment = HorizontalAlignment.Right;
            textBox_Status.VerticalContentAlignment = VerticalAlignment.Bottom;
            textBox_Status.Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 200, 0));
            textBox_Status.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            textBox_Status.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            textBox_Status.BorderThickness = new Thickness(0);
            textBox_Status.Height = result.image.Height * 0.15;
            textBox_Status.FontSize = textBox_Status.Height * fsCoefficient;
            textBox_Status.Width = result.status.Length * textBox_Status.FontSize * widthCoefficient;
            textBox_Status.IsReadOnly = true;
            textBox_Status.FontWeight = FontWeights.Normal;
            textBox_Status.Text = result.status;
            textBox_Sort.TextWrapping = TextWrapping.WrapWithOverflow;
            textBox_Sort.HorizontalAlignment = HorizontalAlignment.Left;
            textBox_Sort.VerticalAlignment = VerticalAlignment.Top;
            textBox_Sort.HorizontalContentAlignment = HorizontalAlignment.Left;
            textBox_Sort.VerticalContentAlignment = VerticalAlignment.Bottom;
            textBox_Sort.Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 200, 0));
            textBox_Sort.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            textBox_Sort.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            textBox_Sort.Margin = new Thickness(textBox_Intro.Margin.Left, textBox_Intro.Margin.Top + textBox_Intro.Height, 0, 0);
            textBox_Sort.BorderThickness = new Thickness(0);
            textBox_Sort.Width = width - result.image.Width - textBox_Status.Width;
            textBox_Sort.Height = result.image.Height * 0.15;
            textBox_Sort.IsReadOnly = true;
            textBox_Sort.FontWeight = FontWeights.Normal;
            textBox_Sort.FontSize = textBox_Sort.Height * fsCoefficient;
            textBox_Sort.Text = result.sort;
            textBox_Status.Margin = new Thickness(textBox_Sort.Margin.Left + textBox_Sort.Width, textBox_Intro.Margin.Top + textBox_Intro.Height, 0, 0);
            
            grid.HorizontalAlignment = HorizontalAlignment.Right;
            grid.VerticalAlignment = VerticalAlignment.Top;
            grid.Margin = new Thickness(0, 2, 0, 2);
            grid.Width = width;
            grid.Height = result.image.Height;
            grid.Background = new SolidColorBrush(Color.FromArgb(100, 43, 43, 43));
            grid.Children.Add(result.image);
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
                    Console.WriteLine(manhuarenHtml.Link(List_SearchResult[listBox.SelectedIndex].link));
                    //進行畫面的排版
                    HtmlDocument doc = webClient.Load(manhuarenHtml.Link(List_SearchResult[listBox.SelectedIndex].link));
                    //Image
                    string coverLink = doc.DocumentNode.SelectSingleNode("/html/body/div[3]/div[1]/img").Attributes["src"].Value;
                    //將網址字串轉為Uri格式
                    Uri uri = new Uri(coverLink);
                    //透過網址下載圖片轉為BitmapImage格式
                    BitmapImage bmg = new BitmapImage(uri);
                    brPageSetting.Bimg = bmg;
                    //Console.WriteLine("CoverLink= " + coverLink);
                    //Grid_browse.Children.Add(image);
                    //Label
                    string Name = doc.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/p[1]").InnerText;
                    brPageSetting.Name = Name;
                    //Console.WriteLine("Name= " + Name);
                    //Grid_browse.Children.Add(lbName);
                    //Label
                    HtmlNodeCollection list_author = doc.DocumentNode.SelectNodes("/html/body/div[3]/div[2]/p[3]/a");
                    if (list_author != null) brPageSetting.Author = list_author;
                    //Console.WriteLine("Author.Count= " + list_author.Count);
                    //Grid_browse.Children.Add(lbAuthor);
                    //Label
                    HtmlNodeCollection list_sort = doc.DocumentNode.SelectNodes("/html/body/div[3]/div[2]/p[4]/span");
                    if (list_sort != null) brPageSetting.Sort = list_sort;
                    //Console.WriteLine("Sort.Count= " + list_sort.Count);
                    //Grid_browse.Children.Add(lbSort);
                    //TextBox
                    string Intro = doc.DocumentNode.SelectSingleNode("/html/body/p").InnerText;
                    brPageSetting.Intro = Intro;
                    //Console.WriteLine("Intro= " + Intro);
                    //Grid_browse.Children.Add(tbIntro);
                    //Button
                    string StartRead = doc.DocumentNode.SelectSingleNode("/html/body/div[7]/a[4]").Attributes["href"].Value;
                    brPageSetting.StartRead = StartRead;
                    //Console.WriteLine("StartRead Link= " + StartRead);
                    //Grid_browse.Children.Add(btnStartRead);
                    //List<Button>
                    HtmlNodeCollection list_chapter = doc.DocumentNode.SelectNodes("/html/body/div[5]/ul[1]/li");
                    brPageSetting.BtnChapter = list_chapter;
                    browsePage = new BrowsePage(tb_SearchBar, Grid_browse, brPageSetting);
                    //Console.WriteLine("Chapter Count= " + list_chapter.Count);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

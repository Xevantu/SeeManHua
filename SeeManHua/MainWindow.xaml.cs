using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
        public MainWindow()
        {
            InitializeComponent();
            #region 搜尋結果清單
            List<Grid> List_manhuaGrid = new List<Grid> { };
            string strManHuaName, strManHuaIntro;
            #endregion

            //建立htmlweb
            HtmlWeb webClient = new HtmlWeb();
            //處理C# 連線 HTTPS 網站發生驗證失敗導致基礎連接已關閉
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
            SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            #region 搜尋動作及結果
            //載入網址資料
            HtmlDocument doc = webClient.Load("https://www.manhuaren.com/search?title=%E8%BC%9D%E5%A4%9Ca%E6%97%A5&language=1");
            //lb_html.Content = doc.Text;   //確認是否有抓到html代碼

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
                List_manhuaGrid.Add(LayoutListSetting(-1, img, strManHuaName, strManHuaIntro));
            }
            //將漫畫清單放到卷軸中
            for (int i = 0; i < List_manhuaGrid.Count; i++)
            {
                //List_manhuaGrid[i].Margin = new Thickness(0, 0, 0, 0.1);

            }

            //設定列表排版
            ListBox_manhua.HorizontalAlignment = HorizontalAlignment.Left;
            ListBox_manhua.Margin = new Thickness(0);
            ListBox_manhua.Width = CoverSize.Width + IntroSize.Width;
            //加入抓到的清單
            ListBox_manhua.ItemsSource = List_manhuaGrid;
            #endregion
        }

        public Grid LayoutListSetting(int mode, Image rImg, string name, string introduction)
        {
            Grid grid = new Grid();
            Label label = new Label();
            Image img = rImg;
            switch (mode)
            {
                case 0:
                    break;
                default:
                    label.Content = name + "\n" + introduction;
                    //設置元件位置
                    img.HorizontalAlignment = HorizontalAlignment.Left;
                    img.VerticalAlignment = VerticalAlignment.Center;
                    img.Margin = new Thickness(2);
                    img.Width = CoverSize.Width;
                    img.Height = CoverSize.Height;
                    label.HorizontalAlignment = HorizontalAlignment.Left;
                    label.VerticalAlignment = VerticalAlignment.Center;
                    label.HorizontalContentAlignment = HorizontalAlignment.Left;
                    label.VerticalContentAlignment = VerticalAlignment.Center;
                    label.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(100, 255, 255, 255));
                    label.Margin = new Thickness(img.Margin.Left + img.Width, img.Margin.Top, 0, 0);
                    label.Width = IntroSize.Width;
                    label.Height = IntroSize.Height;
                    grid.HorizontalAlignment = HorizontalAlignment.Right;
                    grid.VerticalAlignment = VerticalAlignment.Top;
                    grid.Margin = new Thickness(-5);
                    grid.Width = img.Width + label.Width;
                    grid.Height = img.Height;
                    grid.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(100, 43, 43, 43));
                    grid.Children.Add(img);
                    grid.Children.Add(label);
                    break;
            }
            return grid;
        }

        private void Main_szChange(object sender, SizeChangedEventArgs e)
        {

            ListBox_manhua.Height = main_bg.Height;
        }
    }
}

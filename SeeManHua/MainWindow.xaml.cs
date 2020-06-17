using HtmlAgilityPack;
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
using System.Reflection;
using System.IO;

namespace SeeManHua
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 內嵌資源
        Assembly assembly;
        Stream imgStream;
        #endregion
        #region 搜尋結果清單
        SearchList searchList;
        SearchListSetting searchListSetting;
        //ManHuaRenHtml manhuarenHtml = new ManHuaRenHtml();
        #endregion

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
            searchList = new SearchList(Grid_menu, Grid_browse, searchListSetting);
            #endregion

            #region 觀賞區
            Grid_browse.HorizontalAlignment = HorizontalAlignment.Left;
            Grid_browse.VerticalAlignment = VerticalAlignment.Top;
            Grid_browse.Margin = new Thickness(Grid_menu.Width, 0, 0, 0);
            Grid_browse.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
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
            Grid_menu.Height = winH;
            //觀賞區布置
            Grid_browse.Width = winW * 0.5;
            Grid_browse.Height = winH;
        }

        /// <summary>
        /// 取得內嵌圖片。
        /// </summary>
        /// <param name="name">要取得的圖片名稱。</param>
        /// <returns></returns>
        public BitmapSource GetSourceForOnRender(string name)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var bitmap = new BitmapImage();

            using (var stream =
                assembly.GetManifestResourceStream("SeeManHua.resources." + name + ".jpg"))
            {
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
            }
            return bitmap;
        }
    }
}

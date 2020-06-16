using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SeeManHua
{
    class BrowsePageSetting
    {
        BitmapImage bimg;
        string name, author, sort, intro, startRead;
        object btnChapter;

        public BitmapImage Bimg { get => bimg; set => bimg = value; }
        public object BtnChapter { get => btnChapter; set => btnChapter = value; }
        public string Name { get => name; set => name = value; }
        public string Author { get => author; set => author = value; }
        public string Sort { get => sort; set => sort = value; }
        public string Intro { get => intro; set => intro = value; }
        public string StartRead { get => startRead; set => startRead = value; }
    }
}

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
        string name, intro, startRead;
        object btnChapter, author, sort;

        public BitmapImage Bimg { get => bimg; set => bimg = value; }
        public object BtnChapter { get => btnChapter; set => btnChapter = value; }
        public string Name { get => name; set => name = value; }
        public object Author { get => author; set => author = value; }
        public object Sort { get => sort; set => sort = value; }
        public string Intro { get => intro; set => intro = value; }
        public string StartRead { get => startRead; set => startRead = value; }
    }
}

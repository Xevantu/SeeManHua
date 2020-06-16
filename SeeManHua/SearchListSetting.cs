using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SeeManHua
{
    class SearchListSetting
    {
        BitmapImage coverImage;
        string name, Intro, sort, status;
        public BitmapImage CoverImage { get => coverImage; set => coverImage = value; }
        public string Name { get => name; set => name = value; }
        public string Intro1 { get => Intro; set => Intro = value; }
        public string Sort { get => sort; set => sort = value; }
        public string Status { get => status; set => status = value; }
    }
}

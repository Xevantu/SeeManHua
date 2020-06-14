using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeManHua
{
    class MyFormat
    {
        public int ResultCoverHeight = 150;
        public int ResultCoverWidth = 100;
        public int ResultNameHeight = 25;
        public int ResultNameWidth = 25;
        public int ResultIntroHeight = 100;
        public int ResultIntroWidth = 300;
        public int SearchBarHeight = 25;
        public int SearchBarWidth = 375;
        public int SearchIconHeight = 25;
        public int SearchIconWidth = 25;
        public MyFormat() { }
        public struct Size_m
        {
            public double Height { get; set; }
            public double Width { get; set; }
            public double Length { get; set; }
            public double SerialNember { get; set; }
        }
    }
}

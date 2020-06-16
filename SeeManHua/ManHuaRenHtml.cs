using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeManHua
{
    class ManHuaRenHtml
    {
        readonly string Title = "https://www.manhuaren.com";
        readonly string SearchHead = "/search?title=";
        readonly string SearchEnd = "&language=1";
        /// <summary>
        /// 網頁抽取
        /// </summary>
        public ManHuaRenHtml() { }
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
}

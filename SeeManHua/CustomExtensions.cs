using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CustomExtensions
{
    /// <summary>
    /// 依照引數的順序，選擇mode排列。
    /// </summary>
    public enum LayoutMode
    {
        /* Image=   Im
             * Label=   La
             * TextBox= Tb
             * Left=    L
             * Right=   R
             * 
            */
        /// <summary>
        /// 1左2右
        /// </summary>
        LR,
        /// <summary>
        /// 1右2左
        /// </summary>
        RL
    }
}

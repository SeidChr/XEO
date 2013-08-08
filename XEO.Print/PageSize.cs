using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XEO.Print
{
    public struct PageSize
    {
        public static PageSize A4 
        {
            get 
            {
                PageSize result;
                result.Width = 210;
                result.Height = 297;
                return result;
            } 
        }

        public double Width { get; private set; }
        public double Height { get; private set; }
    }
}

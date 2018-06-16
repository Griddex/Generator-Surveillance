using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Panel.BusinessLogic.SnipingToolLogic
{
    public static class SnipingToolHelperMethods
    {
        public static Rectangle GetDestopScreens()
        {
            Rectangle result = new Rectangle();
            foreach (Screen screen in Screen.AllScreens)
            {
                result = Rectangle.Union(result, screen.Bounds);
            }
            return result;
        }
    }
}

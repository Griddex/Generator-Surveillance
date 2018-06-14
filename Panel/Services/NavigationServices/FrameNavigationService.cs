using Panel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Panel.Services.NavigationService
{
    public class FrameNavigationService : INavigationService
    {
        public Frame Frame { get; private  set; }
        public FrameNavigationService(Frame frame)
        {
            this.Frame = frame;
        }

        public void Navigate(string pagePageUri)
        {
            this.Frame.Navigate(new Uri(pagePageUri), UriKind.RelativeOrAbsolute);
        }       
    }
}

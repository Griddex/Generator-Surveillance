using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Panel.DependencyProperties
{
    public class UpdateDataGrid
    {
        public static bool GetUpdate(DependencyObject obj)
        {
            return (bool)obj.GetValue(UpdateProperty);
        }
        public static void SetUpdate(DependencyObject obj, bool value)
        {
            obj.SetValue(UpdateProperty, value);
        }
        public static readonly DependencyProperty UpdateProperty =
            DependencyProperty.RegisterAttached("Update", typeof(bool), typeof(UpdateDataGrid), new UIPropertyMetadata(false,
              (o, e) =>
              {
                  if ((bool)e.NewValue)
                      (o as DataGrid).Items.Refresh();
              }));
    }
}

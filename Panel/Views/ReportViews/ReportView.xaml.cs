using Panel.Interfaces;
using Panel.ViewModels.ReportViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Panel.Views.ReportViews
{
    /// <summary>
    /// Interaction logic for ReportView.xaml
    /// </summary>
    public partial class ReportView : Page, IView
    {
        public ReportView(ReportViewModel reportViewModel)
        {
            InitializeComponent();
            this.DataContext = reportViewModel;
        }
    }
}

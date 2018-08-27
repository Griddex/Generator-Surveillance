using Panel.Interfaces;
using Panel.ViewModels.ReportViewModels;
using System.Windows.Controls;

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

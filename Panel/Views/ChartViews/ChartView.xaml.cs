using Panel.Interfaces;
using Panel.ViewModels.ChartViewModels;
using System.Windows.Controls;

namespace Panel.Views.ChartViews
{
    /// <summary>
    /// Interaction logic for ChartView.xaml
    /// </summary>
    public partial class ChartView : Page, IView
    {
        public ChartView(ChartViewModel chartViewModel)
        {
            InitializeComponent();
            this.DataContext = chartViewModel;
        }
    }
}

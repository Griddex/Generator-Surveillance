using Panel.Interfaces;
using Panel.ViewModels.ChartViewModels;
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
using LiveCharts;
using LiveCharts.Wpf;

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

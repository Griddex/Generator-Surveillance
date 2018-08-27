using Panel.Interfaces;
using Panel.ViewModels.HelpViewModels;
using System.Windows.Controls;

namespace Panel.Views.HelpViews
{
    /// <summary>
    /// Interaction logic for HelpView.xaml
    /// </summary>
    public partial class HelpView : Page, IView
    {
        public HelpView(HelpViewModel helpViewModel)
        {
            InitializeComponent();
            this.DataContext = helpViewModel;
        }
    }
}

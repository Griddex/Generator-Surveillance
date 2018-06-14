using Panel.Interfaces;
using Panel.Models.InputModels;
using Panel.Repositories;
using Panel.ViewModels.InputViewModels;
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
using Unity;

namespace Panel.Views.InputViews
{
    /// <summary>
    /// Interaction logic for Usage.xaml
    /// </summary>
    public partial class UsageView : Page, IView
    {
        public UsageView(UsageViewModel usageViewModel)
        {
            InitializeComponent();
            this.DataContext = usageViewModel;
            this.Loaded += new RoutedEventHandler(UsageView_Loaded);
        }

        public void UsageView_Loaded(object sender, RoutedEventArgs args)
        {
            UnityContainer container = (UnityContainer)Application.Current.Resources["UnityIoC"];
            InputView _inputView = (InputView)container.Resolve<IView>("InputView");
            if (_inputView.lblGenIndicator.Background == (SolidColorBrush)new BrushConverter().ConvertFromString("Red"))
            {
                DateTime lastGenTime = Convert.ToDateTime(this.lblGenRecordDate.Content);
                string strlastGenTime = lastGenTime.ToString("hh:mm:ss tt");
                char[] delimeters = new char[] { ':', ' ' };
                string[] timeParts = strlastGenTime.Split(delimeters, StringSplitOptions.RemoveEmptyEntries);
                
                this.cmbxHrGenStd.SelectedValue = timeParts[0];
                this.cmbxMinGenStd.SelectedValue = timeParts[1];
                this.cmbxSecsGenStd.SelectedValue = timeParts[2];
                this.cmbxAMPMGenStd.SelectedValue = timeParts[3];
            }
        }
    }    
}

using Panel.BusinessLogic.AuxilliaryMethods;
using Panel.Interfaces;
using Panel.ViewModels.InputViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
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

        private string[] TimeParts(DateTime dateTime)
        {
            string strdateTime = dateTime.ToString("hh:mm:ss tt");
            char[] delimeters = new char[] { ':', ' ' };
            string[] timeParts = strdateTime.Split(delimeters,
                                    StringSplitOptions.RemoveEmptyEntries);
            return timeParts;
        }

        public void UsageView_Loaded(object sender, RoutedEventArgs args)
        {

            UnityContainer container =
                      (UnityContainer)Application.Current.Resources["UnityIoC"];

            InputView _inputView = (InputView)container.Resolve<IView>("InputView");

            if (ActiveGeneratorInformation
                .GetActiveGeneratorInformation().IsGenActive)
            {

                DateTime lastGenTime = (DateTime)ActiveGeneratorInformation
                                                .GetActiveGeneratorInformation()
                                                .ActiveGenStartedTime;

                string[] LastGenTimeParts = TimeParts(lastGenTime);
                this.cmbxHrGenStd.SelectedValue = LastGenTimeParts[0];
                this.cmbxMinGenStd.SelectedValue = LastGenTimeParts[1];
                this.cmbxSecsGenStd.SelectedValue = LastGenTimeParts[2];
                this.cmbxAMPMGenStd.SelectedValue = LastGenTimeParts[3];


                DateTime currGenTime = DateTime.Now;
                string[] CurrTimeParts = TimeParts(currGenTime);
                this.cmbxHrGenSpd.SelectedValue = CurrTimeParts[0];
                this.cmbxMinGenSpd.SelectedValue = CurrTimeParts[1];
                this.cmbxSecsGenSpd.SelectedValue = CurrTimeParts[2];
                this.cmbxAMPMGenSpd.SelectedValue = CurrTimeParts[3];
            }
        }
    }    
}

using Panel.BusinessLogic.AuxilliaryMethods;
using Panel.Interfaces;
using Panel.ViewModels.InputViewModels;
using Panel.Views.ChartViews;
using Panel.Views.HelpViews;
using Panel.Views.InputViews;
using Panel.Views.ReportViews;
using Panel.Views.SettingsView;
using Panel.Views.TableViews;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Unity;

namespace Panel
{
    /// <summary>
    /// Interaction logic for Input.xaml
    /// </summary>
    public partial class MainView : Window, IView
    {
        UnityContainer container = (UnityContainer)Application
                                    .Current.Resources["UnityIoC"];
        
        private InputView _inputView;
        public (bool, string, DateTime?, DateTime?, int) ActiveGeneratorInfo { get; set; }

        public MainView(InputView inputView)
        {
            InitializeComponent();
            this._inputView = inputView;
            MainViewFrame.Navigate(this._inputView);
        }

        private void InputViewCmd_CanExecute(object sender,  CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void InputViewCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var ActiveGenerator = ActiveGeneratorInformation.GetActiveGeneratorInformation();

            ActiveGeneratorInfo = ActiveGenerator;
            string ActiveGeneratorName = ActiveGenerator.ActiveGenName;

            if (ActiveGeneratorName != null)
            {
                this._inputView.lblGenIndicator
                               .Background = new BrushConverter()
                                                 .ConvertFromString("#FF3939") 
                                                            as SolidColorBrush;

                this._inputView.wppnlLoadActiveGen.Visibility = Visibility.Visible;
                this._inputView.lblGenName.Content = ActiveGeneratorName;
                this._inputView.lblGenState.Content = "ON";
                this._inputView.cmbxGenInfo.Text = ActiveGeneratorName;
            }
            else
            {
                this._inputView.lblGenIndicator
                               .Background = new BrushConverter()
                                                 .ConvertFromString("Black")
                                                            as SolidColorBrush;

                this._inputView.wppnlLoadActiveGen.Visibility = Visibility.Collapsed;
                this._inputView.lblGenName.Content = "";
                this._inputView.lblGenState.Content = "OFF";
                this._inputView.cmbxGenInfo.SelectedItem = null;
            }

            MainViewFrame.Navigate(this._inputView);
        }

        private void TablesViewCmd_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void TablesViewCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            UsageFuellingTablesView usageFuellingTablesView = (UsageFuellingTablesView)container
                .Resolve<IView>("UsageFuellingTablesView");

            MainViewFrame.Navigate(usageFuellingTablesView);
        }

        private void ChartsViewCmd_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void ChartsViewCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ChartView chartView = (ChartView)container.Resolve<IView>("ChartView");
            MainViewFrame.Navigate(chartView);
        }

        private void ReportsViewCmd_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void ReportsViewCmd_Executed(object sender,  ExecutedRoutedEventArgs e)
        {
            ReportView reportView = (ReportView)container.Resolve<IView>("ReportView");
            MainViewFrame.Navigate(reportView);
        }

        private void HelpViewCmd_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void HelpViewCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            HelpView helpView = (HelpView)container.Resolve<IView>("HelpView");
            MainViewFrame.Navigate(helpView);
        }
        
        private void InputToUsageView_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void InputToUsageView_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            UsageView usageView = (UsageView)container.Resolve<IView>("UsageView");

            usageView.lblCurrentGeneratorName.Content = this._inputView.cmbxGenInfo.Text;

            if(this._inputView.lblGenTimeStarted.Content != null)
                usageView
                    .lblGenRecordDate
                    .Content = $"{this._inputView.dtepkrGenInfo.SelectedDate.Value.ToShortDateString()} " +
                               $"{ this._inputView.lblGenTimeStarted.Content.ToString()}";
            else
                usageView
                    .lblGenRecordDate
                    .Content = $"{this._inputView.dtepkrGenInfo.SelectedDate.Value.ToShortDateString()}";

            if ((string)usageView.lblCurrentGeneratorName
                                 .Content == ActiveGeneratorInformation
                                                    .GetActiveGeneratorInformation()
                                                    .ActiveGenName)
            {
                ParseActiveGeneratorTime();
                GeneratorStartedControls(false);
                GeneratorStoppedControls(true);
            }
            else
            {
                ParseActiveGeneratorTime();
                GeneratorStartedControls(true);
                GeneratorStoppedControls(false);
            }
            
            MainViewFrame.Navigate(usageView);
        }

        public void GeneratorStartedControls(bool truefalse)
        {
            UsageView usageView = (UsageView)container
                                    .Resolve<IView>("UsageView");

            usageView.cmbxHrGenStd.IsHitTestVisible = truefalse;
            usageView.cmbxHrGenStd.Focusable = truefalse;

            usageView.cmbxMinGenStd.IsHitTestVisible = truefalse;
            usageView.cmbxMinGenStd.Focusable = truefalse;

            usageView.cmbxSecsGenStd.IsHitTestVisible = truefalse;
            usageView.cmbxSecsGenStd.Focusable = truefalse;

            usageView.cmbxAMPMGenStd.IsHitTestVisible = truefalse;
            usageView.cmbxAMPMGenStd.Focusable = truefalse;

            usageView.btnGenStarted.IsEnabled = truefalse;
        }

        public void GeneratorStoppedControls(bool truefalse)
        {
            UsageView usageView = (UsageView)container
                                    .Resolve<IView>("UsageView");

            usageView.cmbxHrGenSpd.IsHitTestVisible = truefalse;
            usageView.cmbxHrGenSpd.Focusable = truefalse;

            usageView.cmbxMinGenSpd.IsHitTestVisible = truefalse;
            usageView.cmbxMinGenSpd.Focusable = truefalse;

            usageView.cmbxSecsGenSpd.IsHitTestVisible = truefalse;
            usageView.cmbxSecsGenSpd.Focusable = truefalse;

            usageView.cmbxAMPMGenSpd.IsHitTestVisible = truefalse;
            usageView.cmbxAMPMGenSpd.Focusable = truefalse;

            usageView.btnGenStopped.IsEnabled = truefalse;
        }

        private string[] TimeParts(DateTime dateTime)
        {
            string strdateTime = dateTime.ToString("hh:mm:ss tt");
            char[] delimeters = new char[] { ':', ' ' };
            string[] timeParts = strdateTime.Split(delimeters,
                                    StringSplitOptions.RemoveEmptyEntries);
            return timeParts;
        }

        public void ParseActiveGeneratorTime()
        {
            UsageView usageView = (UsageView)container
                                  .Resolve<IView>("UsageView");

            try
            {
                DateTime lastGenTime = (DateTime)ActiveGeneratorInformation
                                        .GetActiveGeneratorInformation()
                                        .ActiveGenStartedTime;

                usageView.lblGenStartedDate.Content = $"{lastGenTime.ToShortDateString()}  {lastGenTime.ToShortTimeString()}";

                string[] LastGenTimeParts = TimeParts(lastGenTime);
                usageView.cmbxHrGenStd.SelectedValue = LastGenTimeParts[0];
                usageView.cmbxMinGenStd.SelectedValue = LastGenTimeParts[1];
                usageView.cmbxSecsGenStd.SelectedValue = LastGenTimeParts[2];
                usageView.cmbxAMPMGenStd.SelectedValue = LastGenTimeParts[3];

                DateTime currGenTime = DateTime.Now;
                string[] CurrTimeParts = TimeParts(currGenTime);
                usageView.cmbxHrGenSpd.SelectedValue = CurrTimeParts[0];
                usageView.cmbxMinGenSpd.SelectedValue = CurrTimeParts[1];
                usageView.cmbxSecsGenSpd.SelectedValue = CurrTimeParts[2];
                usageView.cmbxAMPMGenSpd.SelectedValue = CurrTimeParts[3];
            }
            catch (InvalidOperationException) { }            
        }

        private void UsageToFuellingView_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void UsageToFuellingView_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            FuellingView fuellingView = (FuellingView)container
                                        .Resolve<IView>("FuellingView");

            MainViewFrame.Navigate(fuellingView);
            (fuellingView.DataContext as FuellingViewModel).RefreshFuelCompCmd.Execute(null);
            //fuellingView.cmbxSelectGenFuelling.ItemsSource = this._inputView.cmbxGenInfo.Items;
        }

        private void FuellingToMaintenanceView_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void FuellingToMaintenanceView_Executed(object sender, 
            ExecutedRoutedEventArgs e)
        {
            MaintenanceView maintenanceView = (MaintenanceView)container
                                              .Resolve<IView>("MaintenanceView");

            MainViewFrame.Navigate(maintenanceView);
            (maintenanceView.DataContext as MaintenanceViewModel).RefreshSchMaintenanceCmd.Execute(null);
        }

        private void UsageMaintToRunningHrsSchedulerView_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void UsageMaintToRunningHrsSchedulerView_Executed(object sender, 
            ExecutedRoutedEventArgs e)
        {
            RunningHrsSchedulingTablesView runningHrsSchedulingTablesView = 
                (RunningHrsSchedulingTablesView)container
                .Resolve<IView>("RunningHrsSchedulingTablesView");

            MainViewFrame.Navigate(runningHrsSchedulingTablesView);
        }
        
        private void RunningHrsSchedulerToUsageMaintView_CanExecute(object sender, 
            CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void RunningHrsSchedulerToUsageMaintView_Executed(object sender, 
            ExecutedRoutedEventArgs e)
        {
            UsageFuellingTablesView usageFuellingTablesView = 
                (UsageFuellingTablesView)container.Resolve<IView>("UsageFuellingTablesView");

            MainViewFrame.Navigate(usageFuellingTablesView);
        }

        private void SettingsView_CanExecute(object sender, 
            CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void SettingsView_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SettingsView settingsView = (SettingsView)container
                                        .Resolve<IView>("SettingsView");

            MainViewFrame.Navigate(settingsView);
        }

        private void Exit_CanExecute(object sender, 
            CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void Exit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBoxResult result =   MessageBox.Show("Do you want to " +
                                        "exit the application?", 
                                        "Confirmation", 
                                        MessageBoxButton.YesNoCancel, 
                                        MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        Application.Current.Shutdown();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"There was an error exiting the " +
                            $"application\n\n{ex.Message}");
                        return;
                    }                    
                    break;
                case MessageBoxResult.No:                    
                case MessageBoxResult.Cancel:                    
                    return;
            }            
        }

        private void CommandBinding_CanExecute(object sender,
            CanExecuteRoutedEventArgs e) => e.CanExecute = true;
    }
}

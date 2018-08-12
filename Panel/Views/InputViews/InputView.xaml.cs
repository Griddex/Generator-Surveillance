using Panel.BusinessLogic.AuxilliaryMethods;
using Panel.Interfaces;
using Panel.Repositories;
using Panel.ViewModels.InputViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Unity;

namespace Panel.Views.InputViews
{
    /// <summary>
    /// Interaction logic for InputView.xaml
    /// </summary>
    public partial class InputView : Page, IView
    {
        private UnitOfWork _unitOfWork;

        public bool IsGenActive { get; set; }

        public InputView(UnitOfWork unitOfWork, InputViewModel inputViewModel)
        {
            InitializeComponent();
                        
            this._unitOfWork = unitOfWork;
            this.DataContext = inputViewModel;
            this.cmbxGenInfo.SelectedIndex = 0;
            this.Loaded += InputView_Loaded;            
        }

        private void InputView_Loaded(object sender, RoutedEventArgs e)
        {
            IsGenActive = ActiveGeneratorInformation
                            .GetActiveGeneratorInformation()
                            .IsGenActive;

            var inputViewModel = this.DataContext as InputViewModel;

            if (IsGenActive)
            {
                inputViewModel.LoadActiveGeneratorRecord
                    .Execute(new Tuple<DatePicker, ComboBox>
                    (this.dtepkrGenInfo,
                    this.cmbxGenInfo));
            }
            else
            {
                this.wppnlLoadActiveGen.Visibility = Visibility.Collapsed;
                this.lblGenName.Content = "";
                this.lblGenState.Content = "OFF";
                this.cmbxGenInfo.SelectedItem = null;
            }
        }

        private string[] TimeParts(DateTime dateTime)
        {
            string strdateTime = dateTime.ToString("hh:mm:ss tt");
            char[] delimeters = new char[] { ':', ' ' };
            string[] timeParts = strdateTime.Split(delimeters,
                                    StringSplitOptions.RemoveEmptyEntries);
            return timeParts;
        }

        private void lblGenIndicator_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsGenActive) return;

            MessageBoxResult result = MessageBox.Show($"Do you want to " +
                                                      $"stop active generator?",
                                                      "Confirmation",
                                                      MessageBoxButton.YesNoCancel,
                                                      MessageBoxImage.Question);

            switch (result)
            {
                case MessageBoxResult.None:
                case MessageBoxResult.Cancel:
                case MessageBoxResult.No:
                    break;
                case MessageBoxResult.OK:
                case MessageBoxResult.Yes:
                    UnityContainer container = (UnityContainer)Application
                                               .Current.Resources["UnityIoC"];

                    UsageView _usageView = (UsageView)container.Resolve<IView>("UsageView");

                    _usageView.lblCurrentGeneratorName.Content = this.cmbxGenInfo.Text;

                    if (this.lblGenTimeStarted.Content != null)
                        _usageView.lblGenRecordDate
                            .Content = $"{this.dtepkrGenInfo.SelectedDate.Value.ToShortDateString()} " +
                                                $"{ this.lblGenTimeStarted.Content.ToString()}";
                    else
                        _usageView.lblGenRecordDate
                            .Content = $"{this.dtepkrGenInfo.SelectedDate.Value.ToShortDateString()}";

                    DateTime lastGenTime = (DateTime)ActiveGeneratorInformation
                                                    .GetActiveGeneratorInformation()
                                                    .ActiveGenStartedTime;

                    string[] LastGenTimeParts = TimeParts(lastGenTime);
                    _usageView.cmbxHrGenStd.SelectedValue = LastGenTimeParts[0];
                    _usageView.cmbxMinGenStd.SelectedValue = LastGenTimeParts[1];
                    _usageView.cmbxSecsGenStd.SelectedValue = LastGenTimeParts[2];
                    _usageView.cmbxAMPMGenStd.SelectedValue = LastGenTimeParts[3];


                    DateTime currGenTime = DateTime.Now;
                    string[] CurrTimeParts = TimeParts(currGenTime);
                    _usageView.cmbxHrGenSpd.SelectedValue = CurrTimeParts[0];
                    _usageView.cmbxMinGenSpd.SelectedValue = CurrTimeParts[1];
                    _usageView.cmbxSecsGenSpd.SelectedValue = CurrTimeParts[2];
                    _usageView.cmbxAMPMGenSpd.SelectedValue = CurrTimeParts[3];

                    IView mainView = container.Resolve<IView>("MainView");
                    (mainView as MainView).GeneratorStartedControls(false);
                    (mainView as MainView).GeneratorStoppedControls(true);
                    (mainView as MainView).MainViewFrame.Navigate(_usageView);

                    break;
                default:
                    break;
            }            
        }
    }
}

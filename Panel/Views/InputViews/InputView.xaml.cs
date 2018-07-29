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
            var inputViewModel = this.DataContext as InputViewModel;
            if (inputViewModel.IsNull)
            {
                inputViewModel.LoadActiveGeneratorRecord
                    .Execute(new Tuple<DatePicker, ComboBox>
                    (this.dtepkrGenInfo,
                    this.cmbxGenInfo));
            }
                
        }

        private void lblGenIndicator_MouseDown(object sender, MouseButtonEventArgs e)
        {
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

                    DateTime lastGenTime = Convert.ToDateTime(_usageView
                                                            .lblGenRecordDate
                                                            .Content);

                    string strlastGenTime = lastGenTime.ToString("hh:mm:ss tt");
                    char[] delimeters = new char[] { ':', ' ' };
                    string[] timeParts = strlastGenTime.Split(delimeters,
                                            StringSplitOptions.RemoveEmptyEntries);

                    _usageView.cmbxHrGenStd.SelectedValue = timeParts[0];
                    _usageView.cmbxMinGenStd.SelectedValue = timeParts[1];
                    _usageView.cmbxSecsGenStd.SelectedValue = timeParts[2];
                    _usageView.cmbxAMPMGenStd.SelectedValue = timeParts[3];


                    DateTime currGenTime = DateTime.Now;
                    string strCurrGenTime = currGenTime.ToString("hh:mm:ss tt");
                    char[] delimeters1 = new char[] { ':', ' ' };
                    string[] timeParts1 = strCurrGenTime.Split(delimeters1,
                        StringSplitOptions.RemoveEmptyEntries);

                    _usageView.cmbxHrGenSpd.SelectedValue = timeParts1[0];
                    _usageView.cmbxMinGenSpd.SelectedValue = timeParts1[1];
                    _usageView.cmbxSecsGenSpd.SelectedValue = timeParts1[2];
                    _usageView.cmbxAMPMGenSpd.SelectedValue = timeParts1[3];

                    _usageView.cmbxHrGenStd.IsHitTestVisible = false;
                    _usageView.cmbxHrGenStd.Focusable = false;

                    _usageView.cmbxMinGenStd.IsHitTestVisible = false;
                    _usageView.cmbxMinGenStd.Focusable = false;

                    _usageView.cmbxSecsGenStd.IsHitTestVisible = false;
                    _usageView.cmbxSecsGenStd.Focusable = false;

                    _usageView.cmbxAMPMGenStd.IsHitTestVisible = false;
                    _usageView.cmbxAMPMGenStd.Focusable = false;

                    _usageView.btnGenStarted.IsEnabled = false;

                    IView mainView = container.Resolve<IView>("MainView");
                    (mainView as MainView).MainViewFrame.Navigate(_usageView);
                    break;
                default:
                    break;
            }
            
        }
    }
}

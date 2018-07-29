using Panel.Commands;
using Panel.Interfaces;
using Panel.Models.InputModels;
using Panel.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Panel.ViewModels.InputViewModels
{
    public class UsageViewModel : ViewModelBase, IViewModel
    {
        public UnitOfWork UnitOfWork { get; private set; }
        public ObservableCollection<GeneratorNameModel> GeneratorNames { get; set; } = 
            new ObservableCollection<GeneratorNameModel>();
        
        public List<int> HoursViewModelList { get; set; } = new List<int>();
        public List<int> MinutesViewModelList { get; set; } = new List<int>();
        public List<int> SecondsViewModelList { get; set; } = new List<int>();
        public List<string> AMPMViewModelList { get; set; } = new List<string>();
        
        public string SelectedGeneratorName { get; set; }
        public DateTime SelectedRecordDate { get; set; }
        public DateTime GeneratorStoppedAnotherDay { get; set; } = DateTime.Now;
        public int SelectedStartHour { get; set; }
        public int SelectedStartMinute { get; set; }
        public int SelectedStartSecond { get; set; }
        public string SelectedStartAMPM { get; set; }
        public int SelectedStopHour { get; set; }
        public int SelectedStopMinute { get; set; }
        public int SelectedStopSecond { get; set; }
        public string SelectedStopAMPM { get; set; }

        public int ActiveGenID { get; set; }

        public UsageViewModel(UnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            InitialiseUsageViewModel();            
        }

        private void InitialiseUsageViewModel()
        {
            Label lblCurrentGenerator = new Label
            {
                Content = ""
            };
            for (int i = 1; i < 13; i++) { HoursViewModelList.Add(i);}
            for (int i = 0; i < 61; i++) { MinutesViewModelList.Add(i);}
            for (int i = 0; i < 61; i++) { SecondsViewModelList.Add(i);}
            AMPMViewModelList.Add( "AM" );
            AMPMViewModelList.Add( "PM" );            
        }
        
        public DateTime GeneratorStartedModel { get; set; }
        public DateTime GeneratorStoppedModel { get; set; }
        private DateTime _parsedStartTime;
        private DateTime _parsedStopTime;

        private ICommand _addGeneratorStartedCmd;
        public ICommand AddGeneratorStartedCmd
        {
            get
            {
                return this._addGeneratorStartedCmd ??
                (
                    this._addGeneratorStartedCmd = new DelegateCommand
                    (
                        x =>
                        {
                            if(SelectedGeneratorName == null || 
                            SelectedGeneratorName == "")
                            {
                                MessageBox.Show($"Please select a " +
                                    $"generator in General Generator Information", 
                                    "Error",
                                    MessageBoxButton.OK, 
                                    MessageBoxImage.Error);
                                return;
                            }

                            Tuple<ComboBox, ComboBox, ComboBox, ComboBox, Button> cmbx4Btn =
                                        (Tuple<ComboBox, ComboBox, ComboBox, ComboBox, Button>)x;

                            cmbx4Btn.Item1.IsHitTestVisible = true;
                            cmbx4Btn.Item1.Focusable = true;

                            cmbx4Btn.Item2.IsHitTestVisible = true;
                            cmbx4Btn.Item2.Focusable = true;

                            cmbx4Btn.Item3.IsHitTestVisible = true;
                            cmbx4Btn.Item3.Focusable = true;

                            cmbx4Btn.Item4.IsHitTestVisible = true;
                            cmbx4Btn.Item4.Focusable = true;

                            cmbx4Btn.Item5.IsEnabled = true;

                            string mergedStartTime = $"{SelectedStartHour.ToString("D2")}:" +
                                                    $"{SelectedStartMinute.ToString("D2")}:" +
                                                    $"{SelectedStartSecond.ToString("D2")} " +
                                                    $"{SelectedStartAMPM}";

                            if(DateTime.TryParseExact(mergedStartTime, "hh:mm:ss tt", 
                                CultureInfo.InvariantCulture, 
                                DateTimeStyles.None, out _parsedStartTime))
                                GeneratorStartedModel = _parsedStartTime;

                            DateTime GeneratorStartedModelTime = SelectedRecordDate.Date + 
                                                                 GeneratorStartedModel.TimeOfDay;

                            UnitOfWork.GeneratorUsage.GeneratorStarted(SelectedRecordDate, 
                                SelectedGeneratorName, GeneratorStartedModelTime);
                            int success = UnitOfWork.Complete();
                            if (success > 0)
                                MessageBox.Show($"Generator started on " +
                                    $"{GeneratorStartedModelTime.ToString("dddd, dd/MMM/yyyy hh:mm:ss tt")}", 
                                    "Success",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                            else
                            {
                                MessageBox.Show($"{GeneratorStartedModelTime.ToLongTimeString()} " +
                                    $"was not saved", 
                                    "Error",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        },
                        y => true
                    )
                );
            }
        }


        private ICommand _addGeneratorStoppedCmd;
        public ICommand AddGeneratorStoppedCmd
        {
            get
            {
                return this._addGeneratorStoppedCmd ??
                (
                    this._addGeneratorStoppedCmd = new DelegateCommand
                    (
                        x =>
                        {
                            if (SelectedGeneratorName == null || 
                            SelectedGeneratorName == "")
                            {
                                MessageBox.Show($"Please select a generator " +
                                    $"in General Generator Information",
                                    "Error",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            Tuple<ComboBox, ComboBox, ComboBox, ComboBox, Button> cmbx4Btn = 
                                        (Tuple<ComboBox, ComboBox, ComboBox, ComboBox, Button>)x;

                            cmbx4Btn.Item1.IsHitTestVisible = true;
                            cmbx4Btn.Item1.Focusable = true;

                            cmbx4Btn.Item2.IsHitTestVisible = true;
                            cmbx4Btn.Item2.Focusable = true;

                            cmbx4Btn.Item3.IsHitTestVisible = true;
                            cmbx4Btn.Item3.Focusable = true;

                            cmbx4Btn.Item4.IsHitTestVisible = true;
                            cmbx4Btn.Item4.Focusable = true;

                            cmbx4Btn.Item5.IsEnabled = true;

                            string mergedStopTime = $"{SelectedStopHour.ToString("D2")}:{SelectedStopMinute.ToString("D2")}:" +
                                                    $"{SelectedStopSecond.ToString("D2")} {SelectedStopAMPM}";
                            if (DateTime.TryParseExact(mergedStopTime, "HH:mm:ss tt", CultureInfo.InvariantCulture, 
                                DateTimeStyles.None, out _parsedStopTime))
                                GeneratorStoppedModel = _parsedStopTime;
                            DateTime GeneratorStoppedModelTime = GeneratorStoppedAnotherDay + GeneratorStoppedModel.TimeOfDay;

                            if((GeneratorStoppedModelTime - DateTime.MinValue).TotalSeconds <=
                            (SelectedRecordDate - DateTime.MinValue).TotalSeconds)
                            {
                                MessageBox.Show($"{GeneratorStoppedModel.ToLongTimeString()} was not stopped\n\n" +
                                    $"Generator must be stopped at a later date", "Error",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            var (IsNull, ActiveGenName, ActiveGenStartedDate, ActiveGenStartedTime, ActiveGenID) =
                                        UnitOfWork.GeneratorInformation.GeneratorStoppedIsNull();
                            UnitOfWork.GeneratorUsage.GeneratorStopped(GeneratorStoppedModelTime,
                                ActiveGenID);
                            int success = UnitOfWork.Complete();
                            if (success > 0)
                                MessageBox.Show($"Generator stopped on " +
                                    $"{GeneratorStoppedModelTime.ToString("dddd, dd/MMM/yyyy hh:mm:ss tt")}", 
                                    "Success",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                            else
                            {
                                MessageBox.Show($"{GeneratorStoppedModel.ToLongTimeString()} was not saved", 
                                    "Error",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        },
                        y => true
                    )
                );
            }
        }
    }
}
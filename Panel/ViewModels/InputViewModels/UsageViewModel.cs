using Panel.Commands;
using Panel.Models.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Panel.Interfaces;
using System.Windows.Navigation;
using Panel.Repositories;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Unity;
using System.Globalization;

namespace Panel.ViewModels.InputViewModels
{
    public class UsageViewModel : ViewModelBase, IViewModel
    {
        public UnitOfWork UnitOfWork { get; private set; }
        public ObservableCollection<GeneratorNameModel> GeneratorNames { get; set; } = new ObservableCollection<GeneratorNameModel>();
        
        public List<int> HoursViewModelList { get; set; } = new List<int>();
        public List<int> MinutesViewModelList { get; set; } = new List<int>();
        public List<int> SecondsViewModelList { get; set; } = new List<int>();
        public List<string> AMPMViewModelList { get; set; } = new List<string>();
        
        public string SelectedGeneratorName { get; set; }
        public DateTime SelectedRecordDate { get; set; }
        public int SelectedStartHour { get; set; }
        public int SelectedStartMinute { get; set; }
        public int SelectedStartSecond { get; set; }
        public string SelectedStartAMPM { get; set; }
        public int SelectedStopHour { get; set; }
        public int SelectedStopMinute { get; set; }
        public int SelectedStopSecond { get; set; }
        public string SelectedStopAMPM { get; set; }

        public UsageViewModel(UnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            InitialiseUsageViewModel();

            //var (IsNull, LastGenName, LastGenStartedDate, LastGenStartedTime) = UnitOfWork.GeneratorInformation.GeneratorStoppedIsNull();
            //this._lastGenStartedTime = LastGenStartedTime;
            //if (IsNull)
            //    LoadLastGenTime();
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
                            string mergedStartTime = $"{SelectedStartHour.ToString("D2")}:{SelectedStartMinute.ToString("D2")}:{SelectedStartSecond.ToString("D2")} {SelectedStartAMPM}";
                            if(DateTime.TryParseExact(mergedStartTime, "hh:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out _parsedStartTime))
                                GeneratorStartedModel = _parsedStartTime;
                            DateTime GeneratorStartedModelTime = DateTime.MinValue + GeneratorStartedModel.TimeOfDay;

                            UnitOfWork.GeneratorUsage.GeneratorStarted(SelectedRecordDate, SelectedGeneratorName, GeneratorStartedModelTime);
                            int success = UnitOfWork.Complete();
                            if (success > 0)
                                MessageBox.Show($"Generator started on {GeneratorStartedModelTime.ToLongTimeString()}", "Success",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                            else
                            {
                                MessageBox.Show($"{GeneratorStartedModelTime.ToLongTimeString()} was not saved", "Error",
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
                            string mergedStopTime = $"{SelectedStopHour.ToString("D2")}:{SelectedStopMinute.ToString("D2")}:{SelectedStopSecond.ToString("D2")} {SelectedStopAMPM}";
                            if (DateTime.TryParseExact(mergedStopTime, "HH:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out _parsedStopTime))
                                GeneratorStoppedModel = _parsedStopTime;
                            DateTime GeneratorStoppedModelTime = DateTime.MinValue + GeneratorStoppedModel.TimeOfDay;

                            UnitOfWork.GeneratorUsage.GeneratorStopped(GeneratorStoppedModelTime);
                            int success = UnitOfWork.Complete();
                            if (success > 0)
                                MessageBox.Show($"Generator stopped on {GeneratorStoppedModel.ToLongTimeString()}", "Success",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                            else
                            {
                                MessageBox.Show($"{GeneratorStoppedModel.ToLongTimeString()} was not saved", "Error",
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
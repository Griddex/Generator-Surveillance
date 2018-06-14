﻿using Panel.Commands;
using Panel.Interfaces;
using Panel.Models.ChartModels;
using Panel.Models.InputModels;
using Panel.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using System.Windows.Controls;
using Panel.BusinessLogic.AuxilliaryMethods;
using Panel.BusinessLogic;
using Panel.BusinessLogic.ChartsLogic.GeneratorChartLogic;
using System.Windows.Media;

namespace Panel.ViewModels.ChartViewModels
{
    public class ChartViewModel : ViewModelBase, IViewModel
    {
        public static int PlotButtonPressedCount = 0;

        public ChartViewModel(UnitOfWork unitOfWork, ExtrudeInterveningDates extrudeInterveningDates)
        {
            UnitOfWork = unitOfWork;
            ExtrudeInterveningDates = extrudeInterveningDates;
            UniqueGeneratorNames = unitOfWork.GeneratorInformation.GetUniqueGeneratorNames();
            InitialiseChartViewModel();
        }

        public UnitOfWork UnitOfWork { get; private set; }
        public ExtrudeInterveningDates ExtrudeInterveningDates { get; set; }
        public ObservableCollection<GeneratorNameModel> UniqueGeneratorNames { get; set; } = new ObservableCollection<GeneratorNameModel>();

        private ObservableCollection<string> _generatorData = new ObservableCollection<string>();
        public ObservableCollection<string> GeneratorData
        {
            get
            {
                _generatorData.Add("Generator Usage Data");
                _generatorData.Add("Generator " +
                    "Maintenance Data");
                return _generatorData;
            }
        }

        private ObservableCollection<string> _chartTypes = new ObservableCollection<string>();
        public ObservableCollection<string> ChartTypes
        {
            get
            {
                _chartTypes.Add("Column");
                _chartTypes.Add("Stacked Column");
                _chartTypes.Add("Stacked Area");
                _chartTypes.Add("Pie");
                _chartTypes.Add("Line");                                
                return _chartTypes;
            }
        }

        public List<int> HoursViewModelList { get; set; } = new List<int>();
        public List<int> MinutesViewModelList { get; set; } = new List<int>();
        public List<int> SecondsViewModelList { get; set; } = new List<int>();
        public List<string> AMPMViewModelList { get; set; } = new List<string>();

        private string _selectedGeneratorName;
        public string SelectedGeneratorName
        {
            get => _selectedGeneratorName;
            set
            {
                _selectedGeneratorName = value;

                SelectedStartDate = UnitOfWork.GeneratorUsage.GetStartedDate(_selectedGeneratorName);
                OnPropertyChanged(nameof(SelectedStartDate));

                SelectedStopDate = UnitOfWork.GeneratorUsage.GetStoppedDate(_selectedGeneratorName);
                OnPropertyChanged(nameof(SelectedStopDate));

                SelectedStartHour = int.Parse(ParseTimeFromDateTime.GetTimePart(UnitOfWork.GeneratorUsage.GetStartedTime(_selectedGeneratorName),"Hours"));
                OnPropertyChanged(nameof(SelectedStartHour));

                SelectedStartMinute = int.Parse(ParseTimeFromDateTime.GetTimePart(UnitOfWork.GeneratorUsage.GetStartedTime(_selectedGeneratorName),"Minutes"));
                OnPropertyChanged(nameof(SelectedStartMinute));

                SelectedStartSecond = int.Parse(ParseTimeFromDateTime.GetTimePart(UnitOfWork.GeneratorUsage.GetStartedTime(_selectedGeneratorName), "Seconds"));
                OnPropertyChanged(nameof(SelectedStartSecond));

                SelectedStartAMPM = ParseTimeFromDateTime.GetTimePart(UnitOfWork.GeneratorUsage.GetStartedTime(_selectedGeneratorName), "AMPM");
                OnPropertyChanged(nameof(SelectedStartAMPM));

                SelectedStopHour = int.Parse(ParseTimeFromDateTime.GetTimePart(UnitOfWork.GeneratorUsage.GetStoppedTime(_selectedGeneratorName),"Hours"));
                OnPropertyChanged(nameof(SelectedStopHour));

                SelectedStopMinute = int.Parse(ParseTimeFromDateTime.GetTimePart(UnitOfWork.GeneratorUsage.GetStoppedTime(_selectedGeneratorName),"Minutes"));
                OnPropertyChanged(nameof(SelectedStopMinute));

                SelectedStopSecond = int.Parse(ParseTimeFromDateTime.GetTimePart(UnitOfWork.GeneratorUsage.GetStoppedTime(_selectedGeneratorName), "Seconds"));
                OnPropertyChanged(nameof(SelectedStopSecond));

                SelectedStopAMPM = ParseTimeFromDateTime.GetTimePart(UnitOfWork.GeneratorUsage.GetStoppedTime(_selectedGeneratorName), "AMPM");
                OnPropertyChanged(nameof(SelectedStopAMPM));
            }
        }

        public string SelectedGeneratorData { get; set; }
        public string SelectedChartType { get; set; }

        public bool? SelectedRadioButtonYear { get; set; }
        public bool SelectedCheckButtonSelectAll { get; set; }
        public List<string> lstBxSelectedItems { get; set; }

        public DateTime SelectedStartDate { get; set; }        
        public DateTime SelectedStopDate { get; set; }
        public int SelectedStartHour { get; set; }
        public int SelectedStartMinute { get; set; }
        public int SelectedStartSecond { get; set; }
        public string SelectedStartAMPM { get; set; }
        public int SelectedStopHour { get; set; }
        public int SelectedStopMinute { get; set; }
        public int SelectedStopSecond { get; set; }
        public string SelectedStopAMPM { get; set; }

        public string SelectedDurationPerspective { get; set; }

        private void InitialiseChartViewModel()
        {
            for (int i = 1; i < 13; i++) { HoursViewModelList.Add(i); }
            for (int i = 0; i < 61; i++) { MinutesViewModelList.Add(i); }
            for (int i = 0; i < 61; i++) { SecondsViewModelList.Add(i); }
            AMPMViewModelList.Add("AM");
            AMPMViewModelList.Add("PM");
        }

        public string MergedStartTime
        {
            set { }
            get
            {
                DateTime GeneratorStartedModelTime;
                DateTime _parsedStartTime;
                string mergedStartTime = $"{SelectedStartHour.ToString("D2")}:{SelectedStartMinute.ToString("D2")}:{SelectedStartSecond.ToString("D2")} {SelectedStartAMPM}";
                
                if (DateTime.TryParseExact(mergedStartTime, "hh:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out _parsedStartTime))
                {
                    GeneratorStartedModelTime = DateTime.MinValue + _parsedStartTime.TimeOfDay;
                    return GeneratorStartedModelTime.ToString();                     
                }
                return "";                
            }            
        }
                
        public string MergedStopTime
        {
            set { }
            get
            {
                DateTime GeneratorStoppedModelTime;
                DateTime _parsedStopTime;
                string mergedStopTime = $"{SelectedStopHour.ToString("D2")}:{SelectedStopMinute.ToString("D2")}:{SelectedStopSecond.ToString("D2")} {SelectedStopAMPM}";
                if (DateTime.TryParseExact(mergedStopTime, "hh:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out _parsedStopTime))
                {
                    GeneratorStoppedModelTime = DateTime.MinValue + _parsedStopTime.TimeOfDay;
                    return GeneratorStoppedModelTime.ToString();
                }
                return "";
            }
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private ICommand _populateLstBoxCmd;
        public ICommand PopulateLstBoxCmd
        {
            get
            {
                return this._populateLstBoxCmd ??
                (
                    this._populateLstBoxCmd = new DelegateCommand
                    (
                        x =>
                        {
                            if (x != null)
                            {
                                StackPanel StkplDurationPerspective = (StackPanel)x;
                                RadioButton rdbtnDurationPerspective = StkplDurationPerspective
                                                                       .Children.OfType<RadioButton>()
                                                                       .FirstOrDefault(r => r.IsChecked == true);
                                if(rdbtnDurationPerspective == null)
                                {
                                    MessageBox.Show("Please select a duration perspective", "Information",
                                        MessageBoxButton.OK, MessageBoxImage.Information);
                                    return;
                                }

                                IEnumerable<TextBlock> xtxblkDurationPerspective = FindVisualChildren<TextBlock>(rdbtnDurationPerspective);
                                foreach (var control in xtxblkDurationPerspective)
                                {
                                    SelectedDurationPerspective = control.Name.Split(new string[] { "txtBlk" }, StringSplitOptions.RemoveEmptyEntries)[0];
                                }                                              
                            }
                        },
                        y => true
                    )
                );
            }
        }

        private ICommand _selectYearPerspectiveCmd;
        public ICommand SelectYearPerspectiveCmd
        {
            get
            {
                return this._selectYearPerspectiveCmd ??
                (
                    this._selectYearPerspectiveCmd = new DelegateCommand
                    (
                        x =>
                        {
                            if (x != null)
                            {
                                StackPanel StkplDurationPerspective = (StackPanel)x;
                                RadioButton rdbtnYear = StkplDurationPerspective.Children.OfType<RadioButton>()
                                                                .FirstOrDefault(r => r.Name == "rdbtnYear");
                                rdbtnYear.IsChecked = true;
                            }
                        },
                        y => true
                    )
                );
            }
        }

        private ICommand _durationPerspectiveCmd;
        public ICommand DurationPerspectiveCmd
        {
            get
            {
                return this._durationPerspectiveCmd ??
                (
                    this._durationPerspectiveCmd = new DelegateCommand
                    (
                        x =>
                        {
                            DateTime StartTime = Convert.ToDateTime(MergedStartTime);
                            DateTime StopTime = Convert.ToDateTime(MergedStopTime);
                            if (SelectedStartDate == SelectedStopDate)
                            {
                                if (TimeSpan.Compare(StartTime.TimeOfDay, StopTime.TimeOfDay) > 0)
                                {
                                    MessageBox.Show($"Stop date must be greater than start date", "Error",
                                         MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                            string sysDateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                            DateTime StartDateTime = Convert.ToDateTime($"{SelectedStartDate.ToString(sysDateFormat)} {StartTime.TimeOfDay.ToString()}");
                            DateTime StopDateTime = Convert.ToDateTime($"{SelectedStopDate.ToString(sysDateFormat)} {StopTime.TimeOfDay.ToString()}");

                            Tuple<TextBlock, ListBox, StackPanel> txtBlkLstBxStkPnlTuple = (Tuple<TextBlock, ListBox, StackPanel>)x;
                            string DateDurationPerspective = txtBlkLstBxStkPnlTuple.Item1.Text;
                            SelectedDurationPerspective = DateDurationPerspective;
                            IEnumerable<string> DatePerspectiveList = ExtrudeInterveningDates.GetInterveningDates(StartDateTime, 
                                                                        StopDateTime, SelectedGeneratorName, DateDurationPerspective);
                            try
                            {
                                txtBlkLstBxStkPnlTuple.Item2.Items.Clear();
                            }
                            catch (Exception) { }                            
                            
                            txtBlkLstBxStkPnlTuple.Item2.ItemsSource = DatePerspectiveList;
                            PlotButtonPressedCount++;
                        },
                        y => true
                    )
                );
            }
        }

        private ICommand _checkedStatusCmd;
        public ICommand CheckedStatusCmd
        {
            get
            {
                return this._checkedStatusCmd ??
                (
                    this._checkedStatusCmd = new DelegateCommand
                    (
                        x =>
                        {
                            if(x != null)
                            {
                                Tuple<bool, ListBox> boolLstBxTuple = (Tuple<bool, ListBox>)x;
                                if (boolLstBxTuple.Item1)
                                    boolLstBxTuple.Item2.SelectAll();
                                else
                                    boolLstBxTuple.Item2.UnselectAll();
                            }                            
                        },
                        y => true
                    )
                );
            }
        }

        private ICommand _getSelectedItemsCmd;
        public ICommand GetSelectedItemsCmd
        {
            get
            {
                return this._getSelectedItemsCmd ??
                (
                    this._getSelectedItemsCmd = new DelegateCommand
                    (
                        x =>
                        {
                            if (x != null)
                            {
                                ListBox LstBx = (ListBox)x;
                                lstBxSelectedItems = LstBx.SelectedItems.Cast<string>().ToList();
                            }
                        },
                        y => true
                    )
                );
            }
        }

        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> XFormatter { get; set; }
        public Func<double,string> YFormatter { get; set; }

        private ICommand _plotChartCmd;
        public ICommand PlotChartCmd
        {
            get
            {
                return this._plotChartCmd ??
                (
                    this._plotChartCmd = new DelegateCommand
                    (
                        x =>
                        {
                            if (SelectedDurationPerspective == null)
                            {
                                MessageBox.Show("Please select a duration perspective", "Information",
                                        MessageBoxButton.OK, MessageBoxImage.Information);
                                return;
                            }

                            Tuple<ListBox, GroupBox> lstBxChtGrpBxStkPnlTuple = (Tuple<ListBox, GroupBox>)x;

                            List<string> lstBoxStringValues = lstBxChtGrpBxStkPnlTuple.Item1.SelectedItems.Cast<string>().ToList();
                            if (lstBoxStringValues.Count == 0)
                            {
                                MessageBox.Show("Please select one or more date values", "Information",
                                       MessageBoxButton.OK, MessageBoxImage.Information);
                                return;
                            }
                            GroupBox chtGroupBox = lstBxChtGrpBxStkPnlTuple.Item2;
                            StackPanel chtStackPanel = (StackPanel)chtGroupBox.Content;                           

                            GeneratorUsageLogic.PlotChart(SelectedGeneratorName, SelectedChartType, SelectedDurationPerspective, 
                                                            lstBoxStringValues, lstBoxStringValues.Count, chtStackPanel);

                        },
                        y => true
                    )
                );
            }
        }       
    }
}

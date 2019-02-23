using Panel.Commands;
using Panel.Interfaces;
using Panel.Models.InputModels;
using Panel.Repositories;
using Panel.Validations;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Panel.ViewModels.SettingsViewModel
{
    public class ConsumptionSettingsViewModel : ViewModelBase, IViewModel
    {
        public ConsumptionSettingsViewModel(UnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;

            UniqueGeneratorNamesUnsorted = unitOfWork.GeneratorInformation
                                                     .GetUniqueGeneratorNames();

            UniqueGeneratorNames = new ObservableCollection<GeneratorNameModel>
                (UniqueGeneratorNamesUnsorted.OrderBy(x => x.GeneratorName));

            AnyConsumptionPage = UnitOfWork.ConsumptionSetting
                                           .GetAnyConsumptionPage();
        }

        public ObservableCollection<GeneratorNameModel> UniqueGeneratorNames { get; set; } =
            new ObservableCollection<GeneratorNameModel>();
        public ObservableCollection<GeneratorNameModel> UniqueGeneratorNamesUnsorted { get; set; } =
            new ObservableCollection<GeneratorNameModel>();
        public ObservableCollection<ConsumptionSetting> AnyConsumptionPage { get; set; } =
            new ObservableCollection<ConsumptionSetting>();

        public UnitOfWork UnitOfWork { get; set; }
        public DateTime ConsumptionDate { get; set; } = DateTime.Now;
        public string GeneratorName { get; set; }

        private double _currentFuelConsumption;
        public double CurrentFuelConsumption
        {
            get => _currentFuelConsumption;
            set
            {
                _currentFuelConsumption = value;                
                ValidConsumptionRuleAsync
                .ValidateConsumptionRuleAsync
                (CurrentFuelConsumption)
                .ContinueWith(t =>
                {
                    lock (_errors)
                    {
                        _errors.Clear();
                        _errors[nameof(CurrentFuelConsumption)] = t.Result;
                        OnErrorsChanged(nameof(CurrentFuelConsumption));
                    }
                });
                OnPropertyChanged(nameof(CurrentFuelConsumption));
            }
        }

        private double _testFuelConsumption;
        public double TestFuelConsumption
        {
            get => _testFuelConsumption;
            set
            {
                _testFuelConsumption = value;
                OnPropertyChanged(nameof(TestFuelConsumption));
                ValidConsumptionRuleAsync
                    .ValidateConsumptionRuleAsync
                    (TestFuelConsumption)
                    .ContinueWith(t =>
                    {
                        lock (_errors)
                        {
                            _errors.Clear();
                            _errors[nameof(TestFuelConsumption)] = t.Result;
                            OnErrorsChanged(nameof(TestFuelConsumption));
                        }
                    });
            }
        }

        private double _standardFuelConsumption;
        public double StandardFuelConsumption
        {
            get => _standardFuelConsumption;
            set
            {
                _standardFuelConsumption = value;
                OnPropertyChanged(nameof(StandardFuelConsumption));
                ValidConsumptionRuleAsync
                    .ValidateConsumptionRuleAsync
                    (StandardFuelConsumption)
                    .ContinueWith(t =>
                    {
                        lock (_errors)
                        {
                            _errors.Clear();
                            _errors[nameof(StandardFuelConsumption)] = t.Result;
                            OnErrorsChanged(nameof(StandardFuelConsumption));
                        }
                    });
            }
        }

        private ICommand _setConsumptionCmd;
        public ICommand SetConsumptionCmd
        {
            get
            {
                return this._setConsumptionCmd ??
                (
                    this._setConsumptionCmd = new DelegateCommand
                    (
                        x =>
                        {
                            if (CurrentFuelConsumption == 0 || TestFuelConsumption == 0
                            || StandardFuelConsumption == 0)
                            {
                                MessageBox.Show($"All fuel consumption data must " +
                                    $"be set to a valid values",
                                    "Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                                return;
                            }

                            UnitOfWork.ConsumptionSetting.SetConsumption(ConsumptionDate,
                                GeneratorName, CurrentFuelConsumption, TestFuelConsumption,
                                StandardFuelConsumption);

                            int Success = UnitOfWork.Complete();
                            if (Success > 0)
                            {
                                AnyConsumptionPage = UnitOfWork.ConsumptionSetting
                                                               .GetAnyConsumptionPage();
                                OnPropertyChanged(nameof(AnyConsumptionPage));

                                MessageBox.Show($"Successfully saved!",
                                    "Information",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                            }                          
                        },
                        y => true
                    )
                );
            }
        }
    }
}

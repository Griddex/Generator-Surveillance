using Panel.Commands;
using Panel.Interfaces;
using Panel.Models.InputModels;
using Panel.Repositories;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Panel.ViewModels.SettingsViewModel
{
    public class ConsumptionSettingsViewModel : ViewModelBase, IViewModel
    {
        //AnyAuthorisersPage
        public ObservableCollection<GeneratorNameModel> UniqueGeneratorNames { get; set; } =
            new ObservableCollection<GeneratorNameModel>();
        public ObservableCollection<GeneratorNameModel> UniqueGeneratorNamesUnsorted { get; set; } =
            new ObservableCollection<GeneratorNameModel>();
        public ObservableCollection<ConsumptionSetting> AnyConsumptionPage { get; set; } =
            new ObservableCollection<ConsumptionSetting>();

        public UnitOfWork UnitOfWork { get; set; }
        public DateTime ConsumptionDate { get; set; } = DateTime.Now;
        public string GeneratorName { get; set; }
        public double CurrentFuelConsumption { get; set; }
        public double TestFuelConsumption { get; set; }
        public double StandardFuelConsumption { get; set; }

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
                                MessageBox.Show($"All fuel consumption values must " +
                                    $"be set to a valid value",
                                    "Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                                return;
                            }

                            UnitOfWork.ConsumptionSetting.SetComsumption(ConsumptionDate,
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

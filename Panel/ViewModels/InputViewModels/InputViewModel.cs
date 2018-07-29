using Panel.Commands;
using Panel.Interfaces;
using Panel.Models.InputModels;
using Panel.Repositories;
using Panel.Validations;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Panel.ViewModels.InputViewModels
{
    public class InputViewModel : ViewModelBase, IViewModel
    {
        public ObservableCollection<GeneratorNameModel> 
            UniqueGeneratorNamesUnsorted { get; set; } = 
            new ObservableCollection<GeneratorNameModel>();

        public ObservableCollection<GeneratorNameModel> 
            UniqueGeneratorNames { get; set; } = 
            new ObservableCollection<GeneratorNameModel>();

        public UnitOfWork UnitOfWork { get; private set; }
        public string ActiveGenerator { get; set; }

        private string _ActiveGenName;
        private DateTime? _ActiveGenStartedDate;

        public InputViewModel(UnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;

            UniqueGeneratorNamesUnsorted = unitOfWork.GeneratorInformation
                                                     .GetUniqueGeneratorNames();

            UniqueGeneratorNames = new ObservableCollection<GeneratorNameModel>
                                (UniqueGeneratorNamesUnsorted
                                .OrderBy(x => x.GeneratorName));

            var (IsNull, ActiveGenName, ActiveGenStartedDate, 
                ActiveGenStartedTime, ActiveGenID) = UnitOfWork
                                                    .GeneratorInformation
                                                    .GeneratorStoppedIsNull();
            this.IsNull = IsNull;
            this.ActiveGenStartedTime = ActiveGenStartedTime;
            ActiveGenerator = ActiveGenName;
            this._ActiveGenName = ActiveGenName;
            this._ActiveGenStartedDate = ActiveGenStartedDate;
        }

        public bool IsNull { get; }
        public DateTime? ActiveGenStartedTime { get; }

        private string _generatorName;
        public string GeneratorName
        {
            get => _generatorName;
            set
            {
                _generatorName = value;
                OnPropertyChanged(nameof(GeneratorName));
                ValidGeneratorNameRuleAsync
                    .ValidateGeneratorNameAsync(GeneratorName)
                    .ContinueWith(t =>
                    {
                        lock (_errors)
                        {
                            _errors[nameof(GeneratorName)] = t.Result;
                            OnErrorsChanged(nameof(GeneratorName));
                        }                        
                    });
            }
        }
        
        private ICommand _loadActiveGeneratorRecord;
        public ICommand LoadActiveGeneratorRecord
        {
            get
            {
                return this._loadActiveGeneratorRecord ??
                (
                    this._loadActiveGeneratorRecord = new DelegateCommand
                    (
                        x =>
                        {
                            if (IsNull)
                            {
                                Tuple<DatePicker, ComboBox> dtpkrcmbx = 
                                (Tuple<DatePicker, ComboBox>)x;

                                dtpkrcmbx.Item1.SelectedDate = 
                                _ActiveGenStartedDate;

                                dtpkrcmbx.Item2.SelectedValue = 
                                _ActiveGenName;                                
                            }                                                    
                        },
                        y =>
                        {
                            return !HasErrors;
                        }
                    )
                );
            }
        }

        private ICommand addGeneratorCmd;
        public ICommand AddGeneratorCmd
        {
            get
            {
                return addGeneratorCmd ??
                (
                    addGeneratorCmd = new DelegateCommand
                    (
                        x =>
                        {
                            ComboBox cmbxGenInfo = x as ComboBox;
                            if (cmbxGenInfo.Text == null || 
                            cmbxGenInfo.Text == "")
                            {
                                MessageBox.Show("Select or Type-in " +
                                    "a valid generator name", "Error", 
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                                return;
                            }
                            GeneratorName = cmbxGenInfo.Text;
                            UnitOfWork.GeneratorInformation
                                      .AddGeneratorName(GeneratorName, 
                                                        UniqueGeneratorNames, 
                                                        cmbxGenInfo);
                        },
                        y =>
                        {
                            return !HasErrors;
                        }                        
                    )
                );
            }
        }

        private ICommand archiveGeneratorCmd;
        public ICommand ArchiveGeneratorCmd
        {
            get
            {
                return archiveGeneratorCmd ??
                (
                    archiveGeneratorCmd = new DelegateCommand
                    (
                        x =>
                        {
                            ComboBox cmbxGenInfo = x as ComboBox;
                            if (cmbxGenInfo.Text == null || 
                            cmbxGenInfo.Text == "")
                            {
                                MessageBox.Show("Select a valid " +
                                    "generator name", "Error", 
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                                return;
                            }

                            GeneratorName = cmbxGenInfo.Text;
                            UnitOfWork.GeneratorInformation
                                      .ArchiveGeneratorName(GeneratorName, 
                                                            UniqueGeneratorNames, 
                                                            cmbxGenInfo);

                            int ArchivalSuccess = UnitOfWork.Complete();
                            if (ArchivalSuccess > 0)
                                MessageBox.Show($"{GeneratorName} has been " +
                                    $"successfully archived!", 
                                    "Success", 
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                            else
                            {
                                MessageBox.Show($"{GeneratorName} could " +
                                    $"not be archived...", 
                                    "Error", 
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                                return;
                            }
                        },
                        y =>
                        {
                            return !HasErrors;
                        }
                    )
                );
            }
        }
    }
}

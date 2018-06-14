using Panel.Commands;
using Panel.Models.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Panel.Interfaces;
using Panel.Services.NavigationService;
using System.Windows;
using System.Windows.Navigation;
using Panel.Repositories;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Panel.Validations;
using System.Windows.Data;
using System.Globalization;
using System.Collections;

namespace Panel.ViewModels.InputViewModels
{
    public class InputViewModel : ViewModelBase, IViewModel
    {
        public ObservableCollection<GeneratorNameModel> UniqueGeneratorNames { get; set; } = new ObservableCollection<GeneratorNameModel>();
        public UnitOfWork UnitOfWork { get; private set; }
        private bool _isNull;
        private DateTime? _lastGenStartedTime;
        private string _lastGenName;
        private DateTime? _lastGenStartedDate;

        public InputViewModel(UnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            UniqueGeneratorNames = unitOfWork.GeneratorInformation.GetUniqueGeneratorNames();
            var (IsNull, LastGenName, LastGenStartedDate, LastGenStartedTime) = UnitOfWork.GeneratorInformation.GeneratorStoppedIsNull();
            this._isNull = IsNull;
            this._lastGenStartedTime = LastGenStartedTime;
            this._lastGenName = LastGenName;
            this._lastGenStartedDate = LastGenStartedDate;
        }

        public bool IsNull { get => _isNull; }
        public DateTime? LastGenStartedTime { get => _lastGenStartedTime; }

        private string _generatorName;
        public string GeneratorName
        {
            get => _generatorName;
            set
            {
                _generatorName = value;
                OnPropertyChanged(nameof(GeneratorName));
                ValidGeneratorNameRuleAsync.ValidateGeneratorNameAsync(GeneratorName)
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
        
        private ICommand _loadLastGeneratorRecord;
        public ICommand LoadLastGeneratorRecord
        {
            get
            {
                return this._loadLastGeneratorRecord ??
                (
                    this._loadLastGeneratorRecord = new DelegateCommand
                    (
                        x =>
                        {
                            if (_isNull)
                            {
                                foreach (var item in x as ArrayList)
                                {
                                    if (item is DatePicker)
                                        (item as DatePicker).SelectedDate = _lastGenStartedDate;
                                    else if (item is ComboBox)
                                        (item as ComboBox).SelectedValue = _lastGenName;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Last Generator Usage Record was updated", "Information", MessageBoxButton.OK,
                                    MessageBoxImage.Information);
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
                            if (cmbxGenInfo.Text == null || cmbxGenInfo.Text == "")
                            {
                                MessageBox.Show("Select or Type-in a valid generator name", "Error", MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                                return;
                            }
                            GeneratorName = cmbxGenInfo.Text;
                            UnitOfWork.GeneratorInformation.AddGeneratorName(GeneratorName, UniqueGeneratorNames, cmbxGenInfo);
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
                            if (cmbxGenInfo.Text == null || cmbxGenInfo.Text == "")
                            {
                                MessageBox.Show("Select a valid generator name", "Error", MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                                return;
                            }
                            GeneratorName = cmbxGenInfo.Text;
                            UnitOfWork.GeneratorInformation.ArchiveGeneratorName(GeneratorName, UniqueGeneratorNames, cmbxGenInfo);
                            int ArchivalSuccess = UnitOfWork.Complete();
                            if (ArchivalSuccess > 0)
                                MessageBox.Show($"{GeneratorName} has been successfully archived!", "Success", MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                            else
                            {
                                MessageBox.Show($"{GeneratorName} could not be archived...", "Error", MessageBoxButton.OK,
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

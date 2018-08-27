using Panel.Commands;
using Panel.Interfaces;
using Panel.Repositories;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Panel.ViewModels.TableViewModels
{
    public class UsageFuellingTablesViewModel : ViewModelBase, IViewModel
    {
        public UsageFuellingTablesViewModel(UnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;

            AllUsageRecords = UnitOfWork.GeneratorUsage.GetAllGeneratorUsages();
            AllFuellingRecords = UnitOfWork.GeneratorFuelling.GetAllGeneratorFuellings();
            AllMaintenanceRecords = UnitOfWork.GeneratorMaintenance.GetAllGeneratorMaintenances();

            AnyPageUsageRecords = UnitOfWork.GeneratorUsage.GetAnyPageGeneratorUsages();
            AnyPageFuellingRecords = UnitOfWork.GeneratorFuelling.GetAnyPageGeneratorFuellings();
            AnyPageMaintenanceRecords = UnitOfWork.GeneratorMaintenance.GetAnyPageGeneratorMaintenance();
        }

        public UnitOfWork UnitOfWork { get; set; }

        public string PageIndexUsage { get; set; } = "1";
        public ComboBoxItem PageSizeUsage { get; set; }        

        public ObservableCollection<GeneratorUsage> AllUsageRecords;
        private ObservableCollection<GeneratorUsage> _anyPageUsageRecords;
        public ObservableCollection<GeneratorUsage> AnyPageUsageRecords
        {
            get => _anyPageUsageRecords;
            set
            {
                _anyPageUsageRecords = value;
                OnPropertyChanged(nameof(AnyPageUsageRecords));
            }
        }

        private string _currentPageOutOfTotalUsage;
        public string CurrentPageOutOfTotalUsage
        {
            get
            {
                int pageIndexUsage = Convert.ToInt32(PageIndexUsage);
                int pageSizeUsage = Convert.ToInt32(PageSizeUsage.Content);
                int Toprecord = ((pageIndexUsage * pageSizeUsage) - 1 < AllUsageRecords.Count)? 
                    (pageIndexUsage * pageSizeUsage) - 1 : AllUsageRecords.Count;
                _currentPageOutOfTotalUsage = $"{(pageIndexUsage - 1) * pageSizeUsage} - " +
                    $"{Toprecord} out of {AllUsageRecords.Count}";
                return _currentPageOutOfTotalUsage;
            }
            set { _currentPageOutOfTotalUsage = value; }
        }

        private ComboBoxItem _selectedPageSizeUsage;
        public ComboBoxItem SelectedPageSizeUsage
        {
            get { return _selectedPageSizeUsage; }
            set
            {
                _selectedPageSizeUsage = value;
                OnPropertyChanged(nameof(SelectedPageSizeUsage));
            }
        }        
        
        private ICommand _nextPageUsageCmd;
        public ICommand NextPageUsageCmd
        {
            get
            {
                return this._nextPageUsageCmd ??
                (
                    this._nextPageUsageCmd = new DelegateCommand
                    (
                        x =>
                        {
                            int pageIndexUsage;
                            int pageSizeUsage;

                            pageIndexUsage = Convert.ToInt32(PageIndexUsage);
                            pageIndexUsage += 1;
                            PageIndexUsage = Convert.ToString(pageIndexUsage);
                            OnPropertyChanged(nameof(PageIndexUsage));

                            pageSizeUsage = Convert.ToInt32(Convert.ToString(PageSizeUsage.Content));
                            CurrentPageOutOfTotalUsage = $"{(pageIndexUsage - 1) * pageSizeUsage} - " +
                            $"{(pageIndexUsage * pageSizeUsage) - 1} out of {AllUsageRecords.Count}";
                            OnPropertyChanged(nameof(CurrentPageOutOfTotalUsage));

                            AnyPageUsageRecords = UnitOfWork.GeneratorUsage
                                                            .GetAnyPageGeneratorUsages(pageIndexUsage, 
                                                            pageSizeUsage);
                        },
                        y => !HasErrors
                    )
                );
            }
        }

        private ICommand _previousPageUsageCmd;
        public ICommand PreviousPageUsageCmd
        {
            get
            {
                return this._previousPageUsageCmd ??
                (
                    this._previousPageUsageCmd = new DelegateCommand
                    (
                        x =>
                        {
                            int pageIndexUsage;
                            int pageSizeUsage;

                            pageIndexUsage = Convert.ToInt32(PageIndexUsage);
                            if (pageIndexUsage == 1)
                                return;
                            pageIndexUsage -= 1;
                            PageIndexUsage = Convert.ToString(pageIndexUsage);
                            OnPropertyChanged(nameof(PageIndexUsage));

                            pageSizeUsage = Convert.ToInt32(Convert.ToString(PageSizeUsage.Content));
                            CurrentPageOutOfTotalUsage = $"{(pageIndexUsage - 1) * pageSizeUsage} - " +
                            $"{(pageIndexUsage * pageSizeUsage) - 1} out of {AllUsageRecords.Count}";
                            OnPropertyChanged(nameof(CurrentPageOutOfTotalUsage));

                            AnyPageUsageRecords = UnitOfWork.GeneratorUsage
                                                            .GetAnyPageGeneratorUsages(pageIndexUsage, 
                                                            pageSizeUsage);
                        },
                        y => !HasErrors
                    )
                );
            }
        }

        
        public string PageIndexFuelling { get; set; } = "1";
        public ComboBoxItem PageSizeFuelling { get; set; }

        public ObservableCollection<GeneratorFuelling> AllFuellingRecords;
        private ObservableCollection<GeneratorFuelling> _anyPageFuellingRecords;
        public ObservableCollection<GeneratorFuelling> AnyPageFuellingRecords
        {
            get => _anyPageFuellingRecords;
            set
            {
                _anyPageFuellingRecords = value;
                OnPropertyChanged(nameof(AnyPageFuellingRecords));
            }
        }

        private string _currentPageOutOfTotalFuelling;
        public string CurrentPageOutOfTotalFuelling
        {
            get
            {
                int pageIndexFuelling = Convert.ToInt32(PageIndexFuelling);
                int pageSizeFuelling = Convert.ToInt32(PageSizeFuelling.Content);
                int TopRecord = ((pageIndexFuelling * pageSizeFuelling) - 1 < AllFuellingRecords.Count) ? 
                    (pageIndexFuelling * pageSizeFuelling) - 1 : AllFuellingRecords.Count;
                _currentPageOutOfTotalFuelling = $"{(pageIndexFuelling - 1) * pageSizeFuelling} - " +
                    $"{TopRecord} out of {AllFuellingRecords.Count}";
                return _currentPageOutOfTotalFuelling;
            }
            set { _currentPageOutOfTotalFuelling = value; }
        }

        private ComboBoxItem _selectedPageSizeFuelling;
        public ComboBoxItem SelectedPageSizeFuelling
        {
            get { return _selectedPageSizeFuelling; }
            set
            {
                _selectedPageSizeFuelling = value;
                OnPropertyChanged(nameof(SelectedPageSizeFuelling));
            }
        }

        private ICommand _nextPageFuellingCmd;
        public ICommand NextPageFuellingCmd
        {
            get
            {
                return this._nextPageFuellingCmd ??
                (
                    this._nextPageFuellingCmd = new DelegateCommand
                    (
                        x =>
                        {
                            int pageIndexFuelling;
                            int pageSizeFuelling;

                            pageIndexFuelling = Convert.ToInt32(PageIndexFuelling);
                            pageIndexFuelling += 1;
                            PageIndexFuelling = Convert.ToString(pageIndexFuelling);
                            OnPropertyChanged(nameof(PageIndexFuelling));

                            pageSizeFuelling = Convert.ToInt32(Convert.ToString(PageSizeFuelling.Content));
                            CurrentPageOutOfTotalFuelling = $"{(pageIndexFuelling - 1) * pageSizeFuelling} - " +
                            $"{(pageIndexFuelling * pageSizeFuelling) - 1} out of {AllFuellingRecords.Count}";
                            OnPropertyChanged(nameof(CurrentPageOutOfTotalFuelling));

                            AnyPageFuellingRecords = UnitOfWork.GeneratorFuelling
                                                               .GetAnyPageGeneratorFuellings(pageIndexFuelling, 
                                                               pageSizeFuelling);
                        },
                        y => !HasErrors
                    )
                );
            }
        }

        private ICommand _previousPageFuellingCmd;
        public ICommand PreviousPageFuellingCmd
        {
            get
            {
                return this._previousPageFuellingCmd ??
                (
                    this._previousPageFuellingCmd = new DelegateCommand
                    (
                        x =>
                        {
                            int pageIndexFuelling;
                            int pageSizeFuelling;

                            pageIndexFuelling = Convert.ToInt32(PageIndexFuelling);
                            if (pageIndexFuelling == 1)
                                return;
                            pageIndexFuelling -= 1;
                            PageIndexFuelling = Convert.ToString(pageIndexFuelling);
                            OnPropertyChanged(nameof(PageIndexFuelling));

                            pageSizeFuelling = Convert.ToInt32(Convert.ToString(PageSizeFuelling.Content));
                            CurrentPageOutOfTotalFuelling = $"{(pageIndexFuelling - 1) * pageSizeFuelling} - " +
                            $"{(pageIndexFuelling * pageSizeFuelling) - 1} out of {AllFuellingRecords.Count}";
                            OnPropertyChanged(nameof(CurrentPageOutOfTotalFuelling));

                            AnyPageFuellingRecords = UnitOfWork.GeneratorFuelling
                                                               .GetAnyPageGeneratorFuellings(pageIndexFuelling, 
                                                               pageSizeFuelling);
                        },
                        y => !HasErrors
                    )
                );
            }
        }

        
        public string PageIndexMaintenance { get; set; } = "1";
        public ComboBoxItem PageSizeMaintenance { get; set; }

        public ObservableCollection<GeneratorMaintenance> AllMaintenanceRecords;
        private ObservableCollection<GeneratorMaintenance> _anyPageMaintenanceRecords;
        public ObservableCollection<GeneratorMaintenance> AnyPageMaintenanceRecords
        {
            get => _anyPageMaintenanceRecords;
            set
            {
                _anyPageMaintenanceRecords = value;
                OnPropertyChanged(nameof(AnyPageMaintenanceRecords));
            }
        }

        private string _currentPageOutOfTotalMaintenance;
        public string CurrentPageOutOfTotalMaintenance
        {
            get
            {
                int pageIndexMaintenance = Convert.ToInt32(PageIndexMaintenance);
                int pageSizeMaintenance = Convert.ToInt32(PageSizeMaintenance.Content);
                int Toprecord = ((pageIndexMaintenance * pageSizeMaintenance) - 1 < AllMaintenanceRecords.Count) ? 
                    (pageIndexMaintenance * pageSizeMaintenance) - 1 : AllMaintenanceRecords.Count;
                _currentPageOutOfTotalMaintenance = $"{(pageIndexMaintenance - 1) * pageSizeMaintenance} - " +
                    $"{Toprecord} out of {AllMaintenanceRecords.Count}";
                return _currentPageOutOfTotalMaintenance;
            }
            set { _currentPageOutOfTotalMaintenance = value; }
        }

        private ComboBoxItem _selectedPageSizeMaintenance;
        public ComboBoxItem SelectedPageSizeMaintenance
        {
            get { return _selectedPageSizeMaintenance; }
            set
            {
                _selectedPageSizeMaintenance = value;
                OnPropertyChanged(nameof(SelectedPageSizeMaintenance));
            }
        }

        private ICommand _nextPageMaintenanceCmd;
        public ICommand NextPageMaintenanceCmd
        {
            get
            {
                return this._nextPageMaintenanceCmd ??
                (
                    this._nextPageMaintenanceCmd = new DelegateCommand
                    (
                        x =>
                        {
                            int pageIndexMaintenance;
                            int pageSizeMaintenance;

                            pageIndexMaintenance = Convert.ToInt32(PageIndexMaintenance);
                            pageIndexMaintenance += 1;
                            PageIndexMaintenance = Convert.ToString(pageIndexMaintenance);
                            OnPropertyChanged(nameof(PageIndexMaintenance));

                            pageSizeMaintenance = Convert.ToInt32(Convert.ToString(PageSizeMaintenance.Content));
                            CurrentPageOutOfTotalMaintenance = $"{(pageIndexMaintenance - 1) * pageSizeMaintenance} - " +
                            $"{(pageIndexMaintenance * pageSizeMaintenance) - 1} out of {AllMaintenanceRecords.Count}";
                            OnPropertyChanged(nameof(CurrentPageOutOfTotalMaintenance));

                            AnyPageMaintenanceRecords = UnitOfWork.GeneratorMaintenance
                                                                  .GetAnyPageGeneratorMaintenance(pageIndexMaintenance, 
                                                                  pageSizeMaintenance);
                        },
                        y => !HasErrors
                    )
                );
            }
        }

        private ICommand _previousPageMaintenanceCmd;
        public ICommand PreviousPageMaintenanceCmd
        {
            get
            {
                return this._previousPageMaintenanceCmd ??
                (
                    this._previousPageMaintenanceCmd = new DelegateCommand
                    (
                        x =>
                        {
                            int pageIndexMaintenance;
                            int pageSizeMaintenance;

                            pageIndexMaintenance = Convert.ToInt32(PageIndexMaintenance);
                            if (pageIndexMaintenance == 1)
                                return;
                            pageIndexMaintenance -= 1;
                            PageIndexMaintenance = Convert.ToString(pageIndexMaintenance);
                            OnPropertyChanged(nameof(PageIndexMaintenance));

                            pageSizeMaintenance = Convert.ToInt32(Convert.ToString(PageSizeMaintenance.Content));
                            CurrentPageOutOfTotalMaintenance = $"{(pageIndexMaintenance - 1) * pageSizeMaintenance} - " +
                            $"{(pageIndexMaintenance * pageSizeMaintenance) - 1} out of {AllMaintenanceRecords.Count}";
                            OnPropertyChanged(nameof(CurrentPageOutOfTotalMaintenance));

                            AnyPageMaintenanceRecords = UnitOfWork.GeneratorMaintenance
                                                                  .GetAnyPageGeneratorMaintenance(pageIndexMaintenance,
                                                                  pageSizeMaintenance);
                        },
                        y => !HasErrors
                    )
                );
            }
        }
    }
}

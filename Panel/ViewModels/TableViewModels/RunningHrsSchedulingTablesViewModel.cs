using Panel.Commands;
using Panel.Interfaces;
using Panel.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Panel.ViewModels.TableViewModels
{
    public class RunningHrsSchedulingTablesViewModel : ViewModelBase, IViewModel
    {
        public RunningHrsSchedulingTablesViewModel(UnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            AnyPageRnHrsRecords = UnitOfWork.GeneratorRunningHr
                                            .GetAnyPageGeneratorRunningHrs();

            AnyPageSchRemRecords = UnitOfWork.GeneratorScheduler
                                            .GetAnyPageGeneratorScheduledRmdrs();
        }

        public UnitOfWork UnitOfWork { get; set; }
        public string PageIndexRnHrs { get; set; } = "1";
        public ComboBoxItem PageSizeRnHrs { get; set; }

        public ObservableCollection<GeneratorRunningHr> AllRnHrsRecords;
        private ObservableCollection<GeneratorRunningHr> _anyPageRnHrsRecords;
        public ObservableCollection<GeneratorRunningHr> AnyPageRnHrsRecords
        {
            get => _anyPageRnHrsRecords;
            set
            {
                _anyPageRnHrsRecords = value;
                OnPropertyChanged(nameof(AnyPageRnHrsRecords));
            }
        }

        private string _currentPageOutOfTotalRnHrs;
        public string CurrentPageOutOfTotalRnHrs
        {
            get
            {
                int pageIndexRnHrs = Convert.ToInt32(PageIndexRnHrs);
                int pageSizeRnHrs = Convert.ToInt32(PageSizeRnHrs.Content);
                int Toprecord = ((pageIndexRnHrs * pageSizeRnHrs) - 1 < AllRnHrsRecords.Count) ?
                    (pageIndexRnHrs * pageSizeRnHrs) - 1 : AllRnHrsRecords.Count;

                _currentPageOutOfTotalRnHrs = $"{(pageIndexRnHrs - 1) * pageSizeRnHrs} - " +
                    $"{Toprecord} out of {AllRnHrsRecords.Count}";

                return _currentPageOutOfTotalRnHrs;
            }
            set { _currentPageOutOfTotalRnHrs = value; }
        }

        private ComboBoxItem _selectedPageSizeRnHrs;
        public ComboBoxItem SelectedPageSizeRnHrs
        {
            get { return _selectedPageSizeRnHrs; }
            set
            {
                _selectedPageSizeRnHrs = value;
                OnPropertyChanged(nameof(SelectedPageSizeRnHrs));
            }
        }

        private ICommand _nextPageRnHrsCmd;
        public ICommand NextPageRnHrsCmd
        {
            get
            {
                return this._nextPageRnHrsCmd ??
                (
                    this._nextPageRnHrsCmd = new DelegateCommand
                    (
                        x =>
                        {
                            int pageIndexRnHrs;
                            int pageSizeRnHrs;

                            pageIndexRnHrs = Convert.ToInt32(PageIndexRnHrs);
                            pageIndexRnHrs += 1;
                            PageIndexRnHrs = Convert.ToString(pageIndexRnHrs);
                            OnPropertyChanged(nameof(PageIndexRnHrs));

                            pageSizeRnHrs = Convert.ToInt32(Convert.ToString(PageSizeRnHrs.Content));
                            CurrentPageOutOfTotalRnHrs = $"{(pageIndexRnHrs - 1) * pageSizeRnHrs} - " +
                            $"{(pageIndexRnHrs * pageSizeRnHrs) - 1} out of {AllRnHrsRecords.Count}";
                            OnPropertyChanged(nameof(CurrentPageOutOfTotalRnHrs));

                            AnyPageRnHrsRecords = UnitOfWork.GeneratorRunningHr
                                                            .GetAnyPageGeneratorRunningHrs
                                                            (pageIndexRnHrs, pageSizeRnHrs);
                        },
                        y => !HasErrors
                    )
                );
            }
        }

        private ICommand _previousPageRnHrsCmd;
        public ICommand PreviousPageRnHrsCmd
        {
            get
            {
                return this._previousPageRnHrsCmd ??
                (
                    this._previousPageRnHrsCmd = new DelegateCommand
                    (
                        x =>
                        {
                            int pageIndexRnHrs;
                            int pageSizeRnHrs;

                            pageIndexRnHrs = Convert.ToInt32(PageIndexRnHrs);
                            if (pageIndexRnHrs == 1)
                                return;
                            pageIndexRnHrs -= 1;
                            PageIndexRnHrs = Convert.ToString(pageIndexRnHrs);
                            OnPropertyChanged(nameof(PageIndexRnHrs));

                            pageSizeRnHrs = Convert.ToInt32(Convert.ToString(PageSizeRnHrs.Content));
                            CurrentPageOutOfTotalRnHrs = $"{(pageIndexRnHrs - 1) * pageSizeRnHrs} - " +
                            $"{(pageIndexRnHrs * pageSizeRnHrs) - 1} out of {AllRnHrsRecords.Count}";
                            OnPropertyChanged(nameof(CurrentPageOutOfTotalRnHrs));

                            AnyPageRnHrsRecords = UnitOfWork.GeneratorRunningHr
                                                            .GetAnyPageGeneratorRunningHrs
                                                            (pageIndexRnHrs, pageSizeRnHrs);
                        },
                        y => !HasErrors
                    )
                );
            }
        }


        public string PageIndexSchRem { get; set; } = "1";
        public ComboBoxItem PageSizeSchRem { get; set; }

        public ObservableCollection<GeneratorScheduler> AllSchRemRecords;
        private ObservableCollection<GeneratorScheduler> _anyPageSchRemRecords;
        public ObservableCollection<GeneratorScheduler> AnyPageSchRemRecords
        {
            get => _anyPageSchRemRecords;
            set
            {
                _anyPageSchRemRecords = value;
                OnPropertyChanged(nameof(AnyPageSchRemRecords));
            }
        }

        private string _currentPageOutOfTotalSchRem;
        public string CurrentPageOutOfTotalSchRem
        {
            get
            {
                int pageIndexSchRem = Convert.ToInt32(PageIndexSchRem);
                int pageSizeSchRem = Convert.ToInt32(PageSizeSchRem.Content);
                int Toprecord = ((pageIndexSchRem * pageSizeSchRem) - 1 < AllSchRemRecords.Count) ?
                    (pageIndexSchRem * pageSizeSchRem) - 1 : AllSchRemRecords.Count;

                _currentPageOutOfTotalSchRem = $"{(pageIndexSchRem - 1) * pageSizeSchRem} - " +
                    $"{Toprecord} out of {AllSchRemRecords.Count}";
                return _currentPageOutOfTotalSchRem;
            }
            set { _currentPageOutOfTotalSchRem = value; }
        }

        private ComboBoxItem _selectedPageSizeSchRem;
        public ComboBoxItem SelectedPageSizeSchRem
        {
            get { return _selectedPageSizeSchRem; }
            set
            {
                _selectedPageSizeSchRem = value;
                OnPropertyChanged(nameof(SelectedPageSizeSchRem));
            }
        }

        private ICommand _nextPageSchRemCmd;
        public ICommand NextPageSchRemCmd
        {
            get
            {
                return this._nextPageSchRemCmd ??
                (
                    this._nextPageSchRemCmd = new DelegateCommand
                    (
                        x =>
                        {
                            int pageIndexSchRem;
                            int pageSizeSchRem;

                            pageIndexSchRem = Convert.ToInt32(PageIndexSchRem);
                            pageIndexSchRem += 1;
                            PageIndexSchRem = Convert.ToString(pageIndexSchRem);
                            OnPropertyChanged(nameof(PageIndexSchRem));

                            pageSizeSchRem = Convert.ToInt32(Convert.ToString(PageSizeSchRem.Content));
                            CurrentPageOutOfTotalSchRem = $"{(pageIndexSchRem - 1) * pageSizeSchRem} - " +
                            $"{(pageIndexSchRem * pageSizeSchRem) - 1} out of {AllSchRemRecords.Count}";
                            OnPropertyChanged(nameof(CurrentPageOutOfTotalSchRem));

                            AnyPageSchRemRecords = UnitOfWork.GeneratorScheduler
                                                            .GetAnyPageGeneratorScheduledRmdrs
                                                            (pageIndexSchRem, pageSizeSchRem);
                        },
                        y => !HasErrors
                    )
                );
            }
        }

        private ICommand _previousPageSchRemCmd;
        public ICommand PreviousPageSchRemCmd
        {
            get
            {
                return this._previousPageSchRemCmd ??
                (
                    this._previousPageSchRemCmd = new DelegateCommand
                    (
                        x =>
                        {
                            int pageIndexSchRem;
                            int pageSizeSchRem;

                            pageIndexSchRem = Convert.ToInt32(PageIndexSchRem);
                            if (pageIndexSchRem == 1)
                                return;
                            pageIndexSchRem -= 1;
                            PageIndexSchRem = Convert.ToString(pageIndexSchRem);
                            OnPropertyChanged(nameof(PageIndexSchRem));

                            pageSizeSchRem = Convert.ToInt32(Convert.ToString(PageSizeSchRem.Content));
                            CurrentPageOutOfTotalSchRem = $"{(pageIndexSchRem - 1) * pageSizeSchRem} - " +
                            $"{(pageIndexSchRem * pageSizeSchRem) - 1} out of {AllSchRemRecords.Count}";
                            OnPropertyChanged(nameof(CurrentPageOutOfTotalSchRem));

                            AnyPageSchRemRecords = UnitOfWork.GeneratorScheduler
                                                            .GetAnyPageGeneratorScheduledRmdrs
                                                            (pageIndexSchRem, pageSizeSchRem);
                        },
                        y => !HasErrors
                    )
                );
            }
        }
    }
}

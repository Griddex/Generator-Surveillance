using Panel.Commands;
using Panel.Interfaces;
using Panel.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Panel.ViewModels.TableViewModels
{
    public class UsageFuellingTablesViewModel : ViewModelBase, IViewModel
    {
        public UsageFuellingTablesViewModel(UnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            AllUsageRecords = UnitOfWork.GeneratorUsage.GetAllGeneratorUsages();
            AnyPageUsageRecords = UnitOfWork.GeneratorUsage.GetAnyPageGeneratorUsages();
            AnyPageFuellingRecords = UnitOfWork.GeneratorFuelling.GetAnyPageGeneratorFuellings();
            AnyPageMaintenanceRecords = UnitOfWork.GeneratorMaintenance.GetAnyPageGeneratorMaintenance();
        }

        public UnitOfWork UnitOfWork { get; set; }
        public string PageIndex { get; set; }
        public string PageSize { get; set; }

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

        private string _currentPageOutOfTotal;
        public string CurrentPageOutOfTotal
        {
            get
            {
                int pageIndex = Convert.ToInt32(PageIndex);
                int pageSize = Convert.ToInt32(PageSize);
                _currentPageOutOfTotal = $"{(pageIndex - 1) * pageSize} - {(pageIndex * pageSize) - 1} out of {AllUsageRecords.Count}";
                return _currentPageOutOfTotal;
            }
            set { _currentPageOutOfTotal = value; }
        }


        public ObservableCollection<GeneratorFuelling> AnyPageFuellingRecords { get; }
        public ObservableCollection<GeneratorMaintenance> AnyPageMaintenanceRecords { get; }
        public static int HowManyTimes = 0;
        private ICommand _nextPageCmd;
        public ICommand NextPageCmd
        {
            get
            {
                return this._nextPageCmd ??
                (
                    this._nextPageCmd = new DelegateCommand
                    (
                        x =>
                        {
                            HowManyTimes++;
                            Tuple<TextBox, TextBox> txtBxTxtBx = (Tuple<TextBox, TextBox>)x;
                            int pageIndex = Convert.ToInt32(txtBxTxtBx.Item1.Text);
                            int pageSize = Convert.ToInt32(txtBxTxtBx.Item2.Text);
                            if (HowManyTimes > 1)
                                pageIndex++;
                            AnyPageUsageRecords = UnitOfWork.GeneratorUsage.GetAnyPageGeneratorUsages(pageIndex, pageSize);
                        },
                        y => !HasErrors
                    )
                );
            }
        }


    }
}

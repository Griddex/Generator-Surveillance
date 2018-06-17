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
            AnyPageUsageRecords = UnitOfWork.GeneratorUsage.GetAnyPageGeneratorUsages();
            AnyPageFuellingRecords = UnitOfWork.GeneratorFuelling.GetAnyPageGeneratorFuellings();
            AnyPageMaintenanceRecords = UnitOfWork.GeneratorMaintenance.GetAnyPageGeneratorMaintenance();
        }
        public UnitOfWork UnitOfWork { get; set; }

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
        public ObservableCollection<GeneratorFuelling> AnyPageFuellingRecords { get; }
        public ObservableCollection<GeneratorMaintenance> AnyPageMaintenanceRecords { get; }



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
                            Tuple<TextBox, TextBox> txtBxTxtBx = (Tuple<TextBox, TextBox>)x;
                            int pageIndex = Convert.ToInt32(txtBxTxtBx.Item1.Text);
                            int pageSize = Convert.ToInt32(txtBxTxtBx.Item2.Text);
                            _anyPageUsageRecords = UnitOfWork.GeneratorUsage.GetAnyPageGeneratorUsages(pageIndex, pageSize);
                        },
                        y => !HasErrors
                    )
                );
            }
        }


    }
}

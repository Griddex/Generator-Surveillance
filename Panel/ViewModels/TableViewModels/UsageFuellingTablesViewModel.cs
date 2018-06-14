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
using System.Windows.Data;
using System.Windows.Input;

namespace Panel.ViewModels.TableViewModels
{
    public class UsageFuellingTablesViewModel : ViewModelBase, IViewModel
    {
        public UsageFuellingTablesViewModel(UnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            AllGeneratorUsageRecords = UnitOfWork.GeneratorUsage.GetAllGeneratorUsages();
            AllGeneratorFuellingRecords = UnitOfWork.GeneratorFuelling.GetAllGeneratorFuellings();
            AllGeneratorMaintenanceRecords = UnitOfWork.GeneratorMaintenance.GetAllGeneratorFuellings();
        }
        public UnitOfWork UnitOfWork { get; set; }
        public ObservableCollection<GeneratorUsage> AllGeneratorUsageRecords { get; }
        public ObservableCollection<GeneratorFuelling> AllGeneratorFuellingRecords { get; }
        public ObservableCollection<GeneratorMaintenance> AllGeneratorMaintenanceRecords { get; }



        //private ICommand _groupByCategory;
        //public ICommand GroupByCategory
        //{
        //    get
        //    {
        //        return _groupByCategory ??
        //        (
        //            _groupByCategory = new DelegateCommand
        //            (
        //                x =>
        //                {
        //                    ICollectionView cvsGeneratorUsages = CollectionViewSource.GetDefaultView()
        //                }
        //            )
        //       );
        //    }
        //}


    }
}

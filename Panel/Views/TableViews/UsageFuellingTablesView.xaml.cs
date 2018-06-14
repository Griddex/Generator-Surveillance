using Panel.Interfaces;
using Panel.ViewModels.TableViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Panel.Views.TableViews
{
    /// <summary>
    /// Interaction logic for UsageFuellingTables.xaml
    /// </summary>
    public partial class UsageFuellingTablesView : Page, IView
    {
        public UsageFuellingTablesView(UsageFuellingTablesViewModel usageFuellingTablesViewModel)
        {
            this.DataContext = usageFuellingTablesViewModel;
            InitializeComponent();
        }

        private void GroupbyGenerator_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView cvsGeneratorUsages = CollectionViewSource.GetDefaultView(this.dtgdGenUsageTable.ItemsSource);
            if(cvsGeneratorUsages != null && cvsGeneratorUsages.CanGroup == true)
            {
                cvsGeneratorUsages.GroupDescriptions.Clear();
                cvsGeneratorUsages.GroupDescriptions.Add(new PropertyGroupDescription("GeneratorName"));
            }
        }

        private void GroupbyArchival_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView cvsGeneratorUsages = CollectionViewSource.GetDefaultView(this.dtgdGenUsageTable.ItemsSource);
            if (cvsGeneratorUsages != null && cvsGeneratorUsages.CanGroup == true)
            {
                cvsGeneratorUsages.GroupDescriptions.Clear();
                cvsGeneratorUsages.GroupDescriptions.Add(new PropertyGroupDescription("IsArchived"));
            }
        }

        private void ClearUsageGrouping_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView cvsGeneratorUsages = CollectionViewSource.GetDefaultView(this.dtgdGenUsageTable.ItemsSource);
            if (cvsGeneratorUsages != null && cvsGeneratorUsages.CanGroup == true)
            {
                cvsGeneratorUsages.GroupDescriptions.Clear();
            }
        }

        private void GroupbyVendor_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView cvsGeneratorFuelling = CollectionViewSource.GetDefaultView(this.dtgdGenFuellingTable.ItemsSource);
            if (cvsGeneratorFuelling != null && cvsGeneratorFuelling.CanGroup == true)
            {
                cvsGeneratorFuelling.GroupDescriptions.Clear();
                cvsGeneratorFuelling.GroupDescriptions.Add(new PropertyGroupDescription("Vendor"));
            }
        }

        private void ClearFuellingGrouping_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView cvsGeneratorFuelling = CollectionViewSource.GetDefaultView(this.dtgdGenFuellingTable.ItemsSource);
            if (cvsGeneratorFuelling != null && cvsGeneratorFuelling.CanGroup == true)
            {
                cvsGeneratorFuelling.GroupDescriptions.Clear();
            }
        }

        private void GroupbyType_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView cvsGeneratorMaintenance = CollectionViewSource.GetDefaultView(this.dtgdGenMaintenanceTable.ItemsSource);
            if (cvsGeneratorMaintenance != null && cvsGeneratorMaintenance.CanGroup == true)
            {
                cvsGeneratorMaintenance.GroupDescriptions.Clear();
                cvsGeneratorMaintenance.GroupDescriptions.Add(new PropertyGroupDescription("MaintenanceType"));
            }
        }

        private void ClearMaintenanceGrouping_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView cvsGeneratorMaintenance = CollectionViewSource.GetDefaultView(this.dtgdGenMaintenanceTable.ItemsSource);
            if (cvsGeneratorMaintenance != null && cvsGeneratorMaintenance.CanGroup == true)
            {
                cvsGeneratorMaintenance.GroupDescriptions.Clear();
            }
        }
    }
}

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
    /// Interaction logic for RunningHrsSchedulingTables.xaml
    /// </summary>
    public partial class RunningHrsSchedulingTablesView : Page, IView
    {
        public RunningHrsSchedulingTablesView(RunningHrsSchedulingTablesViewModel runningHrsSchedulingTablesViewModel)
        {
            InitializeComponent();
            this.DataContext = runningHrsSchedulingTablesViewModel;
        }

        private void GroupbyGeneratorRunningHours_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView cvsGeneratorRunningHours = CollectionViewSource.GetDefaultView(this.dtgdGenRunnungHrsTable.ItemsSource);
            if (cvsGeneratorRunningHours != null && cvsGeneratorRunningHours.CanGroup == true)
            {
                cvsGeneratorRunningHours.GroupDescriptions.Clear();
                cvsGeneratorRunningHours.GroupDescriptions.Add(new PropertyGroupDescription("Generator"));
            }
        }

        private void ClearRunningHoursGrouping_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView cvsGeneratorRunningHours = CollectionViewSource.GetDefaultView(this.dtgdGenRunnungHrsTable.ItemsSource);
            if (cvsGeneratorRunningHours != null && cvsGeneratorRunningHours.CanGroup == true)
            {
                cvsGeneratorRunningHours.GroupDescriptions.Clear();
            }
        }

        private void GroupbyGeneratorScheduler_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView cvsGeneratorRunningHours = CollectionViewSource.GetDefaultView(this.dtgdGenSchdRemdrTable.ItemsSource);
            if (cvsGeneratorRunningHours != null && cvsGeneratorRunningHours.CanGroup == true)
            {
                cvsGeneratorRunningHours.GroupDescriptions.Clear();
                cvsGeneratorRunningHours.GroupDescriptions.Add(new PropertyGroupDescription("GeneratorName"));
            }
        }

        private void ClearSchedulerReminderGrouping_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView cvsGeneratorRunningHours = CollectionViewSource.GetDefaultView(this.dtgdGenSchdRemdrTable.ItemsSource);
            if (cvsGeneratorRunningHours != null && cvsGeneratorRunningHours.CanGroup == true)
            {
                cvsGeneratorRunningHours.GroupDescriptions.Clear();
            }
        }
    }
}

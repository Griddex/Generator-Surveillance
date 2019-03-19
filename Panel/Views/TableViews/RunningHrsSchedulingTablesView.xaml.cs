using Panel.Interfaces;
using Panel.ViewModels.TableViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Panel.Views.TableViews
{
    /// <summary>
    /// Interaction logic for RunningHrsSchedulingTables.xaml
    /// </summary>
    public partial class RunningHrsSchedulingTablesView : Page, IView
    {
        public RunningHrsSchedulingTablesViewModel RunningHrsSchedulingTablesViewModel { get; set; }
        public RunningHrsSchedulingTablesView(RunningHrsSchedulingTablesViewModel 
            runningHrsSchedulingTablesViewModel)
        {
            this.DataContext = runningHrsSchedulingTablesViewModel;
            this.RunningHrsSchedulingTablesViewModel = runningHrsSchedulingTablesViewModel;
            InitializeComponent();
            this.Loaded += RunningHrsSchedulingTablesView_Loaded;
        }

        private void RunningHrsSchedulingTablesView_Loaded(object sender, RoutedEventArgs e)
        {
            RunningHrsSchedulingTablesViewModel.RefreshGeneratorRunningHrTable.Execute(null);
            RunningHrsSchedulingTablesViewModel.RefreshGeneratorSchedMaintTable.Execute(null);
        }

        private void GroupbyGeneratorRunningHours_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView cvsGeneratorRunningHours = CollectionViewSource
                                                       .GetDefaultView(this.dtgdGenRunningHrsTable
                                                                           .ItemsSource);
            if (cvsGeneratorRunningHours != null && cvsGeneratorRunningHours.CanGroup == true)
            {
                cvsGeneratorRunningHours.GroupDescriptions.Clear();
                cvsGeneratorRunningHours.GroupDescriptions
                    .Add(new PropertyGroupDescription("Generator"));
            }
        }

        private void ClearRunningHoursGrouping_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView cvsGeneratorRunningHours = CollectionViewSource
                                                       .GetDefaultView(this.dtgdGenRunningHrsTable
                                                       .ItemsSource);
            if (cvsGeneratorRunningHours != null && cvsGeneratorRunningHours.CanGroup == true)
            {
                cvsGeneratorRunningHours.GroupDescriptions.Clear();
            }
        }

        private void GroupbyGeneratorScheduler_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView cvsGeneratorRunningHours = CollectionViewSource
                                                       .GetDefaultView(this.dtgdGenSchdRemdrTable
                                                       .ItemsSource);
            if (cvsGeneratorRunningHours != null && cvsGeneratorRunningHours.CanGroup == true)
            {
                cvsGeneratorRunningHours.GroupDescriptions.Clear();
                cvsGeneratorRunningHours.GroupDescriptions
                                        .Add(new PropertyGroupDescription("GeneratorName"));
            }
        }

        private void ClearSchedulerReminderGrouping_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView cvsGeneratorRunningHours = CollectionViewSource
                                                       .GetDefaultView(this.dtgdGenSchdRemdrTable
                                                       .ItemsSource);
            if (cvsGeneratorRunningHours != null && cvsGeneratorRunningHours.CanGroup == true)
            {
                cvsGeneratorRunningHours.GroupDescriptions.Clear();
            }
        }
    }
}

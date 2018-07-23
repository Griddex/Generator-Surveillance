using Panel.BusinessLogic.MaintenanceLogic;
using Panel.Interfaces;
using Panel.ViewModels.SettingsViewModel;
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

namespace Panel.Views.SettingsView
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : Page, IView
    {
        public SettingsView(AuthoriserSettingsViewModel authoriserSettingsViewModel,
            ConsumptionSettingsViewModel consumptionSettingsViewModel,
            RemindersConfigViewModel remindersConfigViewModel)
        {
            InitializeComponent();

            this.grpbxFuelConsumption.DataContext =
                consumptionSettingsViewModel;

            this.grpbxAuthorisers.DataContext =
                authoriserSettingsViewModel;

            this.grpbxRemConfig.DataContext =
                remindersConfigViewModel;
        }

        private void GroupbyGeneratorConsumption_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView cvsGeneratorConsumption = CollectionViewSource
                                                      .GetDefaultView(this.dtgdGenFuelConsumptionTable
                                                                          .ItemsSource);
            if (cvsGeneratorConsumption != null && cvsGeneratorConsumption.CanGroup == true)
            {
                cvsGeneratorConsumption.GroupDescriptions.Clear();
                cvsGeneratorConsumption.GroupDescriptions
                                       .Add(new PropertyGroupDescription("GeneratorName"));
            }
        }

        private void ClearGeneratorGroupingConsumption_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView cvsGeneratorConsumption = CollectionViewSource
                                                      .GetDefaultView(this.dtgdGenFuelConsumptionTable
                                                                          .ItemsSource);
            if (cvsGeneratorConsumption != null && cvsGeneratorConsumption.CanGroup == true)
            {
                cvsGeneratorConsumption.GroupDescriptions.Clear();
            }
        }

        private void GroupbyGeneratorAuthorisers_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView cvsRemindersAuthorisers = CollectionViewSource
                                                      .GetDefaultView(this.dtgdAuthoriserTable
                                                                          .ItemsSource);
            if (cvsRemindersAuthorisers != null && cvsRemindersAuthorisers.CanGroup == true)
            {
                cvsRemindersAuthorisers.GroupDescriptions.Clear();
                cvsRemindersAuthorisers.GroupDescriptions
                                       .Add(new PropertyGroupDescription("GeneratorName"));
            }
        }

        private void ClearGeneratorGroupingAuthorisers_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView cvsRemindersAuthorisers = CollectionViewSource
                                                      .GetDefaultView(this.dtgdAuthoriserTable
                                                                          .ItemsSource);
            if (cvsRemindersAuthorisers != null && cvsRemindersAuthorisers.CanGroup == true)
            {
                cvsRemindersAuthorisers.GroupDescriptions.Clear();
            }
        }

        private void expdrAuthorisers_Expanded(object sender, RoutedEventArgs e)
        {
            this.dtgdAuthoriserTable.Items.Refresh();
            ICollectionView cvsGeneratorConsumption = CollectionViewSource
                                                    .GetDefaultView(this.dtgdAuthoriserTable
                                                                        .ItemsSource);
            cvsGeneratorConsumption.Refresh();
        }

        private void GroupbyGeneratorRemConfig_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView cvsRemConfig = CollectionViewSource
                                            .GetDefaultView(this.dtgdGenRemindersConfigTable
                                                                .ItemsSource);
            if (cvsRemConfig != null && cvsRemConfig.CanGroup == true)
            {
                cvsRemConfig.GroupDescriptions.Clear();
                cvsRemConfig.GroupDescriptions
                            .Add(new PropertyGroupDescription("GeneratorName"));
            }
        }

        private void ClearGeneratorGroupingRemConfig_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView cvsRemConfig = CollectionViewSource
                                            .GetDefaultView(this.dtgdGenRemindersConfigTable
                                                                .ItemsSource);
            if (cvsRemConfig != null && cvsRemConfig.CanGroup == true)
            {
                cvsRemConfig.GroupDescriptions.Clear();
            }
        }

        private void DeactivateGeneratorRemConfig_Click(object sender, RoutedEventArgs e)
        {
            var dataGridRowSelected = (dynamic)dtgdGenRemindersConfigTable.SelectedItem;
            string GeneratorName = dataGridRowSelected.GeneratorName;

            MessageBoxResult result = MessageBox.Show($"Do you want to deactivate {GeneratorName}?",
                                                    "Confirmation", MessageBoxButton.YesNoCancel);
            switch (result)
            {
                case MessageBoxResult.No:
                case MessageBoxResult.None:
                case MessageBoxResult.Cancel:
                    return;
                case MessageBoxResult.Yes:
                case MessageBoxResult.OK:
                    ScheduledRemindersMethods.DeactivateGenerator(GeneratorName);
                    break;
                default:
                    break;
            }

            this.dtgdGenRemindersConfigTable.ItemsSource = (this.grpbxRemConfig.DataContext as RemindersConfigViewModel)
                                                            .UnitOfWork
                                                            .GeneratorScheduler.GetActiveGeneratorSchedules();
            this.dtgdGenRemindersConfigTable.Items.Refresh();
            ICollectionView cvsGeneratorReminders = CollectionViewSource
                                                    .GetDefaultView(this.dtgdGenRemindersConfigTable
                                                                        .ItemsSource);
            cvsGeneratorReminders.Refresh();
        }
    }
}

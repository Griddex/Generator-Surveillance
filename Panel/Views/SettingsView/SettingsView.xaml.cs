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

            this.expdrfclSettings.DataContext =
                consumptionSettingsViewModel;

            this.expdrraSettings.DataContext =
                authoriserSettingsViewModel;

            this.expdrrcSettings.DataContext =
                remindersConfigViewModel;

            this.stcpnlfc1Settings.DataContext =
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

            this.dtgdGenRemindersConfigTable
                .ItemsSource = (this.grpbxRemConfig.DataContext as 
                                        RemindersConfigViewModel)
                                        .UnitOfWork
                                        .GeneratorScheduler
                                        .GetActiveGeneratorSchedules();

            this.dtgdGenRemindersConfigTable.Items.Refresh();
            ICollectionView cvsGeneratorReminders = CollectionViewSource
                                                    .GetDefaultView(this.dtgdGenRemindersConfigTable
                                                                        .ItemsSource);
            cvsGeneratorReminders.Refresh();
        }

        private void FuelComp_Expanded(object sender, RoutedEventArgs e)
        {
            this.vwbxSettings.Margin = new Thickness(0, 0, 0, 0);
            this.vwbxSettings.Height = 820;
            this.expdrfclSettings.Height = 820;

            this.expdrraSettings.IsExpanded = false;
            this.expdrrcSettings.IsExpanded = false;
        }

        private void FuelComp_Collapsed(object sender, RoutedEventArgs e)
        {
            this.expdrfclSettings.Height = 300;
            CollapseAllExpanders();
        }

        private void expdrAuthorisers_Expanded(object sender, RoutedEventArgs e)
        {
            this.vwbxSettings.Margin = new Thickness(0, 0, 0, 0);
            this.vwbxSettings.Height = 820;
            this.expdrraSettings.Height = 820;

            this.expdrfclSettings.IsExpanded = false;
            this.expdrrcSettings.IsExpanded = false;
        }

        private void Authorisers_Collapsed(object sender, RoutedEventArgs e)
        {
            this.expdrraSettings.Height = 300;
            CollapseAllExpanders();
        }

        private void expdrReminders_Expanded(object sender, RoutedEventArgs e)
        {
            this.vwbxSettings.Margin = new Thickness(0, 0, 0, 0);
            this.vwbxSettings.Height = 820;
            this.expdrrcSettings.Height = 820;

            this.expdrfclSettings.IsExpanded = false;
            this.expdrraSettings.IsExpanded = false;
        }

        private void Reminders_Collapsed(object sender, RoutedEventArgs e)
        {
            this.expdrrcSettings.Height = 300;
            CollapseAllExpanders();
        }

        private void CollapseAllExpanders()
        {
            if(this.expdrfclSettings.IsExpanded == false ||
                this.expdrraSettings.IsExpanded == false ||
                this.expdrrcSettings.IsExpanded == false)
            {
                this.vwbxSettings.Margin = new Thickness(0, 300, 0, 0);
                this.vwbxSettings.Height = 300;
                this.expdrfclSettings.Height = 300;
                this.expdrraSettings.Height = 300;
                this.expdrrcSettings.Height = 300;

            }
        }

        private void expdrraSettings_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                this.expdrfclSettings.IsExpanded = false;
                this.expdrraSettings.IsExpanded = false;
                this.expdrrcSettings.IsExpanded = false;
                this.stckpnlPassword.Visibility = Visibility.Visible;
            }                
        }

        private void expdrrcSettings_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.expdrfclSettings.IsExpanded = false;
                this.expdrraSettings.IsExpanded = false;
                this.expdrrcSettings.IsExpanded = false;
                this.stckpnlPassword.Visibility = Visibility.Visible;
            }                
        }
    }
}

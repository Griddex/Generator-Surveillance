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
            ConsumptionSettingsViewModel consumptionSettingsViewModel)
        {
            InitializeComponent();

            this.grpbxFuelConsumption.DataContext =
                consumptionSettingsViewModel;

            this.grpbxAuthorisers.DataContext =
                authoriserSettingsViewModel;            
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

        private void ClearUsageGroupingConsumption_Click(object sender, RoutedEventArgs e)
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
    }
}

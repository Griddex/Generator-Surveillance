using Panel.Interfaces;
using Panel.ViewModels.InputViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Panel.Views.InputViews
{
    /// <summary>
    /// Interaction logic for Fuelling.xaml
    /// </summary>
    public partial class FuellingView : Page, IView
    {
        public FuellingView(FuellingViewModel fuellingViewModel)
        {
            InitializeComponent();
            this.DataContext = fuellingViewModel;
        }

        private void GroupbyGenerator_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView cvsGeneratorConsumption = CollectionViewSource
                                                      .GetDefaultView(this.dtgdGenFuelConsumptionTable
                                                                          .ItemsSource);
            if (cvsGeneratorConsumption != null && cvsGeneratorConsumption
                                                    .CanGroup == true)
            {
                cvsGeneratorConsumption.GroupDescriptions.Clear();
                cvsGeneratorConsumption.GroupDescriptions.Add(
                    new PropertyGroupDescription("Generator"));
            }            
        }

        private void ClearGeneratorGrouping_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView cvsGeneratorConsumption = CollectionViewSource
                                                      .GetDefaultView(this.dtgdGenFuelConsumptionTable
                                                                          .ItemsSource);
            if (cvsGeneratorConsumption != null && cvsGeneratorConsumption.CanGroup == true)
            {
                cvsGeneratorConsumption.GroupDescriptions.Clear();
            }
        }

        private void expdrFuelConsumption_Expanded(object sender, RoutedEventArgs e)
        {
            this.dtgdGenFuelConsumptionTable.Items.Refresh();
            ICollectionView cvsGeneratorConsumption = CollectionViewSource.GetDefaultView(this.dtgdGenFuelConsumptionTable.ItemsSource);
            cvsGeneratorConsumption.Refresh();
        }
    }
}

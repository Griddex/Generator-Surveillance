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
        public FuellingViewModel FuellingViewModel { get; set; }
        public FuellingView(FuellingViewModel fuellingViewModel)
        {
            InitializeComponent();
            this.DataContext = fuellingViewModel;
            this.FuellingViewModel = fuellingViewModel;
            this.Loaded += FuellingView_Loaded;
        }

        private void FuellingView_Loaded(object sender, RoutedEventArgs e)
        {
            FuellingViewModel.RefreshFuelCompCmd.Execute(null);
        }

        private void GroupbyGenerator_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView cvsGeneratorConsumption = CollectionViewSource
                .GetDefaultView(this.dtgdGenFuelConsumptionTable.ItemsSource);

            if (cvsGeneratorConsumption != null && cvsGeneratorConsumption.CanGroup == true)
            {
                cvsGeneratorConsumption.GroupDescriptions.Clear();
                cvsGeneratorConsumption.GroupDescriptions.Add(
                    new PropertyGroupDescription("Generator"));
            }            
        }

        private void ClearGeneratorGrouping_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView cvsGeneratorConsumption = CollectionViewSource
                .GetDefaultView(this.dtgdGenFuelConsumptionTable.ItemsSource);

            if (cvsGeneratorConsumption != null && cvsGeneratorConsumption.CanGroup == true)
            {
                cvsGeneratorConsumption.GroupDescriptions.Clear();
            }
        }

        private void expdrFuelConsumption_Expanded(object sender, RoutedEventArgs e)
        {
            this.dtgdGenFuelConsumptionTable.Items.Refresh();
            ICollectionView cvsGeneratorConsumption = CollectionViewSource
                .GetDefaultView(this.dtgdGenFuelConsumptionTable.ItemsSource);

            cvsGeneratorConsumption.Refresh();
        }
    }
}

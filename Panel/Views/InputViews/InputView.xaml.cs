using System;
using System.Collections.Generic;
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
using Panel.Interfaces;
using Panel.Repositories;
using Panel.ViewModels;
using Panel.ViewModels.InputViewModels;
using Unity;

namespace Panel.Views.InputViews
{
    /// <summary>
    /// Interaction logic for InputView.xaml
    /// </summary>
    public partial class InputView : Page, IView
    {
        private UnitOfWork _unitOfWork;
                
        public InputView(UnitOfWork unitOfWork, InputViewModel inputViewModel)
        {
            InitializeComponent();
                        
            this._unitOfWork = unitOfWork;
            this.DataContext = inputViewModel;
            this.cmbxGenInfo.SelectedIndex = 0;
            this.Loaded += InputView_Loaded;            
        }

        private void InputView_Loaded(object sender, RoutedEventArgs e)
        {
            var inputViewModel = this.DataContext as InputViewModel;
            if (inputViewModel.IsNull)
                inputViewModel.LoadLastGeneratorRecord
                    .Execute(new Tuple<DatePicker, ComboBox>
                    (this.dtepkrGenInfo,
                    this.cmbxGenInfo));
        }
    }
}

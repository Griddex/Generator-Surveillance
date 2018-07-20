using Panel.Commands;
using Panel.Models.InputModels;
using Panel.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Panel.ViewModels.SettingsViewModel
{
    public class AuthoriserSettingsViewModel
    {
        public UnitOfWork UnitOfWork { get; set; }
        public DateTime ReminderDate { get; set; } = DateTime.Now;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string JobTitle { get; set; }

        public AuthoriserSettingsViewModel(UnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        private ICommand _setAuthoriserCmd;
        public ICommand SetAuthoriserCmd
        {
            get
            {
                return this._setAuthoriserCmd ??
                (
                    this._setAuthoriserCmd = new DelegateCommand
                    (
                        x =>
                        {
                            DataGrid dtgrd = (DataGrid)x;
                            if (FirstName == null || LastName == null || Email == null
                            || PhoneNumber == null || JobTitle == null)
                            {
                                MessageBox.Show($"All details for the authoriser must " +
                                    $"be set to a valid value",
                                    "Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                                return;
                            }

                            UnitOfWork.AuthoriserSetting.SetAuthorisers(ReminderDate, FirstName, 
                                LastName, Email, PhoneNumber, JobTitle);

                            int Success = UnitOfWork.Complete();
                            if (Success > 0)
                            {
                                dtgrd.ItemsSource = UnitOfWork.ConsumptionSetting.GetAnyConsumptionPage();
                                dtgrd.Items.Refresh();
                                ICollectionView cvsFuelConsumption = CollectionViewSource
                                                                    .GetDefaultView(dtgrd.ItemsSource);
                                cvsFuelConsumption.Refresh();
                            }
                        },
                        y => true
                    )
                );
            }
        }
    }
}

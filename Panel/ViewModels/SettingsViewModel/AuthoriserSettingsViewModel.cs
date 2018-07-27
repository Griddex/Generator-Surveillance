using Panel.Commands;
using Panel.Interfaces;
using Panel.Repositories;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Panel.ViewModels.SettingsViewModel
{
    public class AuthoriserSettingsViewModel : ViewModelBase, IViewModel
    {

        public ObservableCollection<AuthoriserSetting> 
            AnyAuthorisersPage { get; set; } =
            new ObservableCollection<AuthoriserSetting>();

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

            AnyAuthorisersPage = UnitOfWork.AuthoriserSetting
                                           .GetAllAuthorisers();
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
                            if (FirstName == null || LastName == null ||
                                Email == null || PhoneNumber == null ||
                                JobTitle == null)
                            {
                                MessageBox.Show($"All details for the " +
                                    $"authoriser must " +
                                    $"be set to a valid value",
                                    "Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                                return;
                            }

                            UnitOfWork.AuthoriserSetting.SetAuthorisers(
                                ReminderDate, FirstName, 
                                LastName, Email, PhoneNumber, 
                                JobTitle);

                            int Success = UnitOfWork.Complete();
                            if (Success > 0)
                            {
                                AnyAuthorisersPage = UnitOfWork.AuthoriserSetting
                                                               .GetAllAuthorisers();
                                OnPropertyChanged(nameof(AnyAuthorisersPage));
                            }
                        },
                        y => true
                    )
                );
            }
        }
    }
}

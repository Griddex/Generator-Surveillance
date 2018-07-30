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

        public ObservableCollection<ActionPartySetting>
            AnyActionPartiesPage { get; set; } =
            new ObservableCollection<ActionPartySetting>();

        public UnitOfWork UnitOfWork { get; set; }
        public DateTime ReminderDateAuthoriser { get; set; } = DateTime.Now;
        public string FirstNameAuthoriser { get; set; }
        public string LastNameAuthoriser { get; set; }
        public string EmailAuthoriser { get; set; }
        public string PhoneNumberAuthoriser { get; set; }
        public string JobTitleAuthoriser { get; set; }

        public DateTime ReminderDateActionParty { get; set; } = DateTime.Now;
        public string FirstNameActionParty { get; set; }
        public string LastNameActionParty { get; set; }
        public string EmailActionParty { get; set; }
        public string PhoneNumberActionParty { get; set; }
        public string JobTitleActionParty { get; set; }

        public AuthoriserSettingsViewModel(UnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;

            AnyAuthorisersPage = UnitOfWork.AuthoriserSetting
                                           .GetAllAuthorisers();

            AnyActionPartiesPage = UnitOfWork.ActionPartySetting
                                            .GetAllActionParties();
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
                            if (FirstNameAuthoriser == null || LastNameAuthoriser == null ||
                                EmailAuthoriser == null || PhoneNumberAuthoriser == null ||
                                JobTitleAuthoriser == null)
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
                                ReminderDateAuthoriser, FirstNameAuthoriser, 
                                LastNameAuthoriser, EmailAuthoriser, PhoneNumberAuthoriser, 
                                JobTitleAuthoriser);

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

        private ICommand _setActionPartyCmd;
        public ICommand SetActionPartyCmd
        {
            get
            {
                return this._setActionPartyCmd ??
                (
                    this._setActionPartyCmd = new DelegateCommand
                    (
                        x =>
                        {
                            DataGrid dtgrd = (DataGrid)x;
                            if (FirstNameActionParty == null || LastNameActionParty == null ||
                                EmailActionParty == null || PhoneNumberActionParty == null ||
                                JobTitleActionParty == null)
                            {
                                MessageBox.Show($"All details for the " +
                                    $"ActionParty must " +
                                    $"be set to a valid value",
                                    "Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                                return;
                            }

                            UnitOfWork.ActionPartySetting.SetActionParties(
                                ReminderDateActionParty, FirstNameActionParty,
                                LastNameActionParty, EmailActionParty, 
                                PhoneNumberActionParty,
                                JobTitleActionParty);

                            int Success = UnitOfWork.Complete();
                            if (Success > 0)
                            {
                                AnyActionPartiesPage = UnitOfWork.ActionPartySetting
                                                               .GetAllActionParties();
                                OnPropertyChanged(nameof(AnyActionPartiesPage));
                            }
                        },
                        y => true
                    )
                );
            }
        }
    }
}

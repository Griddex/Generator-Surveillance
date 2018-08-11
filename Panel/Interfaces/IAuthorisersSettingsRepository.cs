using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Panel.Interfaces
{
    public interface IAuthorisersSettingsRepository
    {
        void SetAuthorisers(DateTime ReminderDate, string FirstName,
            string LastName, string Email, string PhoneNumber,
            string JobTitle);

        void DeleteAuthoriser(string FirstName, string LastName);

        ObservableCollection<AuthoriserSetting> GetAllAuthorisers();

        List<string> GetAuthorisersFullNames();

        List<string> GetAuthorisersEmails();
    }
}

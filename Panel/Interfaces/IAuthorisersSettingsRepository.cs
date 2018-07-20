using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Interfaces
{
    public interface IAuthorisersSettingsRepository
    {
        void SetAuthorisers(DateTime ReminderDate, string FirstName,
            string LastName, string Email, string PhoneNumber,
            string JobTitle);

        ObservableCollection<AuthoriserSetting> GetAllAuthorisers();
    }
}

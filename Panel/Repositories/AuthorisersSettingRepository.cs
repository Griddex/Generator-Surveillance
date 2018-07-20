using Panel.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Repositories
{
    public class AuthorisersSettingRepository : Repository<GeneratorFuelling>,
        IAuthorisersSettingsRepository
    {
        public AuthorisersSettingRepository(GeneratorSurveillanceDBEntities context)
            : base(context) { }

        public GeneratorSurveillanceDBEntities GeneratorSurveillanceDBContext
        {
            get { return Context as GeneratorSurveillanceDBEntities; }
        }

        public void SetAuthorisers(DateTime ReminderDate, string FirstName,
            string LastName, string Email, string PhoneNumber,
            string JobTitle)
        {
            int NoOfRecords = GeneratorSurveillanceDBContext.AuthoriserSettings.Count();
            GeneratorSurveillanceDBContext.AuthoriserSettings.Add
            (
                new AuthoriserSetting
                {
                    Id = NoOfRecords + 1,
                    Date = ReminderDate,
                    FirstName = FirstName,
                    LastName = LastName,
                    PhoneNumber = PhoneNumber,
                    JobTitle = JobTitle
                }
            );
        }
        
        public ObservableCollection<AuthoriserSetting> GetAllAuthorisers()
        {
            return new ObservableCollection<AuthoriserSetting>
                (
                    GeneratorSurveillanceDBContext.AuthoriserSettings
                    .AsParallel()
                );
        }
    }
}

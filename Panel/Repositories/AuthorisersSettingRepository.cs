using Panel.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Repositories
{
    public class AuthorisersSettingRepository : Repository<AuthoriserSetting>,
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
            int NoOfRecords = GeneratorSurveillanceDBContext
                                            .AuthoriserSettings
                                            .Count();

            GeneratorSurveillanceDBContext.AuthoriserSettings.Add
            (
                new AuthoriserSetting
                {
                    Id = NoOfRecords,
                    Date = ReminderDate,
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
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

        public List<string> GetAuthorisersFullNames()
        {
            var FullNameList = new List<string>();
            foreach (var authoriser in GeneratorSurveillanceDBContext
                                        .AuthoriserSettings)
            {
                FullNameList.Add($"{authoriser.FirstName} {authoriser.LastName}");
            }
            return FullNameList;
        }
    }
}

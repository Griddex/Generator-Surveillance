using Panel.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Panel.Repositories
{
    public class ActionPartySettingRepository : Repository<ActionPartySetting>,
        IActionPartySettingsRepository
    {
        public ActionPartySettingRepository(GeneratorSurveillanceDBEntities context)
            : base(context) { }

        public GeneratorSurveillanceDBEntities GeneratorSurveillanceDBContext
        {
            get { return Context as GeneratorSurveillanceDBEntities; }
        }

        public void SetActionParties(DateTime ReminderDate, string FirstName,
            string LastName, string Email, string PhoneNumber,
            string JobTitle)
        {
            int NoOfRecords = GeneratorSurveillanceDBContext
                                            .ActionPartySettings
                                            .Count();

            GeneratorSurveillanceDBContext.ActionPartySettings.Add
            (
                new ActionPartySetting
                {
                    Id = NoOfRecords,
                    Date = ReminderDate,
                    FirstNameActionParty = FirstName,
                    LastNameActionParty = LastName,
                    EmailActionParty = Email,
                    PhoneNumberActionParty = PhoneNumber,
                    JobTitleActionParty = JobTitle
                }
            );
        }
        
        public ObservableCollection<ActionPartySetting> GetAllActionParties()
        {
            return new ObservableCollection<ActionPartySetting>
                (
                    GeneratorSurveillanceDBContext
                    .ActionPartySettings
                    .AsParallel()
                );
        }

        public List<string> GetActionPartiesFullNames()
        {
            var FullNameList = new List<string>();
            foreach (var ActionParty in GeneratorSurveillanceDBContext
                                       .ActionPartySettings)
            {
                FullNameList.Add($"{ActionParty.FirstNameActionParty} " +
                    $"{ActionParty.LastNameActionParty}");
            }
            return FullNameList;
        }

        public List<string> GetActionPartiesEmails()
        {
            var EmailsList = new List<string>();
            foreach (var ActionParty in GeneratorSurveillanceDBContext
                                       .ActionPartySettings)
            {
                EmailsList.Add(ActionParty.EmailActionParty);
            }
            return EmailsList.Distinct().ToList();
        }
    }
}

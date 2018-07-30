using Panel.Repositories;
using System.Collections.Generic;
using System.Windows;
using Unity;

namespace Panel.Services.MessagingServices
{
    public class ExtractEmails
    {
        public static UnityContainer container =
                (UnityContainer)Application.Current.Resources["UnityIoC"];

        public List<string> GetEmails()
        {
            var gse = container.Resolve<GeneratorSurveillanceDBEntities>();
            var apsr = new ActionPartySettingRepository(gse);
            var asr = new AuthorisersSettingRepository(gse);

            var apsrList = apsr.GetActionPartiesEmails();
            var asrList = asr.GetAuthorisersEmails();
            var fullList = new List<string>(apsrList.Count + asrList.Count);
            fullList.AddRange(apsrList);
            fullList.AddRange(asrList);

            return fullList;               
        }
    }
}

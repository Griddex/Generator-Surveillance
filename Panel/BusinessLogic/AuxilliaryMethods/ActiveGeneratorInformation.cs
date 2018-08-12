using Panel.Repositories;
using System;
using System.Windows;
using Unity;

namespace Panel.BusinessLogic.AuxilliaryMethods
{
    public static class ActiveGeneratorInformation
    {
        static UnityContainer container = (UnityContainer)Application
                                          .Current.Resources["UnityIoC"];

        public static (bool IsGenActive, string ActiveGenName, DateTime? ActiveGenStartedDate,
            DateTime? ActiveGenStartedTime, int ActiveGenID) GetActiveGeneratorInformation()
        {
            var gse = container.Resolve<GeneratorSurveillanceDBEntities>();
            var gir = new GeneratorInformationRepository(gse);

            return gir.GeneratorStoppedIsGenActive();
        }
    }
}

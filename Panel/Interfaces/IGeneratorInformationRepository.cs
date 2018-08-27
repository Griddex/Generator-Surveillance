using Panel.Models.InputModels;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Panel.Interfaces
{
    public interface IGeneratorInformationRepository : IRepository<GeneratorUsage>
    {
        (bool IsGenActive, string ActiveGenName, DateTime? ActiveGenStartedDate, 
            DateTime? ActiveGenStartedTime, int ActiveGenID) GeneratorStoppedIsGenActive();

        ObservableCollection<GeneratorNameModel> GetUniqueGeneratorNames();

        void AddGeneratorName(String GeneratorName, ObservableCollection<GeneratorNameModel> GeneratorNames,
            ComboBox comboBox);

        void ArchiveGeneratorName(String GeneratorName, ObservableCollection<GeneratorNameModel> uniqueGeneratorNames, 
            ComboBox cmbxGenInfo);
    }
}

using Panel.Models.InputModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Panel.Interfaces
{
    public interface IGeneratorInformationRepository : IRepository<GeneratorUsage>
    {
        (bool IsNull, string LastGenName, DateTime? LastGenStartedDate, DateTime? LastGenStartedTime) GeneratorStoppedIsNull();

        ObservableCollection<GeneratorNameModel> GetUniqueGeneratorNames();

        void AddGeneratorName(String GeneratorName, ObservableCollection<GeneratorNameModel> GeneratorNames,
            ComboBox comboBox);

        void ArchiveGeneratorName(String GeneratorName, ObservableCollection<GeneratorNameModel> uniqueGeneratorNames, 
            ComboBox cmbxGenInfo);
    }
}

using Panel.Interfaces;
using Panel.Models.InputModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace Panel.Repositories
{
    public class GeneratorInformationRepository : Repository<GeneratorUsage>, IGeneratorInformationRepository
    {
        public GeneratorInformationRepository(GeneratorSurveillanceDBEntities context) : base(context)
        {
            
        }
                
        public GeneratorSurveillanceDBEntities GeneratorSurveillanceDBContext
        {
            get { return Context as GeneratorSurveillanceDBEntities; }
        }

        public (bool IsNull, string LastGenName, DateTime? LastGenStartedDate, DateTime? LastGenStartedTime) GeneratorStoppedIsNull()
        {
            int NoOfRecords = GeneratorSurveillanceDBContext.GeneratorUsages.Count();
            bool isNull = GeneratorSurveillanceDBContext.GeneratorUsages
                                                       .SingleOrDefault(x => x.Id == NoOfRecords)
                                                       .GeneratorStopped.Year.ToString() == "1" ? true : false;

            string lastGenName = GeneratorSurveillanceDBContext.GeneratorUsages
                                                               .SingleOrDefault(x => x.Id == NoOfRecords - 1)
                                                               .GeneratorName;

            DateTime? lastGenStartedDate = GeneratorSurveillanceDBContext.GeneratorUsages
                                                                        .SingleOrDefault(x => x.Id == NoOfRecords - 1)
                                                                        .Date;

            DateTime? lastGenStartedTime = GeneratorSurveillanceDBContext.GeneratorUsages
                                                                       .SingleOrDefault(x => x.Id == NoOfRecords - 1)
                                                                       .GeneratorStarted;

            return (isNull, lastGenName, lastGenStartedDate, lastGenStartedTime);
        }
        

        public ObservableCollection<GeneratorNameModel> GetUniqueGeneratorNames()
        {
            return new ObservableCollection<GeneratorNameModel>
                (
                      GeneratorSurveillanceDBContext.GeneratorUsages
                     .Where(x => x.Id >= 1 && x.IsArchived != "Yes")
                     .OrderBy(x => x.GeneratorName)
                     .Select(x => new GeneratorNameModel
                        {
                            GeneratorName = x.GeneratorName
                        })
                     .Distinct()
                 );
        }

        public void AddGeneratorName(string GeneratorName, ObservableCollection<GeneratorNameModel> uniqueGeneratorNames,
            ComboBox cmbxGenInfo)
        {
            if (!uniqueGeneratorNames.Where(x => x.GeneratorName == GeneratorName).Any())
            {
                uniqueGeneratorNames.Add(new GeneratorNameModel { GeneratorName = GeneratorName });
                cmbxGenInfo.ItemsSource = uniqueGeneratorNames;
                cmbxGenInfo.SelectedIndex = cmbxGenInfo.Items.Count;
                MessageBox.Show($"{GeneratorName} added!", "Success", MessageBoxButton.OKCancel, 
                    MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"{GeneratorName} already exists", "Error", MessageBoxButton.OKCancel, 
                    MessageBoxImage.Error);
                return;
            }                          
        }

        public void ArchiveGeneratorName(string GeneratorName, ObservableCollection<GeneratorNameModel> uniqueGeneratorNames, 
            ComboBox cmbxGenInfo)
        {
            foreach (GeneratorUsage gu in GeneratorSurveillanceDBContext.GeneratorUsages)
            {
                if (gu.GeneratorName == GeneratorName)
                    gu.IsArchived = "Yes";
            }
            uniqueGeneratorNames.Remove((GeneratorNameModel)cmbxGenInfo.SelectedItem);
            cmbxGenInfo.ItemsSource = uniqueGeneratorNames;
        }
    }
}

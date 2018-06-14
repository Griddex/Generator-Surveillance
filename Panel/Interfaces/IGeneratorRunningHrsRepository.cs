using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Interfaces
{
    public interface IGeneratorRunningHrsRepository : IRepository<GeneratorRunningHr>
    {
        ObservableCollection<GeneratorRunningHr> GetAllRunningHours();
    }
}

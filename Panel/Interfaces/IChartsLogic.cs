using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Interfaces
{
    public interface IChartsLogic
    {
        IEnumerable<string> GetInterveningDates(DateTime startdate, DateTime stopdate, string generatorname, string dateperspective);
    }
}

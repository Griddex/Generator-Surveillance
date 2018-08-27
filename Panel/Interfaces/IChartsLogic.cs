using System;
using System.Collections.Generic;

namespace Panel.Interfaces
{
    public interface IChartsLogic
    {
        IEnumerable<string> GetInterveningDates(DateTime startdate, DateTime stopdate, string generatorname, string dateperspective);
    }
}

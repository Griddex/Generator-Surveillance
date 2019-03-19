using Panel.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data.Entity;

namespace Panel.Repositories
{
    public class GeneratorRunningHrsRepository : Repository<GeneratorRunningHr>, IGeneratorRunningHrsRepository
    {
        public GeneratorRunningHrsRepository(GeneratorSurveillanceDBEntities context) 
            : base(context) { }

        public GeneratorSurveillanceDBEntities GeneratorSurveillanceDBContext
        {
            get { return Context as GeneratorSurveillanceDBEntities; }
        }
        
        public ObservableCollection<GeneratorRunningHr> GetAllRunningHours()
        {
            var AllGeneratorRunningHrs = new ObservableCollection<GeneratorRunningHr>
                                            (
                                                GeneratorSurveillanceDBContext
                                                .GeneratorRunningHrs
                                                .AsParallel<GeneratorRunningHr>()
                                            );
            return AllGeneratorRunningHrs;
        }

        public ObservableCollection<GeneratorRunningHr> GetAnyPageGeneratorRunningHrs(
            int pageIndex = 1, int pageSize = 10)
        {
            int RecordNo = GeneratorSurveillanceDBContext.GeneratorRunningHrs.Count();
            var NextPageLastRowNumber = pageIndex * pageSize;
            int SkipBy = (pageIndex == 1) ? (pageIndex - 1) * pageSize
                                          : ((pageIndex - 1) * pageSize) - 1;
            if ((RecordNo - NextPageLastRowNumber) > pageSize)
            {
                return new ObservableCollection<GeneratorRunningHr>
                        (
                            GeneratorSurveillanceDBContext.GeneratorRunningHrs
                            .OrderBy(x => x.SN)
                            .Skip(SkipBy)
                            .Take(pageSize)
                            .AsParallel<GeneratorRunningHr>()
                        );

            }
            else
            {
                return new ObservableCollection<GeneratorRunningHr>
                        (
                            GeneratorSurveillanceDBContext.GeneratorRunningHrs
                            .OrderBy(x => x.SN)
                            .Skip(SkipBy)
                            .Take(pageSize)
                            .AsParallel<GeneratorRunningHr>()
                        );
            }
        }

        public void Delete(List<GeneratorRunningHr> RowsToDelete)
        {
            base.Delete(RowsToDelete);
        }

        public void DeleteAllRows()
        {
            base.DeleteAll();
        }  
    }
}

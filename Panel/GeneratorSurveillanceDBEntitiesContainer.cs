using System.Data.Entity;

namespace Panel
{
    public partial class GeneratorSurveillanceDBEntities : DbContext
    {
        public GeneratorSurveillanceDBEntities(string connectionString)
            : base(connectionString)
        { }
    }
}

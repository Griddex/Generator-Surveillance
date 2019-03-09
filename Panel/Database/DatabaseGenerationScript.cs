using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Database
{
    public class DatabaseGenerationScript
    {
        public static string GetDatabaseGenerationScript()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            const string DatabaseScript = "Panel.Database.GenerateDatabaseScript.sql";

            using (Stream stream = assembly.GetManifestResourceStream(DatabaseScript))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}

using System.IO;
using System.Reflection;

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

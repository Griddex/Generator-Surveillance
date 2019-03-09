using System.IO;
using System.Reflection;

namespace Panel.Database
{
    public static class TablesDefinitionScript
    {
        public static string GetTablesDefinitionScript()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            const string TablesScript = "Panel.Database.DefineTablesScript.sql";

            using(Stream stream = assembly.GetManifestResourceStream(TablesScript))
            {
                using(StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}

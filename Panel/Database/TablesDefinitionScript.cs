using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

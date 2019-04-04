using Panel.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Windows;

namespace Panel.Startup
{
    public class InitialiseDatabase
    {
        
        public static GeneratorSurveillanceDBEntities ConnectToDatabase()
        {
            string LocalDBFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +
                @"\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB";

            if (!Directory.Exists(LocalDBFolder))
            {
                MessageBox.Show("SQL LocalDB Server is not installed on your computer" +
                    Environment.NewLine + Environment.NewLine +
                    "To install, please visit:" + Environment.NewLine +
                    "https://www.microsoft.com/en-us/download/details.aspx?id=29062",
                    "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Shutdown();
            }

            string CommonAppFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string DBFolder = CommonAppFolder + @"\GeneratorSurveillance";
            DirectoryInfo info = Directory.CreateDirectory(DBFolder);

            string DbNameMDFPath = DBFolder + @"\GeneratorSurveillanceDB.mdf";
            string DbNameLDFPath = DBFolder + @"\GeneratorSurveillanceDB.ldf";
            string providerName = @"System.Data.SqlClient";
            string serverName = @"(LocalDB)\MSSQLLocalDB";
            string ScriptText = DatabaseGenerationScript.GetDatabaseGenerationScript()
                    .Replace("DB_NAME_MDF", DbNameMDFPath)
                    .Replace("DB_NAME_LDF", DbNameLDFPath);

            if (!File.Exists(DbNameMDFPath))
            {
                //Construct Database                
                SqlConnectionStringBuilder CreateDBSQLBuilder = new SqlConnectionStringBuilder
                {
                    DataSource = serverName,
                    InitialCatalog = "master",
                    IntegratedSecurity = true,
                    MultipleActiveResultSets = true,
                    ConnectTimeout = 30
                };
                string CreateDBProviderString = CreateDBSQLBuilder.ToString();

                using (SqlConnection conn = new SqlConnection(CreateDBProviderString))
                {
                    conn.Open();
                    using (var dropDB = new SqlCommand("DROP DATABASE [GeneratorSurveillanceDB]", conn))
                    {
                        try
                        {
                            dropDB.ExecuteNonQuery();
                        }
                        catch (Exception) { }
                    }

                    IEnumerable<string> cmdStrs = Regex.Split(ScriptText, @"^\s*GO\s*$",
                    RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    foreach (string s in cmdStrs)
                    {
                        if (s.Trim() != "")
                        {
                            using (var cmd = new SqlCommand(s, conn))
                            {
                                try
                                {
                                    cmd.ExecuteNonQuery();
                                }
                                catch (SqlException ex)
                                {
                                    string sperror = s.Length > 100 ? s.Substring(0, 100) + " ...\n..." : s;
                                    MessageBox.Show(string.Format($@"Could not add tables to database.
                                                \nPlease run as administrator and restart program
                                                \n{ex.Message}"));
                                    Application.Current.Shutdown();
                                }
                            }
                        }
                    }
                }

                //Add full control permission to database for current windows user
                try
                {
                    DirectoryInfo dInfoMDF = new DirectoryInfo(DbNameMDFPath);
                    DirectorySecurity dSecurityMDF = dInfoMDF.GetAccessControl();
                    dSecurityMDF.AddAccessRule(new FileSystemAccessRule(
                        new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                        FileSystemRights.FullControl,
                        InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                        PropagationFlags.NoPropagateInherit,
                        AccessControlType.Allow));
                    dInfoMDF.SetAccessControl(dSecurityMDF);

                    DirectoryInfo dInfoLDF = new DirectoryInfo(DbNameLDFPath);
                    DirectorySecurity dSecurityLDF = dInfoLDF.GetAccessControl();
                    dSecurityLDF.AddAccessRule(new FileSystemAccessRule(
                        new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                        FileSystemRights.FullControl,
                        InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                        PropagationFlags.NoPropagateInherit,
                        AccessControlType.Allow));
                    dInfoLDF.SetAccessControl(dSecurityLDF);
                }
                catch (Exception) { }


                //Add Tables to database
                SqlConnectionStringBuilder DefineTablesSQLBuilder = new SqlConnectionStringBuilder
                {
                    DataSource = serverName,
                    InitialCatalog = "GeneratorSurveillanceDB",
                    IntegratedSecurity = true,
                    MultipleActiveResultSets = true
                };
                string DefineTablesProviderString = DefineTablesSQLBuilder.ToString();
                string TablesScript = TablesDefinitionScript.GetTablesDefinitionScript()
                    .Replace("GO", "");
                using (SqlConnection tableconn = new SqlConnection(DefineTablesProviderString))
                {
                    SqlCommand createtables = new SqlCommand(TablesScript, tableconn);
                    tableconn.Open();
                    createtables.ExecuteNonQuery();
                }
            }

            //Connect to database via EF
            SqlConnectionStringBuilder ConnectToDBSQLBuilder = new SqlConnectionStringBuilder
            {
                DataSource = serverName,
                InitialCatalog = "GeneratorSurveillanceDB",
                IntegratedSecurity = true,
                MultipleActiveResultSets = true
            };
            string ConnectToDBProviderString = ConnectToDBSQLBuilder.ToString();

            EntityConnectionStringBuilder EntityCSB = new EntityConnectionStringBuilder
            {
                Metadata = @"res://*/GeneratorSurveillanceDBEntities.csdl|res://*/GeneratorSurveillanceDBEntities.ssdl|res://*/GeneratorSurveillanceDBEntities.msl",
                Provider = providerName,
                ProviderConnectionString = ConnectToDBProviderString
            };
            String entityConnStr = EntityCSB.ToString();

            return new GeneratorSurveillanceDBEntities(entityConnStr);
        }
    }
}

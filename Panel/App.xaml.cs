using Panel.BusinessLogic;
using Panel.Database;
using Panel.Interfaces;
using Panel.Repositories;
using Panel.Services.MessagingServices;
using Panel.ViewModels.ChartViewModels;
using Panel.ViewModels.HelpViewModels;
using Panel.ViewModels.InputViewModels;
using Panel.ViewModels.ReportViewModels;
using Panel.ViewModels.SettingsViewModel;
using Panel.ViewModels.TableViewModels;
using Panel.Views.ChartViews;
using Panel.Views.HelpViews;
using Panel.Views.InputViews;
using Panel.Views.ReportViews;
using Panel.Views.SettingsView;
using Panel.Views.TableViews;
using System;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Data;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Panel
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() { }

        public GeneratorSurveillanceDBEntities generatorSurveillanceDBEntities { get; set; }

        private  void Application_Startup(object sender, StartupEventArgs e)
        {

            string CommonAppFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string DBFolder = CommonAppFolder + @"\GeneratorSurveillance";
            DirectoryInfo info =  Directory.CreateDirectory(DBFolder);

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
                        catch (Exception ex) { }                        
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

            generatorSurveillanceDBEntities = new GeneratorSurveillanceDBEntities(entityConnStr);
            InitialiseAllSystems();
        }


        private void InitialiseAllSystems()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterInstance<GeneratorSurveillanceDBEntities>(generatorSurveillanceDBEntities);            

            container.RegisterType<IUnitOfWork, UnitOfWork>("UnitOfWork",
                                                        new ContainerControlledLifetimeManager(),
                                                        new InjectionConstructor
                                                        (
                                                            typeof(GeneratorSurveillanceDBEntities)
                                                        ));

            container.RegisterType<IChartsLogic, ExtrudeInterveningDates>("ExtrudeInterveningDates",
                                                        new ContainerControlledLifetimeManager(),
                                                        new InjectionConstructor
                                                        (
                                                            typeof(UnitOfWork)
                                                        ));

            container.RegisterType<IViewModel, InputViewModel>("InputViewModel",
                                                        new ContainerControlledLifetimeManager(),
                                                        new InjectionConstructor
                                                        (
                                                            typeof(UnitOfWork)
                                                        ));  

            container.RegisterType<IView, InputView>("InputView",
                                                        new ContainerControlledLifetimeManager(),
                                                        new InjectionConstructor
                                                        (
                                                            typeof(UnitOfWork),
                                                            typeof(InputViewModel)
                                                        ));

            container.RegisterType<IViewModel, UsageViewModel>("UsageViewModel",
                                                        new ContainerControlledLifetimeManager(),
                                                        new InjectionConstructor
                                                        (
                                                            typeof(UnitOfWork)
                                                        ));

            container.RegisterType<IView, UsageView>("UsageView",
                                                        new ContainerControlledLifetimeManager(),
                                                        new InjectionConstructor
                                                        (
                                                            typeof(UsageViewModel)
                                                        ));

            container.RegisterType<IView, FuellingView>("FuellingView",
                                                        new ContainerControlledLifetimeManager(),
                                                        new InjectionConstructor
                                                        (
                                                            typeof(FuellingViewModel)
                                                        ));

            container.RegisterType<IView, MaintenanceView>("MaintenanceView",
                                                        new ContainerControlledLifetimeManager(),
                                                        new InjectionConstructor
                                                        (
                                                            typeof(MaintenanceViewModel)
                                                            
                                                        ));

            container.RegisterType<IView, UsageFuellingTablesView>("UsageFuellingTablesView",
                                                       new ContainerControlledLifetimeManager(),
                                                       new InjectionConstructor
                                                       (
                                                           typeof(UsageFuellingTablesViewModel)
                                                       ));

            container.RegisterType<IView, RunningHrsSchedulingTablesView>("RunningHrsSchedulingTablesView",
                                                       new ContainerControlledLifetimeManager(),
                                                       new InjectionConstructor
                                                       (
                                                           typeof(RunningHrsSchedulingTablesViewModel)
                                                       ));

            container.RegisterType<IView, ChartView>("ChartView",
                                                      new ContainerControlledLifetimeManager(),
                                                      new InjectionConstructor
                                                      (
                                                          typeof(ChartViewModel)
                                                      ));

            container.RegisterType<IViewModel, ChartViewModel>("ChartViewModel",
                                                      new ContainerControlledLifetimeManager(),
                                                      new InjectionConstructor
                                                      (
                                                          typeof(UnitOfWork),
                                                          typeof(ExtrudeInterveningDates)
                                                      ));

            container.RegisterType<IView, ReportView>("ReportView",
                                                       new ContainerControlledLifetimeManager(),
                                                       new InjectionConstructor
                                                       (
                                                           typeof(ReportViewModel)
                                                       ));

            container.RegisterType<IView, HelpView>("HelpView",
                                                       new ContainerControlledLifetimeManager(),
                                                       new InjectionConstructor
                                                       (
                                                           typeof(HelpViewModel)
                                                       ));

            container.RegisterType<IView, SettingsView>("SettingsView",
                                                       new ContainerControlledLifetimeManager(),
                                                       new InjectionConstructor
                                                       (
                                                           typeof(AuthoriserSettingsViewModel),
                                                           typeof(ConsumptionSettingsViewModel),
                                                           typeof(RemindersConfigViewModel)
                                                       ));

            IUnitOfWork unitOfWork = container.Resolve<IUnitOfWork>("UnitOfWork");
            IViewModel inputViewModel = container.Resolve<IViewModel>("InputViewModel");

            container.RegisterType<IView, MainView>("MainView",
                                                    new ContainerControlledLifetimeManager(),
                                                    new InjectionConstructor
                                                    (
                                                        typeof(InputView)
                                                    ));

            Application.Current.Resources.Add("UnityIoC", container);

            Task.Run(() => PerpertualNotifier.InitiatePerpertualNotifier());

            IView mainView = container.Resolve<IView>("MainView");
            (mainView as MainView).Show();

            string MyDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string logfilepath = $@"{MyDocuments}\Log.log";
            using (var logfile = new StreamWriter(logfilepath, true))
            {
                logfile.WriteLineAsync($"****************************************************************" + "\n"
                                      + "***UNDELIVERED EMAIL LOGGGING SESSION [" + DateTime.Now + "]***" + "\n"
                                      + "****************************************************************" +"\n");
            }
            Application.Current.Properties["LogFile"] = logfilepath;
        }
    }
}

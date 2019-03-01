using Panel.BusinessLogic;
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
            string AppDataFolder = Environment.CurrentDirectory;

            string providerName = @"System.Data.SqlClient";
            string serverName = @"(LocalDB)\MSSQLLocalDB";
            string databaseName = "GeneratorSurveillanceDB.mdf";

            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder
            {
                DataSource = serverName,
                AttachDBFilename = AppDataFolder + @"\" + databaseName,
                IntegratedSecurity = true,
                MultipleActiveResultSets = true
            };
            string providerString = sqlBuilder.ToString();


            EntityConnectionStringBuilder EntityCSB = new EntityConnectionStringBuilder
            {
                Metadata = @"res://*/GeneratorSurveillanceDBEntities.csdl|res://*/GeneratorSurveillanceDBEntities.ssdl|res://*/GeneratorSurveillanceDBEntities.msl",
                Provider = providerName,
                ProviderConnectionString = providerString
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

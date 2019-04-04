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
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Panel.Startup;

namespace Panel
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() { }

        public GeneratorSurveillanceDBEntities generatorSurveillanceDBEntities { get; set; }
        public bool IsConnected { get; set; }

        private  void Application_Startup(object sender, StartupEventArgs e)
        {
            generatorSurveillanceDBEntities = InitialiseDatabase.ConnectToDatabase();
            bool IsConnected = generatorSurveillanceDBEntities.Database.Exists();

            if (!IsConnected)
            {
                bool IsConnectedManyTries;
                for (int i = 0; i < 6; i++)
                {
                    generatorSurveillanceDBEntities = InitialiseDatabase.ConnectToDatabase();
                    IsConnectedManyTries = generatorSurveillanceDBEntities.Database.Exists();
                    if (IsConnectedManyTries)
                        break;
                    if (i >= 5)
                    {
                        MessageBox.Show("The application could not connect to the" +
                            " database." + Environment.NewLine + Environment.NewLine +
                           "Please restart the application", "Information",
                           MessageBoxButton.OK, MessageBoxImage.Error);

                        Application.Current.Shutdown();
                    }
                }
            }

            InitialiseAllSystems();
        }


        private void InitialiseAllSystems()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterInstance(generatorSurveillanceDBEntities);

            Func<IUnityContainer> FuncUnitOfWork = () => container.RegisterType<IUnitOfWork, UnitOfWork>("UnitOfWork",
                new ContainerControlledLifetimeManager(), new InjectionConstructor(typeof(GeneratorSurveillanceDBEntities)));

            Func<IUnityContainer> FuncExtrudeInterveningDates = () => container.RegisterType<IChartsLogic, ExtrudeInterveningDates>(
                "ExtrudeInterveningDates", new ContainerControlledLifetimeManager(), new InjectionConstructor(typeof(UnitOfWork)));

            Func<IUnityContainer> FuncInputViewModel = () => container.RegisterType<IViewModel, InputViewModel>(
                "InputViewModel", new ContainerControlledLifetimeManager(), new InjectionConstructor(typeof(UnitOfWork)));

            Func<IUnityContainer> FuncInputView = () => container.RegisterType<IView, InputView>("InputView",
                new ContainerControlledLifetimeManager(), new InjectionConstructor(typeof(UnitOfWork), typeof(InputViewModel)));

            Func<IUnityContainer> FuncUsageViewModel = () => container.RegisterType<IViewModel, UsageViewModel>("UsageViewModel", 
                new ContainerControlledLifetimeManager(), new InjectionConstructor(typeof(UnitOfWork)));

            Func<IUnityContainer> FuncUsageView = () => container.RegisterType<IView, UsageView>("UsageView",
                new ContainerControlledLifetimeManager(), new InjectionConstructor(typeof(UsageViewModel)));

            Func<IUnityContainer> FuncFuellingView = () => container.RegisterType<IView, FuellingView>("FuellingView",
                new ContainerControlledLifetimeManager(), new InjectionConstructor(typeof(FuellingViewModel)));

            Func<IUnityContainer> FuncMaintenanceView = () => container.RegisterType<IView, MaintenanceView>("MaintenanceView",
                new ContainerControlledLifetimeManager(), new InjectionConstructor(typeof(MaintenanceViewModel)));

            Func<IUnityContainer> FuncUsageFuellingTablesView = () => container.RegisterType<IView, UsageFuellingTablesView>("UsageFuellingTablesView",
                new ContainerControlledLifetimeManager(), new InjectionConstructor(typeof(UsageFuellingTablesViewModel)));

            Func<IUnityContainer> FuncRunningHrsSchedulingTablesView = () => container.RegisterType<IView, RunningHrsSchedulingTablesView>(
                "RunningHrsSchedulingTablesView", new ContainerControlledLifetimeManager(), new InjectionConstructor(
                    typeof(RunningHrsSchedulingTablesViewModel)));

            Func<IUnityContainer> FuncChartView = () => container.RegisterType<IView, ChartView>("ChartView", new ContainerControlledLifetimeManager(),
                new InjectionConstructor(typeof(ChartViewModel)));

            Func<IUnityContainer> FuncChartViewModel = () => container.RegisterType<IViewModel, ChartViewModel>("ChartViewModel",
                new ContainerControlledLifetimeManager(), new InjectionConstructor(typeof(UnitOfWork), typeof(ExtrudeInterveningDates)));

            Func<IUnityContainer> FuncReportView = () => container.RegisterType<IView, ReportView>("ReportView", new ContainerControlledLifetimeManager(),
                new InjectionConstructor(typeof(ReportViewModel)));

            Func<IUnityContainer> FuncHelpView = () => container.RegisterType<IView, HelpView>("HelpView", new ContainerControlledLifetimeManager(),
                new InjectionConstructor(typeof(HelpViewModel)));

            Func<IUnityContainer> FuncSettingsView = () => container.RegisterType<IView, SettingsView>("SettingsView", new ContainerControlledLifetimeManager(),
                new InjectionConstructor(typeof(AuthoriserSettingsViewModel), typeof(ConsumptionSettingsViewModel), typeof(RemindersConfigViewModel)));

            Func<IUnityContainer> FuncMainView = () => container.RegisterType<IView, MainView>("MainView", new ContainerControlledLifetimeManager(),
                new InjectionConstructor(typeof(InputView)));

            Task[] RegistrationTasks = new Task[16]{ Task.Run(FuncUnitOfWork), Task.Run(FuncExtrudeInterveningDates),
                Task.Run(FuncInputViewModel), Task.Run(FuncInputView), Task.Run(FuncUsageViewModel), Task.Run(FuncUsageView), Task.Run(FuncFuellingView),
                Task.Run(FuncMaintenanceView), Task.Run(FuncUsageFuellingTablesView), Task.Run(FuncRunningHrsSchedulingTablesView), Task.Run(FuncChartView),
                Task.Run(FuncChartViewModel), Task.Run(FuncReportView), Task.Run(FuncHelpView), Task.Run(FuncSettingsView), Task.Run(FuncMainView)};

            try
            {
                Task.WaitAll(RegistrationTasks);
            }
            catch (Exception ex)
            {
                MessageBox.Show("The application encountered an unexpected error" +
                            Environment.NewLine + Environment.NewLine +
                           "Please restart the application" +
                           Environment.NewLine + Environment.NewLine +
                           "Error:" + Environment.NewLine +
                           ex.Message, "Information",
                           MessageBoxButton.OK, MessageBoxImage.Error);

                Application.Current.Shutdown();
            }
            
            Application.Current.Resources.Add("UnityIoC", container);

            IView mainView = container.Resolve<IView>("MainView");
            (mainView as MainView).Show();

            Task.Run(() => PerpertualNotifier.InitiatePerpertualNotifier());

            string MyDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string logfilepath = $@"{MyDocuments}\Log.log";
            if (!File.Exists(logfilepath))
            {
                using (var logfile = new StreamWriter(logfilepath, true))
                {
                    logfile.WriteLineAsync($"****************************************************************" + "\n"
                                          + "***UNDELIVERED EMAIL LOGGGING SESSION [" + DateTime.Now + "]***" + "\n"
                                          + "****************************************************************" + "\n");
                }
                Application.Current.Properties["LogFile"] = logfilepath;
            }
            
        }
    }
}

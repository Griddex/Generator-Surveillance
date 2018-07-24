using Panel.BusinessLogic;
using Panel.Interfaces;
using Panel.Repositories;
using Panel.Services.MessagingServices;
using Panel.UserControls;
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

        private  void Application_Startup(object sender, StartupEventArgs e)
        {
            InitialiseAllSystems();
        }        
       
        private void InitialiseAllSystems()
        {
            IUnityContainer container = new UnityContainer();

            GeneratorSurveillanceDBEntities generatorSurveillanceDBEntities = new GeneratorSurveillanceDBEntities();
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
            IView inputView = container.Resolve<IView>("InputView");

            container.RegisterType<IView, MainView>("MainView",
                                                    new ContainerControlledLifetimeManager(),
                                                    new InjectionConstructor
                                                    (
                                                        typeof(InputView)
                                                    ));

            Application.Current.Resources.Add("UnityIoC", container);

            Notifier.Initialise();

            IView mainView = container.Resolve<IView>("MainView");
            (mainView as MainView).Show();
        }
    }
}

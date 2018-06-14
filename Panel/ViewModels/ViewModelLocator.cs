using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Panel.Interfaces;
using Panel.Services.NavigationService;
using Panel.ViewModels;
using Panel.ViewModels.InputViewModels;

namespace Panel.ViewModels
{
    public static class ViewModelLocator
    {
        public static int accessNumber = 1;
        public static bool GetViewModelSelect(DependencyObject obj) => (bool)obj.GetValue(ViewModelSelectProperty);
        public static void SetViewModelSelect(DependencyObject obj, bool value) => obj.SetValue(ViewModelSelectProperty, value);
        public static readonly DependencyProperty ViewModelSelectProperty =
                                DependencyProperty.RegisterAttached("ViewModelSelect", typeof(bool), 
                                            typeof(ViewModelLocator), new PropertyMetadata(false,ViewModelSelectPropertyChanged));

        private static void ViewModelSelectPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //if (DesignerProperties.GetIsInDesignMode(d) || accessNumber == 2)
            //    return;
            if (DesignerProperties.GetIsInDesignMode(d))
                return;
            Type viewType = d.GetType();
            string ViewModelTypeName = viewType.FullName.Replace(viewType.Name, viewType.Name + "Model")
                                                        .Replace(".Views.", ".ViewModels.");                                    
            Type viewModelType = Type.GetType(ViewModelTypeName);
            
            INavigationService frameNavigationService = new FrameNavigationService(new MainView().MainViewFrame);
            Type frameNavigationServiceType = Type.GetType(frameNavigationService.ToString());            

            ConstructorInfo navigationServiceConstructor = viewModelType.GetConstructor(new Type[] { frameNavigationServiceType });
            var viewModel = navigationServiceConstructor.Invoke(new Object[] { frameNavigationService });
            ((FrameworkElement)d).DataContext = viewModel;

            accessNumber += 1;
        }

        private static Object CreateInstance(Type type)
        {
            INavigationService frameNavigationService = new FrameNavigationService(new MainView().MainViewFrame);
            Type frameNavigationServiceType = Type.GetType(frameNavigationService.ToString());

            var constructors = type.GetConstructors();
            var defaultConstructor = constructors.SingleOrDefault(a => !a.GetParameters().Any());
            if(defaultConstructor == null)
            {
                var navigationServiceConstructor = constructors.FirstOrDefault(
                    b => b.GetParameters().All(
                        c => c.GetCustomAttributes(frameNavigationServiceType, false).Any()));
                if (navigationServiceConstructor == null)
                    throw new InvalidOperationException("No default constructor found");
                return navigationServiceConstructor.Invoke(navigationServiceConstructor.GetParameters()
                    .Select(d => d.DefaultValue).ToArray());
            }
            return Activator.CreateInstance(type);
        }
    }
}

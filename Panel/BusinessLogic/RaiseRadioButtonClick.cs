using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Panel.BusinessLogic
{
    public static class RaiseRadioButtonClick
    {
        public static ICommand GetRaiseClick(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(RaiseClickProperty);
        }

        public static void SetRaiseClick(DependencyObject obj, ICommand value)
        {
            obj.SetValue(RaiseClickProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RaiseClickProperty =
                                                            DependencyProperty.RegisterAttached(
                                                                "RaiseClick", 
                                                                typeof(ICommand), 
                                                                typeof(RaiseRadioButtonClick), 
                                                                new PropertyMetadata(new PropertyChangedCallback(RadioButtonIsClicked)));

        private static void RadioButtonIsClicked(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            UIElement element = target as UIElement;
            if(element != null)
            {
                RadioButton rdbtnYear = element as RadioButton;
                
            }
        }
    }
}

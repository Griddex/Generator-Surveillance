using System.Windows;
using System.Windows.Input;

namespace Panel.DependencyProperties
{
    public class StopGenerator
    {


        public static ICommand GetStopGeneratorCommand(DependencyObject lbl)
        {
            return (ICommand)lbl.GetValue(StopGeneratorCommandProperty);
        }

        public static void SetStopGeneratorCommand(DependencyObject lbl, ICommand cmd)
        {
            lbl.SetValue(StopGeneratorCommandProperty, cmd);
        }

        // Using a DependencyProperty as the backing store for StopGeneratorCommand.  
        // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StopGeneratorCommandProperty =
            DependencyProperty.RegisterAttached("StopGeneratorCommand", 
                typeof(ICommand), 
                typeof(StopGenerator), 
                new PropertyMetadata
                (
                    null, 
                    new PropertyChangedCallback(StopGeneratorNavigation)
                ));

        static void StopGeneratorNavigation(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(e.NewValue != null && e.NewValue != e.OldValue)
            {

            }
        }
    }
}

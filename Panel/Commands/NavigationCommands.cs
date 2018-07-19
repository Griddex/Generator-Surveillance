using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Panel.Commands
{
    public static class NavigationCommands
    {
        public static readonly RoutedUICommand inputToUsageView = new RoutedUICommand
                                                                    (
                                                                    "InputToUsageView",
                                                                    "InputToUsageView",
                                                                    typeof(Frame),
                                                                    new InputGestureCollection()
                                                                    {
                                                                        new KeyGesture(Key.U, ModifierKeys.Alt)
                                                                    });
        public static RoutedUICommand InputToUsageView
        {
            get { return inputToUsageView; }
        }


        public static readonly RoutedUICommand usageToInputView = new RoutedUICommand
                                                                    (
                                                                    "UsageToInputView",
                                                                    "UsageToInputView",
                                                                    typeof(Frame),
                                                                    new InputGestureCollection()
                                                                    {
                                                                        new KeyGesture(Key.I, ModifierKeys.Alt)
                                                                    });
        public static RoutedUICommand UsageToInputView
        {
            get { return usageToInputView; }
        }


        public static readonly RoutedUICommand usageToFuellingView = new RoutedUICommand
                                                                    (
                                                                    "UsageToFuellingView",
                                                                    "UsageToFuellingView",
                                                                    typeof(Frame),
                                                                    new InputGestureCollection()
                                                                    {
                                                                        new KeyGesture(Key.F, ModifierKeys.Alt)
                                                                    });
        public static RoutedUICommand UsageToFuellingView
        {
            get { return usageToFuellingView; }
        }


        public static readonly RoutedUICommand fuellingToUsageView = new RoutedUICommand
                                                                    (
                                                                    "FuellingToUsageView",
                                                                    "FuellingToUsageView",
                                                                    typeof(Frame),
                                                                    new InputGestureCollection()
                                                                    {
                                                                        new KeyGesture(Key.U, ModifierKeys.Alt)
                                                                    });
        public static RoutedUICommand FuellingToUsageView
        {
            get { return fuellingToUsageView; }
        }


        public static readonly RoutedUICommand fuellingToMaintenanceView = new RoutedUICommand
                                                                    (
                                                                    "FuellingToMaintenanceView",
                                                                    "FuellingToMaintenanceView",
                                                                    typeof(Frame),
                                                                    new InputGestureCollection()
                                                                    {
                                                                        new KeyGesture(Key.M, ModifierKeys.Alt)
                                                                    });
        public static RoutedUICommand FuellingToMaintenanceView
        {
            get { return fuellingToMaintenanceView; }
        }


        public static readonly RoutedUICommand maintenanceToFuellingView = new RoutedUICommand
                                                                    (
                                                                    "MaintenanceToFuellingView",
                                                                    "MaintenanceToFuellingView",
                                                                    typeof(Frame),
                                                                    new InputGestureCollection()
                                                                    {
                                                                        new KeyGesture(Key.F, ModifierKeys.Alt)
                                                                    });
        public static RoutedUICommand MaintenanceToFuellingView
        {
            get { return maintenanceToFuellingView; }
        }

        public static readonly RoutedUICommand usageMaintToRunningHrsSchedulerView = new RoutedUICommand
                                                                    (
                                                                    "UsageMaintTorunningHrsSchedulerView",
                                                                    "UsageMaintTorunningHrsSchedulerView",
                                                                    typeof(Frame),
                                                                    new InputGestureCollection()
                                                                    {
                                                                        new KeyGesture(Key.R, ModifierKeys.Alt)
                                                                    });
        public static RoutedUICommand UsageMaintToRunningHrsSchedulerView
        {
            get { return usageMaintToRunningHrsSchedulerView; }
        }

        public static readonly RoutedUICommand runningHrsSchedulerToUsageMaintView = new RoutedUICommand
                                                                    (
                                                                    "RunningHrsSchedulerToUsageMaintView",
                                                                    "RunningHrsSchedulerToUsageMaintView",
                                                                    typeof(Frame),
                                                                    new InputGestureCollection()
                                                                    {
                                                                        new KeyGesture(Key.S, ModifierKeys.Alt)
                                                                    });
        public static RoutedUICommand RunningHrsSchedulerToUsageMaintView
        {
            get { return runningHrsSchedulerToUsageMaintView; }
        }
    }
}

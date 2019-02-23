using System;
using System.Timers;
using System.Diagnostics;

namespace Panel.Services.MessagingServices
{
    public static class PerpertualNotifier
    {
        private static Timer NotifierTimer;

        public static void InitiatePerpertualNotifier()
        {
            OnTimedEvent(null, null);
            Trace.WriteLine($"First Timer fired on: {DateTime.Now}");
            NotifierTimer = new Timer(600 * 1000);            
            NotifierTimer.Elapsed += OnTimedEvent;
            NotifierTimer.AutoReset = true;
            NotifierTimer.Enabled = true;            
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            try
            {
                Trace.WriteLine($"Timer fired on: {e.SignalTime}");
            }
            catch (Exception) { }
            
            Notifier.Initialise();
        }
    }
}

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
            Trace.WriteLine($"Timer fired on: {DateTime.Now}");
            NotifierTimer = new Timer(60 * 1000);            
            NotifierTimer.Elapsed += OnTimedEvent;
            NotifierTimer.AutoReset = true;
            NotifierTimer.Enabled = true;            
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Trace.WriteLine($"Timer fired on: {e.SignalTime}");
            Notifier.Initialise();
        }
    }
}

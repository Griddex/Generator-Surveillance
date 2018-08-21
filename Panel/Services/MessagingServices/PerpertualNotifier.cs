using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Panel.Services.MessagingServices
{
    public static class PerpertualNotifier
    {
        class TimerState
        {
            public int Counter;
        }

        public static void InitiatePerpertualNotifier()
        {
            var timerState = new TimerState { Counter = 0 };

            Timer timer = new Timer
            (
                callback: new TimerCallback(TimerTask),
                state: timerState,
                dueTime: 1000,
                period: 30 * 60 * 1000
            );
        }

        private static void TimerTask(object timerState)
        {
            //Notifier.Initialise(new TimeSpan(0,5,0));
        }
    }
}

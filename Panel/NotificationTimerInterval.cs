using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel
{
    public static class NotificationTimerInterval
    {
        public static TimeSpan Interval { get; set; } = new TimeSpan(0, 60, 0);
    }
}

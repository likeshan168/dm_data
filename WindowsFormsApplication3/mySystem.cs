using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication3
{
    class mySystem
    {
        public static long currentTimeMillis()
        {
            DateTime DateTime1 = new DateTime(1970, 01, 01, 08, 00, 00);
            DateTime DateTime2 = System.DateTime.Now;
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            double i = ts.TotalMilliseconds;
            return Convert.ToInt64(i);
        }
    }
}

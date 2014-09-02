using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication3
{
    class TimeFormat
    {
        public static string getTime(long date)
        {
            DateTime t1 = new DateTime(1970, 1, 1, 08, 00, 00).AddMilliseconds(date);

            int year = t1.Year;
            int month = t1.Month;
            int day = t1.Day;
            int hour = t1.Hour;
            int min = t1.Minute;
            int second = t1.Second;

            string yearStr = "";
            string monthStr = "";
            string dayStr = "";
            string hourStr = "";
            string minStr = "";
            string secondStr = "";
            yearStr = year.ToString();

            if (month < 10)
            {
                monthStr = '0' + month.ToString();
            }
            else
            {
                monthStr = month.ToString();
            }

            if (day < 10)
            {
                dayStr = '0' + day.ToString();
            }
            else
            {
                dayStr = day.ToString();
            }

            if (hour < 10)
            {
                hourStr = '0' + hour.ToString();
            }
            else
            {
                hourStr = hour.ToString();
            }

            if (min < 10)
            {
                minStr = '0' +min.ToString();
            }
            else
            {
                minStr = min.ToString();
            }

            if (second < 10)
            {
                secondStr = '0' + second.ToString();
            }
            else
            {
                secondStr = second.ToString();
            }

            string res = yearStr + monthStr + dayStr + hourStr + minStr + secondStr;
            return res;
          }


        public static string getCurrentTime()
        {
            return getTime(mySystem.currentTimeMillis());
        }

    }
}

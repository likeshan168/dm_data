using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication3
{
    class OperateString
    {
        public static int spcialStarChar = 22;
        public static string getAsciiString(int i)
        {
            char[] a = {(char) i};
            return a.ToString();
        }
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="sSourceString"></param>
        /// <param name="sDevider"></param>
        /// <returns></returns>
        public static string[] getStringCollection(string sSourceString, string sDevider)
        {
            int iCount = getStringCount(sSourceString, sDevider);
            string[] sResult = new string[iCount];
            sResult = sSourceString.Split(Convert.ToChar(sDevider));
            return sResult;
        }
        /// <summary>
        /// 获取分割字符串后，字符串的个数
        /// </summary>
        /// <param name="sSourceString"></param>
        /// <param name="sDevider"></param>
        /// <returns></returns>
        public static int getStringCount(string sSourceString, string sDevider)
        {
            int iCount = 1;
            int j = 0;
            j = sSourceString.IndexOf(sDevider, j);
            while (j > 0)
            {
                iCount += 1;
                j = sSourceString.IndexOf(sDevider, j + sDevider.Length);
            }
            return iCount;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication3
{
    class ClientApp
    {
        public static string id = "";//0002

        public static string serverIP = "";
        public static int PORT =0 ;

        public static string username = "";//wanglj
        public static string password = "";//1

        public static bool isServerOpen = false;
        public static long test_connect_time = 1000 * 10;

        public static string localBase = "";
        //public static string localcon = "";
        public static string Basecon = "";
        //public static string localConfig = "";



        public static string SmtpServer = "";
        public static int SmtpPort = 0;
        public static string SmtpUser = "";
        public static string SmtpPwd = "";


        public static string DefaultShop = "";
        public static string DefaultSales = "";
    
        public static string cardid = "";

        public static string CouponValue = "";
        //public static string MessageInfo = "";
        public static string UseDate = "";
        public static int SMSFlag = 0;
        public static string DefaultCompanyID = "";
        public static string SMSSuffix = "";
        /// <summary>
        /// 是否开机启动
        /// </summary>
        public static bool Boot = false;
    }
}

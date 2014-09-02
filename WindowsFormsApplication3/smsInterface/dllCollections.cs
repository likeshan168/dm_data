using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace smsInterface
{
    public class dllCollections
    {
        [DllImport("EUCPComm.dll", EntryPoint = "SendSMS")]  //即时发送
        public static extern int SendSMS(string sn, string mn, string ct, string priority);  
    }
}

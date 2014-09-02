using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;


namespace WindowsFormsApplication3
{

 

    class Calldll
    {
        [DllImport("EUCPComm.dll", EntryPoint = "SendSMS")]  //即′时骸发ぁ送í
        public static extern int SendSMS(string sn, string mn, string ct, string priority);  
       
    }
}

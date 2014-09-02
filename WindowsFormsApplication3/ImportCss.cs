using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Data;


namespace WindowsFormsApplication3
{
    class ImportCss
    {

        [DllImport("smsInterface.dll", EntryPoint = "smsSend")]
        public static extern string smsSend(string uid, string pwd, string cid, DataTable smsTable);
    }
}

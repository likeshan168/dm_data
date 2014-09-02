using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication3
{
    class CheckMsg
    {
        public bool pass;
        public int msgType;
        public String flowNo;
        public String infoMsg;

        public CheckMsg()
        {
            pass = false;
            infoMsg = "";
            flowNo = "";
            msgType = 0;
        }

        public CheckMsg(bool pass, int msgType, string flowNo, string infoMsg)
        {
            this.pass = pass;
            this.msgType = msgType;
            this.flowNo = flowNo;
            this.infoMsg = infoMsg;
            this.infoMsg = infoMsg;
        }
    }
}

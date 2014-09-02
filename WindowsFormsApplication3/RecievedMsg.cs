using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication3
{
    class RecievedMsg
    {
        public string time;

        public string sessionId;
        public int msgType;
        public bool format;
        public string originalMsg;
        public string memo;
        public string replyMsg;
        public bool recent;
        //  public String updateTime;

        public RecievedMsg()
        {
            time = "";
            msgType = 0;
            format = false;
            originalMsg = "";
            memo = "";
            replyMsg = "";
            sessionId = "";
            recent = false;

        }


        public RecievedMsg(string time, string sessionId, int msgType,
                           bool format, string originalMsg, string memo,
                           string replyMsg, bool recent)
        {
            this.time = time;
            this.sessionId = sessionId;

            this.msgType = msgType;
            this.format = format;
            this.originalMsg = originalMsg;
            this.memo = memo;
            this.replyMsg = replyMsg;
            this.recent = recent;

        }
    }
}

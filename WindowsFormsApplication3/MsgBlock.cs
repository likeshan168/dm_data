using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WindowsFormsApplication3
{
    [Serializable]
    class MsgBlock 
    {
        public int msgType;
        public int time;
        public string msg;
        public string memo;


        public MsgBlock()
        {
            msgType = 0;
            time = 0;
            msg = "";
            memo = "";
        }

        public MsgBlock(int msgType, int time, string msg)
        {
            this.msgType = msgType;
            this.time = time;
            this.msg = msg;
            this.memo = "";
        }

        public MsgBlock(int msgType, int time, string msg, string memo)
        {
            this.msgType = msgType;
            this.time = time;
            this.msg = msg;
            this.memo = memo;
        }

        //public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
            
        //} 
    }
}

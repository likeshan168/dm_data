using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.Serialization;

namespace WindowsFormsApplication3
{
    [Serializable] 
    class CommonMsg //: ISerializable 
    {
        public int id;
        public string sessionId;
        public int size;
        public MsgBlock msgBlock;
        public byte[] Data;

        public CommonMsg()
        {

        }
        //SerializableAttribute mm = new SerializableAttribute();
        //public CommonMsg(int id, String clientId, String mobileNo, String mobileSn, int mobileType,
        //          int size, String msgTime, MsgBlock msgBlock)
        //{
        //    this.id = id;
        //    this.clientId = clientId;
        //    this.mobileNo = mobileNo;
        //    this.mobileSn = mobileSn;
        //    this.mobileType = mobileType;
        //    this.size = size;
        //    this.msgTime = msgTime;
        //    this.msgBlock = msgBlock;
        //}
        //protected CommonMsg(SerializationInfo info, StreamingContext context)
        //{
        //    this.id = info.GetInt32("id");
        //    this.sessionId = info.GetString("sessionId");
        //    this.size = info.GetInt32("size");
        //    //this.msgBlock = info.getob;
        //}
        public CommonMsg(int id, string sessionId, int size, MsgBlock msgBlock)
        {
            this.id = id;
            this.sessionId = sessionId;
            this.size = size;
            this.msgBlock = msgBlock;
        }

        public CommonMsg(int id, string sessionId, int size, byte[] data)
        {
            this.id = id;
            this.sessionId = sessionId;
            this.size = size;
            this.Data = data;
        }


       // public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
       // {
       //     info.AddValue("id", n1);
       //     info.AddValue("sessionId", n2);
       //     info.AddValue("size", str);
       //} 

    }
}

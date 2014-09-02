using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WindowsFormsApplication3
{
    [Serializable] 
    class ClientMsg
    {
        public int id;
        //public int mSize;
        public string sessionId;
        public byte DataType;
        public byte[] Data = null;
        byte code;
        byte[] Msg = null;
        /// <summary>
        ///id,sessionid,datatype 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sessionid"></param>
        /// <param name="datatype"></param>
        public ClientMsg(int id, string sessionid,byte datatype)
        {
            this.id = id;
            this.sessionId = sessionid;
            //this.mSize = msize;
            this.DataType = datatype;
            this.Data = SetDataByte(datatype);
        }
        public ClientMsg(int id, string sessionid, byte datatype,byte[] msg)
        {
            this.id = id;
            this.sessionId = sessionid;
            //this.mSize = msize;
            this.DataType = datatype;
            this.Msg = msg;
            this.Data = SetDataByte(datatype);
            
        }
        public ClientMsg(byte datatype,byte[] msg)
        {
            this.id = 0;
            this.sessionId = "";
            this.DataType = datatype;
            this.Msg = msg;
            this.Data = SetDataByte(datatype);
        }
        private byte[] SetDataByte(byte type)
        {
            MemoryStream ms=null;
            try
            {

                switch (type)
                {
                    
                    case 0x30:
                        code = type;
                        DateTime DateTime1 = new DateTime(1970, 01, 01, 08, 00, 00);
                        DateTime DateTime2 = System.DateTime.Now;
                        TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                        TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                        TimeSpan ts = ts1.Subtract(ts2).Duration();
                        double i = ts.TotalMilliseconds;
                        int time = (int)(i / 1000);
                        byte[] Adate = ByteConvert.intToByteArray(time);
                        byte status = 0;
                        byte[] msbyt = new byte[6];
                        ms= new MemoryStream(msbyt);
                        ms.WriteByte(code);
                        ms.Write(Adate, 0, 4);
                        ms.WriteByte(status);

                        break;

                    //新增 vip制卡信息下载
                    #region 新增 vip制卡信息下载
                    case 0x61:
                        code = type;
                        DateTime DateTime61 = new DateTime(1970, 01, 01, 08, 00, 00);
                        DateTime DateTime62 = System.DateTime.Now;
                        TimeSpan ts61 = new TimeSpan(DateTime61.Ticks);
                        TimeSpan ts62 = new TimeSpan(DateTime62.Ticks);
                        TimeSpan ts6 = ts61.Subtract(ts62).Duration();
                        double it = ts6.TotalMilliseconds;
                        int times = (int)(it / 1000);
                        byte[] Adates = ByteConvert.intToByteArray(times);
                        byte status61 = 0;
                        byte[] msbyt61 = new byte[6];
                        ms = new MemoryStream(msbyt61);
                        ms.WriteByte(code);
                        ms.Write(Adates, 0, 4);
                        ms.WriteByte(status61);

                        break;
                    #endregion
                    case 0x31:
                        code = type;

                        DateTime DateTime3 = new DateTime(1970, 01, 01, 08, 00, 00);
                        DateTime DateTime4 = System.DateTime.Now;
                        TimeSpan ts3 = new TimeSpan(DateTime3.Ticks);
                        TimeSpan ts4 = new TimeSpan(DateTime4.Ticks);
                        TimeSpan tss = ts3.Subtract(ts4).Duration();
                        double mi = tss.TotalMilliseconds;
                        int Mtime = (int)(mi / 1000);
                        byte[] MAdate = ByteConvert.intToByteArray(Mtime);
                        byte Mstatus = 0;

                        byte[] mdata = new ClientData().DataInfoByte();

                        byte[] Msbyt = new byte[10+mdata.Length];
                        ms = new MemoryStream(Msbyt);
                        ms.WriteByte(code);
                        ms.Write(MAdate, 0, 4);
                        ms.WriteByte(Mstatus);

                        byte[] Ldata = ByteConvert.intToByteArray(mdata.Length);
                        ms.Write(Ldata, 0, 4);
                        ms.Write(mdata, 0, mdata.Length);
                        break;

                    case 0x34:
                        code = type;

                         DateTime3 = new DateTime(1970, 01, 01, 08, 00, 00);
                         DateTime4 = System.DateTime.Now;
                         ts3 = new TimeSpan(DateTime3.Ticks);
                         ts4 = new TimeSpan(DateTime4.Ticks);
                         tss = ts3.Subtract(ts4).Duration();
                         mi = tss.TotalMilliseconds;
                         Mtime = (int)(mi / 1000);
                         MAdate = ByteConvert.intToByteArray(Mtime);
                         //以上计算四位时间值
                         //DataInfoByte方法生成上传的数据包
                         mdata = new ClientData().DataInfoByte(this.Msg);

                         Msbyt = new byte[9 + mdata.Length];
                         ms = new MemoryStream(Msbyt);
                         ms.WriteByte(code);//写入一位操作码
                         ms.Write(MAdate, 0, 4);//写入四位时间
                       

                         Ldata = ByteConvert.intToByteArray(mdata.Length);
                         ms.Write(Ldata, 0, 4);//写入四位数据包的长度

                         ms.Write(mdata, 0, mdata.Length);//写入数据
                         break;
                    case 0x35:
                         code = type;

                         DateTime3 = new DateTime(1970, 01, 01, 08, 00, 00);
                         DateTime4 = System.DateTime.Now;
                         ts3 = new TimeSpan(DateTime3.Ticks);
                         ts4 = new TimeSpan(DateTime4.Ticks);
                         tss = ts3.Subtract(ts4).Duration();
                         mi = tss.TotalMilliseconds;
                         Mtime = (int)(mi / 1000);
                         MAdate = ByteConvert.intToByteArray(Mtime);

                         mdata = new ClientData().DataInfoByteUPload(this.Msg);

                         Msbyt = new byte[9 + mdata.Length];
                         ms = new MemoryStream(Msbyt);
                         ms.WriteByte(code);
                         ms.Write(MAdate, 0, 4);


                         Ldata = ByteConvert.intToByteArray(mdata.Length);
                         ms.Write(Ldata, 0, 4);
                         ms.Write(mdata, 0, mdata.Length);
                         break;

                    case 0x36:
                         code = type;
                          DateTime1 = new DateTime(1970, 01, 01, 08, 00, 00);
                          DateTime2 = System.DateTime.Now;
                          ts1 = new TimeSpan(DateTime1.Ticks);
                          ts2 = new TimeSpan(DateTime2.Ticks);
                          ts = ts1.Subtract(ts2).Duration();
                          i = ts.TotalMilliseconds;
                          time = (int)(i / 1000);
                         Adate = ByteConvert.intToByteArray(time);
                          status = 0;
                          msbyt = new byte[6];
                         ms = new MemoryStream(msbyt);
                         ms.WriteByte(code);
                         ms.Write(Adate, 0, 4);
                         ms.WriteByte(status);

                         break;

                    case 0x38:
                         code = type;
                         DateTime1 = new DateTime(1970, 01, 01, 08, 00, 00);
                         DateTime2 = System.DateTime.Now;
                         ts1 = new TimeSpan(DateTime1.Ticks);
                         ts2 = new TimeSpan(DateTime2.Ticks);
                         ts = ts1.Subtract(ts2).Duration();
                         i = ts.TotalMilliseconds;
                         time = (int)(i / 1000);
                         Adate = ByteConvert.intToByteArray(time);
                         //以上计算四位时间值
                         status = 0;
                         msbyt = new byte[6];
                         ms = new MemoryStream(msbyt);
                         ms.WriteByte(code);//写入一位操作码
                         ms.Write(Adate, 0, 4);//写入四位计算出的时间值　
                         ms.WriteByte(status);//写入一位状态

                         break;
                     case 0x24:
                    
                         code = type;

                         DateTime3 = new DateTime(1970, 01, 01, 08, 00, 00);
                         DateTime4 = System.DateTime.Now;
                         ts3 = new TimeSpan(DateTime3.Ticks);
                         ts4 = new TimeSpan(DateTime4.Ticks);
                         tss = ts3.Subtract(ts4).Duration();
                         mi = tss.TotalMilliseconds;
                         Mtime = (int)(mi / 1000);
                         MAdate = ByteConvert.intToByteArray(Mtime);

                         mdata = new ClientData().DataInfoByteUPload(this.Msg);

                         Msbyt = new byte[9 + mdata.Length];
                         ms = new MemoryStream(Msbyt);
                         ms.WriteByte(code);
                         ms.Write(MAdate, 0, 4);


                         Ldata = ByteConvert.intToByteArray(mdata.Length);
                         ms.Write(Ldata, 0, 4);
                         ms.Write(mdata, 0, mdata.Length);
                         break;
                     case 0x26:

                         code = type;

                         DateTime3 = new DateTime(1970, 01, 01, 08, 00, 00);
                         DateTime4 = System.DateTime.Now;
                         ts3 = new TimeSpan(DateTime3.Ticks);
                         ts4 = new TimeSpan(DateTime4.Ticks);
                         tss = ts3.Subtract(ts4).Duration();
                         mi = tss.TotalMilliseconds;
                         Mtime = (int)(mi / 1000);
                         MAdate = ByteConvert.intToByteArray(Mtime);

                         mdata = this.Msg;

                         Msbyt = new byte[9 + mdata.Length];
                         ms = new MemoryStream(Msbyt);
                         ms.WriteByte(code);
                         ms.Write(MAdate, 0, 4);


                         Ldata = ByteConvert.intToByteArray(mdata.Length);
                         ms.Write(Ldata, 0, 4);
                         ms.Write(mdata, 0, mdata.Length);
                         break;
                }

                return ms.ToArray();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("ClientMst-->SetDataByte  ErrInfo:" + ex.Message);
                ErrInfo.WriterErrInfo("ClientMsg", "SetDataByte", ex);
            }
            return null;
            
        }
        
    }
}

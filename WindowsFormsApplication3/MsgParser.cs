using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WindowsFormsApplication3
{
    class MsgParser
    {
        //static string mobileSn;
        public static MsgBlock parse(byte[] byteArray)
        {
            MsgBlock msgBlock = new MsgBlock();
           //单字节操作码
           //4字节时间
           //单字节状态标记
           //实际数据
            byte OPcode = byteArray[0];
            
            byte[] timeArray = new byte[4];
            msgBlock.msgType = byteArray[0];
            for (int i = 0; i < timeArray.Length; i++)
            {
                timeArray[i] = byteArray[i + 1];
            }

            msgBlock.time = ByteConvert.byteArrayToInt(timeArray);

            string str = "";
            string[] msgStr = new string[2];
            msgStr[0] = "";
            msgStr[1] = "";

            switch (byteArray[0])
            {
                case 0x20://VIP用户登陆
                    str = VipLoginWeb(byteArray);
                    msgBlock.msg = str;
                    break;

                case 0x21: //VIP用户资料查询
                    str = VipSelectWeb(byteArray);
                    msgBlock.msg = str;
                    break;

                case 0x22: //VIP用户资料修改
                    str = VipModifyWeb(byteArray);
                    msgBlock.msg = str;
                    break;
                case 0x23:   //VIP用户注册
                    str = RegisterWeb(byteArray);
                    msgBlock.msg = str;
                    break;

                case 0x25:   //VIP用户修改Email地址
                    str = VIPEditEmail(byteArray);
                    msgBlock.msg = str;
                    break;


                    
                case 0x30:  //VIP资料下载
                    str = VipInfomationAll(byteArray);
                    msgBlock.msg = str;
                    break;
                   
                case 0x31:
                    break;

                case 0x32:   //VIP资料修改
                    str = ERPVipModify(byteArray);
                    msgBlock.msg=str;
                    break;

                case 0x34:  //VIP资料批量修改
                    str = ERPVIPBatchModify(byteArray);
                    msgBlock.msg = str;
                    break;

                 //2009-3-17
                //case 0x36:  //营业员基本资料下载
                //    str = VipSalesAll(byteArray);
                //    msgBlock.msg = str;
                //    break;





                case 0x40:  //代理商登陆
                    str = AgentLoginWeb(byteArray);
                    msgBlock.msg = str;
                    break;

                case 0x41:  //代理商资料修改
                    str = AgentModifyWeb(byteArray);
                    msgBlock.msg = str;
                    break;

                case 0x42:   //代理商资料查询
                    str = AgentSelectWeb(byteArray);
                    msgBlock.msg = str;
                    break;

                
            }
            return msgBlock;
        }

        private static string VipLoginWeb(byte[] byteArray)
        {
            try
            {
                Stream byteArrayInputStream = new MemoryStream(byteArray);
                byteArrayInputStream.ReadByte();
                
                string str = MsgMacro.CNPartVipLoginMsgTag + MsgMacro.mainDivide1;

                
                byte[] timeArray = new byte[4];
                byteArrayInputStream.Read(timeArray, 0, 4);
                int lastUpdateTime = ByteConvert.byteArrayToInt(timeArray);
                str += lastUpdateTime + MsgMacro.mainDivide1;


                byteArrayInputStream.ReadByte();

                int lenname = byteArrayInputStream.ReadByte();
                byte[] vipname = new byte[lenname];
                byteArrayInputStream.Read(vipname, 0, vipname.Length);
                str += Encoding.Default.GetString(vipname) + MsgMacro.mainDivide1;

                int lenpwd = byteArrayInputStream.ReadByte();
                byte[] vippwd = new byte[lenpwd];
                byteArrayInputStream.Read(vippwd, 0, vippwd.Length);
                str += Encoding.Default.GetString(vippwd) + MsgMacro.mainDivide1;

                int lenip= byteArrayInputStream.ReadByte();
                byte[] vipip = new byte[lenip];
                byteArrayInputStream.Read(vipip, 0, vipip.Length);
                str += Encoding.Default.GetString(vipip);
                
                
                //Console.WriteLine("数据信息====" + Encoding.Default.GetString(sqlStr));
                byteArrayInputStream.Close();
                               
                return str;
            }
            catch(Exception ex)
            {
                //Console.WriteLine("MsgParser.cs错误提示:" + ex.Message.ToString());
                ErrInfo.WriterErrInfo("MsgParser", "VipLoginWeb", ex);
            }
            return ""; 
        }

        private static string VipSelectWeb(byte[] byteArray)
        {
            
            try
            {
                Stream byteArrayInputStream = new MemoryStream(byteArray);
                byteArrayInputStream.ReadByte();

                string str = MsgMacro.CNPartVipSelectMsgTag + MsgMacro.mainDivide1;

                byte[] timeArray = new byte[4];
                byteArrayInputStream.Read(timeArray,0,4);
                int lastUpdateTime = ByteConvert.byteArrayToInt(timeArray);
                str += lastUpdateTime+MsgMacro.mainDivide1; ;

                byteArrayInputStream.ReadByte();

                int lenCardid = byteArrayInputStream.ReadByte();
                byte[] Cardid = new byte[lenCardid];
                byteArrayInputStream.Read(Cardid, 0, Cardid.Length);
                str += Encoding.Default.GetString(Cardid);

                byteArrayInputStream.Close();
                return str;
            }
            catch (Exception ex)
            {
                //ex.printStackTrace();
                //if (ClientApp.isWriterErrInfo)
                //{
                //    new ErrInfo().WriterErrInfo("", "", ex);
                //}
                ErrInfo.WriterErrInfo("MsgParser", "VipSelectWeb", ex);
            }
            return "";
        }

        public static string VipModifyWeb(byte[] byteArray)
        {
            //格式 ：请求类型操作码（1）+时间（4）+数据长度（1）+数据信息
            try
            {
                Stream byteArrayInputStream = new MemoryStream(byteArray);
                byteArrayInputStream.ReadByte();

                string str = MsgMacro.CNPartVipModifyMsgTag + MsgMacro.mainDivide1;
               
                byte[] timeArray = new byte[4];
                byteArrayInputStream.Read(timeArray, 0, 4);
                int lastUpdateTime = ByteConvert.byteArrayToInt(timeArray);
                str += lastUpdateTime + MsgMacro.mainDivide1;

                byteArrayInputStream.ReadByte();

                int strLength = byteArrayInputStream.ReadByte();
                byte[] sqlStr = new byte[strLength];
                Console.WriteLine("数据长度====" + strLength);
                Console.WriteLine("数据信息====" + Encoding.Default.GetString(sqlStr));
                byteArrayInputStream.Read(sqlStr, 0, sqlStr.Length);
                str += Encoding.Default.GetString(sqlStr);
                byteArrayInputStream.Close();
                return str;

            }
            catch (Exception ex)
            {
                //ex.printStackTrace();
                //if (ClientApp.isWriterErrInfo) 
                //{
                //    new ErrInfo().WriterErrInfo("", "", ex);
                //}
                ErrInfo.WriterErrInfo("MsgParser", "VipModifyWeb", ex);
            }
            return "";

        }

        public static string unknow(byte[] byteArray)
        {
            string str = "";
            return str;
        }

        public static string ERPVipModify(byte[] byteArray)
        {
            try
            {
                Stream byteArrayInputStream = new MemoryStream(byteArray);

                byteArrayInputStream.ReadByte();

                string str = MsgMacro.CNPartErpVipModifyMsgTag + MsgMacro.mainDivide1;

                byte[] timeArray = new byte[4];
                byteArrayInputStream.Read(timeArray, 0, 4);
                int lastUpdateTime = ByteConvert.byteArrayToInt(timeArray);
                str += lastUpdateTime + MsgMacro.mainDivide1;

                int k=(int)byteArrayInputStream.ReadByte();

                str += k.ToString();
                byteArrayInputStream.Close();
                return str;

            }
            catch (Exception ex)
            {
                //ex.printStackTrace();
                //if (ClientApp.isWriterErrInfo) 
                //{
                //    new ErrInfo().WriterErrInfo("", "", ex);
                //}
                ErrInfo.WriterErrInfo("MsgParser", "ERPVipModify", ex);
            }
            return "";
 
        }

        public static string VipInfomationAll(byte[] byteArray)
        {
            try
            {
                Stream byteArrayInputStream = new MemoryStream(byteArray);

                byteArrayInputStream.ReadByte();

                string str = MsgMacro.CNPartVipInformationAllMsgTag + MsgMacro.mainDivide1;

                byte[] timeArray = new byte[4];
                byteArrayInputStream.Read(timeArray, 0, 4);
                int lastUpdateTime = ByteConvert.byteArrayToInt(timeArray);
                str += lastUpdateTime + MsgMacro.mainDivide1;

                int k = (int)byteArrayInputStream.ReadByte();

                str += k.ToString();
                byteArrayInputStream.Close();
                return str;

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message.ToString());
                ErrInfo.WriterErrInfo("MsgParser", "VipInfomationAll", ex);
            }
            return "";
        }

        private static string AgentLoginWeb(byte[] byteArray)
        {
            try
            {
                Stream byteArrayInputStream = new MemoryStream(byteArray);
                byteArrayInputStream.ReadByte();

                string str = MsgMacro.CNPartAgentLoginMsgTag + MsgMacro.mainDivide1;


                byte[] timeArray = new byte[4];
                byteArrayInputStream.Read(timeArray, 0, 4);
                int lastUpdateTime = ByteConvert.byteArrayToInt(timeArray);
                str += lastUpdateTime + MsgMacro.mainDivide1;


                byteArrayInputStream.ReadByte();

                int lenname = byteArrayInputStream.ReadByte();
                byte[] vipname = new byte[lenname];
                byteArrayInputStream.Read(vipname, 0, vipname.Length);
                str += Encoding.Default.GetString(vipname) + MsgMacro.mainDivide1;

                int lenpwd = byteArrayInputStream.ReadByte();
                byte[] vippwd = new byte[lenpwd];
                byteArrayInputStream.Read(vippwd, 0, vippwd.Length);
                str += Encoding.Default.GetString(vippwd);


                //Console.WriteLine("数据信息====" + Encoding.Default.GetString(sqlStr));
                byteArrayInputStream.Close();

                return str;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("MsgParser.cs错误提示:" + ex.Message.ToString());
                ErrInfo.WriterErrInfo("MsgParser", "AgentLoginWeb", ex);
            }
            return "";
        }

        public static string AgentModifyWeb(byte[] byteArray)
        {
            //格式 ：请求类型操作码（1）+时间（4）+数据长度（1）+数据信息
            try
            {
                Stream byteArrayInputStream = new MemoryStream(byteArray);
                byteArrayInputStream.ReadByte();

                string str = MsgMacro.CNPartAgentModifyMsgTag + MsgMacro.mainDivide1;

                byte[] timeArray = new byte[4];
                byteArrayInputStream.Read(timeArray, 0, 4);
                int lastUpdateTime = ByteConvert.byteArrayToInt(timeArray);
                str += lastUpdateTime + MsgMacro.mainDivide1;

                byteArrayInputStream.ReadByte();

                int strLength = byteArrayInputStream.ReadByte();
                byte[] sqlStr = new byte[strLength];
                Console.WriteLine("数据长度====" + strLength);
                Console.WriteLine("数据信息====" + Encoding.Default.GetString(sqlStr));
                byteArrayInputStream.Read(sqlStr, 0, sqlStr.Length);
                str += Encoding.Default.GetString(sqlStr);
                byteArrayInputStream.Close();
                return str;

            }
            catch (Exception ex)
            {
                //ex.printStackTrace();
                //if (ClientApp.isWriterErrInfo) 
                //{
                //    new ErrInfo().WriterErrInfo("", "", ex);
                //}
                ErrInfo.WriterErrInfo("MsgParser", "VipModifyWeb", ex);
            }
            return "";

        }

        public static string AgentSelectWeb(byte[] byteArray)
        {
            Stream byteArrayInputStream = new MemoryStream(byteArray);
            byteArrayInputStream.ReadByte();

            string str = MsgMacro.CNPartAgentSelectMsgTag + MsgMacro.mainDivide1;

            byte[] timeArray = new byte[4];
            byteArrayInputStream.Read(timeArray, 0, 4);
            int lastUpdateTime = ByteConvert.byteArrayToInt(timeArray);
            str += lastUpdateTime + MsgMacro.mainDivide1;

            byte[] temp = new byte[1];
            byteArrayInputStream.Read(temp,0,1);
            int lenDate = (int)temp[0];

            byte[] BeginDate = new byte[lenDate];
            byteArrayInputStream.Read(BeginDate, 0, BeginDate.Length);
            string Bdata = Encoding.Default.GetString(BeginDate);
            str += Bdata + MsgMacro.mainDivide1;


            temp = new byte[1];
            byteArrayInputStream.Read(temp, 0, 1);
            lenDate = (int)temp[0];

            byte[] EndDate = new byte[lenDate];
            byteArrayInputStream.Read(EndDate, 0, EndDate.Length);
            string Edata = Encoding.Default.GetString(EndDate);
            str += Edata + MsgMacro.mainDivide1;
            
            temp = new byte[1];
            byteArrayInputStream.Read(temp, 0, 1);
            lenDate = (int)temp[0];

            byte[] agent = new byte[lenDate];
            byteArrayInputStream.Read(agent, 0, agent.Length);
            string agentid = Encoding.Default.GetString(agent);
            str += agentid + MsgMacro.mainDivide1;

            temp = new byte[2];
            byteArrayInputStream.Read(temp, 0, temp.Length);
            short icout = ByteConvert.byteArrayToShort(temp);
            str += icout.ToString() + MsgMacro.mainDivide1;


            temp = new byte[1];
            byteArrayInputStream.Read(temp, 0, 1);
            lenDate = (int)temp[0];

            byte[] shop = new byte[lenDate];
            byteArrayInputStream.Read(shop, 0, shop.Length);
            string shopid = Encoding.Default.GetString(shop);
            str += shopid + MsgMacro.mainDivide1;



            temp = new byte[2];
            byteArrayInputStream.Read(temp, 0, temp.Length);
            icout = ByteConvert.byteArrayToShort(temp);
            str += icout.ToString() + MsgMacro.mainDivide1;


            temp = new byte[1];
            byteArrayInputStream.Read(temp, 0, 1);
            lenDate = (int)temp[0];

            byte[] person = new byte[lenDate];
            byteArrayInputStream.Read(person, 0, person.Length);
            string personid = Encoding.Default.GetString(person);
            str += personid + MsgMacro.mainDivide1;


            //int strLength = byteArrayInputStream.ReadByte();
            //byte[] sqlStr = new byte[strLength];
            //Console.WriteLine("数据长度====" + strLength);
            //Console.WriteLine("数据信息====" + Encoding.Default.GetString(sqlStr));
            //byteArrayInputStream.Read(sqlStr, 0, sqlStr.Length);
            //str += Encoding.Default.GetString(sqlStr);
            byteArrayInputStream.Close();
            return str;
        }


        public static string RegisterWeb(byte[] byteArray) 
        {
            try
            {
                Stream byteArrayInputStream = new MemoryStream(byteArray);
                byteArrayInputStream.ReadByte();

                string str = MsgMacro.CNPartVipRegisterMsgTag + MsgMacro.mainDivide1;
                
                byte[] timeArray = new byte[4];
                byteArrayInputStream.Read(timeArray, 0, 4);
                int lastUpdateTime = ByteConvert.byteArrayToInt(timeArray);
                str += lastUpdateTime + MsgMacro.mainDivide1;

                byteArrayInputStream.ReadByte();

                int lenname = byteArrayInputStream.ReadByte();
                byte[] vipname = new byte[lenname];
                byteArrayInputStream.Read(vipname, 0, vipname.Length);
                str += Encoding.Default.GetString(vipname) + MsgMacro.mainDivide1;

              
                //Console.WriteLine("数据信息====" + Encoding.Default.GetString(sqlStr));
                byteArrayInputStream.Close();

                return str;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("MsgParser.cs错误提示:" + ex.Message.ToString());
                ErrInfo.WriterErrInfo("MsgParser", "RegisterWeb", ex);
            }
            return ""; 
        }

        public static string VIPEditEmail(byte[] byteArray)
        {
            try
            {
                Stream byteArrayInputStream = new MemoryStream(byteArray);
                byteArrayInputStream.ReadByte();

                string str = MsgMacro.CNPartVipUpdateEmailMsgTag + MsgMacro.mainDivide1;

                byte[] timeArray = new byte[4];
                byteArrayInputStream.Read(timeArray, 0, 4);
                int lastUpdateTime = ByteConvert.byteArrayToInt(timeArray);
                str += lastUpdateTime + MsgMacro.mainDivide1;

                byteArrayInputStream.ReadByte();

                int lenname = byteArrayInputStream.ReadByte();
                byte[] mail = new byte[lenname];
                byteArrayInputStream.Read(mail, 0, mail.Length);
                str += Encoding.Default.GetString(mail) + MsgMacro.mainDivide1;

                 lenname = byteArrayInputStream.ReadByte();
                byte[] vip = new byte[lenname];
                byteArrayInputStream.Read(vip, 0, vip.Length);
                str += Encoding.Default.GetString(vip);


                //Console.WriteLine("数据信息====" + Encoding.Default.GetString(sqlStr));
                byteArrayInputStream.Close();

                return str;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("MsgParser.cs错误提示:" + ex.Message.ToString());
                ErrInfo.WriterErrInfo("MsgParser", "VIPEditEmail", ex);
            }
            return ""; 
        }

        public static string ERPVIPBatchModify(byte[] byteArray)
        {
            try
            {
                Stream byteArrayInputStream = new MemoryStream(byteArray);

                byteArrayInputStream.ReadByte();

                string str = MsgMacro.CNPartVipBatchModifyMsgTag + MsgMacro.mainDivide1;

                byte[] timeArray = new byte[4];
                byteArrayInputStream.Read(timeArray, 0, 4);
                int lastUpdateTime = ByteConvert.byteArrayToInt(timeArray);
                str += lastUpdateTime + MsgMacro.mainDivide1;

                int k = (int)byteArrayInputStream.ReadByte();

                str += k.ToString();
                byteArrayInputStream.Close();
                return str;

            }
            catch (Exception ex)
            {
                //ex.printStackTrace();
                //if (ClientApp.isWriterErrInfo) 
                //{
                //    new ErrInfo().WriterErrInfo("", "", ex);
                //}
                ErrInfo.WriterErrInfo("MsgParser", "ERPVIPBatchModify", ex);
            }
            return "";
        }

        //public static string VipSalesAll(byte[] byteArray)
        //{
        //    try
        //    {
        //        Stream byteArrayInputStream = new MemoryStream(byteArray);

        //        byteArrayInputStream.ReadByte();

        //        string str = MsgMacro.CNPartSalesAllMsgTag + MsgMacro.mainDivide1;

        //        byte[] timeArray = new byte[4];
        //        byteArrayInputStream.Read(timeArray, 0, 4);
        //        int lastUpdateTime = ByteConvert.byteArrayToInt(timeArray);
        //        str += lastUpdateTime + MsgMacro.mainDivide1;

        //        int k = (int)byteArrayInputStream.ReadByte();

        //        str += k.ToString();
        //        byteArrayInputStream.Close();
        //        return str;

        //    }
        //    catch (Exception ex)
        //    {
        //        //Console.WriteLine(ex.Message.ToString());
        //        ErrInfo.WriterErrInfo("MsgParser", "VipInfomationAll", ex);
        //    }
        //    return "";
        //}
    }
}

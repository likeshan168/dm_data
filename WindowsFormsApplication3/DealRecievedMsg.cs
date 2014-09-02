using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WindowsFormsApplication3
{
    class DealRecievedMsg
    {
        public CommonMsg commonMsg;
        private bool isWrite = false;
        WebVipOperator vipOperator = new WebVipOperator();
        AgentOperator agentOperator = new AgentOperator();
        public DealRecievedMsg(CommonMsg commonMsg)
        {
            this.commonMsg = commonMsg;
        }

        public RecievedMsg dealMsg()
        {
            try
            {
                CheckFunction checkFunction = new CheckFunction(commonMsg);
                CheckMsg checkMsg = checkFunction.checkAll(); //处理数据  写入临时表
                RecievedMsg recievedMsg;

                long time = commonMsg.msgBlock.time * 1000L;
                string timeStr = TimeFormat.getTime(time);
                Console.WriteLine("时间字符串==={0}", timeStr);
                recievedMsg = new RecievedMsg(timeStr, commonMsg.sessionId, checkMsg.msgType, checkMsg.pass, commonMsg.msgBlock.msg, commonMsg.msgBlock.memo, checkMsg.infoMsg, true);
                // 存储发送数据
                string sendMsg = "";
                if (!recievedMsg.format)
                {
                    MemoryStream byteArrayOutputStream = new MemoryStream();

                    // 写操作代码
                    byteArrayOutputStream.WriteByte((byte)recievedMsg.msgType);

                    // 写时间
                    int time1 = commonMsg.msgBlock.time;
                    byte[] timeArray = ByteConvert.intToByteArray(time1);
                    byteArrayOutputStream.Write(timeArray, 0, timeArray.Length);

                    // 写状态标志
                    if (recievedMsg.format)
                    {
                        byteArrayOutputStream.WriteByte((byte)DataOperation.SUCCESS);
                    }
                    else
                    {
                        byteArrayOutputStream.WriteByte((byte)DataOperation.FAILURE);
                    }

                    sendMsg = recievedMsg.replyMsg + "\n原始信息:\n" + recievedMsg.originalMsg;
                    Console.WriteLine(sendMsg);
                    byte[] sendMsgArray = Encoding.Default.GetBytes(sendMsg);

                    byte[] msgLngthArr = ByteConvert.shortToByteArray((short)sendMsgArray.Length);

                    byteArrayOutputStream.Write(msgLngthArr, 0, msgLngthArr.Length);

                    byteArrayOutputStream.Write(sendMsgArray, 0, sendMsgArray.Length);

                    byte[] byteArr = byteArrayOutputStream.ToArray();

                    byteArrayOutputStream.Close();

                    byte[] uploadInfo = new CreateSendData().insertSendMsgTable(CommandCode.FROM_DM_WRITE_SENDMSG, recievedMsg.sessionId, 1, 2, byteArr);
                    byte[] returnInfoArr = new SendAndReceive().mySendAndReceive(uploadInfo);
                    string returnInfo = new ReturnData().isSucceedInsertTableSendMsg(returnInfoArr);
                    if (returnInfo != null)
                    {
                        Console.WriteLine("插入send_msg表不成功！");
                    }
                    else
                    {
                        if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFile\\" + commonMsg.id))
                        { //如果文件存在
                            File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFile\\" + commonMsg.id);
                            bool isSucceed = true;
                            Console.WriteLine("222222" + isSucceed);
                        }
                    }
                }


                //if (recievedMsg.msgType == DataOperation.VIPLogin)
                //{
                //    Console.WriteLine("进入vip登陆===========VIPLogin");

                //    //vip信息修改

                //    string str = recievedMsg.originalMsg;
                //    string[] allLine = OperateString.getStringCollection(str, MsgMacro.mainDivide1);

                //    long time2 = Convert.ToInt32(allLine[1]) * 1000L;
                //    Console.WriteLine("time:" + time2);
                //    string timeStr2 = TimeFormat.getTime(time2);

                //    //WebVipOperator viplogin = new WebVipOperator();
                //    string log = vipOperator.VipLogin(allLine[2].ToString(), allLine[3].ToString(),allLine[4].ToString());

                //    byte[] byteArr = Encoding.Default.GetBytes(log);
                //    byte[] uploadInfo = new CreateSendData().insertSendMsgTable(CommandCode.FROM_DM_WRITE_SENDMSG, recievedMsg.sessionId, 1, 2, byteArr);
                //    byte[] returnInfoArr = new SendAndReceive().mySendAndReceive(uploadInfo);
                //    string returnInfo = new ReturnData().isSucceedInsertTableSendMsg(returnInfoArr);
                //    if (returnInfo != null)
                //    {
                //        Console.WriteLine("插入send_msg表不成功！");
                //    }
                //    else
                //    {
                //        // File file = new File();
                //        if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFile\\" + commonMsg.id))
                //        {
                //            File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFile\\" + commonMsg.id);
                //            bool isSucceed = true;
                //            Console.WriteLine("222222" + isSucceed);
                //        }

                //        isWrite = true;
                //    }

                //}

                //else if (recievedMsg.msgType == DataOperation.VIPSelect)
                //{//2009-04-27　以后不会用到该段程序。曼洒特VIP查询网站直接访问数据库。不用平台访问。
                //    Console.WriteLine("进入vip查询===========VIPSelect");
                //    string str = recievedMsg.originalMsg;
                //    string[] allLine = OperateString.getStringCollection(str, MsgMacro.mainDivide1);

                //    long time2 = Convert.ToInt32(allLine[1]) * 1000L;
                //    Console.WriteLine("time:" + time2);
                //    string timeStr2 = TimeFormat.getTime(time2);

                //    string log = vipOperator.VipSelect(allLine[2].ToString());

                //    byte[] byteArr = Encoding.Default.GetBytes(log);
                //    byte[] uploadInfo = new CreateSendData().insertSendMsgTable(CommandCode.FROM_DM_WRITE_SENDMSG, recievedMsg.sessionId, 1, 2, byteArr);
                //    byte[] returnInfoArr = new SendAndReceive().mySendAndReceive(uploadInfo);
                //    string returnInfo = new ReturnData().isSucceedInsertTableSendMsg(returnInfoArr);
                //    if (returnInfo != null)
                //    {
                //        Console.WriteLine("插入send_msg表不成功！");
                //    }
                //    else
                //    {
                //        // File file = new File();
                //        if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFile\\" + commonMsg.id))
                //        {
                //            File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFile\\" + commonMsg.id);
                //            bool isSucceed = true;
                //            Console.WriteLine("222222" + isSucceed);
                //        }
                //        isWrite = true;
                //    }
                //}


                //else if (recievedMsg.msgType == DataOperation.VIPUpdateEmail)
                //{
                //    Console.WriteLine("进入vip修改Email地址===========VIPUpdateEmail");
                //    string str = recievedMsg.originalMsg;
                //    string[] allLine = OperateString.getStringCollection(str, MsgMacro.mainDivide1);

                //    long time2 = Convert.ToInt32(allLine[1]) * 1000L;
                //    Console.WriteLine("time:" + time2);
                //    string timeStr2 = TimeFormat.getTime(time2);

                //    string log = vipOperator.VipUpdateEmailAddress(recievedMsg.sessionId,allLine[2].ToString(), allLine[3].ToString());

                //    if (log.IndexOf("Success") > 0)
                //    {
                //        string ss = vipOperator.SetSend;
                //        byte[] byteArr = vipOperator.DMSendVipMod(DataOperation.VIPEdit, time2, Encoding.Default.GetBytes(ss));
                //        byte[] uploadInfo = new CreateSendData().insertSendMsgTable(CommandCode.FROM_DM_WRITE_CLIENTMSG, recievedMsg.sessionId, 1, 0, byteArr);
                //        byte[] returnInfoArr = new SendAndReceive().mySendAndReceive(uploadInfo);
                //        string returnInfo = new ReturnData().isSucceedInsertTableSendMsg(returnInfoArr);

                //        if (returnInfo != null)
                //        {
                //            Console.WriteLine("VIP修改插入中转平台Client_msg表失败！");

                //        }
                //        else
                //        {
                //            if (vipOperator.VipTempModify(recievedMsg.sessionId))
                //            {
                //                Console.WriteLine("VipTempModify flag change Success");
                //            }
                //        }

                //        if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFile\\" + commonMsg.id))
                //        { //如果文件存在
                //            File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFile\\" + commonMsg.id);
                //            bool isSucceed = true;
                //            Console.WriteLine("222222" + isSucceed);
                //        }

                //        isWrite = true;

                //    }
                //    //byte[] byteArr = Encoding.Default.GetBytes(log);
                //    //byte[] uploadInfo = new CreateSendData().insertSendMsgTable(CommandCode.FROM_DM_WRITE_SENDMSG, recievedMsg.sessionId, 1, 2, byteArr);
                //    //byte[] returnInfoArr = new SendAndReceive().mySendAndReceive(uploadInfo);
                //    //string returnInfo = new ReturnData().isSucceedInsertTableSendMsg(returnInfoArr);
                //    //if (returnInfo != null)
                //    //{
                //    //    Console.WriteLine("插入send_msg表不成功！");
                //    //}
                //    //else
                //    //{
                //    //    // File file = new File();
                //    //    if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFile\\" + commonMsg.id))
                //    //    {
                //    //        File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFile\\" + commonMsg.id);
                //    //        bool isSucceed = true;
                //    //        Console.WriteLine("222222" + isSucceed);
                //    //    }
                //    //    isWrite = true;
                //    //}

                //}
                //else if (recievedMsg.msgType == DataOperation.VIPMod)
                //{
                //    Console.WriteLine("进入vip修改===========VIPMod");
                //    string str = recievedMsg.originalMsg;
                //    string[] allLine = OperateString.getStringCollection(str, MsgMacro.mainDivide1);

                //    long time2 = Convert.ToInt32(allLine[1]) * 1000L;
                //    Console.WriteLine("time:" + time2);
                //    string timeStr2 = TimeFormat.getTime(time2);


                //    string log = vipOperator.VipModifyValue(recievedMsg.sessionId, allLine[2].ToString());

                //    //byte[] byteArr = Encoding.Default.GetBytes(log);
                //    //byte[] uploadInfo = new CreateSendData().insertSendMsgTable(CommandCode.FROM_DM_WRITE_SENDMSG, recievedMsg.sessionId, 1, 2, byteArr);
                //    //byte[] returnInfoArr = new SendAndReceive().mySendAndReceive(uploadInfo);
                //    //string returnInfo = new ReturnData().isSucceedInsertTableSendMsg(returnInfoArr);
                //    //if (returnInfo != null)
                //    //{
                //    //    Console.WriteLine("插入send_msg表不成功！");
                //    //}
                //    //else
                //    //{
                //    if (log.IndexOf("Success") > 0)
                //    {
                //        string ss = vipOperator.SetSend;
                //      byte[] byteArr = vipOperator.DMSendVipMod(DataOperation.VIPEdit, time2, Encoding.Default.GetBytes(ss));
                //      byte[] uploadInfo = new CreateSendData().insertSendMsgTable(CommandCode.FROM_DM_WRITE_CLIENTMSG, recievedMsg.sessionId, 1, 0, byteArr);
                //      byte[] returnInfoArr = new SendAndReceive().mySendAndReceive(uploadInfo);
                //      string returnInfo = new ReturnData().isSucceedInsertTableSendMsg(returnInfoArr);

                //        if (returnInfo != null)
                //        {
                //            Console.WriteLine("VIP修改插入中转平台Client_msg表失败！");

                //        }
                //        else
                //        {
                //            if (vipOperator.VipTempModify(recievedMsg.sessionId))
                //            {
                //                Console.WriteLine("VipTempModify flag change Success");
                //            }
                //        }

                //        if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFile\\" + commonMsg.id))
                //        { //如果文件存在
                //            File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFile\\" + commonMsg.id);
                //            bool isSucceed = true;
                //            Console.WriteLine("222222" + isSucceed);
                //        }

                //        isWrite = true;
                //    }

                //}

                //else if (recievedMsg.msgType == DataOperation.AgentLogin)
                //{
                //    Console.WriteLine("进入Agent登陆===========AgentLogin");

                //    //vip信息修改

                //    string str = recievedMsg.originalMsg;
                //    string[] allLine = OperateString.getStringCollection(str, MsgMacro.mainDivide1);

                //    long time2 = Convert.ToInt32(allLine[1]) * 1000L;
                //    Console.WriteLine("time:" + time2);
                //    string timeStr2 = TimeFormat.getTime(time2);

                //    //WebVipOperator viplogin = new WebVipOperator();
                //    string log = agentOperator.AgentLogin(allLine[2].ToString(), allLine[3].ToString());

                //    byte[] byteArr = Encoding.Default.GetBytes(log);
                //    byte[] uploadInfo = new CreateSendData().insertSendMsgTable(CommandCode.FROM_DM_WRITE_SENDMSG, recievedMsg.sessionId, 1, 2, byteArr);
                //    byte[] returnInfoArr = new SendAndReceive().mySendAndReceive(uploadInfo);
                //    string returnInfo = new ReturnData().isSucceedInsertTableSendMsg(returnInfoArr);
                //    if (returnInfo != null)
                //    {
                //        Console.WriteLine("插入send_msg表不成功！");
                //    }
                //    else
                //    {
                //        // File file = new File();
                //        if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFile\\" + commonMsg.id))
                //        {
                //            File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFile\\" + commonMsg.id);
                //            bool isSucceed = true;
                //            Console.WriteLine("222222" + isSucceed);
                //        }

                //        isWrite = true;
                //    }
                //}


                //else if (recievedMsg.msgType == DataOperation.AgentMod)
                //{
                //    Console.WriteLine("进入Agent修改===========AgentMod");
                //    string str = recievedMsg.originalMsg;
                //    string[] allLine = OperateString.getStringCollection(str, MsgMacro.mainDivide1);

                //    long time2 = Convert.ToInt32(allLine[1]) * 1000L;
                //    Console.WriteLine("time:" + time2);
                //    string timeStr2 = TimeFormat.getTime(time2);


                //    string log = agentOperator.VipModifyValue(recievedMsg.sessionId, allLine[2].ToString());

                //    byte[] byteArr = Encoding.Default.GetBytes(log);
                //    byte[] uploadInfo = new CreateSendData().insertSendMsgTable(CommandCode.FROM_DM_WRITE_SENDMSG, recievedMsg.sessionId, 1, 2, byteArr);
                //    byte[] returnInfoArr = new SendAndReceive().mySendAndReceive(uploadInfo);
                //    string returnInfo = new ReturnData().isSucceedInsertTableSendMsg(returnInfoArr);
                //    if (returnInfo != null)
                //    {
                //        Console.WriteLine("插入send_msg表不成功！");
                //    }

                //    if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFile\\" + commonMsg.id))
                //    { //如果文件存在
                //        File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFile\\" + commonMsg.id);
                //        bool isSucceed = true;
                //        Console.WriteLine("222222" + isSucceed);
                //    }

                //    isWrite = true;
                //}


                //else if (recievedMsg.msgType == DataOperation.AgentSelect)
                //{
                //    Console.WriteLine("进入Agent查询===========AgentSelect");
                //    string str = recievedMsg.originalMsg;
                //    string[] allLine = OperateString.getStringCollection(str, MsgMacro.mainDivide1);

                //    long time2 = Convert.ToInt32(allLine[1]) * 1000L;
                //    Console.WriteLine("time:" + time2);
                //    string timeStr2 = TimeFormat.getTime(time2);


                //    string log = agentOperator.VipSelectWeb(recievedMsg.sessionId, allLine);

                //    byte[] byteArr = Encoding.Default.GetBytes(log);
                //    byte[] uploadInfo = new CreateSendData().insertSendMsgTable(CommandCode.FROM_DM_WRITE_SENDMSG, recievedMsg.sessionId, 1, 2, byteArr);
                //    byte[] returnInfoArr = new SendAndReceive().mySendAndReceive(uploadInfo);
                //    string returnInfo = new ReturnData().isSucceedInsertTableSendMsg(returnInfoArr);
                //    if (returnInfo != null)
                //    {
                //        Console.WriteLine("插入send_msg表不成功！");
                //    }

                //    if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFile\\" + commonMsg.id))
                //    { //如果文件存在
                //        File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFile\\" + commonMsg.id);
                //        bool isSucceed = true;
                //        Console.WriteLine("222222" + isSucceed);
                //    }

                //    isWrite = true;
                //}


                //else if (recievedMsg.msgType == DataOperation.VIPRegister)
                //{
                //    Console.WriteLine("进入VIP注册===========VIPRegister");
                //    string str = recievedMsg.originalMsg;
                //    string[] allLine = OperateString.getStringCollection(str, MsgMacro.mainDivide1);

                //    long time2 = Convert.ToInt32(allLine[1]) * 1000L;
                //    Console.WriteLine("time:" + time2);
                //    string timeStr2 = TimeFormat.getTime(time2);

                //    string log = agentOperator.VipRegisterWeb(recievedMsg.sessionId,allLine);

                //    //byte[] byteArr = Encoding.Default.GetBytes(log);
                //    //byte[] uploadInfo = new CreateSendData().insertSendMsgTable(CommandCode.FROM_DM_WRITE_SENDMSG, recievedMsg.sessionId, 1, 2, byteArr);
                //    //byte[] returnInfoArr = new SendAndReceive().mySendAndReceive(uploadInfo);
                //    //string returnInfo = new ReturnData().isSucceedInsertTableSendMsg(returnInfoArr);
                //    //if (returnInfo != null)
                //    //{
                //    //    Console.WriteLine("插入send_msg表不成功！");
                //    //}

                //    //if (log)
                //    //{



                //        if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFile\\" + commonMsg.id))
                //        { //如果文件存在
                //            if (log.Trim().Length <= 0)
                //            {
                //                File.Copy("C:\\DMService\\" + ClientApp.localBase + "\\TempFile\\" + commonMsg.id, "C:\\DMService\\" + ClientApp.localBase + "\\TempFile\\" + commonMsg.id, false);
                //            }

                //            File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFile\\" + commonMsg.id);
                //            bool isSucceed = true;
                //            Console.WriteLine("222222" + isSucceed);
                //        }

                //        isWrite = true;
                //    //}
                //}

                //else if (recievedMsg.msgType == DataOperation.AgentSelect)
                //{
                //    Console.WriteLine("进入代理商查询===========AgentSelect");

                //    string str = recievedMsg.originalMsg;
                //    string[] allLine = OperateString.getStringCollection(str, MsgMacro.mainDivide1);

                //    long time2 = Convert.ToInt32(allLine[1]) * 1000L;
                //    Console.WriteLine("time:" + time2);
                //    string timeStr2 = TimeFormat.getTime(time2);

                //    string log = vipOperator.VipSelect(allLine[2].ToString());

                //    byte[] byteArr = Encoding.Default.GetBytes(log);
                //    byte[] uploadInfo = new CreateSendData().insertSendMsgTable(CommandCode.FROM_DM_WRITE_SENDMSG, recievedMsg.sessionId, 1, 2, byteArr);
                //    byte[] returnInfoArr = new SendAndReceive().mySendAndReceive(uploadInfo);
                //    string returnInfo = new ReturnData().isSucceedInsertTableSendMsg(returnInfoArr);
                //    if (returnInfo != null)
                //    {
                //        Console.WriteLine("插入send_msg表不成功！");
                //    }
                //    else
                //    {
                //        // File file = new File();
                //        if (File.Exists(@"C:\DMService\TempFile\" + commonMsg.id))
                //        {
                //            File.Delete(@"C:\DMService\TempFile\" + commonMsg.id);
                //            bool isSucceed = true;
                //            Console.WriteLine("222222" + isSucceed);
                //        }
                //        isWrite = true;
                //    }

                //}

                if (isWrite)
                {

                    if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFile\\" + commonMsg.id))
                    { //如果文件存在
                        // Console.WriteLine("删除文件:\t" + file.getName());
                        File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFile\\" + commonMsg.id);
                        //Console.WriteLine("111111" + isSucceed);
                    }
                }

                return recievedMsg;
            }





            catch (Exception ex)
            {
                //  Console.WriteLine("Grace Print Err:" + ex.Message.ToString());
                ErrInfo.WriterErrInfo("DealRecievedMsg", "dealMsg", ex);

                if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFile\\" + commonMsg.id))
                { //如果文件存在
                    File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFile\\" + commonMsg.id);
                    bool isSucceed = true;
                    Console.WriteLine("222222" + isSucceed);
                }
            }

            return null;
        }



    }
}

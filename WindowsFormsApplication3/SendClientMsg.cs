using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data.SqlClient;
namespace WindowsFormsApplication3
{
    public class SendClientMsg
    {
          public Thread thread;
          string returnInfo;
          int MYid;
          public SendClientMsg()
            {
                thread = new Thread(new ThreadStart(ThreadRunSendClientmsg));
                thread.Name = "SendClientMsg";
                thread.Start();
            }

          public void ThreadRunSendClientmsg()
         {
             while (true)
             {
                    long sleepTime = 1000 * 5;
                    if (!ClientApp.isServerOpen)
                    {
                        try
                        {
                            //Thread.CurrentThread.sleep(60 * 1000);
                            Thread.Sleep(5 * 1000);
                        }
                        catch (ThreadInterruptedException ex)
                        {
                            ErrInfo.WriterErrInfo("SendClientMsg", "ThreadRunSendClientmsg", ex);
                        }
                        continue;
                    }

                    if (ClientApp.isServerOpen)
                    {
                        try
                        {
                            Thread.Sleep((int)sleepTime);
                            long beginTime = mySystem.currentTimeMillis();

                            ClientData senddata = new ClientData();

                            // senddata.VIPRegisterTempStor();
                            
                            //查询DM中tempstor表，得到上传服务器的数据
                            ClientMsg[] sendmsg=senddata.getClientMsg();
                            for (int i = 0; i < sendmsg.Length; i++)
                            {
                                MYid = sendmsg[i].id;//autoid

                                byte[] uploadInfo = null;

                                if (sendmsg[i].DataType == DataOperation.VIPDownland)
                                {
                                    //上传数据，将结果存在uploadInfo中.
                                    Console.WriteLine("*********手动下载vip资料的请求开始发送到服务器上(0x30)*********");
                                    uploadInfo = new CreateSendData().insertSendMsgTable(CommandCode.FROM_DM_WRITE_CLIENTMSG, sendmsg[i].sessionId, 1, 0, sendmsg[i].Data);//Data就是datatype的字节数组0x30的字节数组形式
                                }
                                //新增的就是为手动下载vip制卡信息
                                #region 新增的就是为手动下载vip制卡信息 0x61
                                if (sendmsg[i].DataType == DataOperation.VIPCardDownload)
                                {
                                    //上传数据，将结果存在uploadInfo中.
                                    Console.WriteLine("*********vip制卡信息的请求开始发送到服务器上(0x61)*********");
                                    uploadInfo = new CreateSendData().insertSendMsgTable(CommandCode.FROM_DM_WRITE_CLIENTMSG, sendmsg[i].sessionId, 1, 0, sendmsg[i].Data);
                                }
                                #endregion

                                if (sendmsg[i].DataType == DataOperation.VIPConfig)
                                {
                                    Console.WriteLine("*********vip配置信息请求开始发送到服务器上(0x31)*********");
                                    if (sendmsg[i].Data == null)
                                    {
                                        bool flag = new ClientData().DeleteTempStor(sendmsg[i].id, sendmsg[i].sessionId);
                                        if (flag ==true)
                                        {
                                            ErrInfo.WriterErrInfo("SendClientMsg", "ThreadRunSendClientMsg", "VIP配置信息不存在．不能执行请求.已将该请求删除!请先检查DataInfo表是否有数据再发送VIP配置信息!");
                                            Console.WriteLine("vip配置信息不存在．不能执行请求.已将该请求删除!请先检查DataInfo表是否有数据再发送VIP配置信息!");
                                        }
                                        continue;
                                    }
                                    else
                                    {
                                        uploadInfo = new CreateSendData().insertSendMsgTable(CommandCode.FROM_DM_WRITE_CLIENTMSG, sendmsg[i].sessionId, 1, 2, sendmsg[i].Data);
                                    }
                                }
                                if (sendmsg[i].DataType == DataOperation.VIPRegTempStor)
                                {
                                    Console.WriteLine("*********vip注册信息(0x24)上传到服务器*********");
                                    uploadInfo = new CreateSendData().insertSendMsgTable(CommandCode.FROM_DM_WRITE_CLIENTMSG, sendmsg[i].sessionId, 1, 0, sendmsg[i].Data);
                                }

                                if (sendmsg[i].DataType == DataOperation.VIPBatchModify)//0x34
                                {
                                    Console.WriteLine("*********vip资料批量修改(0x34)*********");
                                    uploadInfo = new CreateSendData().insertSendMsgTable(CommandCode.FROM_DM_WRITE_CLIENTMSG, sendmsg[i].sessionId, 1, 0, sendmsg[i].Data);
                                }
                                //这里是讲dm网站中的数据更新到erp软件中
                                if (sendmsg[i].DataType == DataOperation.VIPUpLoad)
                                {
                                    Console.WriteLine("*********vip基本资料导入到erp软件中(0x35)*********");
                                    uploadInfo = new CreateSendData().insertSendMsgTable(CommandCode.FROM_DM_WRITE_CLIENTMSG, sendmsg[i].sessionId, 1, 0, sendmsg[i].Data);
                                }

                                if (sendmsg[i].DataType == DataOperation.VIPSalesDown)
                                {
                                    Console.WriteLine("*********营业员资料下载(0x36)*********");
                                    uploadInfo = new CreateSendData().insertSendMsgTable(CommandCode.FROM_DM_WRITE_CLIENTMSG, sendmsg[i].sessionId, 1, 0, sendmsg[i].Data);
                                }
                                if (sendmsg[i].DataType == DataOperation.VIPShopsDown)
                                {
                                    Console.WriteLine("*********门店资料下载(0x38)*********");
                                    uploadInfo = new CreateSendData().insertSendMsgTable(CommandCode.FROM_DM_WRITE_CLIENTMSG, sendmsg[i].sessionId, 1, 0, sendmsg[i].Data);
                                }

                                byte[] returnInfoArr = new SendAndReceive("SendClientMsg").mySendAndReceive(uploadInfo);
                                returnInfo = new ReturnData().isSucceedInsertTableSendMsg(returnInfoArr);

                                if (returnInfo != null)
                                {
                                    Console.WriteLine("请求数据插入服务端Client_msg表失败！");
                                    senddata.InsertTempReturn(sendmsg[i].sessionId, Encoding.Default.GetBytes("插入Client_msg表失败"));
                                }
                                else
                                {
                                    bool UPflag = ClientData.ChangeClientMsgFlag(MYid);
                                    if (UPflag)
                                    {
                                        Console.WriteLine("表tempstor中状态标志修改成功");
                                    }
                                    else
                                    {
                                        Console.WriteLine("表tempstor中状态标志修改失败");
                                        senddata.DeleteTempStor(sendmsg[i].id, sendmsg[i].sessionId);
                                    }

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("类SendClientMsg方法ThreadRunSendClientmsg出现异常==={0}",ex.Message);
                            Thread.Sleep((int)sleepTime);
                            ErrInfo.WriterErrInfo("SendClientMsg", "ThreadRunSendClientmsg", ex);

                        }
                    }

             }
         }
          public void exitThread()
          {
              thread.Abort();
          }


        
    }
}

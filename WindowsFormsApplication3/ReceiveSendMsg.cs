using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace WindowsFormsApplication3
{
    class ReceiveSendMsg
    {
        public Thread thread;
        public ReceiveSendMsg()
        {
            thread = new Thread(new ThreadStart(ThreadrunSendMsg));
            thread.Name = "ReceiveSendMsg";
            thread.Start();
        }

        public void ThreadrunSendMsg()
        {
            while (true)
            {
                long sleepTime = 1000 * 5;
                if (!ClientApp.isServerOpen)
                {
                    try
                    {
                        //Thread.CurrentThread.sleep(60 * 1000);
                        Thread.Sleep(5 * 1000);//5秒
                    }
                    catch (ThreadInterruptedException ex)
                    {
                        ErrInfo.WriterErrInfo("ReceiveSendMsg", "ThreadrunSendMsg----Sleep", ex);
                        Console.WriteLine("类ReceiveSendMsg方法ThreadrunSendMsg出现异常==={0}",ex.Message);
                    }
                    continue;
                }

                if (ClientApp.isServerOpen)//
                {
                    try
                    {
                        Thread.Sleep((int)sleepTime);
                        long beginTime = mySystem.currentTimeMillis();
                        byte[] uploadInfo = new CreateSendData().getClientMsgTableInfo(CommandCode.GetSendMsgCode);//200203
                        byte[] returnInfoArr = new SendAndReceive("ReceiveSendMsg").mySendAndReceive(uploadInfo);
                        CommonMsg[] commonMsgArr = new ReturnData().getSendMsgTableInfo(returnInfoArr);

                        int rowCount = 0;
                        int[] returnInfo = null;
                        if (commonMsgArr != null && commonMsgArr.Length != 0)
                        {
                            byte[] uploadInfoArr = new CreateSendData().deleteClientMsgTableInfo(CommandCode.DEALRECIVEDMSG_SEND_MSG_DELETE, commonMsgArr);//100146
                            byte[] returnInfoArray = new SendAndReceive().mySendAndReceive(uploadInfoArr);//把需要删除的数据的操作码100146以及要删除的数据的id发送给服务器，将已经成功获取的数据进行删除操作
                            returnInfo = new ReturnData().isSucceedDeleteClientMsgTableInfo(returnInfoArray);
                            if (returnInfo == null)
                            {
                                rowCount = 0;
                            }
                            else
                            {
                                rowCount = returnInfo.Length;
                            }
                        }

                        if (!Directory.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS"))
                        {
                            Directory.CreateDirectory("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS");
                        }


                        for (int i = 0; i < rowCount; i++)
                        {
                            try
                            {
                                for (int j = 0; j < commonMsgArr.Length; j++)
                                {
                                    if (commonMsgArr[j].id == returnInfo[i])
                                    {
                                        Console.WriteLine("写入TempFileRS文件的是:" + commonMsgArr[j].id.ToString());//数据的id
                                        Stream stream = new FileStream("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + returnInfo[i], FileMode.Create, FileAccess.Write, FileShare.None);
                                        BinaryFormatter formatter = new BinaryFormatter();
                                        formatter.Serialize(stream, commonMsgArr[j]);
                                        stream.Close();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("类ReceiveSendMsg方法ThreadrunSendMsg对象序列化出错==={0}",ex.Message);
                                ErrInfo.WriterErrInfo("ReceiveSendMsg", "ThreadrunSendMsg----对象序列化出错", ex);
                            }
                        }
                        string[] fileLength = Directory.GetFiles("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS");
                        rowCount = fileLength.Length;
                        CommonMsg[] commonMsg = new CommonMsg[rowCount];


                        for (int i = 0; i < rowCount; i++)
                        {
                            try
                            {
                                FileStream fileStream = new FileStream(fileLength[i], FileMode.Open, FileAccess.Read, FileShare.Read);
                                BinaryFormatter b = new BinaryFormatter();//SoapFormatter
                                commonMsg[i] = (CommonMsg)b.Deserialize(fileStream);
                                Console.WriteLine("反序列化TempFileRS文件夹下文件的ID：" + commonMsg[i].id);
                                fileStream.Close();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("类ReceiveSendMsg方法ThreadrunSendMsg反对象序列化出错==={0}", ex.Message);
                                ErrInfo.WriterErrInfo("ReceiveSendMsg2", "ThreadrunSendMsg----反对象序列化出错", ex);
                            }
                        }
                        for (int i = 0; i < rowCount; i++)
                        {
                            DelReciveSendMsg dealRecievedMsg = new DelReciveSendMsg(commonMsg[i]);

                            dealRecievedMsg.dealMsg();
                        }

                    }
                    catch
                    {
                        Thread.Sleep((int)sleepTime);
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

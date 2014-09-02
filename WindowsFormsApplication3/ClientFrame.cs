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
    public class ClientFrame
    {

        public class RetrieveThread
        {
            public Thread thread;
            Stream stream = null;
            FileStream fileStream;
            public RetrieveThread()
            {
                thread = new Thread(new ThreadStart(runThread));
                thread.Name = "RetrieveThread";
                thread.Start();
            }

            public void runThread()
            {
                while (true)
                {
                    long sleepTime = 1000 * 5;
                    if (!ClientApp.isServerOpen)
                    {
                        try
                        {
                            Thread.Sleep(5 * 1000);
                        }
                        catch (ThreadInterruptedException ex)
                        {
                            ErrInfo.WriterErrInfo("ClientFrame", "runThreadSleep", ex);
                        }
                        continue;
                    }

                    if (ClientApp.isServerOpen)
                    {
                        try
                        {
                            Thread.Sleep((int)sleepTime);
                            long beginTime = mySystem.currentTimeMillis();

                            byte[] uploadInfo = new CreateSendData().getClientMsgTableInfo(CommandCode.GetClientCode);//200201(发送公司id以及操作操作码)
                            byte[] returnInfoArr = new SendAndReceive("RetrieveThread").mySendAndReceive(uploadInfo);
                            CommonMsg[] commonMsgArr = new ReturnData().getClientMsgTableInfo(returnInfoArr);

                            int rowCount = 0;
                            int[] returnInfo = null;
                            if (commonMsgArr != null && commonMsgArr.Length != 0)
                            {
                                byte[] uploadInfoArr = new CreateSendData().deleteClientMsgTableInfo(CommandCode.DEALRECIVEDMSG_CLIENT_MSG_DELETE, commonMsgArr);
                                byte[] returnInfoArray = new SendAndReceive().mySendAndReceive(uploadInfoArr);
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


                            if (!Directory.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFile"))
                            {
                                Directory.CreateDirectory("C:\\DMService\\" + ClientApp.localBase + "\\TempFile");
                            }
                            if (!Directory.Exists("C:\\DMService\\" + ClientApp.localBase + "\\BackFile"))
                            {
                                Directory.CreateDirectory("C:\\DMService\\" + ClientApp.localBase + "\\BackFile");
                            }

                            for (int i = 0; i < rowCount; i++)
                            {
                                try
                                {
                                    for (int j = 0; j < commonMsgArr.Length; j++)
                                    {
                                        if (commonMsgArr[j].id == returnInfo[i])
                                        {
                                            Console.WriteLine("写入文件的是:" + commonMsgArr[j].id.ToString());
                                            stream = new FileStream("C:\\DMService\\" + ClientApp.localBase + "\\TempFile\\" + returnInfo[i], FileMode.Create, FileAccess.Write, FileShare.None);
                                            BinaryFormatter formatter = new BinaryFormatter();
                                            formatter.Serialize(stream, commonMsgArr[j]);
                                            stream.Close();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ErrInfo.WriterErrInfo("ClientFrame", "RetrieveThread----对象序列化出错", ex);
                                    Console.WriteLine("类RetrieveThread方法runThread对象序列化出现异常==={0}", ex.Message);
                                }
                                finally
                                {
                                    if (stream != null)
                                    {
                                        stream.Close();
                                    }
                                }
                            }
                            string[] fileLength = Directory.GetFiles("C:\\DMService\\" + ClientApp.localBase + "\\TempFile");
                            rowCount = fileLength.Length;
                            CommonMsg[] commonMsg = new CommonMsg[rowCount];
                            for (int i = 0; i < rowCount; i++)
                            {
                                try
                                {
                                    fileStream = new FileStream(fileLength[i], FileMode.Open, FileAccess.Read, FileShare.Read);
                                    BinaryFormatter b = new BinaryFormatter();//SoapFormatter
                                    commonMsg[i] = (CommonMsg)b.Deserialize(fileStream);
                                    Console.WriteLine("反序列化ID：" + commonMsg[i].id);
                                    fileStream.Close();
                                }
                                catch (Exception ex)
                                {
                                    ErrInfo.WriterErrInfo("ClientFrame", "RetrieveThread----反对象序列化出错", ex);
                                    Console.WriteLine("类RetrieveThread方法runThread对象反序列化出现异常==={0}", ex.Message);
                                }
                                finally
                                {
                                    fileStream.Close();
                                }
                            }

                            for (int i = 0; i < rowCount; i++)
                            {
                                DealRecievedMsg dealRecievedMsg = new DealRecievedMsg(commonMsg[i]);
                                RecievedMsg recievedMsg = dealRecievedMsg.dealMsg();
                            }

                        }
                        catch (Exception ex)
                        {
                            ErrInfo.WriterErrInfo("ClientFrame", "runThread", ex);
                            Console.WriteLine("类RetrieveThread方法runThread出现异常==={0}", ex.Message);
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
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WindowsFormsApplication3
{
    class TestConnection
    {
        public Thread restoreThread;
        public int testCount = 0;

        public TestConnection()
        {
            restoreThread = new Thread(new ThreadStart(AutoRestore));
            restoreThread.Name = "TeseConnectionThread";

        }
        public void threadStart()
        {
            restoreThread.Start();
        }

        public void AutoRestore()
        {
            while (true)
            {
                byte[] uploadInfo = new CreateSendData().testConnection();//测试连接服务器
                byte[] returnInfoArr = new SendAndReceive("TestConnection").
                    mySendAndReceive(uploadInfo);//把数据发送到服务器，并获取服务器的 返回数据
                string returnInfo = new ReturnData().testConnection(returnInfoArr);
                Console.WriteLine(returnInfo);
                if (returnInfo.CompareTo("已登陆！") == 0)
                { //正常情况
                    ClientApp.isServerOpen = true;
                    ClientApp.test_connect_time = 1000 * 20;
                    testCount = 0;
                }
                else if (returnInfo.CompareTo("未登陆！") == 0)
                { //服务器重起后
                    uploadInfo = new CreateSendData().logonCommand();
                    returnInfoArr = new SendAndReceive("TestConnection").mySendAndReceive(uploadInfo);
                    Console.WriteLine("服务器重启后:");
                    returnInfo = new ReturnData().isLogonSucced(returnInfoArr);
                    if (returnInfo == null)
                    { //登陆成功
                        ClientApp.isServerOpen = true;
                        ClientApp.test_connect_time = 1000 * 20;
                        testCount = 0;
                    }
                    else
                    { //登陆错误
                        ClientApp.isServerOpen = false;
                        ClientApp.test_connect_time = 1000 * 5;
                        testCount++;
                        if (testCount > 5)
                        {
                            testCount = 0;
                            ClientApp.test_connect_time = 1000 * 5 * 60;
                        }
                    }
                }
                else
                { //连接错误　服务器问题或网络问题
                    ClientApp.isServerOpen = false;
                    ClientApp.test_connect_time = 1000 * 5;
                    testCount++;
                    if (testCount > 5)
                    {
                        testCount = 0;
                        ClientApp.test_connect_time = 1000 * 5 * 60;
                    }
                }
                try
                {
                    Thread.Sleep((int)ClientApp.test_connect_time);
                }
                catch (Exception ex)//InterruptedException
                {
                    Console.WriteLine("类TestConnection方法AutoRestore出现异常==={0}", ex.Message);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace WindowsFormsApplication3
{
    class SendAndReceive
    {

        private byte[] receiveDataByteArray = null;
        private byte[] sendDataArray = null;
        private BufferedStream bsInOn = null;
        // private BufferedStream BSon = null;
        private NetworkStream ns = null;
        private string className = "";
        private int errType = 0;
        private Socket socket = null;

        public SendAndReceive(String className)
        {
            this.className = className;
        }
        public SendAndReceive()
        {
        }

        public byte[] mySendAndReceive(byte[] sendDataArray)
        {
            errType = 0;

            try
            {
                IPAddress ip = IPAddress.Parse(ClientApp.serverIP);
                int post = ClientApp.PORT;
                IPEndPoint point1 = new IPEndPoint(ip, post);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.SendTimeout = 1000 * 60;
                Console.WriteLine("与服务端[{0}]:[{1}]进行连接！", ip.ToString(), post.ToString());
                socket.Connect(point1);
                ns = new NetworkStream(socket);
                Console.WriteLine("与服务端[{0}]:[{1}]成功建立了连接！", ip.ToString(), post.ToString());
            }
            catch (SocketException ex)　//UnknownHostException
            {
                Console.WriteLine("与服务端连接失败！===={0}", ex.Message);
                ErrInfo.WriterErrInfo("SendAndReceive-----------" + className, "mySendAndReceive", ex);
                errType = 1; //err 1 无法确定主机的 IP 地
            }
            catch (Exception ex)
            {
                Console.WriteLine("与服务端连接失败！===={0}", ex.Message);
                ErrInfo.WriterErrInfo("SendAndReceive-----------" + className, "mySendAndReceive", ex);
                errType = 2; //err 2 创建套接字时发生 I/O 错误
            }
            #region 这里是说明发生了错误的情况
            if (errType != 0)//这里是说明发生了错误的情况
            {
                if (socket != null)
                {
                    try
                    {
                        socket.Close();
                        socket = null;
                    }
                    catch (IOException ex)
                    {
                        ErrInfo.WriterErrInfo("SendAndReceive-----------" + className, "mySendAndReceive", ex);
                        Console.WriteLine("类SendAndReceive方法mySendAndReceive中出现异常！===={0}", ex.Message);
                    }
                }
                return returnInfoArr();
            }
            #endregion
            try
            {
                bsInOn = new BufferedStream(ns);
            }
            catch (IOException ex)
            {
                ErrInfo.WriterErrInfo("SendAndReceive-----------" + className, "mySendAndReceive", ex);
                Console.WriteLine("类SendAndReceive方法mySendAndReceive中出现异常！===={0}", ex.Message);
                errType = 3; //err 3 创建输入流时发生 I/O 错误
            }

            #region 这里也是发生错误的情况
            if (errType != 0)
            {
                try
                {
                    if (bsInOn != null)
                    {
                        bsInOn.Close();
                        bsInOn = null;
                    }
                }
                catch (IOException ex)
                {
                    ErrInfo.WriterErrInfo("SendAndReceive-----------" + className, "mySendAndReceive", ex);
                    Console.WriteLine("类SendAndReceive方法mySendAndReceive中出现异常！===={0}", ex.Message);
                }

                if (socket != null)
                {
                    try
                    {
                        socket.Close();
                        socket = null;
                    }
                    catch (IOException ex)
                    {
                        ErrInfo.WriterErrInfo("SendAndReceive-----------" + className, "mySendAndReceive", ex);
                        Console.WriteLine("类SendAndReceive方法mySendAndReceive中出现异常！===={0}", ex.Message);
                    }
                }

                return returnInfoArr();
            }
            #endregion

            this.sendDataArray = sendDataArray; //上传数据

            try
            {
                if (this.sendDataArray != null)
                {
                    if (isConnectionSucceed())
                    { //测试连接
                        if (isSendSucceed())
                        { //上传数据
                            if (isReceiveSucceed())
                            { //下载数据

                                byte[] tempArr = new byte[receiveDataByteArray.Length + 1];

                                tempArr[0] = (byte)1; //正确标志
                                for (int i = 0; i < receiveDataByteArray.Length; i++)
                                { //添加正确标志
                                    tempArr[i + 1] = receiveDataByteArray[i];
                                }
                                return tempArr; //关闭
                            }
                        }
                    }
                }
                return returnInfoArr();
            }
            catch (Exception ex)
            {
                ErrInfo.WriterErrInfo("SendAndReceive-----------" + className, "mySendAndReceive", ex);
                Console.WriteLine("类SendAndReceive方法mySendAndReceive中出现异常！===={0}", ex.Message);
                return returnInfoArr();
            }
            finally
            {
                try
                {
                    bsInOn.Close();
                    bsInOn = null;
                }
                catch (IOException ex)
                {
                    ErrInfo.WriterErrInfo("SendAndReceive-----------" + className, "mySendAndReceive", ex);
                    Console.WriteLine("类SendAndReceive方法mySendAndReceive中出现异常！===={0}", ex.Message);
                }

                try
                {
                    if (socket != null)
                    {
                        socket.Close();
                        socket = null;
                    }
                }
                catch (IOException ex)
                {
                    ErrInfo.WriterErrInfo("SendAndReceive-----------" + className, "mySendAndReceive", ex);
                    Console.WriteLine("类SendAndReceive方法mySendAndReceive中出现异常！===={0}", ex.Message);
                }
            }
        }


        public Boolean isConnectionSucceed() //throws Exception 
        { //测试连接
            String username = ClientApp.username;
            String password = ClientApp.password;
            byte[] usernameArr = Encoding.Default.GetBytes(username);
            byte[] passwordArr = Encoding.Default.GetBytes(password);


            MemoryStream arrOut = new MemoryStream();
            arrOut.WriteByte((byte)usernameArr.Length);
            arrOut.Write(usernameArr, 0, usernameArr.Length);
            arrOut.WriteByte((byte)passwordArr.Length);
            arrOut.Write(passwordArr, 0, passwordArr.Length);
            arrOut.WriteByte((byte)CommandCode.UnionFlag);

            byte[] tempArr = arrOut.ToArray();

            try
            {
                arrOut.Close();
                arrOut = null;
            }
            catch (IOException ex)
            {
                ErrInfo.WriterErrInfo("SendAndReceive-----------" + className, "isConnectionSucceed", ex);
                Console.WriteLine("类SendAndReceive方法isConnectionSucceed中出现异常！===={0}", ex.Message);
            }

            int tempArrLength = tempArr.Length;
            try
            {
                Console.WriteLine("上传用户名和密码====" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                //上传测试数据
                bsInOn.Write(ByteConvert.intToByteArray(tempArrLength), 0, 4);
                bsInOn.Write(tempArr, 0, tempArrLength);
                bsInOn.Flush();

                //下载上传数据长度
                byte[] returnLengthArr = new byte[4];
                int off = 0;
                int len = 0;
                while (off < 4)
                {
                    len = bsInOn.Read(returnLengthArr, off, 4 - off);
                    if (len < 0)
                    {
                        break;
                    }
                    else
                    {
                        off += len;
                    }
                }

                int tempLength = ByteConvert.byteArrayToInt(returnLengthArr);

                if (tempLength == tempArrLength)
                { //长度相等
                    Console.WriteLine("上传用户名和密码成功！====" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                    bsInOn.WriteByte((byte)1);
                    bsInOn.Flush();

                    int isSucced = bsInOn.ReadByte();

                    if (isSucced == 1)
                    {
                        Console.WriteLine("验证用户名和密码成功！====" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("验证用户名和密码失败！====" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        errType = 6; //err 6 用户不存在或密码错误
                        return false;
                    }
                }
                else
                { //长度不等
                    Console.WriteLine("上传用户名和密码失败！,数据丢失！====" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                    bsInOn.WriteByte((byte)0);
                    bsInOn.Flush();
                    errType = 4;
                    return false; //err 4 传输中数据丢失
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("上传用户名和密码异常！====" +ex.Message);
                ErrInfo.WriterErrInfo("SendAndReceive-----------" + className, "isConnectionSucceed", ex);
                errType = 5;
                return false; //err 5 传输中出错
            }
        }

        public byte[] returnInfoArr()
        { //err 错误
            String errInfo = "";
            switch (errType)
            {
                case 0: //正确
                    break;
                case 1:
                    errInfo = "网络超时，请等会重试！";
                    break;
                case 2:
                    errInfo = "网络繁忙，请等会重试！";
                    break;
                case 3:
                    errInfo = "创建输入流时发生 I/O 错误！";
                    break;
                case 4:
                    errInfo = "传输中数据丢失！";
                    break;
                case 5:
                    errInfo = "传输中出错！";
                    break;
                case 6:
                    errInfo = "用户不存在或密码错误！";
                    break;
            }

            byte[] returnArray = new byte[Encoding.Default.GetBytes(errInfo).Length + 1];
            if (errType == 0)
            {
                return null; //正确
            }
            else
            {
                returnArray[0] = 0; //错误
            }

            byte[] errInfoArr = Encoding.Default.GetBytes(errInfo);
            for (int i = 0; i < errInfoArr.Length; i++)
            {
                returnArray[i + 1] = errInfoArr[i];
            }
            return returnArray;
        }

        /// <summary>
        /// 数据请求发送成功 200203  10008 10008的长度
        /// </summary>
        /// <returns></returns>
        public bool isSendSucceed() //throws Exception 
        {
            try
            {
                //上传数据
                bsInOn.Write(ByteConvert.intToByteArray(sendDataArray.Length), 0, 4);
                bsInOn.Write(sendDataArray, 0, sendDataArray.Length);
                bsInOn.Flush();

                //下载上传数据长度
                //System.out.println("上传数据");
                byte[] returnLengthArr = new byte[4];
                int off = 0;
                int len = 0;
                while (off < 4)
                {
                    len = bsInOn.Read(returnLengthArr, off, 4 - off);
                    if (len < 0)
                    {
                        break;
                    }
                    else
                    {
                        off += len;
                    }
                }
                //System.out.println("接收长度");
                int tempLength = ByteConvert.byteArrayToInt(returnLengthArr);
                if (tempLength == sendDataArray.Length)
                { //长度相等
                    bsInOn.WriteByte((byte)1);
                    bsInOn.Flush();
                    return true;
                }
                else
                { //长度不等
                    bsInOn.WriteByte((byte)0);
                    bsInOn.Flush();
                    errType = 4;
                    return false; //err 4 传输中数据丢失
                }
            }
            catch (IOException ex)
            {
                bsInOn.Close();
                ErrInfo.WriterErrInfo("SendAndReceive-----------" + className, "isSendSucceed", ex);
                errType = 6; //err 6 用户不存在或密码错误
                return false;
            }
        }

        public bool isReceiveSucceed() //throws Exception 
        {
            try
            {
                //接收数据长度（由于数据可能比较大，首先发送过来的是数据的长度，由于数据的长度是整数，所以接受长度就比较小）
                byte[] dataLengthArr = new byte[4];
                int off = 0;
                int len = 0;
                while (off < 4)
                {
                    len = bsInOn.Read(dataLengthArr, off, 4 - off);

                    if (len < 0)
                    {
                        break;
                    }
                    else
                    {
                        off += len;
                    }
                }
                int dataLang = ByteConvert.byteArrayToInt(dataLengthArr);
                //接收数据
                receiveDataByteArray = new byte[dataLang];
                off = 0;
                len = 0;
                while (off < dataLang)
                {
                    len = bsInOn.Read(receiveDataByteArray, off, dataLang - off);

                    if (len < 0)
                    {
                        break;
                    }
                    else
                    {
                        off += len;
                    }
                }

                Console.WriteLine("接受到得数据长度:" + off.ToString());


                bsInOn.Write(ByteConvert.intToByteArray(off), 0, 4); //返回接收的数据长度
                bsInOn.Flush();

                byte isSucceed = (byte)bsInOn.ReadByte();

                if (isSucceed == 1)
                {
                    int isCompress = receiveDataByteArray[0];
                    // System.out.println("是否压缩" + isCompress);

                    byte[] compressedByteArray = new byte[receiveDataByteArray.Length - 1];
                    for (int i = 0; i < receiveDataByteArray.Length - 1; i++)
                    { //去除压缩标记
                        compressedByteArray[i] = receiveDataByteArray[i + 1];
                    }

                    if (isCompress == 1)
                    { //解压
                        receiveDataByteArray = ZipCompress.DeCompress(compressedByteArray);
                    }
                    else
                    {
                        receiveDataByteArray = compressedByteArray;
                    }

                    return true;
                }
                else
                {
                    errType = 4;
                    return false; //数据长度不符
                }
            }
            catch (IOException ex)
            {
                ErrInfo.WriterErrInfo("SendAndReceive-----------" + className, "isReceiveSucceed", ex);
            }
            return false;
        }
    }
}

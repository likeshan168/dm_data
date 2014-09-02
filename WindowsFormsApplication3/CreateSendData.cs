using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;

namespace WindowsFormsApplication3
{

    class CreateSendData
    {
        private MemoryStream Msout = null;
        private int dataType; //连接测试
        string userName = "";
        string password = "";
        private BinaryWriter Mout = null;
        MemoryStream mes;

        public byte[] testConnection()
        {
            userName = ClientApp.username;
            password = ClientApp.password;//获取配置文件里自己设置的连接字符串
            //ClientApp.serverIP = "192.168.0.80";
            dataType = CommandCode.TEST_SOCKET;
            mes = new MemoryStream();
            Mout = new BinaryWriter(mes);

            byte[] dataTypeArr = ByteConvert.intToByteArray(dataType);
            byte[] usernameArr = Encoding.Default.GetBytes(userName);
            byte[] passwordArr = Encoding.Default.GetBytes(password);
            Mout.Write(dataTypeArr, 0, 4);
            Mout.Write((byte)usernameArr.Length);
            Mout.Write(usernameArr, 0, usernameArr.Length);
            Mout.Write((byte)passwordArr.Length);
            Mout.Write(passwordArr, 0, passwordArr.Length);
            Mout.Write((byte)CommandCode.UnionFlag);
            byte[] tempArr = mes.ToArray();
            try
            {
                Mout.Close();
            }
            catch (IOException ex)
            {
                ErrInfo.WriterErrInfo("CreateSendData", "testConnection", ex);
                Console.WriteLine("类CreateSendData方法testConnection出现异常==={0}", ex.Message);
            }

            return compressData(tempArr);

        }



        public byte[] logonCommand()
        {
            byte[] tempArr = null;
            //格式：数据类型＋用户名长度＋用户名＋密码长度＋密码
            try
            {
                dataType = CommandCode.LOGON_COMMAND; //登陆请求
                Console.WriteLine("logonCommand:操作码======" + dataType);
                Msout = new MemoryStream();
                string username = ClientApp.username;
                Console.WriteLine("logonCommand:用户名======" + username);
                string password = ClientApp.password;
                Console.WriteLine("logonCommand:密码======" + password);
                byte[] dataTypeArr = ByteConvert.intToByteArray(dataType);

                byte[] usernameArr = Encoding.Default.GetBytes(username);
                byte[] passwordArr = Encoding.Default.GetBytes(password);

                Msout.Write(dataTypeArr, 0, 4); //数据类型　
                Msout.WriteByte((byte)usernameArr.Length);
                Msout.Write(usernameArr, 0, usernameArr.Length);
                Msout.WriteByte((byte)passwordArr.Length);
                Msout.Write(passwordArr, 0, passwordArr.Length);
                Msout.WriteByte((byte)CommandCode.UnionFlag);
                tempArr = Msout.ToArray();
            }
            catch (SocketException ex)
            {
                ErrInfo.WriterErrInfo("CreateSendData", "logonCommand==SocktErr", ex);
                Console.WriteLine("类CreateSendData方法logonCommand出现异常:{0}", ex.Message);
            }
            try
            {
                Msout.Close();
            }
            catch (IOException ex)
            {
                ErrInfo.WriterErrInfo("CreateSendData", "logonCommand==IOErr", ex);
                Console.WriteLine("类CreateSendData方法logonCommand出现异常:{0}", ex.Message);
            }
            //格式：是否压缩＋数据
            return compressData(tempArr);
        }


        public byte[] getClientMsgTableInfo(int datatype)
        {
            if (datatype == CommandCode.GetClientCode)//200201
            {
                dataType = CommandCode.GetClientCode;
            }
            else if (datatype == CommandCode.GetSendMsgCode)//200203
            {
                dataType = CommandCode.GetSendMsgCode;
            }

            Msout = new MemoryStream();
            Console.WriteLine("操作码==={0}", datatype);
            string id = ClientApp.id;//
            Console.WriteLine("公司ID===" + id);
            byte[] dataTypeArr = ByteConvert.intToByteArray(dataType);
            byte[] idArr = Encoding.Default.GetBytes(id);
            Msout.Write(dataTypeArr, 0, 4);
            Msout.WriteByte((byte)idArr.Length);
            Msout.Write(idArr, 0, idArr.Length);
            Console.WriteLine("公司ID长度===" + idArr.Length.ToString());
            byte[] tempArr = Msout.ToArray();
            try
            {
                Msout.Close();
            }
            catch (IOException ex)
            {
                Msout.Close();
                ErrInfo.WriterErrInfo("CreateSendData", "getClientMsgTableInfo", ex);
                Console.WriteLine("类CreateSendData方法getClientMsgTableInfo出现异常==={0}", ex.Message);
            }
            //格式：是否压缩＋数据
            return compressData(tempArr);

        }

        public byte[] insertSendMsgTable(int datatype, string sessionId, int message, int target, byte[] data)
        {
            if (datatype == CommandCode.FROM_DM_WRITE_SENDMSG)
            {
                dataType = CommandCode.FROM_DM_WRITE_SENDMSG;
            }
            else if (datatype == CommandCode.FROM_DM_WRITE_CLIENTMSG)
            {
                dataType = CommandCode.FROM_DM_WRITE_CLIENTMSG;
            }
            //插入 dm_send_msg 表中的数据信息  //发送短消息

            MemoryStream mout = new MemoryStream();
            //1、命令码
            mout.Write(ByteConvert.intToByteArray(dataType), 0, 4);
            Console.WriteLine("操作码==={0}", datatype);
            //2、公司id
            byte[] clientIdArr = Encoding.Default.GetBytes(ClientApp.id.Trim());
            mout.Write(ByteConvert.intToByteArray(clientIdArr.Length), 0, 4);
            mout.Write(clientIdArr, 0, clientIdArr.Length);
            Console.WriteLine("公司ID==={0}", ClientApp.id);
            //3、会话Id
            byte[] sessionIdArr = Encoding.Default.GetBytes(sessionId.Trim());
            mout.Write(ByteConvert.intToByteArray(sessionIdArr.Length), 0, 4);
            mout.Write(sessionIdArr, 0, sessionIdArr.Length);
            Console.WriteLine("会话ID==={0}", sessionId);
            //4、源标记
            mout.Write(ByteConvert.intToByteArray(message), 0, 4);
            Console.WriteLine("源标记==={0}", message);
            //5、目标
            mout.Write(ByteConvert.intToByteArray(target), 0, 4);
            Console.WriteLine("目标==={0}", target);
            //6、时间
            string currentTime = TimeFormat.getCurrentTime().Trim();
            Console.WriteLine("时间字符串==={0}", currentTime);
            byte[] localTimeArr = Encoding.Default.GetBytes(currentTime); //本地当前时间
            mout.Write(ByteConvert.intToByteArray(localTimeArr.Length), 0, 4);
            mout.Write(localTimeArr, 0, localTimeArr.Length);
            //7、数据 0x30 就是说明说明类型的操作码
            if (data != null)
            {
                mout.Write(ByteConvert.intToByteArray(data.Length), 0, 4);
                mout.Write(data, 0, data.Length);
            }
            byte[] tempArr = mout.ToArray();
            try
            {
                mout.Close();
            }
            catch (IOException ex)
            {
                Console.WriteLine("类CreateSendData方法insertSendMsgTable出现异常==={0}", ex.Message);
                ErrInfo.WriterErrInfo("CreateSendData", "insertSendMsgTable", ex);
            }

            return compressData(tempArr); //格式：是否压缩＋数据
        }


        public byte[] compressData(byte[] tempArr)   //是否压缩数据
        {
            byte[] compressArr = ZipCompress.Compress(tempArr);
            byte[] uploadDataArr = null;
            if (compressArr.Length > tempArr.Length)
            { //压缩后大
                uploadDataArr = new byte[tempArr.Length + 1];
                uploadDataArr[0] = 0; //不压缩
                for (int i = 0; i < tempArr.Length; i++)
                {
                    uploadDataArr[i + 1] = tempArr[i];
                }
            }
            else
            {
                uploadDataArr = new byte[compressArr.Length + 1];
                uploadDataArr[0] = 1; //压缩
                for (int i = 0; i < compressArr.Length; i++)
                {
                    uploadDataArr[i + 1] = compressArr[i];
                }
            }
            return uploadDataArr;
        }

        public byte[] deleteClientMsgTableInfo(int datatype, CommonMsg[] commonMsg)
        {
            if (datatype == CommandCode.DEALRECIVEDMSG_CLIENT_MSG_DELETE)
            {
                dataType = CommandCode.DEALRECIVEDMSG_CLIENT_MSG_DELETE;
            }
            else if (datatype == CommandCode.DEALRECIVEDMSG_SEND_MSG_DELETE)//100146
            {
                dataType = CommandCode.DEALRECIVEDMSG_SEND_MSG_DELETE;
            }
            Console.WriteLine("操作码==={0}", datatype);
            Msout = new MemoryStream();
            byte[] dataTypeArr = ByteConvert.intToByteArray(dataType);//100146
            Msout.Write(dataTypeArr, 0, 4);
            int count = commonMsg.Length;//获取要删除的数据个数
            Console.WriteLine("有" + count + "个要删除的数据");
            Msout.WriteByte((byte)count);

            for (int i = 0; i < count; i++)
            {
                int id = commonMsg[i].id;
                Console.WriteLine("需要删除ID:" + id);
                byte[] idArr = ByteConvert.intToByteArray(id);
                Msout.Write(idArr, 0, 4);
            }
            byte[] tempArr = Msout.ToArray();
            try
            {
                Msout.Close();
            }
            catch (IOException ex)
            {
                ErrInfo.WriterErrInfo("CreateSendData", "deleteClientMsgTableInfo", ex);
                Console.WriteLine("类CreateSendData方法deleteClientMsgTableInfo出现异常==={0}", ex.Message);
            }
            //格式：是否压缩＋数据
            return compressData(tempArr);
        }
    }
}

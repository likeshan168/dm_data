using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WindowsFormsApplication3
{
    class ReturnData
    {
        public string testConnection(byte[] returnInfoArr)
        {    //返回：
            //已登陆
            //未登陆
            //错误信息
            if (returnInfoArr[0] == (byte)0)
            {
                return errInfo(returnInfoArr);
            }

            byte[] returnLengthArr = new byte[4];

            for (int i = 0; i < 4; i++)
            { //第一位是是否正确
                returnLengthArr[i] = returnInfoArr[i + 1];
            }
            int length = ByteConvert.byteArrayToInt(returnLengthArr);

            byte[] returnInfoArray = new byte[length];
            for (int i = 0; i < length; i++)
            {
                returnInfoArray[i] = returnInfoArr[i + 5];
            }
            string returnInfo = Encoding.Default.GetString(returnInfoArray);


            return returnInfo;
        }


        public String errInfo(byte[] returnInfoArr)
        {
            //返回
            //正确返回　null
            byte[] tempArr = null;
            if (returnInfoArr[0] == (byte)0)
            {
                tempArr = new byte[returnInfoArr.Length - 1];
                for (int i = 0; i < tempArr.Length; i++)
                { //去除网络错误标记
                    tempArr[i] = returnInfoArr[i + 1];
                }
                return Encoding.Default.GetString(tempArr);
            }
            else if (returnInfoArr[1] == (byte)0)
            { //去除处理错误标记
                tempArr = new byte[returnInfoArr.Length - 2];
                for (int i = 0; i < tempArr.Length; i++)
                {
                    tempArr[i] = returnInfoArr[i + 2];
                }
                return Encoding.Default.GetString(tempArr);
            }
            return null;
        }


        public string isLogonSucced(byte[] returnInfoArr)
        {
            //返回：null　登陆成功

            if (returnInfoArr[0] == (byte)0)
            {
                return errInfo(returnInfoArr);
            }

            Stream Min = new MemoryStream(returnInfoArr);
            Min.ReadByte();

            byte[] returnLengthArr = new byte[4];
            Min.Read(returnLengthArr, 0, 4);
            int length = ByteConvert.byteArrayToInt(returnLengthArr);

            byte[] returnInfoArray = new byte[length];
            Min.Read(returnInfoArray, 0, length);
            string returnInfo = Encoding.Default.GetString(returnInfoArray);
            Console.WriteLine("GracePrint:" + returnInfo);

            try
            {
                Min.Close();
            }
            catch (IOException ex)
            {
                //ex.printStackTrace();
                //if (ClientApp.isWriterErrInfo) 
                //{
                //  new ErrInfo().WriterErrInfo("", "", ex);
                //}
                ErrInfo.WriterErrInfo("ReturnData", "isLogonSucced", ex);
            }

            if (returnInfo.Trim().CompareTo("重复登陆！") == 0)
            {
                return returnInfo;
            }
            else
            {
                if (returnInfo.Trim().Length > 1)
                {
                    ClientApp.id = returnInfo;
                }
                else
                {
                    ClientApp.id = ClientApp.DefaultCompanyID;
                }
                Console.WriteLine("ClientApp.id =" + returnInfo);
            }
            return null;
        }




        public CommonMsg[] getClientMsgTableInfo(byte[] returnInfoArr)
        {
            //返回　CommonMsg
            CommonMsg[] commonMsg = null;
            if (returnInfoArr.Length > 0)
            {
                Stream Min = new MemoryStream(returnInfoArr);
                try
                {
                    int errCode = Min.ReadByte(); //第一位是是否正确
                    if (errCode == 0)
                    {
                        return null;
                    }
                    Min.Seek(4, SeekOrigin.Current); //数据长度
                    byte[] returnUnitArr = new byte[4];
                    Min.Read(returnUnitArr, 0, 4);
                    int unit = ByteConvert.byteArrayToInt(returnUnitArr);
                    commonMsg = new CommonMsg[unit];
                    for (int i = 0; i < unit; i++)
                    {
                        //格式：序号（4）＋客户编号长度（1）＋客户编号＋终端名称长度（1）＋终端名称＋终端编号长度（1）＋终端编号＋终端类型（4）＋操作时间长度（1）＋操作时间＋数据长度（4）＋数据
                        byte[] idArr = new byte[4];
                        Min.Read(idArr, 0, 4);
                        int id = ByteConvert.byteArrayToInt(idArr); //序号
                        Console.WriteLine("客户ID：" + id);

                        int sessionLength = Min.ReadByte();//会话编号长度
                        byte[] sessionArr = new byte[sessionLength];
                        Min.Read(sessionArr, 0, sessionArr.Length);
                        string sessionId = Encoding.Default.GetString(sessionArr);
                        Console.WriteLine("会话ID:" + sessionId);

                        byte[] sizeArr = new byte[4];
                        Min.Read(sizeArr, 0, 4);
                        int size = ByteConvert.byteArrayToInt(sizeArr); //数据长度
                        Console.WriteLine("数据长度:" + size);

                        byte[] dataArr = new byte[size];
                        Min.Read(dataArr, 0, dataArr.Length); //数据

                        MsgBlock msgBlock = null;
                        msgBlock = MsgParser.parse(dataArr);                                 
                        commonMsg[i] = new CommonMsg(id, sessionId, size, msgBlock);
                    }

                    Min.Close();
                }

                catch (IOException ex)
                {
                    ErrInfo.WriterErrInfo("ReturnData", "isLogonSucced", ex);
                    Console.WriteLine("类ReturnData方法getClientMsgTableInfo出现异常==={0}",ex.Message);
                    Min.Close();
                    return null;
                }
            }
            return commonMsg;
        }

        public CommonMsg[] getSendMsgTableInfo(byte[] returnInfoArr)
        {
            CommonMsg[] commonMsg = null;
            Stream Min = new MemoryStream(returnInfoArr);
            try
            {
                int errCode = Min.ReadByte(); //第一位是是否正确
                if (errCode == 0)
                {
                    return null;
                }

                Min.Seek(4, SeekOrigin.Current); //数据长度

                byte[] returnUnitArr = new byte[4];

                Min.Read(returnUnitArr, 0, 4);
                int unit = ByteConvert.byteArrayToInt(returnUnitArr);

                commonMsg = new CommonMsg[unit];

                for (int i = 0; i < unit; i++)
                {

                    byte[] idArr = new byte[4];
                    Min.Read(idArr, 0, 4);
                    int id = ByteConvert.byteArrayToInt(idArr); //序号
                    Console.WriteLine("客户ID：" + id);

                    int sessionLength = Min.ReadByte();//会话编号长度
                    byte[] sessionArr = new byte[sessionLength];
                    Min.Read(sessionArr, 0, sessionArr.Length);
                    string sessionId = Encoding.Default.GetString(sessionArr);
                    Console.WriteLine("会话ID:" + sessionId);

                    byte[] sizeArr = new byte[4];
                    Min.Read(sizeArr, 0, 4);
                    int size = ByteConvert.byteArrayToInt(sizeArr); //数据长度
                    Console.WriteLine("数据长度:" + size);

                    byte[] dataArr = new byte[size];
                    Min.Read(dataArr, 0, dataArr.Length); //数据

                    //MsgBlock msgBlock = null;
                    //msgBlock = MsgParser.parse(dataArr);
                    //// testcommonMsg[i] = new TestcommonMsg(id,sessionId,size,msgBlock);                                   
                    commonMsg[i] = new CommonMsg(id, sessionId, size, dataArr);
                }

                Min.Close();
            }
            catch (IOException ex)
            {
                Min.Close();
                ErrInfo.WriterErrInfo("ReturnData", "isLogonSucced", ex);
                return null;
             }
            return commonMsg;
        }
        /// <summary>
        /// 返回服务端删除成功的数据的id的数组
        /// </summary>
        /// <param name="returnInfoArr"></param>
        /// <returns></returns>
        public int[] isSucceedDeleteClientMsgTableInfo(byte[] returnInfoArr)
        {
            //返回
            //成功　返回　删除的id
            Stream Min = new MemoryStream(returnInfoArr);
            int isSucceed = Min.ReadByte();
            if (isSucceed == 0)
            {
                try
                {
                    Min.Close();
                }
                catch (IOException ex)
                {
                    ErrInfo.WriterErrInfo("ReturnData", "isLogonSucced", ex);
                    Console.WriteLine("类ReturnData方法isLogonSucced出现异常==={0}",ex.Message);
                }
                return null; //网络传输中出错
            }

            Min.Seek(4, SeekOrigin.Current);
            try
            {
                int count = Min.ReadByte(); //删除的个数
                int[] id = new int[count]; //删除的id
                for (int i = 0; i < count; i++)
                {
                    byte[] idArr = new byte[4];
                    Min.Read(idArr, 0, 4);
                    id[i] = ByteConvert.byteArrayToInt(idArr);
                }
                Min.Close();
                return id;
            }
            catch (IOException ex)
            {
                ErrInfo.WriterErrInfo("ReturnData", "isLogonSucced", ex);
                Console.WriteLine("类ReturnData方法isLogonSucced出现异常==={0}", ex.Message);
                return null;
            }
        }

        public string isSucceedInsertTableSendMsg(byte[] returnInfoArr)
        { //返回：null　插入成功

            if (returnInfoArr[0] == (byte)0)
            {
                return errInfo(returnInfoArr);
            }

            MemoryStream min = new MemoryStream(returnInfoArr);
            min.Seek(1, SeekOrigin.Current);

            byte[] returnLengthArr = new byte[4];
            min.Read(returnLengthArr, 0, 4);
            int length = ByteConvert.byteArrayToInt(returnLengthArr);
            Console.WriteLine("=======================================================================================lengthWidth：" + length.ToString());
            byte[] returnInfoArray = new byte[length];
            min.Read(returnInfoArray, 0, length);
            string returnInfo = Encoding.Default.GetString(returnInfoArray);
            Console.WriteLine("=======================================================================================返回值：" + returnInfo);
            try
            {
                min.Close();
            }
            catch (IOException ex)
            {
                //ex.printStackTrace();
                //if (ClientApp.isWriterErrInfo)
                //{
                //    new ErrInfo().WriterErrInfo("", "", ex);
                //}
                ErrInfo.WriterErrInfo("ReturnData", "isLogonSucced", ex);
            }

            if (returnInfo.Trim().CompareTo("成功插入！") == 0)
            {
                return null;
            }
            else
            {
                return returnInfo;
            }
        }



    }
}

//}

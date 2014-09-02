using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using log4net;
using System.Reflection;
namespace WindowsFormsApplication3
{
    class ErrInfo
    {
        public ErrInfo()
        { }

        static string nowTime = "";

        public static void WriterErrInfo(string className, string functionName, Exception errInfo)
        {


            try
            {
                if (creatErrFile())
                {
                    FileStream fs = new FileStream("C:\\DMService\\" + ClientApp.localBase + "\\errInfo\\" + nowTime.Substring(0, 8) + ".log", FileMode.Append,FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine("当前时间:\t" + TimeFormat.getCurrentTime());
                    sw.WriteLine("类名:\t" + className);
                    sw.WriteLine("方法名:\t" + functionName);
                    sw.WriteLine("错误信息:\t" + errInfo);
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                }
            }

            catch (IOException ex)
            {
                //如果发生 I/O 错误
                Console.WriteLine(ex.Message.ToString());
                //WriterErrInfo("ErrInfo", "creatErrFile", ex);
                //Log4netHelper.InvokeErrorLog(MethodBase.GetCurrentMethod().DeclaringType, ex.Message, ex);
            }
        }

        public static void WriterErrInfo(string className, string functionName, string errInfo)
        {


            try
            {
                if (creatErrFile())
                {
                    FileStream fs = new FileStream("C:\\DMService\\" + ClientApp.localBase + "\\errInfo\\" + nowTime.Substring(0, 8) + ".log", FileMode.Append);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine("当前时间:\t" + TimeFormat.getCurrentTime());
                    sw.WriteLine("类名:\t" + className);
                    sw.WriteLine("方法名:\t" + functionName);
                    sw.WriteLine("错误信息:\t" + errInfo);
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                }
            }

            catch (IOException ex)
            {
                //如果发生 I/O 错误
                Console.WriteLine(ex.Message.ToString());
               // WriterErrInfo("ErrInfo", "creatErrFile", ex);
               // Log4netHelper.InvokeErrorLog(MethodBase.GetCurrentMethod().DeclaringType, ex.Message, ex);
            }
        }

        public static bool creatErrFile()
        {
            //检查目录是否存在
            try
            {
                if (!Directory.Exists("C:\\DMService\\" + ClientApp.localBase + "\\errInfo"))
                {
                    Directory.CreateDirectory("C:\\DMService\\" + ClientApp.localBase + "\\errInfo");
                }
                //检查文件是否存在
                nowTime = TimeFormat.getCurrentTime();//当前时间
                string fileAdd = "C:\\DMService\\" + ClientApp.localBase + "\\errInfo\\" + nowTime.Substring(0, 8) + ".log";
                if (!File.Exists(fileAdd))
                {
                    File.Create(fileAdd);
                }
                return true;

            }
            catch (IOException ex)
            {
                //如果发生 I/O 错误
                Console.WriteLine(ex.Message.ToString());
               // WriterErrInfo("ErrInfo", "creatErrFile", ex);
                //Log4netHelper.InvokeErrorLog(MethodBase.GetCurrentMethod().DeclaringType, ex.Message, ex);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                //WriterErrInfo("ErrInfo", "creatErrFile", ex);
               // Log4netHelper.InvokeErrorLog(MethodBase.GetCurrentMethod().DeclaringType, ex.Message, ex);
                return false;
            }
        }
  
    }
}

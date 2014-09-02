using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WindowsFormsApplication3
{
    class CreatErrFile
    {
        // public static File errFile = null; 
        public static bool creatErrFile()
        {
            //检查目录是否存在
            try
            {
                if (!Directory.Exists("./errInfo"))
                {
                    Directory.CreateDirectory("./errInfo");
                }

                //检查文件是否存在
                string nowTime = TimeFormat.getCurrentTime();//当前时间
                string fileAdd = "./errInfo/" + nowTime.Substring(0, 8) + ".log";
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
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return false;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            System.Threading.Mutex mutex = new System.Threading.Mutex(false, "wst_process");
            //判断互斥体是否使用中。   
            bool Running = !mutex.WaitOne(0, false);
            if (!Running)
            {
                Application.Run(new frmMain());
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("数据处理平台已经启动！");
                Console.WriteLine(string.Format("{0}数据处理平台已经启动==={1}", ClientApp.username, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")));
                return;
            }
            

        }
    }
}

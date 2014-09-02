using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Data.OleDb;
using MySQLDriverCS;
using System.Data.Odbc;
using Microsoft.Win32;


namespace WindowsFormsApplication3
{
    public partial class frmMain : Form
    {
        int testint = 0;
        bool flag = false;

        private Icon mNetTrayIcon = new Icon("102.ico");
        private NotifyIcon TrayIcon;
        private ContextMenu notifyiconMnu;
        MenuItem[] mnuItms = new MenuItem[5];
        Thread ServiceMe;

        ClientFrame.RetrieveThread RClient;
        SendClientMsg SClient;
        ReceiveSendMsg RSendMsg;
        WEBCoupon webCoupon;
        VIPService vipser;
        SMSContent smscont;
        SmsSendDataClass sendclass;
        clsAutoSendSmsThread autoSms;
        //delegate void SetTextCallback(string text);
        Get_Kdt_Vip_Info vipKdt;
        Get_Vip_Kdt_Sale vipKdtale;
        public frmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Console.WriteLine(string.Format("{0}数据处理平台正在运行==={1}", ClientApp.username, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")));
            readAppconfig.readConfig();//读取配置配置

            init();//初始化窗体界面

            try
            {
                if (readAppconfig.CreateSQLTempTable())
                {
                    readAppconfig.CreateSmsSqlTable();

                    #region 修改：李克善
                    readAppconfig.procInsert();//更新表clientInfo中procText_insert字段的值（这两个都是为在进行vip信息进行插入的时候使用的）
                    readAppconfig.procUpdate();//更新表clientInfo中procText_update字段的值
                    #endregion
                    ServiceMe = new Thread(new ThreadStart(startData));
                    ServiceMe.Start();
                }
            }
            catch (Exception ex)
            {
                ErrInfo.WriterErrInfo("Main", "Main", ex);
                Console.WriteLine("系统出现异常:{0}==={1}", ex.Message, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            }

        }

        private void startData()
        {
            try
            {
                while (!flag)
                {
                    byte[] temp = new CreateSendData().logonCommand();
                    byte[] returnInfoArr = new SendAndReceive("logonCommand").mySendAndReceive(temp);
                    Console.WriteLine("1成功输出:" + Encoding.Default.GetString(returnInfoArr));
                    string ss = new ReturnData().isLogonSucced(returnInfoArr);
                    if (ss == null)
                    {
                        Console.WriteLine("2成功输出:" + Encoding.Default.GetString(returnInfoArr));
                        ClientApp.isServerOpen = true;
                        TestConnection Mcon = new TestConnection();
                        Mcon.threadStart();
                        // Process.Start(Application.StartupPath + "\\byteToSql.exe", "-" + ClientApp.localBase);//调用陶思宇的byteToSql.exe
                        RClient = new ClientFrame.RetrieveThread();//发送网站申请数据请求到105ServerProject 
                        autoSms = new clsAutoSendSmsThread();//自动短信发送（数据发送完之后会在表automessage中更新字段lastSendDate为发送短信的时间）
                        SClient = new SendClientMsg();//这个是获取dm的请求发送到服务器中去。(就是获取tempstor中的数据)
                        RSendMsg = new ReceiveSendMsg();//从serverProject接收定时下载及返回网站申请数据(如vip信息，销售信息,制卡信息等)
                        vipser = new VIPService();//处理VIP开卡，销售，积分换礼短信
                        smscont = new SMSContent();//发送批量短信(就是发送dm网站的直复短信)发送表smsModel_zf中的数据
                        sendclass = new SmsSendDataClass();//发送senddata表中的数据

                        vipKdt = new Get_Kdt_Vip_Info();//获取口袋通中的vip信息
                        vipKdtale = new Get_Vip_Kdt_Sale();//获取口袋通中的vip销售信息
                        bool bcoupon = false;
                        if (bcoupon)
                        {
                            webCoupon = new WEBCoupon();
                        }
                        testint = 0;
                        flag = true;
                    }
                    else
                    {
                        testint++;
                        if (testint % 5 == 0)
                        {
                            testint = 0;
                            Thread.Sleep(1000 * 10 * 60);
                        }
                        else
                        {
                            Thread.Sleep(1000 * 25);
                        }
                        flag = false;

                    }
                }
            }
            catch (Exception err)
            {
                ErrInfo.WriterErrInfo("Form1", "startData", err);
                Console.WriteLine("类Form1方法startData中出现异常==={0}", err.Message);
            }
        }
        private void label2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.Hide();
            this.mnuItms[2].Text = "显示主窗体";
            TrayIcon.ShowBalloonTip(1000, ClientApp.username + "数据处理平台", "界面已隐藏,双击该图标恢复显示.", ToolTipIcon.Info);
        }

        private void init()
        {
            this.ControlBox = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.None;
            TrayIcon = new NotifyIcon();
            TrayIcon.Icon = mNetTrayIcon;
            TrayIcon.Text = ClientApp.username + "数据处理平台";
            TrayIcon.Visible = true;
            TrayIcon.DoubleClick += new System.EventHandler(this.showWindow);

            mnuItms[0] = new MenuItem(ClientApp.username + "数据处理平台");

            mnuItms[1] = new MenuItem("-");

            mnuItms[2] = new MenuItem();
            mnuItms[2].Text = "隐藏主窗体";
            mnuItms[2].Click += new System.EventHandler(this.showWindow);
            mnuItms[2].DefaultItem = true;

            mnuItms[3] = new MenuItem("-");


            mnuItms[4] = new MenuItem();
            mnuItms[4].Text = "退出系统";
            mnuItms[4].Click += new System.EventHandler(this.Exit);

            notifyiconMnu = new ContextMenu(mnuItms);
            TrayIcon.ContextMenu = notifyiconMnu;
            lblState.Text = ClientApp.username + "数据处理平台运行中...";

            #region 开机启动的设置（新加的）
            //RegistryKey HKLM = Registry.LocalMachine;
            //RegistryKey Run = HKLM.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);//如果存在就打开读写
            //object value = Run.GetValue("DataProcessSystem");
            //string reallyValue = this.GetType().Assembly.Location;
            //if (value == null)
            //{
            //    lbl_start.Text = "设置开机启动";
            //}
            //else
            //{
            //    if (value.ToString().Equals(reallyValue, StringComparison.OrdinalIgnoreCase))
            //    {
            //        lbl_start.Text = "已设置开机启动";
            //    }
            //    else
            //    {
            //        lbl_start.Text = "设置开机启动";
            //        Run.DeleteValue("DataProcessSystem");
            //    }
            //}
            //HKLM.Close();
            #endregion

        }

        private void showWindow(object o, EventArgs e)
        {
            if (this.Visible == false)//如果窗体不可见
            {
                this.Show();
                this.mnuItms[2].Text = "隐藏主窗体";
                this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            }
            else if (this.WindowState == System.Windows.Forms.FormWindowState.Normal)
            {
                this.Hide();
                this.mnuItms[2].Text = "显示主窗体";
                //this.mnuItms[0].Text = ClientApp.username + "数据处理平台";
            }
        }

        private void Exit(object o, EventArgs e)
        {
            ServiceMe.Abort();
            TrayIcon.Visible = false;
            Application.Exit();
            System.Environment.Exit(System.Environment.ExitCode);
        }
        /// <summary>
        /// 开机启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void lbl_start_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        RegistryKey HKLM = Registry.LocalMachine;
        //        RegistryKey Run = HKLM.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");//如果存在就打开读写
        //        object value = Run.GetValue("DataProcessSystem");
        //        if (value == null)//如果不是开机启动，那就设置为开机启动
        //        {
        //            Run.SetValue("DataProcessSystem", this.GetType().Assembly.Location);
        //            HKLM.Close();
        //            lbl_start.Text = "已设置开机启动";
        //            MessageBox.Show("设置开机自动启动成功!", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //        else//如果是开机启动，那么就设置为不是开机启动
        //        {
        //            Run.DeleteValue("DataProcessSystem");
        //            HKLM.Close();
        //            lbl_start.Text = "设置开机启动";
        //            MessageBox.Show("已取消开机自动启动!", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //}
        /// <summary>
        /// 退出系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_exit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("您确定退出" + ClientApp.username + "数据处理平台！", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                Exit(sender, e);
            }
        }





    }
}


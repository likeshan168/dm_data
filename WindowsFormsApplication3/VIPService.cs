using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Data;

namespace WindowsFormsApplication3
{
    class VIPService
    {
        public Thread thVip;
        public VIPService()
        {
            thVip = new Thread(new ThreadStart(ThreadVip));
            thVip.Name = "vipService";
            thVip.Start();
        }
        public void ThreadVip()
        {
            while (true)
            {
                long sleepTime = 1000 * 5;
                if (!ClientApp.isServerOpen)
                {
                    try
                    {
                        Thread.Sleep((int)sleepTime);

                    }
                    catch (ThreadInterruptedException ex)
                    {
                        ErrInfo.WriterErrInfo("VIPService", "ThreadVip----Sleep", ex);
                        Console.WriteLine("类VIPService方法ThreadVip出现异常==={0}", ex.Message);
                    }
                    continue;
                }

                if (ClientApp.isServerOpen)
                {
                    try
                    {
                        if (ClientApp.SMSFlag == 1)//SMSFlag判断是否执行该线程中代码。1：执行；0：不执行。
                        {
                            string strautoid = "";

                            //调用webservice
                            Getcoupon.couponService dd = new Getcoupon.couponService();
                            //调用webservice的GetZF_SMS_DATA方法，获取数据
                            Console.WriteLine("获取表lpc_zfsms_table中的数据==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            DataSet sqldt = dd.GetZF_SMS_DATA(ClientApp.id);//获取表lpc_zfsms_table中的数据
                            int rowC = sqldt.Tables[0].Rows.Count;
                            if (rowC > 0)
                            {
                                Console.WriteLine("表lpc_zfsms_table中有数据==={0}条", rowC);
                                SendSMSDll smsd = new SendSMSDll();
                                //数据存入表中DM_zfsms_table
                                smsd.InsertBackupZF(sqldt);

                                clsDSet dset = new clsDSet();
                                DataAccess da = new DataAccess(dset);
                                //处理数据
                                foreach (DataRow dr in sqldt.Tables[0].Rows)
                                {
                                    dset.AutoId = Convert.ToInt32(dr["autoId"].ToString());
                                    Console.WriteLine("短息编号==={0}", dset.AutoId);
                                    dset.CompanyID = dr["companyid"].ToString();
                                    Console.WriteLine("公司ID==={0}", dset.CompanyID);

                                    dset.InsTime = Convert.ToDateTime(dr["insTime"].ToString());
                                    Console.WriteLine("短信日期==={0}", dset.InsTime.ToString("yyyy-MM-dd hh:mm:ss"));
                                    dset.SmsType = dr["smsType"].ToString();
                                    Console.WriteLine("短信类型==={0}", dset.SmsType);
                                    dset.SysContent = dr["sysContent"].ToString();
                                    Console.WriteLine("短信内容==={0}", dset.SysContent);
                                    dset.Istate = Convert.ToInt32(dr["state"].ToString());
                                    Console.WriteLine("短信状态标志==={0}", dset.Istate);
                                    //SendMsg方法是处理数据的主要方法
                                    bool success = da.SendMsg();
                                    if (success)
                                    {
                                        //处理成功，记录数据的ID
                                        Console.WriteLine("pos机短信处理成功==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                        strautoid = strautoid + dset.AutoId + ",";
                                    }
                                }
                                strautoid = strautoid.Substring(0, strautoid.Length - 1);
                                Console.WriteLine("处理成功的数据的ID==={0}", strautoid);

                                if (da.UpdateDll())
                                {
                                    //处理成功，调用webserice的SetSMS_DATA_STATE方法删除服务器端的数据，参数是公司ID,处理成功数据的ID
                                    string flag = dd.SetSMS_DATA_STATE(ClientApp.id, strautoid);//把表lpc_zfsms_table中处理成功的数据删除掉
                                    Console.WriteLine("调用webserice的SetSMS_DATA_STATE方法删除服务器端的数据,返回结果==={0}", flag);
                                }
                            }
                            else
                            {
                                Console.WriteLine("没有获取到表lpc_zfsms_table中的数据==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            }
                        }
                        Thread.Sleep(1000 * 5);
                    }
                    catch (Exception er)
                    {
                        Console.WriteLine("处理VIP开卡，销售，积分换礼短信出现异常==={0}",er.Message);
                        Thread.Sleep(1000 * 10);
                    }
                }
            }
        }
        public void exitThread()
        {
            thVip.Abort();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;
using System.Xml;
using System.Collections;
using System.Xml.Serialization;
using System.Data.SqlClient;

namespace WindowsFormsApplication3
{
    class SmsSendDataClass
    {

        public Thread thSendData;
        EmailPhone ep = new EmailPhone();
        SMSService.smsService service;
        int Icount = 0;
        DateTime ds = DateTime.Parse("01/01/1900 00:00:00");

        int AMten = 0;
        int PMthree = 0;

        public SmsSendDataClass()
        {
            thSendData = new Thread(new ThreadStart(ThreadSendData));
            thSendData.Name = "SMSSendData";
            thSendData.Start();
        }
        /// <summary>
        /// 发送表senddata表中的数据
        /// </summary>
        public void ThreadSendData()
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
                        Console.WriteLine("类SmsSendDataClass方法ThreadSendData出现异常==={0}", ex.Message);
                        ErrInfo.WriterErrInfo("SmsSendDataClass", "ThreadSendData----Sleep", ex);
                    }
                    continue;
                }
                if (ClientApp.isServerOpen)
                {
                    string autoid = "";

                    Console.WriteLine("发送表senddata中的数据==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                    try
                    {

                        //查询用户名和密码
                        string ss = "select uid,pwd from smsSysNum";
                        Console.WriteLine("获取短信用户名和密码==={0}", ss);
                        DataTable dt = new DataTable();
                        dt = SqlbyString(ss);

                        string udi = dt.Rows[0]["uid"].ToString();
                        string pwd = dt.Rows[0]["pwd"].ToString();
                        Console.WriteLine("短信用户名:{0} 密码:{1}==={2}", udi, pwd, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        //查询50条待发短信
                        ss = "select top 50 * from senddata";
                        Console.WriteLine("获取表senddata中的数据==={0}", ss);
                        dt = new DataTable();
                        dt = SqlbyString(ss);

                        DataTable newTb = new DataTable("Tbsms");
                        newTb.Columns.Add("autoid", typeof(int));
                        newTb.Columns.Add("Mobile", typeof(string));//手机号
                        newTb.Columns.Add("Content", typeof(string));//内容
                        newTb.Columns.Add("flag", typeof(int));//pri
                        newTb.Columns.Add("channel", typeof(int));//通道1
                        newTb.Columns.Add("mdate", typeof(string));//now
                        newTb.Columns.Add("state", typeof(string));//wait

                        //早晨10点自动给固定手机号发送一条短信
                        if (DateTime.Now.TimeOfDay.Hours == 10 && AMten == 0)
                        {
                            Console.WriteLine("早晨10点自动给固定手机号发送一条短信");
                            ds = DateTime.Now;
                            // newTb.Rows.Add(new object[] { 0, "13910030764", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ClientApp.SMSSuffix, 1, 1, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), "wait" });
                            // newTb.Rows.Add(new object[] { 0, "13910030764", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ClientApp.SMSSuffix, 1, 2, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), "wait" });
                            AMten++;
                        }
                        //下午3点自动给固定手机号发送一条短信
                        if (DateTime.Now.TimeOfDay.Hours == 16 && PMthree == 0)
                        {
                            Console.WriteLine("下午3点自动给固定手机号发送一条短信");
                            ds = DateTime.Now;
                            // newTb.Rows.Add(new object[] { 0, "13910030764", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ClientApp.SMSSuffix, 1, 1, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), "wait" });
                            // newTb.Rows.Add(new object[] { 0, "13910030764", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ClientApp.SMSSuffix, 1, 2, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), "wait" });
                            PMthree++;
                        }

                        if (ds.AddDays(1).Date == DateTime.Now.Date)
                        {
                            AMten = 0;
                            PMthree = 0;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            Icount++;

                            //Console.WriteLine("第--"+Icount+"--次优惠券短信处理开始时间：＝＝＝＝" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                autoid = autoid + dt.Rows[i]["ID"].ToString() + ",";

                                if (ep.IsChinaUnicomNumber(dt.Rows[i]["Mobile"].ToString()))
                                {
                                    string Modeltext = dt.Rows[i]["Text"].ToString();

                                    Console.WriteLine("短信内容==={0}", Modeltext);
                                    int msgCount = Modeltext.Length + ClientApp.SMSSuffix.Length;
                                    Console.WriteLine("短信内容长度==={0}", msgCount);
                                    if (msgCount > 70)
                                    {
                                        int count = (Modeltext.Length) / 59;// 60;
                                        int yacon = (Modeltext.Length) % 59;// 60;
                                        if (yacon > 0)
                                        { count++; }
                                        int index = 0;
                                        for (int ic = 1; ic <= count; ic++)
                                        {
                                            string sendStr = "[" + ic + "/" + count + "]" + (ic != count ? Modeltext.Substring(index, 59) + ClientApp.SMSSuffix : Modeltext.Substring(index) + ClientApp.SMSSuffix);
                                            index = ic * 59;
                                            newTb.Rows.Add(new object[] { 0, dt.Rows[i]["Mobile"].ToString(), sendStr, dt.Rows[i]["pri"].ToString(), 1, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), "wait" });

                                        }
                                    }
                                    else
                                    {
                                        newTb.Rows.Add(new object[] { 0, dt.Rows[i]["Mobile"].ToString(), Modeltext + ClientApp.SMSSuffix, dt.Rows[i]["pri"].ToString(), 1, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), "wait" });
                                    }

                                }
                            }
                        }

                        if (newTb.Rows.Count > 0)
                        {
                            try
                            {
                                Console.WriteLine("调用webservice发送短信==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                if (service == null)
                                {
                                    //调用webservice
                                    service = new SMSService.smsService();
                                    service.init(ClientApp.DefaultCompanyID);
                                    Console.WriteLine("初始化公司ID==={0}", ClientApp.DefaultCompanyID);
                                }
                                //序列化
                                StringBuilder sb = new StringBuilder();
                                XmlWriter writer = XmlWriter.Create(sb);
                                XmlSerializer serializer = new XmlSerializer(typeof(DataTable));
                                serializer.Serialize(writer, newTb);
                                Console.WriteLine("序列化短信内容==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                writer.Close();
                                Console.WriteLine("发送短信中==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                //发送短信
                                string Sreturn = service.sendSMS(udi, pwd, ClientApp.DefaultCompanyID, sb.ToString());

                                //Console.WriteLine("优惠券处理结束时间：＝＝＝＝" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

                                Console.WriteLine("短信发送完成，返回结果==={0}", Sreturn);
                                //if (Sreturn.ToUpper() == "SUCCESS" && autoid.Trim().Length > 0)
                                Console.WriteLine("短信发送成功，删除表senddata中的数据==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                if (autoid.Trim().Length > 0)
                                {
                                    //短信发送成功后，删除senddata表
                                    // Console.WriteLine("优惠券处理结果：＝＝＝＝" + Sreturn);
                                    ArrayList ar = new ArrayList();
                                    ar.Add("delete from senddata where id in(" + autoid.Substring(0, autoid.Length - 1) + ")");

                                    bool bflag = sqlExcuteNonQuery(ar);
                                    autoid = "";
                                }

                            }
                            catch (Exception e)
                            {
                                throw e;
                            }

                            //Console.WriteLine("删除SendDate时间：＝＝＝＝" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        }
                        Thread.Sleep(2000);
                    }
                    catch (Exception er)
                    {
                        Console.WriteLine("发送表sendata表中短信出现异常==={0}", er.Message);
                        ErrInfo.WriterErrInfo("SmsSendDataClass", "ThreadSendData----Sleep", er);
                        Thread.Sleep(5000);
                    }
                }
            }

        }
        public DataTable SqlbyString(string str)
        {
            SqlDataAdapter adp;
            SqlConnection con = new SqlConnection(ClientApp.Basecon);
            try
            {
                adp = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                con.Open();
                adp.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            { con.Close(); }
        }

        public bool sqlExcuteNonQuery(ArrayList al)
        {
            bool flag = false;
            SqlCommand cmd = new SqlCommand();
            SqlConnection con = new SqlConnection(ClientApp.Basecon);
            SqlTransaction tran;
            cmd.Connection = con;
            con.Open();
            tran = con.BeginTransaction();
            try
            {
                for (int i = 0; i < al.Count; i++)
                {
                    cmd.CommandText = al[i].ToString();
                    cmd.Transaction = tran;
                    cmd.ExecuteNonQuery();
                }
                tran.Commit();
                flag = true;

                return flag;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
            finally { con.Close(); }
        }
    }
}

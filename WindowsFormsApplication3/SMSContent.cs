using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

namespace WindowsFormsApplication3
{
    class SMSContent
    {
        public Thread thSMS;
        EmailPhone ep = new EmailPhone();
        SMSService.smsService service;
        int Icount = 0;

        public SMSContent()
        {
            thSMS = new Thread(new ThreadStart(ThreadSMS));
            thSMS.Name = "SMSContent";
            thSMS.Start();
        }


        public void ThreadSMS()
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
                        Console.WriteLine("类SMSContent方法方法ThreadSMS出现异常==={0}", ex.Message);
                        ErrInfo.WriterErrInfo("SMSContent", "ThreadSMS----Sleep", ex);
                    }
                    continue;
                }
                if (ClientApp.isServerOpen)
                {
                    try
                    {
                        Thread.Sleep(2000);
                        ReplaceContent();

                    }
                    catch (Exception er)
                    {
                        ErrInfo.WriterErrInfo("SMSContent", "ThreadSMS----Sleep", er);
                        Thread.Sleep(5000);
                    }
                    finally { }
                }
            }
        }

        public void ReplaceContent()
        {
            string Sreturn = "";
            DataTable dt;

            //System.Diagnostics.Stopwatch MyWatch= new System.Diagnostics.Stopwatch(); 
            try
            {

                Console.WriteLine("获取需要发送的直复短信：＝＝＝＝" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

                //读取短信模板内容　
                string ss = "select top 1 mid,modeltext,channel from smsModel_zf";
                Console.WriteLine("获取直复短信：＝＝＝＝" + ss);
                dt = new DataTable();
                dt = SqlbyString(ss);
                int rows = dt.Rows.Count;
                if (rows > 0)
                {
                    Console.WriteLine(string.Format("有需要发送的直复短信：{0}条＝＝＝＝{1}", rows, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")));
                    string mid = dt.Rows[0]["mid"].ToString();
                    string Modeltext = dt.Rows[0]["modeltext"].ToString();
                    string schannel = dt.Rows[0]["channel"].ToString();
                    //查询优先级
                    ss = "select pri from PRIsetting where smsType='直复'";
                    Console.WriteLine("查询直复短信的优先级：＝＝＝＝" + ss);
                    int Ipri = (int)ValuebyString(ss);
                    Console.WriteLine(string.Format("直复短信的优先级：{0}＝＝＝＝{1}", Ipri, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")));
                    //查询用户名及密码
                    ss = "select uid,pwd from smsSysNum";
                    Console.WriteLine("查询短信用户名及密码：＝＝＝＝" + ss);
                    dt = new DataTable();
                    dt = SqlbyString(ss);
                    string udi = dt.Rows[0]["uid"].ToString();
                    string pwd = dt.Rows[0]["pwd"].ToString();
                    Console.WriteLine(string.Format("发送短信的用户名:{0} 密码:{1}＝＝＝＝{2}", udi, pwd, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")));
                    //查询VIP表字段名
                    ss = "select ENname,CNname From datainfo order by ENname";
                    Console.WriteLine("查询VIP表字段名：＝＝＝＝" + ss);
                    DataTable mt = new DataTable();
                    mt = SqlbyString(ss);

                    #region 新增的多增一列可用积分的项
                    DataRow spareScoreRow = mt.NewRow();//可用积分列
                    spareScoreRow[0] = "spareScore";
                    spareScoreRow[1] = "可用积分";
                    mt.Rows.Add(spareScoreRow);
                    #endregion

                    string str = "";
                    for (int j = 0; j < mt.Rows.Count; j++)
                    {
                        if (mt.Rows[j]["ENname"].ToString().ToUpper() == "CARD_ID")
                        { str = str + "a.[" + mt.Rows[j]["ENname"] + "],"; }
                        else
                        { str = str + "[" + mt.Rows[j]["ENname"] + "],"; }
                    }
                    str = str.Substring(0, str.LastIndexOf(","));

                    //查询待发短信的VIP卡号
                    ss = "select card_id from smsCard_zf where mid='" + mid + "'";
                    Console.WriteLine("查询待发短信的VIP卡号：＝＝＝＝" + ss);
                    DataTable mdt = new DataTable();
                    mdt = SqlbyString(ss);

                    string WhereIn = "";

                    DataTable newTb = new DataTable("Tbsms");
                    newTb.Columns.Add("autoid", typeof(int));
                    newTb.Columns.Add("Mobile", typeof(string));
                    newTb.Columns.Add("Content", typeof(string));
                    newTb.Columns.Add("flag", typeof(int));
                    newTb.Columns.Add("channel", typeof(int));
                    newTb.Columns.Add("mdate", typeof(string));
                    newTb.Columns.Add("state", typeof(string));


                    ArrayList arr = new ArrayList();
                    //每次处理50个VIP卡号
                    for (int i = 0; i < mdt.Rows.Count; i++)
                    {
                        if (i % 50 != 0 && i > 0 && i < mdt.Rows.Count - 1)
                        {
                            WhereIn = WhereIn + "'" + mdt.Rows[i]["card_id"] + "',";
                        }
                        else if (i % 50 == 0 && i == 0 && i < mdt.Rows.Count - 1)
                        {
                            WhereIn = WhereIn + "'" + mdt.Rows[i]["card_id"] + "',";
                        }
                        else
                        {
                            //根据VIP卡号查询具体数据
                            WhereIn = WhereIn + "'" + mdt.Rows[i]["card_id"] + "',";
                            WhereIn = WhereIn.Substring(0, WhereIn.LastIndexOf(","));

                            //string sql = "select " + str + " from cardinfo a left join UD_Fileds b on a.card_id=b.card_id where a.card_id in (" + WhereIn + ")";
                            string sql = "select " + str + " from vipInfo_view a where a.card_id in (" + WhereIn + ")";//修改（李克善2012-09-05关于可用积分的替换问题，由原来的查询cardInfo表换成vipInfo_view视图）
                            //string sql = "select 手机号 as userMobile from mobile";
                            Console.WriteLine("根据VIP卡号查询具体数据：＝＝＝＝" + sql);
                            DataTable Tmt = new DataTable();
                            Tmt = SqlbyString(sql);

                            Icount++;
                            string strii = Modeltext;
                            for (int k = 0; k < Tmt.Rows.Count; k++)
                            {
                                //验证手机号

                                if (ep.IsChinaUnicomNumber(Tmt.Rows[k]["userMobile"].ToString()))
                                {
                                    Console.WriteLine("验证手机正确！：＝＝＝＝" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                    //Console.WriteLine(Tmt.Rows[k]["username"].ToString() + "用户手机号码:" + Tmt.Rows[k]["userMobile"].ToString());
                                    //替换短信内容
                                    for (int l = 0; l < mt.Rows.Count; l++)
                                    {
                                        Modeltext = Modeltext.Replace("[%" + mt.Rows[l]["CNname"] + "%]", Tmt.Rows[k][mt.Rows[l]["ENname"].ToString()].ToString());
                                    }


                                    //if (Modeltext.Length + ClientApp.SMSSuffix.Length > 70)//60Modeltext.Length+ClientApp.SMSSuffix.Length>70
                                    //判断是否需要拆分短信
                                    if (Modeltext.Length + ClientApp.SMSSuffix.Length > 70)
                                    {
                                        Console.WriteLine(string.Format("直复短信长度：{0}个字符＝＝＝＝{1}", Modeltext.Length, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")));
                                        int count = (Modeltext.Length) / 59;// 60;
                                        int yacon = (Modeltext.Length) % 59;// 60;
                                        if (yacon > 0)
                                        { count++; }
                                        int index = 0;
                                        //拆分短信
                                        for (int ic = 1; ic <= count; ic++)
                                        {
                                            string sendStr = "[" + ic + "/" + count + "]" + (ic != count ? Modeltext.Substring(index, 59) + ClientApp.SMSSuffix : Modeltext.Substring(index) + ClientApp.SMSSuffix);
                                            index = ic * 59;
                                            newTb.Rows.Add(new object[] { 0, Tmt.Rows[k]["userMobile"].ToString(), sendStr, Ipri, schannel, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), "wait" });
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine(string.Format("直复短信长度：{0}个字符＝＝＝＝{1}", Modeltext.Length, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")));
                                        newTb.Rows.Add(new object[] { 0, Tmt.Rows[k]["userMobile"].ToString(), Modeltext + ClientApp.SMSSuffix, Ipri, schannel, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), "wait" });
                                    }
                                    Modeltext = strii;
                                }
                                else
                                {
                                    Console.WriteLine("手机号码不符合规范，验证失败！：＝＝＝＝" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                }
                            }
                            try
                            {
                                if (service == null)
                                {
                                    //调用webservice，发送短信
                                    service = new SMSService.smsService();
                                    service.init(ClientApp.DefaultCompanyID);
                                }
                                StringBuilder strbd = new StringBuilder();
                                //序列化数据
                                XmlWriter writer = XmlWriter.Create(strbd);
                                XmlSerializer serializer = new XmlSerializer(typeof(DataTable));
                                serializer.Serialize(writer, newTb);
                                writer.Close();
                                //调用webserivce发送短信的方法。参数（用户名，密码，公司ID,序列化后的数据）
                                Console.WriteLine("正在发送直复短信：＝＝＝＝" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                Sreturn = service.sendSMS(udi, pwd, ClientApp.DefaultCompanyID, strbd.ToString());
                                //处理成功后删除待发短信列表
                                if (Sreturn.ToUpper() == "SUCCESS")
                                {
                                    Console.WriteLine("成功发送直复短信：＝＝＝＝" + Sreturn);
                                    newTb.Rows.Clear();
                                    sql = string.Format("delete from smsCard_zf where mid='{0}' and card_id in({1})", mid, WhereIn);
                                    arr.Add(sql);
                                    Console.WriteLine("删除已发直复短信列表：＝＝＝＝" + sql);
                                }
                                else if (Sreturn.ToUpper().IndexOf("ERROR") > 0)
                                {
                                    newTb.Rows.Clear();
                                    sql = string.Format("delete from smsCard_zf where mid='{0}' and card_id in({1})", mid, WhereIn);
                                    arr.Add(sql);
                                    Console.WriteLine("删除出错直复短信列表：＝＝＝＝" + sql);
                                }
                                WhereIn = string.Empty;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("发送直复短信异常：" + e.Message + "＝＝＝＝" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                throw e;
                            }
                        }
                    }
                    bool arrflag = sqlExcuteNonQuery(arr);

                    if (Sreturn.ToUpper() == "SUCCESS")
                    {

                        ArrayList ar = new ArrayList();
                        //删除短信模板表
                        string sql = string.Format("delete from smsModel_zf where mid='{0}'", mid);
                        Console.WriteLine("删除直复短信内容：＝＝＝＝" + sql);
                        ar.Add(sql);
                        bool bflag = sqlExcuteNonQuery(ar);
                    }
                }
                else//说明没有直复短信需要发送
                {
                    Console.WriteLine(string.Format("没有需要发送的直复短信：＝＝＝＝{0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")));
                }
            }
            catch (Exception er)
            {
                Console.WriteLine("发送直复短信异常：" + er.Message + "＝＝＝＝" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                throw er;
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

        public object ValuebyString(string str)
        {
            SqlCommand cmd;
            SqlConnection con = new SqlConnection(ClientApp.Basecon);
            try
            {
                cmd = new SqlCommand(str, con);
                con.Open();
                return cmd.ExecuteScalar();
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

        public void exitThread()
        {
            thSMS.Abort();
        }

    }
}

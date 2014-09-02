using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

namespace WindowsFormsApplication3
{
    /// <summary>
    /// author:李克善
    /// funciton:获取口袋通vip信息
    /// date：2014-08-18
    /// </summary>
    public class Get_Kdt_Vip_Info
    {
        public Thread thread;
        public Get_Kdt_Vip_Info()
        {
            thread = new Thread(new ThreadStart(ThreadGetKdtVipInfo));
            thread.Name = "ThreadGetKdtVipInfo";
            thread.Start();
        }

        private void ThreadGetKdtVipInfo()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("获取口袋同vip信息开始================" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    LogicModel logic = new LogicModel();
                    string jsonStr = logic.GetVipKdtInfo(1, 100);
                    JObject jo = JObject.Parse(jsonStr);
                    int total;
                    int.TryParse(jo["response"]["total_results"].ToString(), out total);

                    int total_page = total / 100;
                    if (total % 100 > 0)
                    {
                        total_page += 1;
                    }

                    Console.WriteLine("口袋同vip信息总页数：{0}================{1}", total_page.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    string sqlStr = "delete from cardinfo where remark='kdt'";
                    ExcuteNonQuery(sqlStr);

                    Console.WriteLine("删除原始口袋同vip信息完成================" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    JArray ja = JArray.Parse(jo["response"]["users"].ToString());
                    insert_vip_kdt_info(ja,logic);

                    for (int i = 2; i <= total_page; i++)
                    {
                        jsonStr = logic.GetVipKdtInfo(i, 100);
                        jo = JObject.Parse(jsonStr);
                        ja = JArray.Parse(jo["response"]["users"].ToString());
                        insert_vip_kdt_info(ja,logic);
                    }

                    Console.WriteLine("获取口袋同vip信息完成================" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Thread.Sleep(1000 * 60 * 60 * 12);
                }
                catch (Exception ex)
                {

                    Console.WriteLine("类Get_Kdt_Vip_Info方法ThreadGetKdtVipInfo获取口袋通vip信息出错==={0}", ex.Message);
                    ErrInfo.WriterErrInfo("Get_Kdt_Vip_Info", "ThreadGetKdtVipInfo----获取口袋通vip信息出错", ex);
                    Thread.Sleep(1000 * 60 * 60);//毫秒1000*60*60  60m
                }

            }
        }

        private void insert_vip_kdt_info(JArray ja,LogicModel logic)
        {
            StringBuilder sb = new StringBuilder(); 
            string rst = string.Empty;
            string phoneNumber = string.Empty;
            for (int i = 0; i < ja.Count; i++)
            {

                rst = logic.GetVipSaleKdtInfo(ja[i]["user_id"].ToString(), 1, 100);//获取手机号

                JObject o = JObject.Parse(rst);
                if (o["response"]["trades"].HasValues)
                {
                    JArray a = JArray.Parse(o["response"]["trades"].ToString());
                    phoneNumber = a[0]["receiver_mobile"].ToString();
                }
                sb.AppendFormat("insert into cardinfo(card_id,card_type,card_discount,username,usersex,begindate,remark,userPhone,userMobile) values('{0}','普通会员卡',100,'{1}','{2}','{3}','kdt','{4}','{5}');", ja[i]["weixin_openid"].ToString(), ja[i]["nick"].ToString().Replace("'", ""), ja[i]["sex"].ToString() == "m" ? "男" : "女", ja[i]["follow_time"].ToString(),phoneNumber,phoneNumber);

                phoneNumber = "";
            }
            if (ja.Count > 0)
                ExcuteNonQuery(sb.ToString());
        }

        private void ExcuteNonQuery(string sqlStr)
        {
            using (SqlConnection con = new SqlConnection(ClientApp.Basecon))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand(sqlStr, con);
                    cmd.Transaction = tran;
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Threading;

namespace WindowsFormsApplication3
{
    public class Get_Vip_Kdt_Sale
    {

        public Thread thread;
        public Get_Vip_Kdt_Sale()
        {
            thread = new Thread(new ThreadStart(GetKdtVipSaleInfo));
            thread.Name = "ThreadGetKdtVipInfo";
            thread.Start();
        }
        public void GetKdtVipSaleInfo()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("获取口袋同vip销售信息开始================" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    LogicModel logic = new LogicModel();
                    string jsonStr = logic.GetVipSaleKdtInfo(string.Empty,1, 100);
                    JObject jo = JObject.Parse(jsonStr);
                    int total;
                    int.TryParse(jo["response"]["total_results"].ToString(), out total);

                    int total_page = total / 100;
                    if (total % 100 > 0)
                    {
                        total_page += 1;
                    }

                    Console.WriteLine("口袋同vip销售信息总页数：{0}================{1}", total_page.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    string sqlStr = "delete from lants_sale_mst where bill_id  like 'E%';delete from lants_sale_dtl where bill_id like 'E%';delete from shopInfo where clientId='eissy';delete from salerInfo where salerId='kdt';";
                    ExcuteNonQuery(sqlStr);


                    sqlStr = "insert into shopInfo(clientId,clientName) values('eissy','上元端');insert salerInfo(salerId,salerName) values('kdt','Eissy微商城');";
                    ExcuteNonQuery(sqlStr);


                    Console.WriteLine("删除原始口袋通vip销售信息完成================" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    #region 新增的，查看销售记录的(视图)
                    sqlStr = "select COUNT(*) from INFORMATION_SCHEMA.VIEWS where TABLE_NAME =N'saleCount'";

                    int j = (int)(sqlExecuteScalar(sqlStr));
                    if (j == 0)
                    {
                        sqlStr = "create view [dbo].[saleCount] as select cif.sendMan as '发卡人',cif.userName as '持卡人',cif.card_id as '卡号',ls.bill_id as 销售单号,ls.sale_time as '销售时间',cif.card_Type as '卡类型',ls.amount as '金额',ct.times as '消费次数',ls.qty as '购买数量' from (select mst.vip_id,mst.bill_id,mst.sale_time,sum(cast(dtl.amount as float)) as amount,sum(dtl.qty) as qty from lants_sale_mst mst inner join lants_sale_dtl dtl on mst.bill_id = dtl.bill_id group by mst.bill_id,mst.vip_id,mst.sale_time) ls inner join cardInfo cif on ls.vip_id = cif.card_id inner join (select vip_id,count(vip_id) as times from lants_sale_mst group by vip_id) ct on cif.card_id = ct.vip_id";
                        ExcuteNonQuery(sqlStr);
                    }
                    #endregion

                    #region 新增的
                    sqlStr = "select COUNT(*) from INFORMATION_SCHEMA.VIEWS  where TABLE_NAME =N'salesParticularView'";
                    j = Convert.ToInt32(sqlExecuteScalar(sqlStr));
                    Console.WriteLine("判断视图saleCount是否存在，如果不存在就创建");
                    if (j == 0)
                    {
                        sqlStr = "CREATE VIEW [dbo].[salesParticularView] as select cif.sendMan as sendMan,cif.userName as userName,cif.card_id as card_id,cif.card_Type,ls.bill_id as bill_id,ls.sale_time as sale_time,ls.amount as amount,ct.times as times,ls.qty as qty,status from (select mst.vip_id,mst.bill_id,mst.sale_time,sum(cast(dtl.amount as float)) as amount,sum(dtl.qty) as qty,status from lants_sale_mst mst inner join lants_sale_dtl dtl on mst.bill_id = dtl.bill_id group by mst.bill_id,mst.vip_id,mst.sale_time,mst.status) ls inner join cardInfo cif on ls.vip_id = cif.card_id inner join (select vip_id,count(vip_id) as times from lants_sale_mst group by vip_id) ct on cif.card_id = ct.vip_id";

                        ExcuteNonQuery(sqlStr);
                    }

                    #endregion

                    JArray ja = JArray.Parse(jo["response"]["trades"].ToString());
                    insert_vip_kdt_sale_info(ja, logic);

                    for (int i = 2; i <= total_page; i++)
                    {
                        jsonStr = logic.GetVipSaleKdtInfo(string.Empty,i, 100);
                        jo = JObject.Parse(jsonStr);
                        ja = JArray.Parse(jo["response"]["trades"].ToString());
                        insert_vip_kdt_sale_info(ja, logic);
                    }

                    Console.WriteLine("获取口袋同vip销售信息完成================" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Thread.Sleep(1000 * 60 * 60 * 12);//12小时
                }
                catch (Exception ex)
                {

                    Console.WriteLine("类Get_Vip_Kdt_Sale方法GetKdtVipSaleInfo获取口袋通vip销售信息出错==={0}", ex.Message);
                    ErrInfo.WriterErrInfo("Get_Vip_Kdt_Sale", "GetKdtVipSaleInfo----获取口袋通vip销售信息出错", ex);
                    Thread.Sleep(1000 * 60);//1min

                }
            }

        }

        private void insert_vip_kdt_sale_info(JArray ja, LogicModel logic)
        {
            StringBuilder sb1 = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            string wx_id = string.Empty;
            for (int i = 0; i < ja.Count; i++)
            {
                wx_id = logic.GetWeiXinUserInfo(ja[i]["weixin_user_id"].ToString());
                sb1.AppendFormat("insert into lants_sale_mst(bill_id,sale_time,client_id,saler_id,vip_id,status) values('{0}','{1}','{2}','{3}','{4}',{5});", ja[i]["tid"].ToString(), ja[i]["created"].ToString(), "eissy", "kdt", wx_id, GetStatus(ja[i]["status"].ToString()));

                JArray jb = JArray.Parse(ja[i]["orders"].ToString());
                for (int j = 0; j < jb.Count; j++)
                {
                    sb2.AppendFormat("insert into lants_sale_dtl(bill_id,product_id,price,discount,qty,amount,product_name) values('{0}','{1}',{2},{3},{4},{5},'{6}');", ja[i]["tid"].ToString(), jb[j]["outer_sku_id"].ToString(), float.Parse(jb[j]["price"].ToString()), float.Parse(jb[j]["discount_fee"].ToString()), int.Parse(jb[j]["num"].ToString()), float.Parse(jb[j]["total_fee"].ToString()), jb[j]["title"].ToString());
                }

            }
            if (ja.Count > 0)
            {
                ExcuteNonQuery(sb1.ToString());
                ExcuteNonQuery(sb2.ToString());
            }
        }

        /// <summary>
        /// 获取订单状态
        /// </summary>
        /// <param name="statusStr">口袋通返回的状态字符串</param>
        /// <returns></returns>
        private int GetStatus(string statusStr)
        {
            switch (statusStr)
            {
                case "TRADE_NO_CREATE_PAY"://没有创建支付交易
                    return 0;
                case "WAIT_BUYER_PAY"://等待买家付款
                    return 1;
                case "TRADE_CLOSED_BY_USER"://付款以前，卖家或买家主动关闭交易
                    return 2;
                case "WAIT_SELLER_SEND_GOODS"://等待卖家发货，即：买家已付款
                    return 3;
                case "WAIT_BUYER_CONFIRM_GOODS"://等待买家确认收货，即：卖家已发货
                    return 4;
                case "TRADE_BUYER_SIGNED"://买家已签收
                    return 5;
                case "TRADE_CLOSED"://付款以后用户退款成功，交易自动关闭
                    return 6;
                default:
                    return 0;
            }
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

        public object sqlExecuteScalar(string inputString)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ClientApp.Basecon))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(inputString, con);
                    return cmd.ExecuteScalar();
                }
               
                
            }
            catch (Exception ex)
            { throw ex; }
            
        }
    }
}

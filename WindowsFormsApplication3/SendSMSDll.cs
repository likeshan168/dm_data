using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
namespace WindowsFormsApplication3
{
    class SendSMSDll
    {
        public static string Str_VIP;
        public static string Str_Sales;
        public static string Str_Point;
        public static string Str_PointErr;
        public static string Str_Uid;
        public static string Str_Pwd;
        public DataTable TbDll;
        EmailPhone ep = new EmailPhone();
        string strMsg = "";
        SqlConnection Bcon = new SqlConnection(ClientApp.Basecon);

        public SendSMSDll()
        {
            GetSMSContent();
            TbDll = new DataTable("Tbsms");
            TbDll.Columns.Add("autoid", typeof(int));
            TbDll.Columns.Add("Mobile", typeof(string));
            TbDll.Columns.Add("Content", typeof(string));
            TbDll.Columns.Add("flag", typeof(int));
            TbDll.Columns.Add("channel", typeof(int));//短信通道
            TbDll.Columns.Add("mdate", typeof(string));
            TbDll.Columns.Add("state", typeof(string));
        }
        /// <summary>
        /// 获取各个种类的pos机短信内容
        /// </summary>
        private void GetSMSContent()
        {
            try
            {
                string str = "select * from sms_model order by serialNumber";
                SqlCommand smd = new SqlCommand(str, Bcon);
                DataTable dt = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter(smd);
                Bcon.Open();
                adp.Fill(dt);
                Bcon.Close();
                if (dt.Rows.Count > 0)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (dt.Rows[j]["messageType"].ToString().Trim() == "开卡")
                        {
                            Str_VIP = dt.Rows[j]["messageContent"].ToString().Trim();
                        }
                        if (dt.Rows[j]["messageType"].ToString().Trim() == "销售")
                        {
                            Str_Sales = dt.Rows[j]["messageContent"].ToString().Trim();
                        }
                        if (dt.Rows[j]["messageType"].ToString().Trim() == "积分换礼(成功)")
                        {
                            Str_Point = dt.Rows[j]["messageContent"].ToString().Trim();
                        }
                        if (dt.Rows[j]["messageType"].ToString().Trim() == "积分换礼(失败)")
                        {
                            Str_PointErr = dt.Rows[j]["messageContent"].ToString().Trim();
                        }
                    }
                }
                str = "select uid,pwd from smsSysNum";
                smd = new SqlCommand(str, Bcon);
                dt = new DataTable();
                adp = new SqlDataAdapter(smd);
                Bcon.Open();
                adp.Fill(dt);
                Bcon.Close();

                Str_Uid = dt.Rows[0]["uid"].ToString();
                Str_Pwd = dt.Rows[0]["pwd"].ToString();
            }
            catch (Exception e)
            {
                ErrInfo.WriterErrInfo("SendSMSDll", "GetSMSContent", e.Message.ToString());
            }
            finally
            { Bcon.Close(); }
        }
        /// <summary>
        /// 验证手机号
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public bool IsChinaUnicomNumber(string mobile)
        {
            string sPattern = "(13)[0-9]{1}\\d{8}|(15)[0-9]{1}\\d{8}|(18)[0-9]{1}\\d{8}";
            bool isChinaUnicom = Regex.IsMatch(mobile, sPattern);
            return isChinaUnicom;
        }


        /// <summary>
        /// 开卡短信处理
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="vip"></param>
        /// <returns></returns>
        public bool SendSMSVIP(DataTable dt, VIP vip)
        {
            Console.WriteLine("pos机开卡短信处理中==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            SqlCommand cmd;
            int icou = 0;
            string strCoupon = "";

            SqlTransaction transaction;
            Bcon.Open();
            transaction = Bcon.BeginTransaction();

            try
            {
                //查询VIP是否存在
                string str = "select count(*) from cardinfo where card_id='" + dt.Rows[0][4].ToString() + "'";
                Console.WriteLine("查询是否有该vip卡号的vip==={0}", str);
                cmd = new SqlCommand(str, Bcon, transaction);
                icou = Convert.ToInt16(cmd.ExecuteScalar());

                if (icou <= 0)
                {
                    Console.WriteLine("不存在，将该vip信息插入到cardinfo表中");
                    //VIP不存在，插入到cardinfo表中
                    str = "insert into cardinfo (";
                    string strvalue = " values (";
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        str = str + dt.Rows[j][0].ToString() + ",";
                        strvalue = strvalue + "'" + dt.Rows[j][4].ToString() + "',";
                    }
                    str = str.Substring(0, str.Length - 1) + ")" + strvalue.Substring(0, strvalue.Length - 1) + ")";

                    str = str + ";insert into UD_Fileds(card_id) values('" + dt.Rows[0][4].ToString() + "')";
                    cmd = new SqlCommand(str, Bcon, transaction);


                    icou = cmd.ExecuteNonQuery();
                }
                if (icou > 0)
                {
                    Console.WriteLine("存在该vip卡号==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                    //替换短信内容
                    str = ReplaceValue(dt, vip, strCoupon);
                    if (str.IndexOf("ReplaceValue-----ERROR") < 0)
                    {

                        string sql = "select pri from PRIsetting where ltrim(rtrim(smsType))='开卡'";
                        Console.WriteLine("查询短信的优先级==={0}", sql);
                        cmd = new SqlCommand(sql, Bcon, transaction);
                        //查询优先级　
                        icou = Convert.ToInt16(cmd.ExecuteScalar());
                        Console.WriteLine("短息优先级为==={0}", icou.ToString());
                        //sql = " insert into senddata(Mobile,type,[text],insTime,errorcount,pri) " +
                        //  " values('" + dt.Rows[8][4].ToString() + "','开卡','" + str + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',0," + icou + ")";

                        //cmd = new SqlCommand(sql, Bcon, transaction);

                        //icou = cmd.ExecuteNonQuery();
                        transaction.Commit();

                        // TbDll.Rows.Add(new object[] { 0, dt.Rows[8][4].ToString(), str, icou, 1, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), "wait" });
                        //验证手机号
                        string phone = dt.Rows[8][4].ToString();
                        if (ep.IsChinaUnicomNumber(phone))
                        {
                            Console.WriteLine("验证手机号==={0}正确", phone);
                            //判断是否需要进行短信拆分
                            int msgCount = str.Length + ClientApp.SMSSuffix.Length;
                            Console.WriteLine("短信的长度==={0}", msgCount);
                            if (msgCount > 70)
                            {
                                int count = (str.Length) / 59;// 60;
                                int yacon = (str.Length) % 59;// 60;
                                if (yacon > 0)
                                { count++; }
                                int index = 0;
                                //for循环进行短信拆分
                                for (int ic = 1; ic <= count; ic++)
                                {
                                    string sendStr = "[" + ic + "/" + count + "]" + (ic != count ? str.Substring(index, 59) + ClientApp.SMSSuffix : str.Substring(index) + ClientApp.SMSSuffix);
                                    index = ic * 59;
                                    TbDll.Rows.Add(new object[] { 0, dt.Rows[8][4].ToString(), sendStr, icou, 1, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), "wait" });
                                    //默认的是使用通道1
                                }
                            }
                            else//不需要进行短信拆分
                            {
                                TbDll.Rows.Add(new object[] { 0, dt.Rows[8][4].ToString(), str + ClientApp.SMSSuffix, icou, 1, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), "wait" });
                            }
                        }
                        else
                        {
                            Console.WriteLine("手机号{0}验证失败", phone);
                        }
                        Bcon.Close();
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("短信内容替换失败！==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        return false;
                    }
                }
                else
                {
                    ErrInfo.WriterErrInfo("SendSMSDll", "SendSMSVIP", "插入cardinfo表出错，开卡失败!!卡号为:" + dt.Rows[0][4].ToString());
                    Console.WriteLine("插入cardinfo表出错，开卡失败!!卡号为{0}", dt.Rows[0][4].ToString());
                    return false;
                }
            }
            catch (Exception e)
            {
                ErrInfo.WriterErrInfo("SendSMSDll", "SendSMSVIP", e.Message.ToString());
                Console.WriteLine("开卡出现异常==={0}", e.Message);
                transaction.Rollback();
                return false;
            }
            finally
            {
                Bcon.Close();
            }
        }

        public bool SendSMSSale(Sales sale)
        {
            int icou = 1;

            SqlTransaction transaction;
            Bcon.Open();
            transaction = Bcon.BeginTransaction();

            try
            {
                //替换短信内容
                string str = ReplaceValue(sale);

                if (str.IndexOf("ReplaceValue-----ERROR") < 0)
                {
                    SqlCommand cmd;
                    string sql = "select pri from PRIsetting where ltrim(rtrim(smsType))='销售'";
                    cmd = new SqlCommand(sql, Bcon, transaction);
                    //Bcon.Open();
                    icou = Convert.ToInt16(cmd.ExecuteScalar());
                    // sql = " insert into senddata(Mobile,type,[text],insTime,errorcount,pri) " +
                    //" values('" + sale.TelPhone + "','销售','" + str + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',0," + icou + ")";

                    // cmd = new SqlCommand(sql, Bcon, transaction);
                    // icou = cmd.ExecuteNonQuery();
                    transaction.Commit();

                    // TbDll.Rows.Add(new object[] { 0, sale.TelPhone, str, icou, 1, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), "wait" });
                    //验证手机号码
                    if (ep.IsChinaUnicomNumber(sale.TelPhone))
                    {
                        //判断短信内容是否要拆分
                        if (str.Length + ClientApp.SMSSuffix.Length > 70)
                        {
                            int count = (str.Length) / 59;// 60;
                            int yacon = (str.Length) % 59;// 60;
                            if (yacon > 0)
                            { count++; }
                            int index = 0;
                            //短信拆分
                            for (int ic = 1; ic <= count; ic++)
                            {
                                string sendStr = "[" + ic + "/" + count + "]" + (ic != count ? str.Substring(index, 59) + ClientApp.SMSSuffix : str.Substring(index) + ClientApp.SMSSuffix);
                                index = ic * 59;
                                TbDll.Rows.Add(new object[] { 0, sale.TelPhone, sendStr, icou, 1, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), "wait" });

                            }
                        }
                        else
                        {
                            TbDll.Rows.Add(new object[] { 0, sale.TelPhone, str + ClientApp.SMSSuffix, icou, 1, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), "wait" });
                        }

                    }
                    Bcon.Close();


                    return true;
                }
                else
                { return false; }
            }
            catch (Exception e)
            {
                transaction.Rollback();
                ErrInfo.WriterErrInfo("SendSMSDll", "SendSMSSale", e.Message.ToString());
                return false;
            }
            finally
            {
                Bcon.Close();
            }
        }

        public bool SendSMSCoupon(Point cou)
        {

            int icou = 1;
            SqlTransaction transaction;
            Bcon.Open();
            transaction = Bcon.BeginTransaction();

            try
            {
                //替换短信内容
                string str = ReplaceValue(cou);
                if (str.IndexOf("ReplaceValue-----ERROR") < 0)
                {
                    //判断发送方式,if==0短信 else 邮件
                    if (cou.LinkType == "0")
                    {
                        //查询短信优先级
                        SqlCommand cmd;
                        string sql = "select pri from PRIsetting where ltrim(rtrim(smsType))='直复'";

                        cmd = new SqlCommand(sql, Bcon, transaction);
                        // Bcon.Open();
                        icou = Convert.ToInt16(cmd.ExecuteScalar());

                        //sql = " insert into senddata(Mobile,type,[text],insTime,errorcount,pri) " +
                        //  " values('" + cou.Telephone + "','直复','" + str + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',0," + icou + ")";

                        //cmd = new SqlCommand(sql, Bcon, transaction);
                        //icou = cmd.ExecuteNonQuery();
                        transaction.Commit();

                        //  TbDll.Rows.Add(new object[] { 0, cou.Telephone, str, icou, 1, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), "wait" });
                        //验证手机号
                        if (ep.IsChinaUnicomNumber(cou.Telephone))
                        {
                            //判断是否需要拆分短信
                            if (str.Length + ClientApp.SMSSuffix.Length > 70)
                            {
                                int count = (str.Length) / 59;// 60;
                                int yacon = (str.Length) % 59;// 60;
                                if (yacon > 0)
                                { count++; }
                                int index = 0;
                                //短信拆分
                                for (int ic = 1; ic <= count; ic++)
                                {
                                    string sendStr = "[" + ic + "/" + count + "]" + (ic != count ? str.Substring(index, 59) + ClientApp.SMSSuffix : str.Substring(index) + ClientApp.SMSSuffix);
                                    index = ic * 59;
                                    TbDll.Rows.Add(new object[] { 0, cou.Telephone, sendStr, icou, 1, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), "wait" });

                                }
                            }
                            else
                            {
                                TbDll.Rows.Add(new object[] { 0, cou.Telephone, str + ClientApp.SMSSuffix, icou, 1, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), "wait" });
                            }
                        }
                        Bcon.Close();
                        return true;

                    }
                    else
                    {
                        //发邮件
                        bool success = sendMail(cou.Email, str, 0, "发送优惠券");
                        return success;
                    }
                }
                else
                { return false; }
            }
            catch (Exception e)
            {
                transaction.Rollback();
                ErrInfo.WriterErrInfo("SendSMSDll", "SendSMSCoupon", e.Message.ToString());
                return false;
            }
            finally
            {
                Bcon.Close();
            }
        }
        /// <summary>
        /// 开卡短信字段替换
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="vip"></param>
        /// <param name="coupon"></param>
        /// <returns></returns>
        private string ReplaceValue(DataTable dt, VIP vip, string coupon)
        {
            try
            {
                Console.WriteLine("短信字段替换==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                string message = "";
                message = Str_VIP;
                if (message.IndexOf("[姓名]") >= 0)
                {
                    message = message.Replace("[姓名]", dt.Rows[3][4].ToString());
                }
                if (message.IndexOf("[卡号]") >= 0)
                {
                    message = message.Replace("[卡号]", dt.Rows[0][4].ToString());
                }
                if (message.IndexOf("[密码]") >= 0)
                {
                    if (dt.Rows[18][4].ToString().Trim().Length <= 0)
                    {
                        message = message.Replace("[密码]", vip.Defpwd);
                    }
                    else
                    {
                        message = message.Replace("[密码]", dt.Rows[18][4].ToString());
                    }
                }
                if (message.IndexOf("[开卡时间]") >= 0)
                {
                    message = message.Replace("[开卡时间]", dt.Rows[15][4].ToString());
                }
                if (message.IndexOf("[优惠券编号]") >= 0)
                {
                    message = message.Replace("[优惠券编号]", coupon);
                }
                Console.WriteLine("替换之后的短信内容==={0}", message);
                return message;
            }
            catch (Exception ex)
            {
                ErrInfo.WriterErrInfo("SendSMSDll", "ReplaceValue-----VIP开卡", ex.Message.ToString());
                Console.WriteLine("短信替换出现异常==={0}", ex.Message);
                return "ReplaceValue-----ERROR";
            }
        }


        /// <summary>
        /// 销售短信字段替换
        /// </summary>
        /// <param name="sale"></param>
        /// <returns></returns>
        private string ReplaceValue(Sales sale)
        {
            try
            {
                string message = "";
                message = Str_Sales;
                if (message.IndexOf("[姓名]") >= 0)
                {
                    message = message.Replace("[姓名]", sale.Name);
                }
                if (message.IndexOf("[消费地点]") >= 0)
                {
                    message = message.Replace("[消费地点]", sale.ClientName);
                }
                if (message.IndexOf("[消费金额]") >= 0)
                {
                    message = message.Replace("[消费金额]", sale.Money);
                }
                if (message.IndexOf("[消费时间]") >= 0)
                {
                    message = message.Replace("[消费时间]", sale.SaleTime);
                }
                if (message.IndexOf("[当前积分]") >= 0)
                {
                    message = message.Replace("[当前积分]", sale.Points.Split('.')[0]);
                }

                return message;
            }
            catch (Exception ex)
            {
                ErrInfo.WriterErrInfo("SendSMSDll", "ReplaceValue-----Sales", ex.Message.ToString());
                return "ReplaceValue-----ERROR";
            }
        }

        /// <summary>
        /// 优惠券兑换短息字段替换
        /// </summary>
        /// <param name="coupon"></param>
        /// <returns></returns>
        private string ReplaceValue(Point coupon)
        {
            try
            {
                //<Data vipId="vip卡号"  points ="积分"  money="优惠券面值" lpcID=“优惠券ID” isOK=“是否成功” linkType="联系方式" backData="返回信息" telephone="手机号码" email="Email" currPoints="当前积分">
                //</Data>string message = "";
                string message = "";

                if (coupon.IsOK == 1)
                {
                    message = Str_Point;
                    if (message.IndexOf("[卡号]") >= 0)
                    {
                        message = message.Replace("[卡号]", coupon.VIPid);
                    }
                    if (message.IndexOf("[当前积分]") >= 0)
                    {
                        message = message.Replace("[当前积分]", coupon.CurrPoints);
                    }
                    if (message.IndexOf("[优惠券面值]") >= 0)
                    {
                        message = message.Replace("[优惠券面值]", coupon.Money);
                    }
                    if (message.IndexOf("[优惠券编号]") >= 0)
                    {
                        message = message.Replace("[优惠券编号]", coupon.LpcID);
                    }
                }
                else
                {
                    message = Str_PointErr;
                    if (message.IndexOf("[卡号]") >= 0)
                    {
                        message = message.Replace("[卡号]", coupon.VIPid);
                    }
                    if (message.IndexOf("[当前积分]") >= 0)
                    {
                        message = message.Replace("[当前积分]", coupon.CurrPoints);
                    }
                    if (message.IndexOf("[优惠券面值]") >= 0)
                    {
                        message = message.Replace("[优惠券面值]", coupon.Money);
                    }
                    if (message.IndexOf("[兑换积分]") >= 0)
                    {
                        message = message.Replace("[兑换积分]", coupon.Points);
                    }
                    if (message.IndexOf("[返回信息]") >= 0)
                    {
                        message = message.Replace("[返回信息]", coupon.BackData);
                    }
                }
                return message;
            }
            catch (Exception ex)
            {
                ErrInfo.WriterErrInfo("SendSMSDll", "ReplaceValue-----Pointcoupon", ex.Message.ToString());
                return "ReplaceValue-----ERROR";
            }
        }

        private string ReplaceValue(clsEntity entity)
        {
            try
            {
                string message = "";
                message = Str_Point;
                if (message.IndexOf("卡号") >= 0)
                {
                    message = message.Replace("卡号", entity.vipId);
                }
                if (message.IndexOf("当前积分") >= 0)
                {
                    message = message.Replace("当前积分", entity.point.ToString());
                }
                if (message.IndexOf("优惠券面值") >= 0)
                {
                    message = message.Replace("优惠券面值", entity.par_Value.ToString());
                }
                if (message.IndexOf("优惠券编号") >= 0)
                {
                    message = message.Replace("优惠券编号", entity.lpcID);
                }
                return message;
            }
            catch (Exception ex)
            {
                ErrInfo.WriterErrInfo("SendSMSDll", "ReplaceValue-----Pointcoupon", ex.Message.ToString());
                return "ReplaceValue-----ERROR";
            }
        }


        public bool MST_SendMsg(clsEntity entity)
        {

            bool success = false;
            try
            {

                string returnStr = "";
                //string returnStr = "您卡号为:" + entity.vipId + ",申请" + entity.point + "积分换礼";
                if (entity.isOK == 1)
                {
                    //returnStr += "成功!优惠券号:" + entity.lpcID + ",请您接店铺通知后凭优惠券号到曼洒特专卖店领取";
                    returnStr = Str_Point;
                }
                else
                {
                    returnStr += "申请失败!原因:" + entity.errInfo;
                    if (Bcon.State == ConnectionState.Closed)
                    {
                        Bcon.Open();
                    }
                    string sqlStr = "delete from dbo.couponlist where vipid='" + entity.vipId + "' and coupon='" + entity.lpcID + "'";
                    SqlCommand cmd = new SqlCommand(sqlStr, Bcon);
                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine(returnStr);
                if (entity.sendType == 0 || entity.sendType == 2)
                {
                    if (Bcon.State == ConnectionState.Closed)
                    {
                        Bcon.Open();
                    }
                    string sqlStr = "select pri from dbo.PRIsetting where smstype='直复' ";
                    SqlCommand cmd = new SqlCommand(sqlStr, Bcon);
                    int resInt = 1;
                    object res = cmd.ExecuteScalar();
                    if (res != null)
                        resInt = Convert.ToInt32(res);

                    sqlStr = "insert into dbo.sendData(mobile,type,[text],instime,errorCount,pri) " +
                                    "values('" + entity.mobileNo + "','直复','" + returnStr + "',getdate(),0," + resInt + ")";
                    cmd = new SqlCommand(sqlStr, Bcon);
                    cmd.ExecuteNonQuery();
                    success = true;
                }
                if (entity.sendType == 1 || entity.sendType == 2)
                {
                    success = sendMail(entity.email, returnStr, 0, "发送优惠券");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                if (Bcon.State == ConnectionState.Open)
                {
                    Bcon.Close();
                }
            }
            return success;
        }

        public static bool sendMail(string STo, string mailbody, int type, string title)
        {
            bool breturn = false;

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("DMtest@empox.cn", "EMPOX");
            mailMessage.To.Add(STo);
            mailMessage.Subject = title;
            if (type == 0)
            {
                string body = mailbody;
                mailMessage.Body = body;
            }
            else
            {
                mailMessage.IsBodyHtml = true;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mailbody, null, "text/html");
                mailMessage.AlternateViews.Add(htmlView);
            }

            SmtpClient smtpclient = new SmtpClient();
            smtpclient.EnableSsl = false;
            smtpclient.Host = "mail.empox.cn"; //*******************************************
            smtpclient.Port = 25;
            smtpclient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpclient.EnableSsl = false;
            smtpclient.Credentials = new System.Net.NetworkCredential("DMtest@empox.cn", "111111");//*******************************************
            try
            {
                smtpclient.Send(mailMessage);
                breturn = true;
            }
            catch (Exception ex)
            {
                ErrInfo.WriterErrInfo("SendSMSDll", "sendMail", ex.Message.ToString());
                breturn = false;
            }
            return breturn;
        }
        /// <summary>
        /// 将表lpc_zfsms_table中的数据插入到表DM_zfsms_table中
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public bool InsertBackupZF(DataSet ds)
        {
            try
            {
                //    SqlDataAdapter sdp = new SqlDataAdapter();
                //    SqlCommand smd = new SqlCommand();
                //    DataTable dt = new DataTable();
                //    smd.CommandText = "INSERT INTO [DM_zfsms_table] ([companyID], [insTime], [smsType], [sysContent], [" +
                //       "state]) VALUES (@companyID, @insTime, @smsType, @sysContent, @state)";
                //    smd.Connection = Bcon;
                //    Console.WriteLine("将表lpc_zfsms_table中的数据插入到表DM_zfsms_table中==={0}");
                //    smd.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
                //new System.Data.SqlClient.SqlParameter("@companyID", System.Data.SqlDbType.VarChar, 0, "companyID"),
                //new System.Data.SqlClient.SqlParameter("@insTime", System.Data.SqlDbType.DateTime, 0, "insTime"),
                //new System.Data.SqlClient.SqlParameter("@smsType", System.Data.SqlDbType.VarChar, 0, "smsType"),
                //new System.Data.SqlClient.SqlParameter("@sysContent", System.Data.SqlDbType.VarChar, 0, "sysContent"),
                //new System.Data.SqlClient.SqlParameter("@state", System.Data.SqlDbType.Int, 0, "state")});
                //    sdp.InsertCommand = smd;
                //    dt = ds.Tables[0].Copy();//复制和克隆是有区别的
                //    foreach (DataRow drow in ds.Tables[0].Rows)
                //    {
                //        DataRow dr = dt.NewRow();
                //        dr.BeginEdit();
                //        dr[0] = drow["autoId"].ToString();
                //        dr[1] = drow["companyid"].ToString();
                //        dr[2] = drow["insTime"].ToString();
                //        dr[3] = drow["smsType"].ToString();
                //        dr[4] = drow["sysContent"].ToString();
                //        dr[5] = drow["state"].ToString();
                //        dr.EndEdit();
                //        dt.Rows.Add(dr);
                //    }
                //    Bcon.Open();
                //    sdp.Update(dt);
                //    Bcon.Close();
                //    return true;
                SqlBulkCopy bulk = new SqlBulkCopy(Bcon);
                bulk.DestinationTableName = "DM_zfsms_table";
                Console.WriteLine("将表lpc_zfsms_table中的数据插入到表DM_zfsms_table中");
                bulk.BatchSize = ds.Tables[0].Rows.Count;
                bulk.BulkCopyTimeout = 360;
                Bcon.Open();
                bulk.WriteToServer(ds.Tables[0]);
                bulk.Close();
                Bcon.Close();
                return true;
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message.ToString());
                Console.WriteLine("将表lpc_zfsms_table中的数据插入到表DM_zfsms_table中出现异常==={0}", e.Message);
                ErrInfo.WriterErrInfo("SendSMSDll", "InsertBackupZF", e);
                return false;
            }
            finally
            {
                Bcon.Close();
            }
        }
        /// <summary>
        /// 这里是短信的发送
        /// </summary>
        /// <returns></returns>
        public string ImportDLL()
        {
            try
            {
                Console.WriteLine("pos机短信的发送==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                SMSService.smsService sms = new SMSService.smsService();
                Console.Write("调用webservice(smsService)发送短信");
                sms.init(ClientApp.DefaultCompanyID);
                Console.Write("初始化公司ID==={0}", ClientApp.DefaultCompanyID);
                StringBuilder strbd = new StringBuilder();
                XmlWriter writer = XmlWriter.Create(strbd);
                XmlSerializer serializer = new XmlSerializer(typeof(DataTable));
                serializer.Serialize(writer, TbDll);
                Console.WriteLine("序列化短信内容==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                writer.Close();
                Console.WriteLine("短信发送中==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                string Sreturn = sms.sendSMS(Str_Uid, Str_Pwd, ClientApp.DefaultCompanyID, strbd.ToString());
                Console.WriteLine("短信已经发送，返回结果==={0}", Sreturn);
                return "SUCCESS";

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                TbDll.Clear();
            }
        }

    }
}

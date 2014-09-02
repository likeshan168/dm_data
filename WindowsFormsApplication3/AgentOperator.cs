using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Web.Services;
using MySQLDriverCS;

namespace WindowsFormsApplication3
{
    class AgentOperator
    {
        SqlConnection Bcon;
        EmailPhone MailPhone;
        //SqlConnection Mcon;
        //DataTable columnTable;

        //SqlCommand scom;

        private int Iidentity = 0;
        static char[] splitChar1 = { (char)21 };
        static char[] splitChar2 = { (char)22 };
        static char[] splitChar3 = { (char)23 };
       
        public string SetSend="";

       public AgentOperator()
        {
            Bcon = new SqlConnection(ClientApp.Basecon);
            //Mcon = new SqlConnection(ClientApp.localcon);
        }
        public string AgentLogin(string username, string Pwd)
        {
            try
            {
                // string str = "use " + ClientApp.localBase + " select count(*) from cardinfo where Card_id=@username and [password]=@userPwd";
                string str = "select count(*) from acting where cid=@username and [password]=@userPwd";
                SqlParameter Uname = new SqlParameter("@username", SqlDbType.VarChar, 30);
                SqlParameter Upwd = new SqlParameter("@userPwd", SqlDbType.VarChar, 30);
                Uname.Value = username;
                Upwd.Value = Pwd;
                SqlCommand smd1 = new SqlCommand(str);
                smd1.Parameters.Add(Uname);
                smd1.Parameters.Add(Upwd);
                smd1.Connection = Bcon;
                Bcon.Open();
                //SqlDataReader myreader =smd1.ExecuteReader();

                //if (myreader.Read())
                //{
                //    str = GetENnameByChname(username);
                //    return str;
                //}
                //else
                //{
                //    return "Login Error";
                //}
                int j = (int)smd1.ExecuteScalar();
                Bcon.Close();
                if (j > 0)
                {
                    str = GetENnameByChname(username);
                    return str;
                }
                else
                {
                    return "Login Error";
                }
            }
            catch (SqlException ex)
            {
                //Console.Write("VIP登陆验证出错" + ex.Message.ToString());
                ErrInfo.WriterErrInfo("AgentOperator", "AgentLogin", ex);
                Bcon.Close();
                return "AgentLogin error";
            }
            finally
            {
                Bcon.Close();

            }
        }


        private string GetENnameByChname(string cardid)
        {
            try
            {
                string strReturn = "";
                            
                //using (SqlConnection conn = new SqlConnection(ClientApp.localcon))
                //{
                       string str ="select Cname,password,fzr,lxr,phone,mobile,mobile2,cz,email,diqu,address from acting where cid='" + cardid + "'";

                        SqlDataAdapter adp = new SqlDataAdapter(str, Bcon);
                        DataTable mt = new DataTable();
                        Bcon.Open();
                        adp.Fill(mt);
                        Bcon.Close();
                        for (int l = 0; l < mt.Columns.Count; l++)
                        {
                            strReturn = strReturn + mt.Columns[l].ColumnName + (char)21;
                        }
                        strReturn = strReturn.Substring(0, strReturn.LastIndexOf((char)21)) + (char)22;
                        if (mt.Rows.Count > 0)
                        {
                            for (int Rcou = 0; Rcou < mt.Rows.Count; Rcou++)
                            {
                                for (int Ccou = 0; Ccou < mt.Columns.Count; Ccou++)
                                {
                                    strReturn = strReturn + mt.Rows[Rcou][Ccou] + (char)21;
                                }
                                strReturn = strReturn.Substring(0, strReturn.LastIndexOf((char)21)) + (char)22;
                            }
                        }
                
                        str = "select * from dbo.mendian where cid='" + cardid + "'";
                        adp = new SqlDataAdapter(str, Bcon);
                        mt = new DataTable();
                        Bcon.Open();
                        adp.Fill(mt);
                        Bcon.Close();

                        string strid = "";
                        string strname= "";
                        for (int k = 0; k < mt.Rows.Count; k++)
                        {
                            strid = strid + mt.Rows[k]["mid"] + (char)21;
                            strname = strname + mt.Rows[k]["name"] + (char)21;
                        }
                        strid = strid.Trim().Substring(0, strid.LastIndexOf((char)21));
                        strname = strname.Trim().Substring(0, strname.LastIndexOf((char)21));

                        strReturn = strReturn + strid + (char)22 + strname;
                        
                return strReturn;
                
            }
            catch (Exception err)
            {
                ErrInfo.WriterErrInfo("AgentOperator", "GetENnameByChname---Exception", err);
                return "GetChname Error";

            }

        }

        public string VipModifyValue(string sessionid, string strupdata)
        {
            string StrSet = "";
            string StrSelect = "";
            SqlDataAdapter adp;
            DataTable tb;
            string strErp = "";

            try
            {
                string[] strvalue = strupdata.Split((char)22);
                string[] ColValue = new string[] { "Cname", "password", "fzr", "lxr", "phone", "mobile", "mobile2", "cz", "email", "diqu", "address" };
                string[] rowValue = strvalue[0].Split((char)21);

                //Cname,password,fzr,lxr,phone,mobile,mobile2,cz,email,diqu,address;
                for (int i = 0; i < ColValue.Length; i++)
                {
                    StrSet = StrSet + ColValue[i] + "='" + rowValue[i+1] + "', ";
                }



                StrSelect = " update acting set " + StrSet.Trim().Substring(0, StrSet.LastIndexOf(",")) + "  where cid='" + rowValue[0] + "'";

                SqlCommand cmdupdate = new SqlCommand(StrSelect);
                cmdupdate.Connection = Bcon;
                Bcon.Open();
                int m = cmdupdate.ExecuteNonQuery();
                Bcon.Close();

              
                if (m > 0)
                {
                    return "insert TempVipModify Success";
                }
                else
                {
                    return "insert TempVipModify Error";
                }


            }
            catch (Exception ex)
            {
                //Console.Write("VIP修改出错" + ex.Message.ToString());
                ErrInfo.WriterErrInfo("AgentOperator", "AgentModify", ex);
                Bcon.Close();
                return "Agent Updata Error";
            }
            finally
            {
                Bcon.Close();
            }
        }

        public string VipSelectWeb(string sessionid, string[] allLine)
        {
            string strwhere1 = "";
            string strwhere2 = "";
            string strReturn = "";
            SqlDataAdapter adp;
            DataTable mdt;
            try
            {
                //System.IFormatProvider format=new System.Globalization.CultureInfo("zh-CN",true); 
               
                //DateTime Bdate = DateTime.ParseExact(allLine[2],"yyyy-MM-dd",format);
                //DateTime Edate = DateTime.ParseExact(allLine[3],"yyyy-MM-dd",format);

                DateTime Bdate = Convert.ToDateTime(allLine[2].Substring(0, 4) + "-" + allLine[2].Substring(4, 2) + "-" + allLine[2].Substring(6, 2));
                DateTime Edate = Convert.ToDateTime(allLine[3].Substring(0, 4) + "-" + allLine[3].Substring(4, 2) + "-" + allLine[3].Substring(6, 2));
                string agentid = allLine[4];
                int aa = Convert.ToInt16(allLine[5]);
                if (aa > 0)
                {
                   string[] shop = allLine[6].Split(',');
                   for (int i = 0; i < shop.Length; i++)
                   {
                       strwhere1 = strwhere1 + "mid='" + shop[i] + "' or ";
                   }
                   strwhere1 = "(" + strwhere1.Substring(0, strwhere1.LastIndexOf("or")) + " )";
                }

                int bb = Convert.ToInt16(allLine[7]);
                if (bb > 0)
                {
                    string[] person = allLine[8].Split(',');
                    for (int j = 0; j < person.Length; j++)
                    {
                        strwhere2 = strwhere2 + "sales='" + person[j] + "' or ";
                    }
                    strwhere2 = "(" + strwhere2.Substring(0, strwhere2.LastIndexOf("or")) + " )";
                }

                if (strwhere1.Trim().Length > 1)
                {
                    strwhere1 = " and " + strwhere1;
                }

                if (strwhere2.Trim().Length > 1)
                {
                    strwhere2 = " and " + strwhere2;
                }
                string str = "select * from SelectSMS where convert(char(10),senddate,121)>='" + Bdate.Date.ToString("yyyy-MM-dd") + "' and convert(char(10),senddate,121)<='" + Edate.Date.ToString("yyyy-MM-dd") + "' and cid='" + agentid.Trim() + "'" + strwhere1 + strwhere2;
                adp = new SqlDataAdapter(str, Bcon);
                mdt = new DataTable();
                Bcon.Open();
                adp.Fill(mdt);
                Bcon.Close();

                if (mdt.Rows.Count > 0 && mdt!=null)
                {
                    for (int k = 0; k < mdt.Rows.Count; k++)
                    {
                        for (int c = 0; c < mdt.Columns.Count; c++)
                        {
                            strReturn = strReturn + mdt.Rows[k][c].ToString() + ",";
                        }
                        strReturn = strReturn.Substring(0, strReturn.LastIndexOf(",")) + "\r\n";
                    }
                    strReturn = strReturn.Substring(0, strReturn.LastIndexOf("\r\n"));
                    return strReturn;
                }
                else
                {
                    return "Agent Select NoResult";
                }
            }
            catch (Exception ex)
            {
                ErrInfo.WriterErrInfo("AgentOperator", "VipSelectWeb", ex);
                Bcon.Close();
                return "Agent Select Error";
            }
            finally
            {
                Bcon.Close();
            }
        }
        //
        public string VipRegisterWeb(string sessionid, string[] allLine)
        {
            try
            {
                //SqlDataAdapter adp;
                //SqlConnection Mcon;
                //DataTable mt;
                //string shopid = "";
                //string Salesid = "";
                //string insWeb = "";
                string Coupon = "";
               

               MailPhone = new EmailPhone();



                string[] strcon = allLine[2].Split((char)21);
                string[] strdata = strcon[2].Split((char)22);

                #region "暂时不用"
                //bool bmail = MailPhone.ValidatEmail(strdata[3]);

                //if (bmail)
                //{
                //    Mcon = new SqlConnection(ClientApp.localcon);

                //    bool Bshop;
                //    bool BSales;

                //    Bshop = CheckValue("select count(*) from shopinfo where use_id='" + strcon[0] + "'");
                //    if (Bshop)
                //    {
                //        shopid = strcon[0];
                //    }
                //    else
                //    {
                //        shopid = ClientApp.DefaultShop;
                //    }
                //    BSales = CheckSalesId(strcon[1]);
                //    if (BSales)
                //    {
                //        Salesid = strcon[1];
                //    }
                //    else
                //    {
                //        Salesid = ClientApp.DefaultSales;
                //    }

                //    string strIns = "";
                //    string strVal = "";
                //    string cmd = "select ENname,Cnname from datainfo where DBname='" + ClientApp.localBase + "' and (CNname='姓名' or CNname='密码'or CNname='电子邮箱'or CNname='手机号码'or CNname='卡号' or CNname='生日' or CNname='性别') order by cnname";
                //    adp = new SqlDataAdapter(cmd, Mcon);
                //    mt = new DataTable();
                //    Mcon.Open();
                //    adp.Fill(mt);
                //    Mcon.Close();

                //    strIns = "insert into cardinfo (put_nm,card_no,initpoint,posid,put_dt,Inp_dt,Use_dt,Use_bz,Def_reb,is_upload,";
                //    strVal = "values('" + Salesid + "','积分卡会员',50,'" + shopid + "','" + DateTime.Now.ToShortDateString() + "','" + DateTime.Now.ToShortDateString() + "','" + DateTime.Now.AddYears(Convert.ToInt16(ClientApp.UseDate)).ToShortDateString() + "','1',100,3,";
                //    if (mt.Rows.Count > 0)
                //    {
                //        for (int i = 0; i < mt.Rows.Count; i++)
                //        {
                //            strIns = strIns + mt.Rows[i]["ENname"] + ",";
                //        }
                //    }

                //    strVal = strVal + "'" + strdata[3] + "',@cardid,'" + strdata[1] + "','" + strdata[5] + "','" + strdata[2] + "','" + strdata[0] + "','" + strdata[4] + "'";

                //    string cmdInsert = GetInsertValues(strIns.Substring(0, strIns.LastIndexOf(",")), strVal);
                //    string VipNum = CreateVIPNum();
                //    if (VipNum.IndexOf("error") >= 0)
                //    {
                //        ErrInfo.WriterErrInfo("AgentOperator", "VipRegisterWeb", "VIP卡号生成失败");
                //        return "failure";
                //        //在此要有一个返回值。如果生成VIP卡号失败的话要有错误处理。
                //    }
                //    else
                //    {
                //     
                //        //string s = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.UTF8.GetBytes(str)); 
                //        //   insWeb = "insert into customer(customerid,customername,password,email,create_time,birthday,mobile,nickname,type,status)" +
                //        //"values ('" + VipNum + "','" + Encoding.GetEncoding("GBK").GetString(Encoding.Default.GetBytes(strdata[0])) + "','" + strdata[1] + "','" + strdata[3] + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','" + strdata[5] + "','" + strdata[2] + "','',1,1)";

                //        insWeb = "insert into customer(customerid,customername,password,email,create_time,birthday,mobile,nickname,type,status)" +
                //    "values ('" + strdata[3] + "','" + Encoding.GetEncoding("gb2312").GetString(Encoding.Default.GetBytes(strdata[0])) + "','" + strdata[1] + "','" + strdata[3] + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','" + strdata[5] + "','" + strdata[2] + "','',1,1)";


                //        bool Binsert = InsertVIP(cmdInsert, VipNum, insWeb);

                //        if (Binsert)
                //        {

                #endregion


                Coupon = GetCouponId(strdata[0], strcon[5], strcon[4]);
                //        }
                string strTitle = "";
                if (strdata[4] == "男")
                {
                    strTitle = "先生";
                }
                else if (strdata[4] == "女")
                {
                    strTitle = "女士";
                }

                if (Coupon.Trim().Length > 0 && Coupon.IndexOf("error") < 0)
                {
                  
                    if (strcon[3].Trim().ToUpper() == "MOBILE")
                    {
                        SendSMS(strdata[0], strcon[5], strdata[2], Coupon, strTitle, strdata[1]);
                    }

                    if (strcon[3].Trim().ToUpper() == "EMAIL")
                    {
                        SendEmail(strdata[0], strcon[5], strdata[3], Coupon, strTitle, strdata[1]);
                    }

                    if (strcon[3].Trim().ToUpper() == "ALL")
                    {
                        SendSMS(strdata[0], strcon[5], strdata[2], Coupon, strTitle, strdata[1]);
                        SendEmail(strdata[0], strcon[5], strdata[3], Coupon, strTitle, strdata[1]);
                    }
        
                    return "Successful";
                }
                else
                { return ""; }

            }

            catch (Exception ex)
            {

                ErrInfo.WriterErrInfo("AgentOperator", "VipRegisterWeb", ex.Message.ToString());
                return "failure";
            }

        }

        public void SendSMS(string Uname,string vipcode,string phone,string coupon,string title,string pwd)
        {
            if (MailPhone == null)
            { MailPhone = new EmailPhone(); }
            if (MailPhone.IsChinaUnicomNumber(phone))
            {
                string ss = strMessage(Uname, vipcode, ClientApp.CouponValue, coupon,pwd,title);
                if (!MailPhone.SendSMS(phone, ss))
                {
                    ErrInfo.WriterErrInfo("AgentOperator", "SendSMS", vipcode + "VipNum插入sendData表失败");
                }
            }
            else
            {
                ErrInfo.WriterErrInfo("AgentOperator", "SendSMS", vipcode + "手机号码验证失败");
            }
 
        }

        public void SendEmail(string Uname, string vipcode, string mail, string coupon,string title,string pwd)
        {
            if (MailPhone == null)
            { MailPhone = new EmailPhone(); }
            if (MailPhone.IsEmail(mail))
            {
                string ss = strMessage(Uname, vipcode, ClientApp.CouponValue, coupon,pwd,title);
                if (!MailPhone.BsendMail(mail, ss,0,"注册送好礼"))
                {
                    ErrInfo.WriterErrInfo("AgentOperator", "SendEmail", vipcode + "发送邮件失败");
                }
            }
            else
            {
                ErrInfo.WriterErrInfo("AgentOperator", "SendEmail", vipcode + "电子邮箱验证失败");
            }

        }
        //@Uname  @vipcode  @value  @id
        public string strMessage(string name, string vip, string value, string cou,string pwd,string title)
        {
            //string ss = ClientApp.MessageInfo;
            string ss = ClientApp.DefaultCompanyID;
            ss = ss.Replace("@Uname", name.Trim());
            ss = ss.Replace("@title", title.Trim());
            ss = ss.Replace("@vipcode", vip.Trim());
            ss = ss.Replace("@pwd", pwd.Trim());
            ss = ss.Replace("@value", value.Trim());
            ss = ss.Replace("@id", cou.Trim());
            return ss;
        }


       public string CreateVIPNum()
        {
            string msg = "";
            string str = "";
            DataTable mdt;
            SqlDataAdapter adp;
            int max, Cur = 0;
            str = "select MaxNum,CurrentNum+step as VIP from vipcode where type='VIP'";

            adp = new SqlDataAdapter(str, Bcon);
            mdt = new DataTable();
            Bcon.Open();
            adp.Fill(mdt);
            Bcon.Close();

            if (mdt.Rows.Count > 0)
            {
                max = (int)mdt.Rows[0]["MaxNum"];
                Cur = (int)mdt.Rows[0]["VIP"];

                if (max > Cur)
                {
                    msg = mdt.Rows[0]["VIP"].ToString();
                }
                else
                {
                    msg = "error.VIP超出最大限值:" + max.ToString();
                }
            }
            else
            {
                msg = "error.VIP卡号成生失败。";
            }

            return msg;

        }

        SqlTransaction trans;
        //public bool InsertVIP(string cmdIns,string vipNum,string insweb)
        //{
                       
        //    SqlCommand inCmd;
        //    SqlCommand upCmd;
        //    MySQLConnection DBConn = null;
            
        //    try
        //    {

        //        Bcon.Open();

        //        trans = Bcon.BeginTransaction("Mytrans");
                
        //        SqlParameter cardid = new SqlParameter("@cardid", SqlDbType.VarChar, 16);
        //        cardid.Value = vipNum;
        //        inCmd = new SqlCommand(cmdIns + " ;SELECT SCOPE_IDENTITY() from cardinfo");
        //        inCmd.Parameters.Add(cardid);

        //        inCmd.Connection = Bcon;
        //        inCmd.Transaction = trans;
        

        //        upCmd = new SqlCommand("update vipcode set CurrentNum=" + Convert.ToInt32(vipNum) + "where type='VIP'");
        //        upCmd.Transaction = trans;
        //        upCmd.Connection = Bcon;


        //        Iidentity = Convert.ToInt32(inCmd.ExecuteScalar());
        //       // inCmd.ExecuteScalar();
        //        //inCmd.ExecuteNonQuery();
        //        upCmd.ExecuteNonQuery();
        //        DBConn = new MySQLConnection(new MySQLConnectionString("127.0.0.1", "fuzhuang", MysqlSource.User, MysqlSource.Pwd, 3333).AsString);
        //        MySQLCommand cmd = new MySQLCommand("set   charset   gb2312", DBConn);
               
        //        DBConn.Open();
        //        cmd.ExecuteNonQuery();
                
        //        cmd.Dispose();   

                
        //        MySQLCommand DBComm = new MySQLCommand();
        //        DBComm.CommandText = Encoding.GetEncoding("gb2312").GetString(Encoding.GetEncoding("gb2312").GetBytes(insweb));
        //        DBComm.Connection=DBConn;
        //        //DBConn.Open();
        //        int iexec = DBComm.ExecuteNonQuery();
        //        if (iexec > 0)
        //        {
        //            trans.Commit();
        //            Bcon.Close();
        //            DBConn.Close();
        //            return true;
        //        }
        //        else
        //        {
        //            trans.Rollback();
        //            Bcon.Close();
        //            DBConn.Close();
        //            return false;
        //        }
           
        //    }
        //    catch (Exception err)
        //    {
        //        trans.Rollback();
        //        Bcon.Close();
        //        DBConn.Close();
        //        ErrInfo.WriterErrInfo("AgentOperator", "InsertVIP", err.Message.ToString());
        //        return false;
        //    }
        //    finally
        //    {
        //        Bcon.Close();
        //    }

        //}

        public bool CheckSalesId(string id)
        {
            try
            {
                SqlCommand cmd;
                cmd = new SqlCommand("select count(*) from salerInfo where set_id='" + id + "'", Bcon);
                Bcon.Open();
                int icou = (int)cmd.ExecuteScalar();
                Bcon.Close();
                if (icou > 0)
                { return true; }
                else { return false; }
            }
            catch (Exception e)
            {
                ErrInfo.WriterErrInfo("AgentOperator", "CheckSalesId", e.Message);
                return false;
            }
            finally
            { Bcon.Close(); }
        }
        public bool CheckValue(string strcmd)
        {
            try
            {
                SqlCommand cmd;
                cmd = new SqlCommand(strcmd, Bcon);
                Bcon.Open();
                int icou = (int)cmd.ExecuteScalar();
                Bcon.Close();

                if (icou > 0)
                { return true; }
                else { return false; }
            }
            catch (Exception e)
            {
                ErrInfo.WriterErrInfo("AgentOperator", "CheckValue", e.Message);
                return false;
            }
            finally
            { Bcon.Close(); }
        }
        
        public string GetInsertValues(string strins,string strvalue)
        {
            try
            {
                SqlDataAdapter sqladp;
                DataTable dt;

                string cmd = "select column_name,column_Default,is_nullable,data_type" +
               //  " from information_schema.columns a left join " + ClientApp.localConfig + ".dbo.datainfo b on a.column_name=b.enname" +
                " from information_schema.columns a left join datainfo b on a.column_name=b.enname" +
                " where a.table_name='cardinfo' and  b.dbname='" + ClientApp.localBase + "' and is_nullable='NO' and column_Default is null and" +
                " CNname<>'姓名' and CNname<>'密码'and CNname<>'电子邮箱'and CNname<>'手机号码'and CNname<>'卡号' and CNname<>'生日' and CNname<>'性别' order by ordinal_position";

                sqladp = new SqlDataAdapter(cmd, Bcon);
                dt = new DataTable();
                Bcon.Open();
                sqladp.Fill(dt);
                Bcon.Close();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        switch (dt.Rows[i]["data_type"].ToString())
                        {
                            case "int":
                            case "tinyint":
                            case "smallint":
                            case "bigint": strins = strins +","+ dt.Rows[i]["column_name"]; strvalue = strvalue + ",0"  ;
                                break;
                            case "char":
                            case "varchar":
                            case "nvarchar":
                            case "nchar": strins = strins + "," + dt.Rows[i]["column_name"]; strvalue = strvalue + ",''";
                                break;
                            case "datetime": strins = strins + "," + dt.Rows[i]["column_name"]; strvalue = strvalue + ",'1900-01-01'";
                                break;
                            case "float":
                            case "double":
                            case "numeric":
                            case "money": strins = strins + "," + dt.Rows[i]["column_name"]; strvalue = strvalue + ",0.0";
                                break;
                            default: strins = strins + "," + dt.Rows[i]["column_name"]; strvalue = strvalue + ",'0'";
                                break;
                        }
                    }
                }

                strins = strins + ")" + strvalue + ")";
                return strins;
            }
                            
            catch(Exception e)
            {

                ErrInfo.WriterErrInfo("AgentOperator", "GetInsertValues", e.Message);
                return "error";
            }
        }
        
        public string GetCouponId(string uname,string vip,string IP)
        {
            //try
            //{
            //  //  string ComPanyID = "00002";
            //    MySQLConnection DBCon;
            //    bool bSwitch = false;

            //    CreateXML cc = new CreateXML();
            //    string ss = cc.CreateXmlStr(uname, vip, ClientApp.id, IP, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),ClientApp.CouponValue);
                
            //    Getcoupon.couponService dd = new Getcoupon.couponService();
            //    string returnSS = dd.GetNewPC(ss);
            //    if (returnSS.IndexOf("cid")>=0)
            //    {
            //        ss = cc.Xmlstr(returnSS.Trim());
            //        string[] temp = ss.Split((char)21);
            //        //string msg = "";
            //        //for (int i = 0; i < temp.Length; i++)
            //        //{
            //        //    msg = msg + temp[i] + "\n";
            //        //}
            //        if (bSwitch)
            //        {
            //            try
            //            {
            //                DBCon = new MySQLConnection(new MySQLConnectionString(MysqlSource.StrCon, MysqlSource.DB, MysqlSource.User, MysqlSource.Pwd, MysqlSource.Port).AsString);

            //                string str = "insert into gift_cards(id,card_no,password,account,create_time,type,msrepl_synctran_ts,price,applicant,handle_man,note,catalog,isvalid," +
            //                             "validdaytime,availability,isfill,isonce,max_no,who_use,user_id,order_serialno,flag)" +
            //                "values('" + temp[0] + "','" + temp[0] + "','',0,'" + DateTime.Now.ToShortDateString() + "','C001','','" + temp[1] + ",'" + vip + "','','','',1,'" + ss[2] + "',1,0,1,0,'','','',0)";
            //                MySQLCommand cmd = new MySQLCommand(str, DBCon);
            //                DBCon.Open();
            //                cmd.ExecuteNonQuery();
            //                cmd.Dispose();
            //                DBCon.Close();
            //            }
            //            catch(Exception ex)
            //            {
            //                ErrInfo.WriterErrInfo("AgentOperator", "GetCouponId---------将优惠券记录插入mysql数据库失败.ID:"+vip, ex.Message);
            //            }
            //        }
            //        return temp[2];
            //    }
            //    else
            //    {
            //        return "error";
            //    }
            //}
            //catch (Exception e)
            //{
            //    ErrInfo.WriterErrInfo("AgentOperator", "GetCouponId", e.Message);
              return "error";
            //}
        }




       
        
             
    }
}

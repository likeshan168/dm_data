using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using MySQLDriverCS;
using System.Configuration;
namespace WindowsFormsApplication3
{
    public class WebVipOperator
    {
        SqlConnection Bcon;
        // SqlConnection Mcon;

        SqlCommand scom;
        DataTable columnTable;
        SqlTransaction tran;
        //SqlTransaction Btran;

        string[] row;
        string[] column;
        int columnCount;
        static char[] splitChar1 = { (char)21 };
        static char[] splitChar2 = { (char)22 };
        static char[] splitChar3 = { (char)23 };
        string CardId = "";
        public string SetSend = "";

        public WebVipOperator()
        {
            Bcon = new SqlConnection(ClientApp.Basecon);
            //Mcon = new SqlConnection(ClientApp.localcon);
        }
        public string VipLogin(string username, string Pwd, string ip)
        {
            #region "VIP登陆现在从网站直接登陆。以下代码不再使用"
            //string UName = "";
            //string Enname = "";
            //string email = "";
            //string vid = "";
            //string strEmail = "";
            //SqlDataAdapter mdp;
            //DataTable dt;
            //try
            //{
            //    // string str = "use " + ClientApp.localBase + " select count(*) from cardinfo where Card_id=@username and [password]=@userPwd";

            //    string sql = "select enname from datainfo where dbname='" + ClientApp.localBase + "' and (cNname='密码' or cNname='姓名' or cnname='卡号' or cnname='电子邮箱') order by cnname";
            //    SqlCommand cmd = new SqlCommand(sql);
            //    cmd.Connection = Mcon;

            //    dt = new DataTable();
            //    mdp = new SqlDataAdapter(cmd);
            //    Mcon.Open();
            //    mdp.Fill(dt);
            //    Mcon.Close();

            //    if (dt.Rows.Count == 4)
            //    {
            //        Enname = dt.Rows[2][0].ToString();
            //        UName = dt.Rows[3][0].ToString();
            //        email = dt.Rows[0][0].ToString();
            //        vid = dt.Rows[1][0].ToString();
            //    }
            //    else
            //    {
            //        return "Login Error";
            //    }

            //    string str = "select empox_favor," + UName + "," + email + " from cardinfo where ([" + vid + "]=@username or  [" + email + "]=@username) and [" + Enname + "]=@userPwd";
            //    SqlParameter Uname = new SqlParameter("@username", SqlDbType.VarChar, 30);
            //    SqlParameter Upwd = new SqlParameter("@userPwd", SqlDbType.VarChar, 30);
            //    Uname.Value = username;
            //    Upwd.Value = Pwd;
            //    SqlCommand smd1 = new SqlCommand(str);
            //    smd1.Parameters.Add(Uname);
            //    smd1.Parameters.Add(Upwd);
            //    smd1.Connection = Bcon;

            //    mdp = new SqlDataAdapter(smd1);
            //    dt = new DataTable();

            //    Bcon.Open();
            //    mdp.Fill(dt);
            //    Bcon.Close();



            //    if (dt.Rows.Count < 1)
            //    {
            //        return "Login Error";
            //    }
            //    else
            //    {
            //        UName = dt.Rows[0][1].ToString();//为姓名赋值

            //        int j = Convert.ToInt16(dt.Rows[0]["empox_favor"]);
            //        strEmail = dt.Rows[0][2].ToString();
            //        string strInfo = strEmail.Length > 0 ? "Completed" : "Not filled";
            //        if (j > 0)
            //        {

            //            str = GetENnameByChname(username) + (char)22 + "HaveSend" + (char)22 + strInfo;

            //        }
            //        else
            //        {
            //            AgentOperator ao = new AgentOperator();
            //            string id = ao.GetCouponId(UName, username, ip);
            //            str = GetENnameByChname(username) + (char)22 + id + (char)22 + strInfo;
            //            smd1.CommandText = "update cardinfo set empox_favor=1 where Card_id='" + username + "'";
            //            Bcon.Open();
            //            smd1.ExecuteNonQuery();
            //            Bcon.Close();

            //        }
            //        return str;
            //    }
            //}
            //catch (SqlException ex)
            //{
            //    //Console.Write("VIP登陆验证出错" + ex.Message.ToString());
            //    ErrInfo.WriterErrInfo("WebVipOperator", "VipLogin", ex);
            //    Bcon.Close();
            //    return "VIP Login error";
            //}
            //finally
            //{
            //    Bcon.Close();

            //}
            return "";
            #endregion
        }

        public string VipSelect(string Carid)
        {
            try
            {

                string strinfo = "";

                SqlCommand smd = new SqlCommand();

                //smd.CommandText = " select * from cardinfo where Card_id=@mcardid";
                smd.CommandText = "select card_id,card_no,VipDot+initpoint as VipDot,use_nm,pwd,sex_lb,title,bir_dt,pas_no,e_mail,rea_no from cardinfo where e_mail=@mail";
                //smd.CommandText = "ExecSelect";
                //smd.CommandType = CommandType.StoredProcedure;
                SqlParameter cardid = new SqlParameter("@mail", SqlDbType.VarChar, 40);
                cardid.Value = Carid;
                smd.Parameters.Add(cardid);
                smd.Connection = Bcon;
                SqlDataAdapter adp = new SqlDataAdapter(smd);
                DataTable mt = new DataTable();
                Bcon.Open();
                adp.Fill(mt);
                // reinfo = smd.ExecuteScalar().ToString();
                Bcon.Close();
                if (mt.Rows.Count > 0)
                {
                    #region"2009-04-16注释无用代码"
                    ////for (int i = 0; i < mt.Rows.Count; i++)
                    ////    {
                    ////        for (int j = 0; j < mt.Columns.Count; j++)
                    ////        {
                    ////      strinfo = strinfo + mt.Columns[j].ColumnName + MsgMacro.Split1 + mt.Rows[i][j] + MsgMacro.Split2;
                    ////        }
                    ////        strinfo.Substring(0, strinfo.LastIndexOf(MsgMacro.Split2));
                    ////        strinfo = strinfo + MsgMacro.Split3;
                    ////    }
                    ////    strinfo.Substring(0, strinfo.LastIndexOf(MsgMacro.Split3));
                    ////    return strinfo.Trim();

                    //for (int j = 0; j < mt.Columns.Count; j++)
                    //{
                    //    if (mt.Columns[j].ColumnName.ToString().Trim() != "Iden_ID")
                    //    {
                    //        strinfo = strinfo + mt.Columns[j].ColumnName + MsgMacro.Split1;
                    //    }
                    //}
                    //strinfo = strinfo.Substring(0, strinfo.LastIndexOf(MsgMacro.Split1)) + MsgMacro.Split2;

                    //for (int i = 0; i < mt.Rows.Count; i++)
                    //{
                    //    for (int k = 0; k < mt.Columns.Count; k++)
                    //    {
                    //        if (mt.Columns[k].ColumnName.ToString().Trim() != "Iden_ID")
                    //        {
                    //            strinfo = strinfo + mt.Rows[i][k] + MsgMacro.Split1;
                    //        }
                    //    }
                    //    strinfo = strinfo.Substring(0, strinfo.LastIndexOf(MsgMacro.Split1));//+ MsgMacro.Split3
                    //}
                    #endregion

                    for (int i = 0; i < mt.Rows.Count; i++)
                    {
                        string[] arr = { "123 " }; //=Convert.ToString(mt.Rows[i].ItemArray);
                        for (int k = 0; k < arr.Length; k++)
                        {
                            strinfo = strinfo + arr[k] + (char)21;
                        }
                        strinfo = strinfo.Substring(0, strinfo.LastIndexOf((char)21));
                    }
                    return strinfo.Trim();
                }
                else
                {
                    return "Select Error";
                }
            }
            catch (Exception ex)
            {
                //Console.Write("VIP查询出错" + ex.Message.ToString());
                ErrInfo.WriterErrInfo("WebVipOperator", "VipSelect", ex);
                Bcon.Close();
                return "VIP Select Error";
            }
            finally
            {
                Bcon.Close();
            }
        }



        #region "以前代码．现不用"

        //public string VipModify(string sessionid,string strupdata)
        //{
        //    try
        //    {

        //        SqlCommand cmdupdate = new SqlCommand(strupdata);
        //        cmdupdate.Connection = con;
        //        con.Open();
        //        int j = cmdupdate.ExecuteNonQuery();
        //        con.Close();


        //        string str = "insert into tempvipModify (sessionid,strCommand) values('" + sessionid + "',@myfil)";

        //        SqlParameter mm = new SqlParameter("@myfil", SqlDbType.Image);
        //        mm.Value = Encoding.Default.GetBytes(strupdata);

        //        SqlCommand cmdinsert = new SqlCommand(str);
        //        cmdinsert.Parameters.Add(mm);
        //        cmdinsert.Connection = con;
        //        con.Open();
        //        int k = cmdinsert.ExecuteNonQuery();
        //        con.Close();
        //        if (j>0 && k > 0)
        //        {
        //            return "insert TempVipModify Success";
        //        }
        //        else
        //        {
        //            return "insert TempVipModify Error";
        //         }
        //    }
        //    catch (Exception ex)
        //    {
        //        //Console.Write("VIP修改出错" + ex.Message.ToString());
        //        ErrInfo.WriterErrInfo("WebVipOperator", "VipModify", ex);
        //        con.Close();
        //        return "VIP Updata Error";
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }

        //}
        #endregion

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
                string[] ColValue = strvalue[1].Split((char)21);
                string[] rowValue = strvalue[2].Split((char)21);
                //Mcon.Open();
                Bcon.Open();
                for (int i = 0; i < ColValue.Length; i++)
                {
                    //StrSelect = "select type,udefined From datainfo where ENname='" + ColValue[i] + "' and DBname='" + ClientApp.localBase + "'";

                    StrSelect = "select type,udefined From datainfo where ENname='" + ColValue[i] + "'";
                    //adp = new SqlDataAdapter(StrSelect, Mcon);
                    adp = new SqlDataAdapter(StrSelect, Bcon);
                    tb = new DataTable();
                    adp.Fill(tb);

                    if (tb.Rows.Count > 0)
                    {
                        for (int j = 0; j < tb.Rows.Count; j++)
                        {

                            if (tb.Rows[j]["type"].ToString() == "整数型" || tb.Rows[j]["type"].ToString() == "小数型")
                            {
                                StrSet = StrSet + ColValue[i] + "=" + rowValue[i] + " , ";
                            }
                            if (tb.Rows[j]["type"].ToString() == "文本型" || tb.Rows[j]["type"].ToString() == "日期型")
                            {
                                StrSet = StrSet + ColValue[i] + "='" + rowValue[i] + "' , ";
                            }

                            if (!(bool)tb.Rows[j]["udefined"])
                            {
                                if (tb.Rows[j]["type"].ToString() == "整数型" || tb.Rows[j]["type"].ToString() == "小数型")
                                {
                                    strErp = strErp + ColValue[i] + "=" + rowValue[i] + " , ";
                                }
                                if (tb.Rows[j]["type"].ToString() == "文本型" || tb.Rows[j]["type"].ToString() == "日期型")
                                {
                                    strErp = strErp + ColValue[i] + "='" + rowValue[i] + "' , ";
                                }
                            }

                        }
                    }
                }

                //                Mcon.Close();
                Bcon.Close();

                //StrSelect = " update cardinfo set " + StrSet.Trim().Substring(0, StrSet.LastIndexOf(",")) + " where card_id='" + strvalue[0] + "'";

                //SqlCommand cmdupdate = new SqlCommand(StrSelect);
                //cmdupdate.Connection = Bcon;
                //Bcon.Open();
                //int m = cmdupdate.ExecuteNonQuery();
                //Bcon.Close();

                SetSend = "set " + strErp.Trim().Substring(0, strErp.LastIndexOf(",")) + " where card_id='" + strvalue[0] + "'";


                string str = "insert into tempvipModify (sessionid,strCommand,companyid) values('" + sessionid + "',@SetSend,'" + ClientApp.id + "')";

                SqlCommand cmdinsert = new SqlCommand(str);

                SqlParameter Command = new SqlParameter("@SetSend", SqlDbType.Image);
                Command.Value = Encoding.Default.GetBytes(SetSend);
                cmdinsert.Parameters.Add(Command);
                //cmdinsert.Connection = Mcon;
                //Mcon.Open();
                cmdinsert.Connection = Bcon;
                Bcon.Open();
                int k = cmdinsert.ExecuteNonQuery();
                //Mcon.Close();
                Bcon.Close();
                // if (m > 0 && k > 0)
                if (k > 0)
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
                ErrInfo.WriterErrInfo("WebVipOperator", "VipModifyValue", ex);
                //Mcon.Close();
                Bcon.Close();
                return "VIP Updata Error";
            }
            finally
            {
                //Mcon.Close();
                Bcon.Close();
            }
        }

        public bool VipTempModify(string sessionid)
        {
            try
            {
                //Mcon = new SqlConnection(ClientApp.Basecon);
                Bcon = new SqlConnection(ClientApp.Basecon);
                string strupdata = "update tempvipModify set flag=1 where sessionid='" + sessionid + "'";
                SqlCommand cmd = new SqlCommand(strupdata);
                //cmd.Connection = Mcon;
                cmd.Connection = Bcon;
                //Mcon.Open();
                Bcon.Open();
                int j = cmd.ExecuteNonQuery();
                Bcon.Close();
                //Mcon.Close();
                if (j > 0)
                {
                    return true;
                }
                else { return false; }
            }
            catch (SqlException ex)
            {
                //Console.WriteLine(ex.Message.ToString());
                ErrInfo.WriterErrInfo("WebVipOperator", "VipTempModify", ex);
            }
            return false;
        }

        public string VipUpdateEmailAddress(string sessionid, string email, string cardid)
        {
            try
            {
                //EmailPhone ep = new EmailPhone();
                //bool bin = ep.ValidatEmail(sessionid);
                //if (bin)
                //{
                //    scom = new SqlCommand("update cardinfo set E_mail='" + sessionid + "' where card_id='" + email + "'");
                //    scom.Connection = Bcon;
                //    Bcon.Open();
                //    i=scom.ExecuteNonQuery();
                //    Bcon.Close();
                //}
                //else
                //{
                //    return "has been";
                //}
                //if (i > 0)
                //    return "success";
                //else
                //    return "error";
                SetSend = "set E_mail='" + email + "' where card_id='" + cardid + "'";

                string str = "insert into tempvipModify (sessionid,strCommand,companyid) values('" + sessionid + "',@SetSend,'" + ClientApp.id + "')";

                SqlCommand cmdinsert = new SqlCommand(str);

                SqlParameter Command = new SqlParameter("@SetSend", SqlDbType.Image);
                Command.Value = Encoding.Default.GetBytes(SetSend);
                cmdinsert.Parameters.Add(Command);
                //cmdinsert.Connection = Mcon;
                cmdinsert.Connection = Bcon;
                //Mcon.Open();
                Bcon.Open();
                int k = cmdinsert.ExecuteNonQuery();
                //Mcon.Close();
                Bcon.Close();
                if (k > 0)
                {

                    return "insert TempVipModify Success";
                }
                else
                {
                    return "insert TempVipModify Error";
                }

            }
            catch (Exception e)
            {
                ErrInfo.WriterErrInfo("WebVipOperator", "VipUpdateEmailAddress", e.Message.ToString());
                return "error";
            }

        }


        public byte[] DMSendVipMod(byte code, long mtime, byte[] data)
        {
            int time = (int)(mtime / 1000);
            byte[] Adate = ByteConvert.intToByteArray(time);

            byte status = 0;

            byte[] msbyt = new byte[7 + data.Length];
            MemoryStream ms = new MemoryStream(msbyt);

            ms.WriteByte(code);
            ms.Write(Adate, 0, 4);
            ms.WriteByte(status);
            ms.WriteByte((byte)data.Length);
            ms.Write(data, 0, data.Length);

            return ms.ToArray();
        }

        public bool VipTempDel(string sessionid)
        {
            try
            {
                // Mcon = new SqlConnection(ClientApp.Basecon);
                Bcon = new SqlConnection(ClientApp.Basecon);
                string strDel = "delete from TempVipModify where sessionid='" + sessionid + "'";
                SqlCommand cmd = new SqlCommand(strDel);
                //cmd.Connection = Mcon;
                cmd.Connection = Bcon;
                //Mcon.Open();
                Bcon.Open();
                int j = cmd.ExecuteNonQuery();
                //Mcon.Close();
                Bcon.Close();
                if (j > 0)
                {
                    return true;
                }
                else { return false; }
            }
            catch (SqlException ex)
            {
                //Console.WriteLine(ex.Message.ToString());
                ErrInfo.WriterErrInfo("WebVipOperator", "VipTempDel", ex);
            }
            return false;
        }
        /// <summary>
        /// 定时下载vip基本资料（这个时间是有直复平台设定）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool TimingVipDownload(byte[] data)
        {
            int timeUpd = 0;
            int oldtime = 0;
            int newtime = 0;
            string nowTime = "";
            try
            {

                byte[] bytesr = data;
                if (bytesr != null)
                {

                    scom = new SqlCommand();
                    scom.Connection = Bcon;



                    string vipData = Encoding.GetEncoding("GB2312").GetString(bytesr);

                    #region 有数据
                    if (!string.IsNullOrEmpty(vipData))
                    {
                        try
                        {
                            Console.WriteLine("有vip定时下载的数据==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            columnTable = new DataTable();
                            columnTable.Columns.Add("colName", typeof(string));
                            columnTable.Columns.Add("colType", typeof(string));
                            columnTable.Columns.Add("colLenth", typeof(string));
                            columnTable.Columns.Add("colCName", typeof(string));
                            string[] t = vipData.Split(splitChar3[0]);
                            //t[0]="";
                            //t[1]="0";
                            //t[2]=card_Idvarchar20卡号card_Typevarchar20卡类型card_Discountnumeric4折扣userNamevarchar12姓名userSexchar2性别userTitlevarchar20称呼userBirthdayvarchar10生日userPhonevarchar20电话userMobilevarchar20手机号码userEmailvarchar60电子邮箱userCodevarchar100证件号userPostvarchar10邮编userAddressvarchar60联系地址sendClientvarchar20发卡店铺sendManvarchar20发卡人beginDatechar10启用日期endDatechar10截止日期pointsnumeric4积分pwdvarchar20密码remarkvarchar100备注

                            //t[3]=15
                            //t[4]=A001白银卡95许炜女小炜1985-2-271358150902713581509027xuweiling852270@126.com核桃2012-8-312012-8-3136.00000000eissy

                            //int iRow = ByteConvert.byteArrayToInt(Encoding.Default.GetBytes(t[3]));
                            int iRow = Convert.ToInt32(t[3]);
                            //判断ERP传过来的数据行数是否大于0
                            if (iRow > 0)
                            {

                                string[] columnInfo = t[2].Split(splitChar1[0]);
                                for (int i = 1; i < columnInfo.Length; i++)
                                {
                                    DataRow myrow = columnTable.NewRow();
                                    myrow.ItemArray = columnInfo[i].Split(splitChar2[0]);
                                    columnTable.Rows.Add(myrow);
                                }
                                row = t[4].Split(splitChar1[0]);
                                //columnCount = row[1].ToString().Split(splitChar2[0]).Length;
                                //string datasql = "select count(size) from datainfo where DBname='" + ClientApp.localBase + "'";
                                //查询datainfo表是否有数据
                                string datasql = "select count(size) from datainfo";
                                Console.WriteLine("查询datainfo表是否有数据==={0}", datasql);
                                scom.CommandText = datasql;
                                Bcon.Open();
                                int j = Convert.ToInt32(scom.ExecuteScalar());
                                Bcon.Close();
                                try
                                {
                                    //判断datainfo表是否有数据
                                    if (j == 0)
                                    {
                                        Console.WriteLine("datainfo表中没有数据==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                        Bcon.Open();
                                        tran = Bcon.BeginTransaction();
                                        scom.Transaction = tran;

                                        Console.WriteLine("向datainfo表中插入数据==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                        //datainfo表没有数据，插入数据
                                        for (int i = 0; i < columnTable.Rows.Count; i++)
                                        {
                                            string insdata = "insert into datainfo values('" + columnTable.Rows[i][0].ToString() +
                                                "','" + columnTable.Rows[i][3].ToString() + "',";
                                            switch (columnTable.Rows[i][1].ToString())
                                            {
                                                case "int":
                                                case "tinyint":
                                                case "smallint":
                                                case "bigint": insdata += "'整数型',4,";
                                                    break;
                                                case "char":
                                                case "varchar":
                                                case "nchar": insdata += "'文本型'," + columnTable.Rows[i][2].ToString() + ",";
                                                    break;
                                                case "datetime": insdata += "'日期型',8,";
                                                    break;
                                                case "float":
                                                case "double":
                                                case "numeric":
                                                case "money": insdata += "'小数型',8,";
                                                    break;
                                                default: insdata += "'文本型',1000,";
                                                    break;
                                            }
                                            // insdata += "null,0,0,0,0,0,0,'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                            insdata += "null,0,0,0,0,'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                            scom.CommandText = insdata;
                                            //scom.Transaction = tran;
                                            scom.ExecuteNonQuery();

                                        }
                                        //string insdata2 = "insert into datainfo values('" + ClientApp.localBase + "','is_Upload','是否为导入数据','整数型',4,null,0,0,0,0,0,1,'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                        string insdata2 = "insert into datainfo values('is_Upload','是否为导入数据','整数型',4,null,0,0,0,1,'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                        scom.CommandText = insdata2;
                                        //scom.Transaction = tran;
                                        scom.ExecuteNonQuery();

                                        string insdata3 = "insert into datainfo values('empox_favor','已发优惠券','整数型',4,null,0,0,0,1,'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                        scom.CommandText = insdata3;
                                        //scom.Transaction = tran;
                                        scom.ExecuteNonQuery();
                                        tran.Commit();
                                    }
                                    else
                                    {
                                        Console.WriteLine("(定时下载vip资料)datainfo表中已经有数==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                    }

                                }
                                catch (SqlException e)
                                {
                                    tran.Rollback();
                                    ErrInfo.WriterErrInfo("WebVipOperator", "TimingVipDownload---------SqlException", e.Message.ToString());
                                    Console.WriteLine("(定时下载vip资料)向datainfo表中插入数据出现异常==={0}", e.Message);
                                }
                                finally
                                { Bcon.Close(); }




                                string UDFileSql = " if not exists(select * from sysobjects where id = object_id(N'[dbo].[UD_Fileds]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) create table UD_Fileds(card_id varchar(20) not null, is_Upload int default 0 not null,empox_favor bit default 1 not null,Iden_ID int identity(1,1) not null)";
                                Console.WriteLine("判断表UD_Fileds是否存在，如果不存在就创建==={0}", UDFileSql);
                                scom.CommandText = UDFileSql;
                                Bcon.Open();
                                scom.ExecuteNonQuery();

                                //string checkTableSql = "use " + ClientApp.localBase + " select count(*) from sysobjects where name='cardInfo' and type='U'";
                                //查询系统表看cardinfo表是否存在
                                string checkTableSql = " select count(*) from sysobjects where name='cardInfo' and type='U'";
                                Console.WriteLine("判断表cardInfo是否存在，如果不存在就创建==={0}", checkTableSql);
                                scom.CommandText = checkTableSql;
                                //scom.Connection = Bcon;
                                //Bcon.Open();
                                int x = Convert.ToInt32(scom.ExecuteScalar());
                                Bcon.Close();
                                if (x == 0)
                                {
                                    //cardinfo表不存在，建表
                                    try
                                    {
                                        Console.WriteLine("表cardInfo不存在==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                        //建cardinfo表
                                        Bcon.Open();
                                        tran = Bcon.BeginTransaction();
                                        scom.Transaction = tran;

                                        int cardPlace = Convert.ToInt32(t[1].ToString());
                                        //string createTablesql = "use " + ClientApp.localBase + " if not exists(select * from sysobjects where id = object_id(N'[dbo].[cardInfo]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) create table cardInfo(";
                                        string createTablesql = " if not exists(select * from sysobjects where id = object_id(N'[dbo].[cardInfo]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) create table cardInfo(";
                                        for (int i = 0; i < columnTable.Rows.Count; i++)
                                        {
                                            switch (columnTable.Rows[i][1].ToString())
                                            {
                                                case "int":
                                                case "tinyint":
                                                case "smallint":
                                                case "bigint": createTablesql += "[" + columnTable.Rows[i][0] + "]" + " int,";
                                                    break;
                                                case "char":
                                                case "varchar":
                                                case "nchar": createTablesql += "[" + columnTable.Rows[i][0] + "]" + " varchar(" + columnTable.Rows[i][2].ToString() + "),";
                                                    break;
                                                case "datetime": createTablesql += "[" + columnTable.Rows[i][0] + "]" + " datetime,";
                                                    break;
                                                case "float":
                                                case "double":
                                                case "numeric": //createTablesql += "[" + columnTable.Rows[i][0] + "]" + " numeric(6,3),"; //2009.11.25 edit
                                                case "money": createTablesql += "[" + columnTable.Rows[i][0] + "]" + " float,";
                                                    break;
                                                default: createTablesql += columnTable.Rows[i][0] + " varchar(1000),";
                                                    break;
                                            }
                                            if (i == cardPlace)
                                                createTablesql = createTablesql.Substring(0, createTablesql.Length - 1) + " primary key not null,";
                                        }
                                        //createTablesql += "is_Upload int default 0 not null,empox_favor bit default 1 not null,Iden_ID int identity(1,1) not null)";
                                        //createTablesql += "is_Upload int default 0 not null,Iden_ID int identity(1,1) not null)";
                                        createTablesql = createTablesql.TrimEnd(',');
                                        createTablesql += ")";
                                        scom.CommandText = createTablesql;
                                        Console.WriteLine("创建表cardInfo的语句==={0}", createTablesql);
                                        scom.ExecuteNonQuery();

                                        //建临时表cardinf
                                        string createTablesql_trans = " if not exists(select * from sysobjects where id = object_id(N'[dbo].[cardInf]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) create table cardInf(";
                                        for (int i = 0; i < columnTable.Rows.Count; i++)
                                        {
                                            switch (columnTable.Rows[i][1].ToString())
                                            {
                                                case "int":
                                                case "tinyint":
                                                case "smallint":
                                                case "bigint": createTablesql_trans += "[" + columnTable.Rows[i][0] + "]" + " int,";
                                                    break;
                                                case "char":
                                                case "varchar":
                                                case "nchar": createTablesql_trans += "[" + columnTable.Rows[i][0] + "]" + " varchar(" + columnTable.Rows[i][2].ToString() + "),";
                                                    break;
                                                case "datetime": createTablesql_trans += "[" + columnTable.Rows[i][0] + "]" + " datetime,";
                                                    break;
                                                case "float":
                                                case "double":
                                                case "numeric":
                                                case "money": createTablesql_trans += "[" + columnTable.Rows[i][0] + "]" + " float,";
                                                    break;
                                                default: createTablesql_trans += columnTable.Rows[i][0] + " varchar(1000),";
                                                    break;
                                            }
                                            if (i == cardPlace)
                                                createTablesql_trans = createTablesql_trans.Substring(0, createTablesql_trans.Length - 1) + " primary key not null,";
                                        }
                                        createTablesql_trans = createTablesql_trans.TrimEnd(',');
                                        createTablesql_trans += ")";
                                        scom.CommandText = createTablesql_trans;
                                        Console.WriteLine("判断表cardinf是否存在，如果不存在那么就创建==={0}", createTablesql_trans);
                                        scom.ExecuteNonQuery();

                                        #region 新增的 vip信息视图
                                        scom.CommandText = "select COUNT(*) from INFORMATION_SCHEMA.VIEWS where TABLE_NAME=N'vipInfo_view'";
                                        if ((int)scom.ExecuteScalar() == 0)
                                        {
                                            scom.CommandText = "create view [dbo].[vipInfo_view] as select zl.beginDate,zl.card_Discount,zl.card_Type,zl.empox_favor,zl.endDate,zl.is_Upload,zl.points,zl.pwd,zl.remark,zl.sendClient,zl.sendMan,zl.userAddress,zl.userBirthday,zl.userCode,zl.userEmail,zl.userMobile,zl.userName,zl.userPhone,zl.userPost,zl.userSex,zl.userTitle,jf.spareScore,jf.card_id from (select beginDate, card_Discount, card_Type, ud.empox_favor, endDate, ud.is_Upload, points, pwd, remark, sendClient, sendMan, userAddress, userBirthday, userCode, userEmail, userMobile, userName, userPhone, userPost, userSex, userTitle, cardInfo.card_id from cardInfo left join (select distinct card_id,is_Upload,empox_favor from  UD_Fileds) as ud on cardInfo.card_id = ud.card_id) zl left join (select isnull(a.points,0)-isnull(b.score,0) as spareScore,a.card_id from (select cardInfo.card_id,isnull(points,0) as points from cardInfo) a left join (select cm.vipID,sum(ct.score) as score from couponMain as cm inner join couponType as ct on cm.tid=ct.tid where cm.deduction=0 and endDate>=getdate() group by cm.vipID ) b on a.card_id = b.vipID) jf on zl.card_id = jf.card_id ";

                                            scom.ExecuteNonQuery();
                                        }
                                        Console.WriteLine("创建视图vipInfo_view(查询vip基本资料的视图)==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                        #endregion

                                        #region 新增的 查看优惠券的视图
                                        scom.CommandText = "select COUNT(*) from INFORMATION_SCHEMA.VIEWS where TABLE_NAME=N'couponConsumeRecordView'";
                                        if ((int)scom.ExecuteScalar() == 0)
                                        {
                                            scom.CommandText = "CREATE VIEW [dbo].[couponConsumeRecordView] AS SELECT  dbo.cardInfo.card_Id, dbo.cardInfo.userName, dbo.cardInfo.userMobile, dbo.couponMain.cid, dbo.couponMain.getDate, dbo.couponMain.endDate, dbo.couponMain.consumed, dbo.couponMain.applyPlace, ISNULL(dbo.couponMain.consumedPlace, '尚未使用') AS consumedPlace, dbo.couponType.typeDetails, CASE (dbo.couponType.ctype) WHEN 'LOC' THEN '代金券' WHEN 'GOODS' THEN '实物券' END AS type, dbo.couponMain.applyOperator, ISNULL(CAST(dbo.couponMain.consumedDate AS varchar(50)), '尚未使用') AS consumedDate, dbo.couponType.score, CASE (dbo.couponType.ctype) WHEN 'LOC' THEN CAST(dbo.couponType.[money] AS varchar(50))  + '元' ELSE CAST(dbo.couponType.[money] AS varchar(50)) END AS money,dbo.couponType.article FROM dbo.cardInfo INNER JOIN dbo.couponMain ON dbo.cardInfo.card_Id = dbo.couponMain.vipID INNER JOIN dbo.couponType ON dbo.couponMain.tid = dbo.couponType.tid";

                                            scom.ExecuteNonQuery();
                                        }
                                        Console.WriteLine("创建视图couponConsumeRecordView(查询优惠券消费记录的视图)==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                        #endregion

                                        #region 新增的 视图view_yyjf
                                        scom.CommandText = "select COUNT(*) from INFORMATION_SCHEMA.VIEWS where TABLE_NAME=N'view_yyjf'";
                                        if ((int)(scom.ExecuteScalar()) == 0)
                                        {
                                            scom.CommandText = "create view [dbo].[view_yyjf] as select vip.card_id,isnull(vip.points,0) as points,isnull((vip.points-isnull(sdjf.tscore,0)),0) as kyjf,isnull(sdjf.tscore,0) as tscore from cardinfo vip left join (select cm.vipid,sum(ct.score) tscore from couponMain cm left join coupontype ct on cm.tid=ct.tid where ((cm.consumed=0 or cm.deduction=1) and getdate()<cm.enddate)  group by cm.vipid) sdjf on sdjf.vipid = vip.card_id  ";
                                            scom.ExecuteNonQuery();
                                        }
                                        Console.WriteLine("创建视图view_yyjf==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                        #endregion

                                        #region 新增的 存储过程p_getAutoMsgVipData（获取自动短信的存储过程）
                                        scom.CommandText = "select count(*) from sys.procedures where name=N'p_getAutoMsgVipData' and type=N'P'";
                                        if ((int)scom.ExecuteScalar() == 0)
                                        {
                                            StringBuilder sb = new StringBuilder();
                                            sb.AppendFormat("create proc [dbo].[p_getAutoMsgVipData] @msgId int,@someStr varchar(400) as ");
                                            sb.AppendFormat("BEGIN declare @type varchar(10),@condition varchar(400),@mainField varchar(20),");
                                            sb.AppendFormat("@engField varchar(20),@xxDate datetime,@addIndex int,@sqlStr varchar(1000) ");
                                            sb.AppendFormat("select @type=type,@condition=condition,@mainField=mainField ,@addIndex=[dateadd] ");
                                            sb.AppendFormat("from dbo.autoMessage where id=@msgId;if @type='固定日期' begin if Len(@condition)>0 ");
                                            sb.AppendFormat("begin set @sqlStr= 'select '+@someStr+' from cardinfo join UD_Fileds on cardinfo.card_id=UD_Fileds.card_id where '+@condition;");
                                            sb.AppendFormat("end else begin set @sqlStr= 'select '+@someStr+' from cardinfo join UD_Fileds on cardinfo.card_id=UD_Fileds.card_id  ';");
                                            sb.AppendFormat("end end ELSE begin select @xxDate=DATEADD(day,-(@addIndex),getdate());");
                                            sb.AppendFormat("select @engField=enname from datainfo where cnname=@mainField;");
                                            sb.AppendFormat("set @sqlStr = 'select '+@someStr+' from cardinfo join UD_Fileds on cardinfo.card_id=UD_Fileds.card_id where '+@condition ;");
                                            sb.AppendFormat("if Len(@condition)>0 begin set @sqlStr =@sqlStr+' and month('+@engField+')=month('+convert(varchar(10),@xxDate,23)+') and  day('+@engField+')=day('+convert(varchar(10),@xxDate,23)+')';");
                                            sb.AppendFormat("end else begin set @sqlStr =@sqlStr+'  month('+@engField+')=month('+convert(varchar(10),@xxDate,23)+') and  day('+@engField+')=day('+convert(varchar(10),@xxDate,23)+')';");
                                            sb.AppendFormat("end end exec(@sqlStr);END");
                                            scom.CommandText = sb.ToString();
                                            scom.ExecuteNonQuery();
                                        }
                                        Console.WriteLine("如果存储过程p_getAutoMsgVipData(获取自动发送短信)不存在就创建==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                        #endregion

                                        tran.Commit();
                                        Bcon.Close();
                                        //写文本文件，将数据写到指定目录下。
                                        if (!Directory.Exists("C:\\DMService\\" + ClientApp.localBase + "\\ImportFile"))
                                        {
                                            Directory.CreateDirectory("C:\\DMService\\" + ClientApp.localBase + "\\ImportFile");
                                        }

                                        nowTime = TimeFormat.getCurrentTime();
                                        Console.WriteLine("时间字符串(创建txt文件名)==={0}", nowTime);
                                        string fileAdd = "C:\\DMService\\" + ClientApp.localBase + "\\ImportFile\\" + nowTime + ".txt";
                                        if (!File.Exists(fileAdd))
                                        {
                                            using (FileStream fs = File.Create(fileAdd))
                                            {
                                                byte[] brow = Encoding.GetEncoding("gb2312").GetBytes(t[4]);// Encoding.Default.GetBytes(t[3]);
                                                BufferedStream bs = new BufferedStream(fs);
                                                bs.Write(brow, 0, brow.Length);
                                                bs.Flush();
                                                bs.Close();
                                                fs.Close();
                                                Console.WriteLine("将vip基本资料写入到文件{0}.txt中", nowTime);
                                            }
                                        }
                                        else
                                        {
                                            using (FileStream fs = new FileStream("C:\\DMService\\" + ClientApp.localBase + "\\ImportFile\\" + nowTime + ".txt", FileMode.Create))
                                            {
                                                byte[] brow = Encoding.GetEncoding("gb2312").GetBytes(t[4]);// Encoding.Default.GetBytes(t[3]);
                                                BufferedStream bs = new BufferedStream(fs);
                                                bs.Write(brow, 0, brow.Length);
                                                bs.Flush();
                                                bs.Close();
                                                fs.Close();
                                                Console.WriteLine("将vip基本资料写入到文件{0}.txt中", nowTime);
                                            }
                                        }



                                        //SqlCommand inscmd = new SqlCommand();
                                        //将文本文件导入到临时表cardinf
                                        Bcon.Open();
                                        tran = Bcon.BeginTransaction();
                                        scom.Transaction = tran;
                                        scom.CommandText = " delete from cardinf ; BULK INSERT cardinf  " +
                                        "  FROM 'C:\\DMService\\" + ClientApp.localBase + "\\ImportFile\\" + nowTime + ".txt'" +
                                        " WITH " +
                                        " ( " +
                                        " FIELDTERMINATOR = '', " +
                                        " ROWTERMINATOR = '\\n'  " +
                                        ")";
                                        scom.CommandTimeout = 60 * 1000 * 10;
                                        //inscmd.Connection = Bcon;
                                        //inscmd.Transaction = tran;
                                        scom.ExecuteNonQuery();
                                        Console.WriteLine("将文件{0}.txt文件中的vip资料插入到表cardinf(先删除表cardinf中的所有数据)", nowTime);
                                        //inscmd.CommandText = "select * from sysobjects where id = object_id(N'[dbo].[BatchInsert]') and xtype='P'";
                                        scom.CommandText = "select count(*) from sysobjects where id = object_id(N'[dbo].[BatchInsert]') and xtype='P'";
                                        Console.WriteLine("先判断存储BatchInsert(用于将表cardinf中的数据插入到表cardinfo中)是否存在，如果不存在就创建");
                                        int ihas = Convert.ToInt32(scom.ExecuteScalar());
                                        tran.Commit();
                                        Bcon.Close();
                                        // Bcon.Close();
                                        if (ihas <= 0)
                                        {
                                            CreateProc.CreateProcInsert(ClientApp.Basecon);
                                        }
                                        //执行存储过程BatchInsert，将cardinf表的数据写入到CardInfo.
                                        SqlCommand smd = new SqlCommand();

                                        Bcon.Open();
                                        tran = Bcon.BeginTransaction();
                                        scom.Transaction = tran;
                                        smd.CommandText = "BatchInsert";//由于这里是第一次创建表cardinf所以就使用存储过程BatchInsert
                                        smd.CommandType = CommandType.StoredProcedure;
                                        smd.Transaction = tran;
                                        smd.Connection = Bcon;
                                        //Bcon.Open();
                                        Console.WriteLine("执行存储过程BatchInsert(将表cardinf中的数据插入到表cardinfo中)");
                                        smd.ExecuteNonQuery();
                                        //Bcon.Close();
                                        tran.Commit();
                                        Bcon.Close();
                                    }
                                    catch (SqlException e)
                                    {
                                        tran.Rollback();
                                        ErrInfo.WriterErrInfo("WebVipOperator", "TimingVipDownload---------SqlException", e.Message.ToString());
                                        Console.WriteLine("定时下载vip基本资料出现异常==={0}", e.Message);
                                    }
                                    finally
                                    {
                                        if (Bcon.State == ConnectionState.Open)
                                            Bcon.Close();
                                    }

                                }
                                else
                                {
                                    //cardinfo表存在，不用建表，直接将数据写入文本文件
                                    try
                                    {
                                        Console.WriteLine("表cardInfo已经存在==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                        //数据写入文本文件
                                        if (!Directory.Exists("C:\\DMService\\" + ClientApp.localBase + "\\ImportFile"))
                                        {
                                            Directory.CreateDirectory("C:\\DMService\\" + ClientApp.localBase + "\\ImportFile");
                                        }

                                        nowTime = TimeFormat.getCurrentTime();
                                        Console.WriteLine("时间字符串==={0}", nowTime);
                                        string fileAdd = "C:\\DMService\\" + ClientApp.localBase + "\\ImportFile\\" + nowTime + ".txt";
                                        Console.WriteLine("创建文件{0}.txt==={1}", nowTime, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                        if (!File.Exists(fileAdd))
                                        {
                                            using (FileStream fs = File.Create(fileAdd))
                                            {
                                                byte[] brow = Encoding.GetEncoding("gb2312").GetBytes(t[4]);// Encoding.Default.GetBytes(t[3]);
                                                BufferedStream bs = new BufferedStream(fs);
                                                bs.Write(brow, 0, brow.Length);
                                                bs.Flush();
                                                bs.Close();
                                                fs.Close();
                                                Console.WriteLine("将定时下载的vip基本资料写入文件{0}.txt中", nowTime);
                                            }
                                        }
                                        else
                                        {
                                            using (FileStream fs = new FileStream("C:\\DMService\\" + ClientApp.localBase + "\\ImportFile\\" + nowTime + ".txt", FileMode.Create))
                                            {
                                                byte[] brow = Encoding.GetEncoding("gb2312").GetBytes(t[4]);// Encoding.Default.GetBytes(t[3]);
                                                BufferedStream bs = new BufferedStream(fs);
                                                bs.Write(brow, 0, brow.Length);
                                                bs.Flush();
                                                bs.Close();
                                                fs.Close();
                                                Console.WriteLine("将定时下载的vip基本资料写入文件{0}.txt中", nowTime);
                                            }
                                        }


                                        SqlCommand inscmd = new SqlCommand();


                                        //将文本文件导入到临时表cardinf
                                        inscmd.CommandText = "delete from cardinf ; BULK INSERT cardinf  " +
                                        "  FROM 'C:\\DMService\\" + ClientApp.localBase + "\\ImportFile\\" + nowTime + ".txt'" +
                                        " WITH " +
                                        " ( " +
                                        " FIELDTERMINATOR = '', " +
                                        " ROWTERMINATOR = '\\n'  " +
                                        ")";
                                        inscmd.Connection = Bcon;
                                        Bcon.Open();
                                        tran = Bcon.BeginTransaction();
                                        inscmd.Transaction = tran;
                                        inscmd.CommandTimeout = 36000;
                                        inscmd.ExecuteNonQuery();
                                        Console.WriteLine("将文件{0}.txt文件中的定时下载的vip资料导入到表cardinf中(先删除表cardinf中的所有数据)",nowTime);

                                        inscmd.CommandText = "select count(*) from sysobjects where id = object_id(N'[dbo].[BatchUpdataInsert]') and xtype='P'";
                                        Console.WriteLine("判断存储过程BatchUpdataInsert是否存储,不存在就创建");
                                        int ihas = Convert.ToInt32(inscmd.ExecuteScalar());
                                        tran.Commit();
                                        Bcon.Close();
                                        if (ihas <= 0)
                                        {
                                            CreateProc.CreateProcUPdataInsert(ClientApp.Basecon);
                                        }
                                        else
                                        {
                                            Console.WriteLine("存储过程BatchUpdataInsert已经存在");
                                        }
                                        //执行存储过程BatchUpdataInsert，将cardinfo表中已有数据update,cardinfo表中没有的数据insert
                                        //具体实现方法查看存储过程BatchUpdataInsert
                                        SqlCommand smd = new SqlCommand();
                                        smd.CommandText = "BatchUpdataInsert";
                                        smd.CommandType = CommandType.StoredProcedure;
                                        smd.Connection = Bcon;
                                        Bcon.Open();
                                        tran = Bcon.BeginTransaction();
                                        smd.Transaction = tran;
                                        smd.CommandTimeout = 1000 * 60 * 4;
                                        Console.WriteLine("执行存储过程BatchUpdataInsert(将表cardinf中的数据更新插入到表cardinfo中)");
                                        smd.ExecuteNonQuery();
                                        tran.Commit();
                                        Bcon.Close();

                                    }
                                    catch (Exception e)
                                    {
                                        tran.Rollback();
                                        ErrInfo.WriterErrInfo("WebVipOperator", "TimingVipDownload---------SqlException2", e.Message.ToString());
                                        Console.WriteLine("定时下载vip基本资料出现异常==={0}", e.Message);
                                    }
                                    finally
                                    {
                                        if (Bcon.State == ConnectionState.Open)
                                        {
                                            Bcon.Close();
                                        }

                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (Bcon.State == ConnectionState.Open)
                                Bcon.Close();
                            ErrInfo.WriterErrInfo("WebVipOperator", "TimingVipDownload", ex);
                            Console.WriteLine("定时下载vip基本资料出现异常==={0}", ex.Message);
                            return false;
                        }
                    }
                    #endregion
                    else
                    {
                        Console.WriteLine("没有接受定时下载的vip基本资料==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                    }
                }
                else
                {
                    ErrInfo.WriterErrInfo("WebVipOperator", "TimeingVipDownload", "没有VIP定时更新的数据");
                    Console.WriteLine("没有接受定时下载的vip基本资料==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

                }

                return true;
            }

            catch (Exception err)
            {
                ErrInfo.WriterErrInfo("WebVipOperator", "TimingVipDownload", err);
                Console.WriteLine("定时下载的vip基本资料出现异常==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                return false;
            }
            finally
            {
                Bcon.Close();
                if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\ImportFile\\" + nowTime + ".txt"))
                {
                    File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\ImportFile\\" + nowTime + ".txt");
                    Console.WriteLine("定时下载vip基本资料成功，删除文件{0}.txt", nowTime);
                }
            }
        }

        //新增，是为了下载vip卡号信息（这个是自动进行的，就是dm数据处理平台自动进行vip卡号数据的下载）
        /// <summary>
        /// 新增，是为了下载vip卡号信息（这个是自动进行的，就是dm数据处理平台自动进行vip卡号数据的下载）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        #region 这里是处理vip卡信息（就是将可用vip卡号更新到dm网站当中）TimingVipCardDownload
        public bool TimingVipCardDownload(byte[] data)
        {
            int timeUpd = 0;
            int oldtime = 0;
            int newtime = 0;
            string nowTime = "";
            try
            {

                byte[] bytesr = data;
                if (bytesr != null)
                {

                    scom = new SqlCommand();
                    scom.Connection = Bcon;

                    string vipData = Encoding.GetEncoding("GB2312").GetString(bytesr);
                    if (!string.IsNullOrEmpty(vipData))
                    {
                        try
                        {
                            Console.WriteLine("有定时下载vip制卡信息==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            columnTable = new DataTable();
                            columnTable.Columns.Add("colName", typeof(string));
                            columnTable.Columns.Add("colType", typeof(string));
                            columnTable.Columns.Add("colLenth", typeof(string));
                            columnTable.Columns.Add("colCName", typeof(string));
                            string[] t = vipData.Split(splitChar3[0]);


                            int iRow = Convert.ToInt32(t[3]);

                            if (iRow > 0)
                            {

                                string[] columnInfo = t[2].Split(splitChar1[0]);
                                for (int i = 1; i < columnInfo.Length; i++)
                                {
                                    DataRow myrow = columnTable.NewRow();
                                    myrow.ItemArray = columnInfo[i].Split(splitChar2[0]);
                                    columnTable.Rows.Add(myrow);
                                }
                                row = t[4].Split(splitChar1[0]);

                                string datasql = "select count(size) from CardDataInfo";
                                Console.WriteLine("获取表CardDataInfo中是否有数据==={0}", datasql);
                                scom.CommandText = datasql;

                                Bcon.Open();
                                int j = (Int32)(scom.ExecuteScalar());
                                Bcon.Close();
                                try
                                {

                                    if (j == 0)
                                    {
                                        Console.WriteLine("表CardDataInfo中没有数据，进行数据的插入");
                                        //datainfo表没有数据，插入数据
                                        for (int i = 0; i < columnTable.Rows.Count; i++)
                                        {
                                            string insdata = "insert into CardDataInfo values('" + columnTable.Rows[i][0].ToString() +
                                                "','" + columnTable.Rows[i][3].ToString() + "',";
                                            switch (columnTable.Rows[i][1].ToString())
                                            {
                                                case "int":
                                                case "tinyint":
                                                case "smallint":
                                                case "bigint": insdata += "'整数型',4,";
                                                    break;
                                                case "char":
                                                case "varchar":
                                                case "nchar": insdata += "'文本型'," + columnTable.Rows[i][2].ToString() + ",";
                                                    break;
                                                case "datetime": insdata += "'日期型',8,";
                                                    break;
                                                case "float":
                                                case "double":
                                                case "numeric":
                                                case "money": insdata += "'小数型',8,";
                                                    break;
                                                default: insdata += "'文本型',1000,";
                                                    break;
                                            }
                                            // insdata += "null,0,0,0,0,0,0,'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                            insdata += "null)";
                                            Bcon.Close();
                                            tran = Bcon.BeginTransaction();
                                            scom.Transaction = tran;
                                            scom.CommandText = insdata;
                                            scom.Transaction = tran;
                                            scom.ExecuteNonQuery();
                                            tran.Commit();
                                        }

                                    }
                                    else
                                    {
                                        Console.WriteLine("表CardDataInfo中有数据==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                    }

                                }
                                catch (SqlException e)
                                {
                                    tran.Rollback();
                                    ErrInfo.WriterErrInfo("WebVipOperator", "TimingVipCardDownload---------SqlException", e.Message.ToString());
                                    Console.WriteLine("向表CardDataInfo中插入数据出现异常==={0}", e.Message);
                                }
                                finally
                                {
                                    Bcon.Close();
                                }

                                string checkTableSql = " select count(*) from sysobjects where name='VipSet' and type='U'";
                                scom.CommandText = checkTableSql;
                                Console.WriteLine("判断表VipSet是否存在，如果不存在就创建==={0}", checkTableSql);
                                Bcon.Open();
                                int x = Convert.ToInt32(scom.ExecuteScalar());
                                Bcon.Close();
                                if (x == 0)
                                {

                                    try
                                    {
                                        Console.WriteLine("表VipSet不存在,正在创建==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                        int cardPlace = Convert.ToInt32(t[1].ToString());

                                        string createTablesql = " if not exists(select * from sysobjects where id = object_id(N'[dbo].[VipSet]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) create table VipSet(";
                                        for (int i = 0; i < columnTable.Rows.Count; i++)
                                        {
                                            switch (columnTable.Rows[i][1].ToString())
                                            {
                                                case "int":
                                                case "tinyint":
                                                case "smallint":
                                                case "bigint": createTablesql += "[" + columnTable.Rows[i][0] + "]" + " int,";
                                                    break;
                                                case "char":
                                                case "varchar":
                                                case "nchar": createTablesql += "[" + columnTable.Rows[i][0] + "]" + " varchar(" + columnTable.Rows[i][2].ToString() + "),";
                                                    break;
                                                case "datetime": createTablesql += "[" + columnTable.Rows[i][0] + "]" + " datetime,";
                                                    break;
                                                case "float":
                                                case "double":
                                                case "numeric":
                                                case "money": createTablesql += "[" + columnTable.Rows[i][0] + "]" + " float,";
                                                    break;
                                                default: createTablesql += columnTable.Rows[i][0] + " varchar(1000),";
                                                    break;
                                            }
                                            if (i == cardPlace)
                                                createTablesql = createTablesql.Substring(0, createTablesql.Length - 1) + " primary key not null,";
                                        }
                                        createTablesql = createTablesql.Substring(0, createTablesql.Length - 1);
                                        createTablesql += ")";
                                        Bcon.Close();
                                        tran = Bcon.BeginTransaction();
                                        scom.Transaction = tran;
                                        scom.CommandText = createTablesql;
                                        Console.WriteLine("创建表VipSet的语句==={0}", createTablesql);
                                        scom.ExecuteNonQuery();

                                        string createTablesql_trans = " if not exists(select * from sysobjects where id = object_id(N'[dbo].[TempVipSet]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) create table TempVipSet(";
                                        Console.WriteLine("判断表TempVipSet是否存在，如果不存在则创建===={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                        for (int i = 0; i < columnTable.Rows.Count; i++)
                                        {
                                            switch (columnTable.Rows[i][1].ToString())
                                            {
                                                case "int":
                                                case "tinyint":
                                                case "smallint":
                                                case "bigint": createTablesql_trans += "[" + columnTable.Rows[i][0] + "]" + " int,";
                                                    break;
                                                case "char":
                                                case "varchar":
                                                case "nchar": createTablesql_trans += "[" + columnTable.Rows[i][0] + "]" + " varchar(" + columnTable.Rows[i][2].ToString() + "),";
                                                    break;
                                                case "datetime": createTablesql_trans += "[" + columnTable.Rows[i][0] + "]" + " datetime,";
                                                    break;
                                                case "float":
                                                case "double":
                                                case "numeric":
                                                case "money": createTablesql_trans += "[" + columnTable.Rows[i][0] + "]" + " float,";
                                                    break;
                                                default: createTablesql_trans += columnTable.Rows[i][0] + " varchar(1000),";
                                                    break;
                                            }
                                            if (i == cardPlace)
                                                createTablesql_trans = createTablesql_trans.Substring(0, createTablesql_trans.Length - 1) + " primary key not null,";
                                        }
                                        createTablesql_trans = createTablesql_trans.Substring(0, createTablesql_trans.Length - 1);
                                        createTablesql_trans += ")";
                                        scom.CommandText = createTablesql_trans;
                                        Console.WriteLine("创建表TempVipSet的语句==={0}", createTablesql_trans);
                                        scom.ExecuteNonQuery();
                                        tran.Commit();
                                        Bcon.Close();

                                        if (!Directory.Exists("C:\\DMService\\" + ClientApp.localBase + "\\ImportFile"))
                                        {
                                            Directory.CreateDirectory("C:\\DMService\\" + ClientApp.localBase + "\\ImportFile");
                                        }

                                        nowTime = TimeFormat.getCurrentTime();
                                        Console.WriteLine("时间字符串==={0}", nowTime);
                                        string fileAdd = "C:\\DMService\\" + ClientApp.localBase + "\\ImportFile\\" + nowTime + ".txt";
                                        Console.WriteLine("创建文件{0}.txt==={1}", nowTime, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                        if (!File.Exists(fileAdd))
                                        {
                                            using (FileStream fs = File.Create(fileAdd))
                                            {
                                                byte[] brow = Encoding.GetEncoding("gb2312").GetBytes(t[4]);
                                                BufferedStream bs = new BufferedStream(fs);
                                                bs.Write(brow, 0, brow.Length);
                                                bs.Flush();
                                                bs.Close();
                                                fs.Close();
                                                Console.WriteLine("将vip制卡信息写入文件{0}.txt中", nowTime);
                                            }
                                        }
                                        else
                                        {
                                            using (FileStream fs = new FileStream("C:\\DMService\\" + ClientApp.localBase + "\\ImportFile\\" + nowTime + ".txt", FileMode.Create))
                                            {
                                                byte[] brow = Encoding.GetEncoding("gb2312").GetBytes(t[4]);
                                                BufferedStream bs = new BufferedStream(fs);
                                                bs.Write(brow, 0, brow.Length);
                                                bs.Flush();
                                                bs.Close();
                                                fs.Close();
                                                Console.WriteLine("将vip制卡信息写入文件{0}.txt中", nowTime);
                                            }
                                        }
                                        Console.WriteLine("将文件{0}.txt中的vip制卡信息导入到表TempVipSet中(先将表TempVipSet中的数据全部删除掉)",nowTime);
                                        scom.CommandText = " delete from TempVipSet ; BULK INSERT TempVipSet  " +
                                        "  FROM 'C:\\DMService\\" + ClientApp.localBase + "\\ImportFile\\" + nowTime + ".txt'" +
                                        " WITH " +
                                        " ( " +
                                        " FIELDTERMINATOR = '', " +
                                        " ROWTERMINATOR = '\\n'  " +
                                        ")";
                                        scom.CommandTimeout = 60 * 1000 * 10;
                                        Bcon.Open();
                                        tran = Bcon.BeginTransaction();
                                        scom.Transaction = tran;
                                        scom.ExecuteNonQuery();
                                        scom.CommandText = "select count(*) from sysobjects where id = object_id(N'[dbo].[BatchVipCardInsert]') and xtype='P'";
                                        int ihas = Convert.ToInt32(scom.ExecuteScalar());
                                        tran.Commit();
                                        Bcon.Close();

                                        Console.WriteLine("判断存储过程BatchVipCardInsert是否存储，不存在就创建");
                                        if (ihas <= 0)
                                        {
                                            CreateProc.CreateVipCardInsertProc(ClientApp.Basecon);
                                        }
                                        else
                                        {
                                            Console.WriteLine("存储过程BatchVipCardInsert已经存在==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                        }
                                        SqlCommand smd = new SqlCommand();
                                        smd.CommandText = "BatchVipCardInsert";
                                        smd.CommandType = CommandType.StoredProcedure;
                                        Bcon.Open();
                                        tran = Bcon.BeginTransaction();
                                        Console.WriteLine("调用存储过程BatchVipCardInsert==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                        smd.Transaction = tran;
                                        smd.Connection = Bcon;
                                        smd.ExecuteNonQuery();
                                        tran.Commit();

                                    }
                                    catch (SqlException e)
                                    {
                                        tran.Rollback();
                                        ErrInfo.WriterErrInfo("WebVipOperator", "TimingVipCardDownload---------SqlException", e.Message.ToString());
                                        Console.WriteLine("定时下载vip制卡信息出现异常==={0}", e.Message);
                                    }
                                    finally
                                    {

                                        if (Bcon.State == ConnectionState.Open)
                                        {
                                            Bcon.Close();
                                        }
                                    }

                                }
                                else
                                {
                                    Console.WriteLine("表VipSet已经存在==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

                                    try
                                    {

                                        if (!Directory.Exists("C:\\DMService\\" + ClientApp.localBase + "\\ImportFile"))
                                        {
                                            Directory.CreateDirectory("C:\\DMService\\" + ClientApp.localBase + "\\ImportFile");
                                        }

                                        nowTime = TimeFormat.getCurrentTime();
                                        Console.WriteLine("时间字符串==={0}", nowTime);
                                        string fileAdd = "C:\\DMService\\" + ClientApp.localBase + "\\ImportFile\\" + nowTime + ".txt";
                                        Console.WriteLine("创建文件{0}.txt==={1}", fileAdd, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                        if (!File.Exists(fileAdd))
                                        {
                                            using (FileStream fs = File.Create(fileAdd))
                                            {
                                                byte[] brow = Encoding.GetEncoding("gb2312").GetBytes(t[4]);
                                                BufferedStream bs = new BufferedStream(fs);
                                                bs.Write(brow, 0, brow.Length);
                                                bs.Flush();
                                                bs.Close();
                                                fs.Close();
                                                Console.WriteLine("将vip制卡信息写入文件{0}.txt中", nowTime);
                                            }
                                        }
                                        else
                                        {
                                            using (FileStream fs = new FileStream("C:\\DMService\\" + ClientApp.localBase + "\\ImportFile\\" + nowTime + ".txt", FileMode.Create))
                                            {
                                                byte[] brow = Encoding.GetEncoding("gb2312").GetBytes(t[4]);
                                                BufferedStream bs = new BufferedStream(fs);
                                                bs.Write(brow, 0, brow.Length);
                                                bs.Flush();
                                                bs.Close();
                                                fs.Close();
                                                Console.WriteLine("将vip制卡信息写入文件{0}.txt中", nowTime);
                                            }
                                        }
                                        Console.WriteLine("将文件{0}.txt中的vip制卡信息导入到表TempVipSet中(先将表TempVipSet中的所有数据删除掉)",nowTime);
                                        SqlCommand inscmd = new SqlCommand();
                                        inscmd.CommandText = "delete from TempVipSet ; BULK INSERT TempVipSet  " +
                                        "  FROM 'C:\\DMService\\" + ClientApp.localBase + "\\ImportFile\\" + nowTime + ".txt'" +
                                        " WITH " +
                                        " ( " +
                                        " FIELDTERMINATOR = '', " +
                                        " ROWTERMINATOR = '\\n'  " +
                                        ")";
                                        inscmd.Connection = Bcon;
                                        inscmd.CommandTimeout = 36000;
                                        Bcon.Open();
                                        tran = Bcon.BeginTransaction();
                                        inscmd.Transaction = tran;
                                        inscmd.ExecuteNonQuery();
                                        inscmd.CommandText = "select count(*) from sysobjects where id = object_id(N'[dbo].[BatchVipCardUpdate]') and xtype='P'";
                                        int ihas = Convert.ToInt32(inscmd.ExecuteScalar());
                                        tran.Commit();
                                        Bcon.Close();
                                        Console.WriteLine("判断存储过程BatchVipCardUpdate是否存在，如果不存在就创建");
                                        if (ihas <= 0)
                                        {
                                            CreateProc.CreateVipCardUpdateInsertProc(ClientApp.Basecon);
                                        }
                                        else
                                        {
                                            Console.WriteLine("已经存在存储过程BatchVipCardUpdate==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                        }
                                        SqlCommand smd = new SqlCommand();
                                        Console.WriteLine("调用存储过程BatchVipCardUpdate==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                        smd.CommandText = "BatchVipCardUpdate";
                                        smd.CommandType = CommandType.StoredProcedure;
                                        smd.Connection = Bcon;
                                        Bcon.Open();
                                        tran = Bcon.BeginTransaction();
                                        smd.Transaction = tran;
                                        smd.CommandTimeout = 1000 * 60 * 4;
                                        smd.ExecuteNonQuery();
                                        tran.Commit();

                                    }
                                    catch (Exception e)
                                    {
                                        tran.Rollback();
                                        ErrInfo.WriterErrInfo("WebVipOperator", "TimingVipCardDownload---------SqlException2", e.Message.ToString());
                                        Console.WriteLine("定时下载vip制卡信息出现异常==={0}", e.Message);
                                    }
                                    finally
                                    {
                                        if (Bcon.State == ConnectionState.Open)
                                        {
                                            Bcon.Close();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (Bcon.State == ConnectionState.Open)
                            {
                                Bcon.Close();
                            }
                            ErrInfo.WriterErrInfo("WebVipOperator", "TimingVipCardDownload", ex);
                            Console.WriteLine("定时下载vip制卡信息出新异常==={0}", ex.Message);
                            return false;
                        }
                    }

                }
                else
                {
                    ErrInfo.WriterErrInfo("WebVipOperator", "TimingVipCardDownload", "没有VIP制卡信息定时更新的数据");

                }

                return true;
            }

            catch (Exception err)
            {
                ErrInfo.WriterErrInfo("WebVipOperator", "TimingVipCardDownload", err);
                Console.WriteLine("定时下载vip制卡信息出新异常==={0}", err.Message);
                return false;
            }
            finally
            {
                if (Bcon.State == ConnectionState.Open)
                {
                    Bcon.Close();
                }
                if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\ImportFile\\" + nowTime + ".txt"))
                {
                    File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\ImportFile\\" + nowTime + ".txt");

                }
            }
        }
        #endregion
        //public string GetCardid()
        //{
        //    SqlConnection con = new SqlConnection(ClientApp.Basecon);
        //    try
        //    {
        //        //string str = "select ENname From datainfo where CNname='卡号' and DBname='" + ClientApp.localBase + "'";
        //        string str = "select ENname From datainfo where CNname='卡号'";
        //        SqlCommand smd1 = new SqlCommand(str);
        //        smd1.Connection = con;
        //        con.Open();
        //        string j = smd1.ExecuteScalar().ToString();
        //        con.Close();
        //        if (j.Trim().Length > 0)
        //        {
        //            return j.Trim();
        //        }
        //        else
        //        {
        //            return "";
        //        }
        //    }

        //    catch (SqlException ex)
        //    {
        //        //Console.Write("VIP登陆验证出错" + ex.Message.ToString());
        //        ErrInfo.WriterErrInfo("WebVipOperator", "GetCardid---SqlException", ex);
        //        con.Close();
        //        return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        //Console.Write("VIP登陆验证出错" + ex.Message.ToString());
        //        ErrInfo.WriterErrInfo("WebVipOperator", "GetCardid---Exception", ex);
        //        con.Close();
        //        return "";
        //    }

        //    finally
        //    {
        //        con.Close();

        //    }
        //}

        //private string GetENnameByChname(string cardid)
        //{
        //    try
        //    {
        //        string strReturn = "";
        //        string[] StrEn = null;
        //        string[] StrCh = null;
        //        string str = "select ENname,CNname from datainfo where (CNname='姓名'or CNname='卡类型' or CNname='折扣') ";
        //        using (SqlConnection conn = new SqlConnection(ClientApp.Basecon))
        //        {
        //            SqlCommand command = new SqlCommand(str, conn);
        //            SqlDataAdapter adp = new SqlDataAdapter(command);
        //            DataTable mt = new DataTable();
        //            conn.Open();
        //            adp.Fill(mt);
        //            conn.Close();
        //            if (mt.Rows.Count > 0)
        //            {
        //                StrEn = new string[mt.Rows.Count];
        //                StrCh = new string[mt.Rows.Count];
        //                for (int i = 0; i < mt.Rows.Count; i++)
        //                {
        //                    StrEn[i] = mt.Rows[i][0].ToString();
        //                    StrCh[i] = mt.Rows[i][1].ToString();
        //                }


        //                str = " select ";
        //                string temp = "";
        //                for (int k = 0; k < StrEn.Length; k++)
        //                {
        //                    temp = temp + StrEn[k] + ",";
        //                }
        //                str = str + temp.Trim().Substring(0, temp.LastIndexOf(",")) + " from cardinfo where card_id='" + cardid + "'";

        //                //command = new SqlCommand(str, conn);
        //                adp = new SqlDataAdapter(str, Bcon);
        //                mt = new DataTable();
        //                Bcon.Open();
        //                adp.Fill(mt);
        //                Bcon.Close();
        //                for (int l = 0; l < StrCh.Length; l++)
        //                {
        //                    strReturn = strReturn + StrCh[l] + (char)21;
        //                }
        //                strReturn = strReturn.Substring(0, strReturn.LastIndexOf((char)21)) + (char)22;
        //                if (mt.Rows.Count > 0)
        //                {
        //                    for (int Rcou = 0; Rcou < mt.Rows.Count; Rcou++)
        //                    {
        //                        for (int Ccou = 0; Ccou < mt.Columns.Count; Ccou++)
        //                        {
        //                            strReturn = strReturn + mt.Rows[Rcou][Ccou] + (char)21;
        //                        }
        //                        strReturn = strReturn.Substring(0, strReturn.LastIndexOf((char)21));
        //                    }
        //                }
        //            }
        //            return strReturn;
        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        ErrInfo.WriterErrInfo("WebVipOperator", "GetENnameByChname---Exception", err);
        //        return "GetChname Error";

        //    }

        //}


        //private string GetEnameByCname(string cardid)
        //{
        //    try
        //    {
        //        string strReturn = "";
        //        string Strtemp = "";

        //        string str = "select ENname,CNname from datainfo where CNname='姓名'or CNname='卡类型' or CNname='折扣'";
        //        using (SqlConnection conn = new SqlConnection(ClientApp.Basecon))
        //        {

        //            SqlDataAdapter adp = new SqlDataAdapter(str, conn);
        //            DataTable mt = new DataTable();
        //            conn.Open();
        //            adp.Fill(mt);
        //            conn.Close();
        //            if (mt.Rows.Count > 0)
        //            {
        //                for (int i = 0; i < mt.Rows.Count; i++)
        //                {
        //                    Strtemp = mt.Rows[i][0].ToString() + "as" + mt.Rows[i][1].ToString() + ",";
        //                }
        //                Strtemp = Strtemp.Trim().Substring(0, Strtemp.LastIndexOf(","));
        //            }
        //            str = " select ";

        //            str = str + Strtemp + " from cardinfo where card_id='" + cardid + "'";
        //            adp = new SqlDataAdapter(str, Bcon);
        //            mt = new DataTable();
        //            Bcon.Open();
        //            adp.Fill(mt);
        //            Bcon.Close();
        //            for (int l = 0; l < mt.Columns.Count; l++)
        //            {
        //                strReturn = strReturn + mt.Columns[l].ColumnName + (char)21;
        //            }
        //            strReturn = strReturn.Substring(0, strReturn.LastIndexOf((char)21)) + (char)22;
        //            if (mt.Rows.Count > 0)
        //            {
        //                for (int Rcou = 0; Rcou < mt.Rows.Count; Rcou++)
        //                {
        //                    for (int Ccou = 0; Ccou < mt.Columns.Count; Ccou++)
        //                    {
        //                        strReturn = strReturn + mt.Rows[Rcou][Ccou] + (char)21;
        //                    }
        //                    strReturn = strReturn.Substring(0, strReturn.LastIndexOf((char)21));
        //                }
        //            }
        //            return strReturn;
        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        ErrInfo.WriterErrInfo("WebVipOperator", "GetCardid---Exception", err);
        //        return "";

        //    }

        //}


        public string GetVipTempModeify(string sessionid)
        {
            string ss = "";
            string strcmd = "";
            SqlDataAdapter adp;
            ss = "select * from TempVipModify where sessionid='" + sessionid + "' and companyid='" + ClientApp.id + "'";

            //scom = new SqlCommand(ss, Mcon);
            scom = new SqlCommand(ss, Bcon);
            DataTable mt = new DataTable();
            adp = new SqlDataAdapter(scom);
            //Mcon.Open();
            Bcon.Open();
            adp.Fill(mt);
            Bcon.Close();
            if (mt.Rows.Count > 0)
            {

                strcmd = Encoding.Default.GetString((byte[])mt.Rows[0]["StrCommand"]);

            }

            return strcmd;

        }
        /// <summary>
        /// 定时下载营业员基本资料
        /// </summary>
        /// <param name="bytesr"></param>
        /// <returns></returns>
        public bool TimingSalesDownload(byte[] bytesr)
        {
            bool flag = false;
            SqlConnection scon = new SqlConnection(ClientApp.Basecon);
            scom = new SqlCommand();
            scom.Connection = scon;
            scon.Open();

            tran = scon.BeginTransaction();
            if (bytesr != null)
            {
                string salerData = Encoding.GetEncoding("GB2312").GetString(bytesr);
                //0salerIdvarchar20salerNamevarchar50salerStorevarchar201001测试营业员C001
                #region 有数据
                if (salerData != null && salerData != "")
                {
                    try
                    {
                        Console.WriteLine("有定时下载的营业员基本资料的数据==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        columnTable = new DataTable();
                        columnTable.Columns.Add("colName", typeof(string));
                        columnTable.Columns.Add("colType", typeof(string));
                        columnTable.Columns.Add("colLenth", typeof(string));
                        string[] t = salerData.Split(splitChar3[0]);
                        //t[0]="";
                        //t[1]="0";
                        //t[2]="salerIdvarchar20salerNamevarchar50salerStorevarchar20"
                        //t[3]="1"
                        //t[4]="001测试营业员C001";
                        string[] columnInfo = t[2].Split(splitChar1[0]);
                        for (int i = 1; i < columnInfo.Length; i++)
                        {
                            DataRow myrow = columnTable.NewRow();
                            myrow.ItemArray = columnInfo[i].Split(splitChar2[0]);
                            columnTable.Rows.Add(myrow);
                        }
                        row = t[4].Split(splitChar1[0]);
                        columnCount = row[1].ToString().Split(splitChar2[0]).Length;
                        int cardPlace = Convert.ToInt32(t[1].ToString());
                        //string dropTablesql = "use " + ClientApp.localBase + " if exists(select * from sysobjects where id = object_id(N'[dbo].[salerInfo]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table salerInfo";
                        //删除　salerInfo
                        string dropTablesql = " if exists(select * from sysobjects where id = object_id(N'[dbo].[salerInfo]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table salerInfo";
                        scom.CommandText = dropTablesql;
                        Console.WriteLine("判断表salerInfo是否存在，如果存在就删除==={0}", dropTablesql);
                        scom.Transaction = tran;
                        scom.ExecuteNonQuery();
                        Thread.Sleep(3000);
                        //string createTablesql = "use " + ClientApp.localBase + " if not exists(select * from sysobjects where id = object_id(N'[dbo].[salerInfo]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) create table salerInfo(";
                        //新建salerInfo
                        string createTablesql = " if not exists(select * from sysobjects where id = object_id(N'[dbo].[salerInfo]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) create table salerInfo(";
                        for (int i = 0; i < columnTable.Rows.Count; i++)
                        {
                            switch (columnTable.Rows[i][1].ToString())
                            {
                                case "int":
                                case "tinyint":
                                case "smallint":
                                case "bigint": createTablesql += "[" + columnTable.Rows[i][0] + "]" + " int,";
                                    break;
                                case "char":
                                case "varchar":
                                case "nchar": createTablesql += "[" + columnTable.Rows[i][0] + "]" + " varchar(" + columnTable.Rows[i][2].ToString() + "),";
                                    break;
                                case "datetime": createTablesql += "[" + columnTable.Rows[i][0] + "]" + " datetime,";
                                    break;
                                case "float":
                                case "double":
                                case "numeric":
                                case "money": createTablesql += "[" + columnTable.Rows[i][0] + "]" + " float,";
                                    break;
                                default: createTablesql += columnTable.Rows[i][0] + " varchar(1000),";
                                    break;
                            }
                        }
                        createTablesql = createTablesql.Substring(0, createTablesql.Length - 1) + ")";
                        scom.CommandText = createTablesql;
                        Console.WriteLine("创建表salerInfo的语句==={0}", createTablesql);
                        scom.Transaction = tran;
                        scom.ExecuteNonQuery();
                        string insertDataSql = "";
                        //salerInfo表插入数据
                        Console.WriteLine("向表salerInfo中插入接受到的营业员基本数据");
                        for (int i = 0; i < row.Length - 1; i++)
                        {
                            //insertDataSql = "use " + ClientApp.localBase + " insert into salerInfo values(";
                            insertDataSql = " insert into salerInfo values(";
                            column = row[i].ToString().Split(splitChar2[0]);
                            for (int m = 0; m < column.Length - 1; m++)
                            {
                                switch (columnTable.Rows[m][1].ToString())
                                {
                                    case "int":
                                    case "tinyint":
                                    case "smallint":
                                    case "bigint":
                                    case "bool":
                                    case "float":
                                    case "double":
                                    case "numeric":
                                    case "money": insertDataSql += column[m].ToString() + ",";
                                        break;
                                    case "datetime": insertDataSql += column[m].ToString().ToLower() == "null" ? ("'',") : ("'" + Convert.ToDateTime(column[m].ToString()).ToString("yyyy-MM-dd") + "',");
                                        break;
                                    default: insertDataSql += column[m].ToString().ToLower() == "null" ? ("'',") : "'" + column[m].ToString() + "',";
                                        break;
                                }
                            }
                            insertDataSql = insertDataSql.Substring(0, insertDataSql.Length - 1) + ")";
                            scom.CommandText = insertDataSql;
                            scom.Transaction = tran;
                            scom.ExecuteNonQuery();
                        }
                        tran.Commit();
                        flag = true;
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        flag = false;
                        ErrInfo.WriterErrInfo("WebVipOperator", "TimingVipDownload", ex.Message);
                        Console.WriteLine("插入营业员基本资料出现异常==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                    }
                    finally
                    {
                        scon.Close();
                    }

                }
                #endregion
                else
                {
                    Console.WriteLine("没有定时下载的营业员数据==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                }
            }
            else
            {
                Console.WriteLine("没有定时下载的营业员数据==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            }
            return flag;
        }

        /// <summary>
        /// 定时下载门店基本资料
        /// </summary>
        /// <param name="bytesr"></param>
        /// <returns></returns>
        public bool TimingShopsDownLoad(byte[] bytesr)
        {
            SqlConnection scon;
            scon = new SqlConnection(ClientApp.Basecon);
            scon.Open();
            scom = new SqlCommand();
            scom.Connection = scon;
            tran = scon.BeginTransaction();
            if (bytesr != null)
            {
                string shopData = Encoding.GetEncoding("GB2312").GetString(bytesr);
                #region
                if (shopData != null && shopData != "")
                {
                    try
                    {
                        Console.WriteLine("有定时下载的门店数据==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        //将shopinfo表结构读入columnTable
                        columnTable = new DataTable();
                        columnTable.Columns.Add("colName", typeof(string));
                        columnTable.Columns.Add("colType", typeof(string));
                        columnTable.Columns.Add("colLenth", typeof(string));
                        string[] t = shopData.Split(splitChar3[0]);
                        string[] columnInfo = t[2].Split(splitChar1[0]);
                        for (int i = 1; i < columnInfo.Length; i++)
                        {
                            DataRow myrow = columnTable.NewRow();
                            myrow.ItemArray = columnInfo[i].Split(splitChar2[0]);
                            columnTable.Rows.Add(myrow);
                        }
                        row = t[4].Split(splitChar1[0]);
                        columnCount = row[1].ToString().Split(splitChar2[0]).Length;
                        int cardPlace = Convert.ToInt32(t[1].ToString());
                        //删除shopinfo表
                        //string dropTablesql = "use " + ClientApp.localBase + " if exists(select * from sysobjects where id = object_id(N'[dbo].[shopInfo]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table shopInfo";
                        string dropTablesql = " if exists(select * from sysobjects where id = object_id(N'[dbo].[shopInfo]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table shopInfo";
                        scom.CommandText = dropTablesql;
                        Console.WriteLine("判断表shopInfo是否存在，如果存在就删除==={0}", dropTablesql);
                        scom.Transaction = tran;
                        scom.ExecuteNonQuery();
                        Thread.Sleep(3000);
                        //建shopinfo表
                        //string createTablesql = "use " + ClientApp.localBase + " if not exists(select * from sysobjects where id = object_id(N'[dbo].[shopInfo]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) create table shopInfo(";
                        string createTablesql = " if not exists(select * from sysobjects where id = object_id(N'[dbo].[shopInfo]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) create table shopInfo(";
                        for (int i = 0; i < columnTable.Rows.Count; i++)
                        {
                            switch (columnTable.Rows[i][1].ToString())
                            {
                                case "int":
                                case "tinyint":
                                case "smallint":
                                case "bigint": createTablesql += "[" + columnTable.Rows[i][0] + "]" + " int,";
                                    break;
                                case "char":
                                case "varchar":
                                case "nchar": createTablesql += "[" + columnTable.Rows[i][0] + "]" + " varchar(" + columnTable.Rows[i][2].ToString() + "),";
                                    break;
                                case "datetime": createTablesql += "[" + columnTable.Rows[i][0] + "]" + " datetime,";
                                    break;
                                case "float":
                                case "double":
                                case "numeric":
                                case "money": createTablesql += "[" + columnTable.Rows[i][0] + "]" + " float,";
                                    break;
                                default: createTablesql += columnTable.Rows[i][0] + " varchar(1000),";
                                    break;
                            }
                        }
                        createTablesql = createTablesql.Substring(0, createTablesql.Length - 1) + ")";
                        scom.CommandText = createTablesql;
                        Console.WriteLine("创建表shopInfo的语句==={0}", createTablesql);
                        scom.Transaction = tran;
                        scom.ExecuteNonQuery();
                        string insertDataSql = "";
                        //数据插入shopinfo表
                        Console.WriteLine("向表shopInfo中插入定时接收到的门店资料");
                        for (int i = 0; i < row.Length - 1; i++)
                        {
                            //insertDataSql = "use " + ClientApp.localBase + " insert into shopInfo values(";
                            insertDataSql = " insert into shopInfo values(";
                            column = row[i].ToString().Split(splitChar2[0]);
                            for (int m = 0; m < column.Length - 1; m++)
                            {
                                switch (columnTable.Rows[m][1].ToString())
                                {
                                    case "int":
                                    case "tinyint":
                                    case "smallint":
                                    case "bigint":
                                    case "bool":
                                    case "float":
                                    case "double":
                                    case "numeric":
                                    case "money": insertDataSql += column[m].ToString() + ",";
                                        break;
                                    case "datetime": insertDataSql += column[m].ToString().ToLower() == "null" ? ("'',") : ("'" + Convert.ToDateTime(column[m].ToString()).ToString("yyyy-MM-dd") + "',");
                                        break;
                                    default: insertDataSql += column[m].ToString().ToLower() == "null" ? ("'',") : "'" + column[m].ToString() + "',";
                                        break;
                                }
                            }
                            insertDataSql = insertDataSql.Substring(0, insertDataSql.Length - 1) + ")";
                            scom.CommandText = insertDataSql;
                            scom.Transaction = tran;
                            scom.ExecuteNonQuery();
                        }
                        //string operateSql = "use " + ClientApp.localBase + " insert into operateRecord values('" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','门店定时数据下载. 状态:成功.')";
                        //成功，写操作日志
                        string operateSql = " insert into operateRecord values('" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','门店定时数据下载. 状态:成功.')";
                        scom.CommandText = operateSql;
                        Console.WriteLine("写入操作日志==={0}", operateSql);
                        scom.Transaction = tran;
                        scom.ExecuteNonQuery();
                        tran.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("插入定时下载的门店基本资料出现异常==={0}", ex.Message);
                        tran.Rollback();
                        //string operateSql = "use " + ClientApp.localBase + " insert into operateRecord values('" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','门店定时数据下载. 状态:失败[" + ex.Message.Replace("\"", "").Replace("'", "").Replace("\r\n", "") + "].')";
                        //失败，写操作日志
                        string operateSql = " insert into operateRecord values('" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','门店定时数据下载. 状态:失败[" + ex.Message.Replace("\"", "").Replace("'", "").Replace("\r\n", "") + "].')";
                        scom.CommandText = operateSql;
                        scom.ExecuteNonQuery();
                        return false;
                    }
                    finally
                    {
                        scon.Close();
                    }
                }
                #endregion
                else
                {
                    Console.WriteLine("没有接受定时下载的门店数据==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                    return false;
                }

            }
            else
            {
                Console.WriteLine("没有接受定时下载的门店数据==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                return false;
            }


        }

        public void TimingSendCoupon()
        {
            try
            {
                DataTable dt;
                AgentOperator ao = new AgentOperator();
                string str = "select top 30 use_nm,tel_no,card_id,Title,pwd from cardinfo where empox_favor=0 ";
                SqlCommand smd = new SqlCommand(str, Bcon);
                SqlDataAdapter adp = new SqlDataAdapter(smd);
                dt = new DataTable();
                Bcon.Open();
                adp.Fill(dt);
                //Bcon.Close();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string id = ao.GetCouponId(dt.Rows[i]["use_nm"].ToString(), dt.Rows[i]["card_id"].ToString(), "0.0.0.0.");
                    if (id.IndexOf("error") < 0)
                    {
                        ao.SendSMS(dt.Rows[i]["use_nm"].ToString().Trim(), dt.Rows[i]["card_id"].ToString().Trim(), dt.Rows[i]["tel_no"].ToString().Trim(), id, dt.Rows[i]["Title"].ToString().Trim(), dt.Rows[i]["pwd"].ToString().Trim());
                        str = "update cardinfo set empox_favor=1 where card_id='" + dt.Rows[i]["card_id"].ToString() + "'";
                        smd = new SqlCommand(str, Bcon);
                        //  Bcon.Open();
                        smd.ExecuteNonQuery();
                        //Bcon.Close();
                    }
                }
                Bcon.Close();
            }
            catch (Exception e)
            {
                ErrInfo.WriterErrInfo("WebVipOperator", "TimingSendCoupon", e.Message.ToString());
            }
        }

        public bool TimeingSaleInfoDown(byte[] bytesr)
        {

            int timeUpd = 0;
            int oldtime = 0;
            int newtime = 0;
            string nowTime = "";
            try
            {
                if (bytesr != null)
                {

                    scom = new SqlCommand();
                    //scom.Connection = Mcon;
                    string vipData = Encoding.GetEncoding("GB2312").GetString(bytesr);
                    #region 有数据
                    if (vipData != null && vipData != "")
                    {
                        try
                        {
                            Console.WriteLine("有销售数据==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            columnTable = new DataTable();

                            columnTable.Columns.Add("colName", typeof(string));
                            columnTable.Columns.Add("colType", typeof(string));
                            columnTable.Columns.Add("colLenth", typeof(string));

                            string[] t = vipData.Split(splitChar3[0]);

                            int iRow = Convert.ToInt32(t[3]);// ByteConvert.byteArrayToInt(Encoding.Default.GetBytes(t[3]));

                            //判断表的行数是否大于0
                            if (iRow > 0)
                            {
                                //将表的结构写入到columnTable中
                                string[] columnInfo = t[2].Split(splitChar1[0]);

                                for (int i = 1; i < columnInfo.Length; i++)
                                {
                                    DataRow myrow = columnTable.NewRow();
                                    myrow.ItemArray = columnInfo[i].Split(splitChar2[0]);
                                    columnTable.Rows.Add(myrow);
                                }

                                row = t[4].Split(splitChar1[0]);
                                try
                                {
                                    //建表lants_sale_mst
                                    int cardPlace = Convert.ToInt32(t[1].ToString());
                                    Console.WriteLine("判断表lants_sale_mst(销售主表)是否存在，如果不存在就创建");
                                    string createTablesql = " if exists(select * from sysobjects where id = object_id(N'[dbo].[lants_sale_mst]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table lants_sale_mst;create table lants_sale_mst(";
                                    for (int i = 0; i < columnTable.Rows.Count; i++)
                                    {
                                        switch (columnTable.Rows[i][1].ToString())
                                        {
                                            case "int":
                                            case "tinyint":
                                            case "smallint":
                                            case "bigint": createTablesql += "[" + columnTable.Rows[i][0] + "]" + " int,";
                                                break;
                                            case "char":
                                            case "varchar":
                                            case "nchar": createTablesql += "[" + columnTable.Rows[i][0] + "]" + " varchar(" + columnTable.Rows[i][2].ToString() + "),";
                                                break;
                                            case "datetime": createTablesql += "[" + columnTable.Rows[i][0] + "]" + " datetime,";
                                                break;
                                            case "float":
                                            case "double":
                                            case "numeric": //createTablesql += "[" + columnTable.Rows[i][0] + "]" + " numeric(6,3),"; //2009.11.25 edit
                                            case "money": createTablesql += "[" + columnTable.Rows[i][0] + "]" + " float,";
                                                break;
                                            default: createTablesql += columnTable.Rows[i][0] + " varchar(1000),";
                                                break;
                                        }
                                        if (i == cardPlace)
                                            createTablesql = createTablesql.Substring(0, createTablesql.Length - 1) + " primary key not null,";
                                    }
                                    //createTablesql += "is_Upload int default 0 not null,empox_favor int default 1 not null,Iden_ID int identity(1,1) not null)";
                                    createTablesql = createTablesql.Substring(0, createTablesql.Length - 1);
                                    createTablesql += ")";
                                    scom.CommandText = createTablesql;
                                    Console.WriteLine("创建表lants_sale_mst语句==={0}", createTablesql);
                                    scom.Connection = Bcon;
                                    Bcon.Open();
                                    scom.ExecuteNonQuery();
                                    Bcon.Close();
                                    //写文本文件
                                    if (!Directory.Exists("C:\\DMService\\" + ClientApp.localBase + "\\ImportFile"))
                                    {
                                        Directory.CreateDirectory("C:\\DMService\\" + ClientApp.localBase + "\\ImportFile");
                                    }

                                    nowTime = TimeFormat.getCurrentTime();
                                    Console.WriteLine("时间字符串==={0}", nowTime);
                                    string fileAdd = "C:\\DMService\\" + ClientApp.localBase + "\\ImportFile\\" + nowTime + ".txt";
                                    Console.WriteLine("创建文件{0}.txt==={1}", nowTime, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

                                    #region 修改之后
                                    using (FileStream fs = new FileStream(fileAdd, FileMode.Create))
                                    {
                                        byte[] brow = Encoding.GetEncoding("gb2312").GetBytes(t[4]);// Encoding.Default.GetBytes(t[3]);
                                        BufferedStream bs = new BufferedStream(fs);
                                        bs.Write(brow, 0, brow.Length);
                                        bs.Flush();
                                        bs.Close();
                                        fs.Close();
                                        Console.WriteLine("将销售数据写入文件{0}.txt中去", nowTime);
                                    }
                                    #endregion

                                    SqlCommand inscmd = new SqlCommand();
                                    Console.WriteLine("将文件{0}.txt中销售数据导入到表lants_sale_mst中(先删除表lants_sale_mst中的所有数据)",nowTime);
                                    //文本数据写入到数据表中。
                                    inscmd.CommandText = " delete from lants_sale_mst ; BULK INSERT lants_sale_mst  " +
                                    "  FROM 'C:\\DMService\\" + ClientApp.localBase + "\\ImportFile\\" + nowTime + ".txt'" +
                                    " WITH " +
                                    " ( " +
                                    " FIELDTERMINATOR = '', " +
                                    " ROWTERMINATOR = '\\n'  " +
                                    ")";
                                    inscmd.Connection = Bcon;
                                    //inscmd.Transaction = tran;
                                    Bcon.Open();
                                    inscmd.ExecuteNonQuery();
                                    Bcon.Close();

                                }
                                catch (SqlException e)
                                {
                                    Console.WriteLine("接受销售数据出现异常==={0}", e.Message);
                                    ErrInfo.WriterErrInfo("WebVipOperator", "TimeingsaleInfo---------SalesMain", e.Message.ToString());
                                }
                                finally
                                {
                                    Bcon.Close();
                                }

                            }

                            //iRow = ByteConvert.byteArrayToInt(Encoding.Default.GetBytes(t[6]));
                            iRow = Convert.ToInt32(t[6]);
                            if (iRow > 0)
                            {
                                columnTable = new DataTable();
                                columnTable.Columns.Add("colName", typeof(string));
                                columnTable.Columns.Add("colType", typeof(string));
                                columnTable.Columns.Add("colLenth", typeof(string));

                                string[] columnInfo = t[5].Split(splitChar1[0]);

                                for (int i = 1; i < columnInfo.Length; i++)
                                {
                                    //将表结构写入columnTable
                                    DataRow myrow = columnTable.NewRow();
                                    myrow.ItemArray = columnInfo[i].Split(splitChar2[0]);
                                    columnTable.Rows.Add(myrow);
                                }

                                row = t[7].Split(splitChar1[0]);
                                try
                                {
                                    //int cardPlace = Convert.ToInt32(t[1].ToString());
                                    //建表lants_sale_dtl
                                    Console.WriteLine("判断表lants_sale_dtl是否存在，如果不存在就创建");
                                    string createTablesql = " if exists(select * from sysobjects where id = object_id(N'[dbo].[lants_sale_dtl]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [lants_sale_dtl]; create table [lants_sale_dtl](";
                                    for (int i = 0; i < columnTable.Rows.Count; i++)
                                    {
                                        switch (columnTable.Rows[i][1].ToString())
                                        {
                                            case "int":
                                            case "tinyint":
                                            case "smallint":
                                            case "bigint": createTablesql += "[" + columnTable.Rows[i][0] + "]" + " int,";
                                                break;
                                            case "char":
                                            case "varchar":
                                            case "nchar": createTablesql += "[" + columnTable.Rows[i][0] + "]" + " varchar(" + columnTable.Rows[i][2].ToString() + "),";
                                                break;
                                            case "datetime": createTablesql += "[" + columnTable.Rows[i][0] + "]" + " datetime,";
                                                break;
                                            case "float":
                                            case "double":
                                            case "numeric": //createTablesql += "[" + columnTable.Rows[i][0] + "]" + " numeric(6,3),"; //2009.11.25 edit
                                            case "money": createTablesql += "[" + columnTable.Rows[i][0] + "]" + " float,";
                                                break;
                                            default: createTablesql += columnTable.Rows[i][0] + " varchar(1000),";
                                                break;
                                        }
                                        //if (i == cardPlace)
                                        //    createTablesql = createTablesql.Substring(0, createTablesql.Length - 1) + " primary key not null,";
                                    }
                                    //createTablesql += "is_Upload int default 0 not null,empox_favor int default 1 not null,Iden_ID int identity(1,1) not null)";
                                    createTablesql = createTablesql.Substring(0, createTablesql.Length - 1);
                                    createTablesql += ")";
                                    scom.CommandText = createTablesql;
                                    Console.WriteLine("创建表lants_sale_dtl的语句==={0}", createTablesql);
                                    Bcon.Open();
                                    scom.ExecuteNonQuery();


                                    #region 新增的，查看销售记录的(视图)
                                    scom.CommandText = "select COUNT(*) from INFORMATION_SCHEMA.VIEWS where TABLE_NAME =N'saleCount'";
                                    Console.WriteLine("判断视图saleCount是否存在，如果不存在就创建");
                                    int j = (int)(scom.ExecuteScalar());
                                    if (j == 0)
                                    {
                                        scom.CommandText = "create view [dbo].[saleCount] as select cif.sendMan as '发卡人',cif.userName as '持卡人',cif.card_id as '卡号',ls.bill_id as 销售单号,ls.sale_time as '销售时间',cif.card_Type as '卡类型',ls.amount as '金额',ct.times as '消费次数',ls.qty as '购买数量' from (select mst.vip_id,mst.bill_id,mst.sale_time,sum(cast(dtl.amount as float)) as amount,sum(dtl.qty) as qty from lants_sale_mst mst inner join lants_sale_dtl dtl on mst.bill_id = dtl.bill_id group by mst.bill_id,mst.vip_id,mst.sale_time) ls inner join cardInfo cif on ls.vip_id = cif.card_id inner join (select vip_id,count(vip_id) as times from lants_sale_mst group by vip_id) ct on cif.card_id = ct.vip_id";
                                        scom.ExecuteNonQuery();
                                    }
                                    #endregion

                                    #region 新增的
                                    scom.CommandText = "select COUNT(*) from INFORMATION_SCHEMA.VIEWS  where TABLE_NAME =N'salesParticularView'";
                                    j = Convert.ToInt32(scom.ExecuteScalar());
                                    Console.WriteLine("判断视图saleCount是否存在，如果不存在就创建");
                                    if (j == 0)
                                    {
                                        scom.CommandText = "CREATE VIEW [dbo].[salesParticularView] as select cif.sendMan as sendMan,cif.userName as userName,cif.card_id as card_id,cif.card_Type,ls.bill_id as bill_id,ls.sale_time as sale_time,ls.amount as amount,ct.times as times,ls.qty as qty from (select mst.vip_id,mst.bill_id,mst.sale_time,sum(cast(dtl.amount as float)) as amount,sum(dtl.qty) as qty from lants_sale_mst mst inner join lants_sale_dtl dtl on mst.bill_id = dtl.bill_id group by mst.bill_id,mst.vip_id,mst.sale_time) ls inner join cardInfo cif on ls.vip_id = cif.card_id inner join (select vip_id,count(vip_id) as times from lants_sale_mst group by vip_id) ct on cif.card_id = ct.vip_id";

                                        scom.ExecuteNonQuery();
                                    }

                                    #endregion

                                    Bcon.Close();

                                    //写文本文件
                                    if (!Directory.Exists("C:\\DMService\\" + ClientApp.localBase + "\\ImportFile"))
                                    {
                                        Directory.CreateDirectory("C:\\DMService\\" + ClientApp.localBase + "\\ImportFile");
                                    }

                                    nowTime = TimeFormat.getCurrentTime();
                                    Console.WriteLine("时间字符串==={0}", nowTime);
                                    string fileAdd = "C:\\DMService\\" + ClientApp.localBase + "\\ImportFile\\" + nowTime + ".txt";
                                    Console.WriteLine("创建文件{0}.txt==={1}", nowTime, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                    if (!File.Exists(fileAdd))
                                    {
                                        using (FileStream fs = File.Create(fileAdd))
                                        {
                                            byte[] brow = Encoding.GetEncoding("gb2312").GetBytes(t[7]);// Encoding.Default.GetBytes(t[3]);
                                            BufferedStream bs = new BufferedStream(fs);
                                            bs.Write(brow, 0, brow.Length);
                                            bs.Flush();
                                            bs.Close();
                                            fs.Close();
                                            Console.WriteLine("将销售数据写入文件{0}.txt中==={1}", nowTime, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                        }
                                    }
                                    else
                                    {
                                        using (FileStream fs = new FileStream("C:\\DMService\\" + ClientApp.localBase + "\\ImportFile\\" + nowTime + ".txt", FileMode.Create))
                                        {
                                            byte[] brow = Encoding.GetEncoding("gb2312").GetBytes(t[7]);// Encoding.Default.GetBytes(t[3]);
                                            BufferedStream bs = new BufferedStream(fs);
                                            bs.Write(brow, 0, brow.Length);
                                            bs.Flush();
                                            bs.Close();
                                            fs.Close();
                                            Console.WriteLine("将销售数据写入文件{0}.txt中==={1}", nowTime, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                        }
                                    }
                                    Console.WriteLine("将文件{0}.txt中销售数据导入到表lants_sale_dtl(销售明细表)中(先删除表中所有的数据)",nowTime);
                                    //文本数据写入数据表中
                                    SqlCommand inscmd = new SqlCommand();
                                    inscmd.CommandText = " delete from [lants_sale_dtl] ; BULK INSERT [lants_sale_dtl]  " +
                                    "  FROM 'C:\\DMService\\" + ClientApp.localBase + "\\ImportFile\\" + nowTime + ".txt'" +
                                    " WITH " +
                                    " ( " +
                                    " FIELDTERMINATOR = '', " +
                                    " ROWTERMINATOR = '\\n'  " +
                                    ")";
                                    inscmd.Connection = Bcon;
                                    //inscmd.Transaction = tran;
                                    Bcon.Open();
                                    inscmd.ExecuteNonQuery();
                                    Bcon.Close();

                                    


                                    

                                }
                                catch (SqlException e)
                                {
                                    Console.WriteLine("接受销售明细出现异常==={0}", e.Message);
                                    ErrInfo.WriterErrInfo("WebVipOperator", "TimeingsaleInfo---------SalesDetails", e.Message.ToString());
                                }
                                finally
                                {
                                    Bcon.Close();
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            ErrInfo.WriterErrInfo("WebVipOperator", "TimeingSaleInfoDown", ex);
                            Console.WriteLine("接受销售数据出现异常==={0}", ex.Message);
                            return false;
                        }
                    }
                    #endregion
                    else
                    {
                        Console.WriteLine("没有销售数据==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                    }

                    
                }
                else
                {
                    ErrInfo.WriterErrInfo("WebVipOperator", "TimeingSaleInfoDown", "没有销售数据");
                    Console.WriteLine("没有销售数据==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                }

                return true;
            }

            catch (Exception err)
            {
                ErrInfo.WriterErrInfo("WebVipOperator", "TimingVipDownload", err);
                return false;
            }
            finally
            {
                //Mcon.Close();
                Bcon.Close();
                if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\ImportFile\\" + nowTime + ".txt"))
                {
                    File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\ImportFile\\" + nowTime + ".txt");
                    Console.WriteLine("删除文件{0}.txt==={1}", nowTime, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                }
            }
        }

        /// <summary>
        /// 接受商品资料
        /// </summary>
        /// <param name="bytesr"></param>
        /// <returns></returns>
        public bool TimeingProductInfoDown(byte[] bytesr)
        {

            string nowTime = "";
            try
            {
                if (bytesr != null)
                {

                    scom = new SqlCommand();

                    string vipData = Encoding.GetEncoding("GB2312").GetString(bytesr);
                    #region
                    if (vipData != null && vipData != "")
                    {
                        try
                        {
                            Console.WriteLine("有商品资料数据==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            //Console.WriteLine(vipData);
                            columnTable = new DataTable();

                            columnTable.Columns.Add("colName", typeof(string));
                            columnTable.Columns.Add("colType", typeof(string));
                            columnTable.Columns.Add("colLenth", typeof(string));

                            string[] t = vipData.Split(splitChar3[0]);
                            //t[0]="";
                            //t[1]="0";
                            //t[2]="productCodevarchar50productNamevarchar50pricenumeric4stylevarchar20colorvarchar20sizevarchar20sizeGroupIdint4";
                            //t[3]="993";
                            //t[4]="23002WO1-BF2VE-1B1外套649.0000000023002WO1-BF2VE-1B11"
                            int iRow = Convert.ToInt32(t[3]);

                            string[] columnInfo = t[2].Split(splitChar1[0]);

                            for (int i = 1; i < columnInfo.Length; i++)
                            {
                                DataRow myrow = columnTable.NewRow();
                                myrow.ItemArray = columnInfo[i].Split(splitChar2[0]);
                                columnTable.Rows.Add(myrow);
                            }

                            row = t[4].Split(splitChar1[0]);

                            try
                            {
                                int cardPlace = Convert.ToInt32(t[1].ToString());
                                Console.WriteLine("判断表lants_Product(商品表)是否存在，如果不存在就创建");
                                string createTablesql = " if exists(select * from sysobjects where id = object_id(N'[dbo].[lants_Product]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table lants_Product;create table lants_Product(";
                                for (int i = 0; i < columnTable.Rows.Count; i++)
                                {
                                    switch (columnTable.Rows[i][1].ToString())
                                    {
                                        case "int":
                                        case "tinyint":
                                        case "smallint":
                                        case "bigint": createTablesql += "[" + columnTable.Rows[i][0] + "]" + " int,";
                                            break;
                                        case "char":
                                        case "varchar":
                                        case "nchar": createTablesql += "[" + columnTable.Rows[i][0] + "]" + " varchar(" + columnTable.Rows[i][2].ToString() + "),";
                                            break;
                                        case "datetime": createTablesql += "[" + columnTable.Rows[i][0] + "]" + " datetime,";
                                            break;
                                        case "float":
                                        case "double":
                                        case "numeric": //createTablesql += "[" + columnTable.Rows[i][0] + "]" + " numeric(6,3),"; //2009.11.25 edit
                                        case "money": createTablesql += "[" + columnTable.Rows[i][0] + "]" + " float,";
                                            break;
                                        default: createTablesql += columnTable.Rows[i][0] + " varchar(1000),";
                                            break;
                                    }
                                    if (i == cardPlace)
                                        createTablesql = createTablesql.Substring(0, createTablesql.Length - 1) + " primary key not null,";
                                }
                                //createTablesql += "is_Upload int default 0 not null,empox_favor int default 1 not null,Iden_ID int identity(1,1) not null)";
                                createTablesql = createTablesql.TrimEnd(',');
                                createTablesql += ")";
                                scom.CommandText = createTablesql;
                                Console.WriteLine("创建表lants_Product的语句==={0}", createTablesql);
                                scom.Connection = Bcon;
                                Bcon.Open();
                                scom.ExecuteNonQuery();
                                Bcon.Close();

                                if (iRow > 0)
                                {
                                    if (!Directory.Exists("C:\\DMService\\" + ClientApp.localBase + "\\ImportFile"))
                                    {
                                        Directory.CreateDirectory("C:\\DMService\\" + ClientApp.localBase + "\\ImportFile");
                                    }

                                    nowTime = TimeFormat.getCurrentTime();
                                    Console.WriteLine("时间字符串==={0}", nowTime);
                                    string fileAdd = "C:\\DMService\\" + ClientApp.localBase + "\\ImportFile\\" + nowTime + ".txt";
                                    Console.WriteLine("创建文件{0}.txt==={1}", nowTime, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                    if (!File.Exists(fileAdd))
                                    {
                                        using (FileStream fs = File.Create(fileAdd))
                                        {
                                            byte[] brow = Encoding.GetEncoding("gb2312").GetBytes(t[4]);// Encoding.Default.GetBytes(t[3]);
                                            BufferedStream bs = new BufferedStream(fs);
                                            bs.Write(brow, 0, brow.Length);
                                            bs.Flush();
                                            bs.Close();
                                            fs.Close();
                                            Console.WriteLine("将商品数据写入文件{0}.txt中==={1}", nowTime, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                        }
                                    }
                                    else
                                    {
                                        using (FileStream fs = new FileStream("C:\\DMService\\" + ClientApp.localBase + "\\ImportFile\\" + nowTime + ".txt", FileMode.Create))
                                        {
                                            byte[] brow = Encoding.GetEncoding("gb2312").GetBytes(t[4]);// Encoding.Default.GetBytes(t[3]);
                                            BufferedStream bs = new BufferedStream(fs);
                                            bs.Write(brow, 0, brow.Length);
                                            bs.Flush();
                                            bs.Close();
                                            fs.Close();
                                            Console.WriteLine("将商品数据写入文件{0}.txt中==={1}", nowTime, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                        }
                                    }
                                    SqlCommand inscmd = new SqlCommand();
                                    Console.WriteLine("将文件{0}.txt文件中的商品导入表lants_Product(先删除表中所有的数据)",nowTime);
                                    inscmd.CommandText = " delete from lants_Product ; BULK INSERT lants_Product  " +
                                    "  FROM 'C:\\DMService\\" + ClientApp.localBase + "\\ImportFile\\" + nowTime + ".txt'" +
                                    " WITH " +
                                    " ( " +
                                    " FIELDTERMINATOR = '', " +
                                    " ROWTERMINATOR = '\\n'  " +
                                    ")";
                                    inscmd.Connection = Bcon;
                                    //inscmd.Transaction = tran;
                                    Bcon.Open();
                                    inscmd.ExecuteNonQuery();
                                    Bcon.Close();
                                }
                                else
                                {
                                    ErrInfo.WriterErrInfo("WebVipOperator", "TimeingProductInfoDown---lants_Product", "没有商品数据，但已建表！");
                                    Console.WriteLine("没有商品数据==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                }
                            }
                            catch (SqlException e)
                            {
                                Console.WriteLine("下载商品资料出现异常==={0}", e.Message);
                                ErrInfo.WriterErrInfo("WebVipOperator", "TimeingProductInfoDown---lants_Product", e.Message.ToString());
                            }
                            finally
                            {
                                Bcon.Close();
                            }

                        }
                        catch (Exception ex)
                        {
                            ErrInfo.WriterErrInfo("WebVipOperator", "TimeingProductInfoDown", ex);
                            Console.WriteLine("下载商品资料出现异常==={0}", ex.Message);
                            return false;
                        }
                    }
                    #endregion
                    else
                    {
                        Console.WriteLine("没有商品数据==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                    }

                }
                else
                {
                    ErrInfo.WriterErrInfo("WebVipOperator", "TimeingProductInfoDown", "没有商品数据");
                    Console.WriteLine("没有商品数据==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                }

                return true;
            }

            catch (Exception err)
            {
                ErrInfo.WriterErrInfo("WebVipOperator", "TimeingProductInfoDown", err);
                Console.WriteLine("接受商品数据出现异常==={0}", err.Message);
                return false;
            }
            finally
            {
                Bcon.Close();
                if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\ImportFile\\" + nowTime + ".txt"))
                {
                    File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\ImportFile\\" + nowTime + ".txt");
                    Console.WriteLine("删除文件{0}.txt==={1}", nowTime, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                }
            }
        }
        /// <summary>
        /// vip删除增量下载
        /// </summary>
        /// <param name="bytesr"></param>
        /// <returns></returns>
        internal bool TimingVipDelete(byte[] bytesr)
        {
            SqlConnection scon;
            scon = new SqlConnection(ClientApp.Basecon);
            scon.Open();
            scom = new SqlCommand();
            scom.Connection = scon;
            tran = scon.BeginTransaction();
            if (bytesr != null)
            {
                string vipDelData = Encoding.GetEncoding("GB2312").GetString(bytesr);
                if (vipDelData != null && vipDelData != "")
                {
                    Console.WriteLine("有vip删除增量的数据==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                    try
                    {
                        string[] t = vipDelData.Split(splitChar3[0]);
                        int delCount = Int32.Parse(t[2]);

                        if (delCount > 0)
                        {
                            string vipContent = string.Empty;
                            string[] vipStrs = t[3].Split(splitChar1[0]);

                            for (int i = 0; i < vipStrs.Length; i++)
                            {
                                vipContent += "'";
                                vipContent += vipStrs[i];
                                vipContent += "'";
                                if (i != vipStrs.Length - 1)
                                    vipContent += ",";
                            }

                            string sqlStr = "delete from cardInfo where card_id in (" + vipContent + ")";
                            Console.WriteLine("需要删除的vip==={0}", sqlStr);
                            scom.CommandText = sqlStr;
                            scom.Transaction = tran;
                            scom.ExecuteNonQuery();
                        }
                        string operateSql = " insert into operateRecord values('" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','vip删除增量下载=" + delCount + ". 状态:成功.')";
                        Console.WriteLine("写入操作日志==={0}", operateSql);
                        scom.CommandText = operateSql;
                        scom.Transaction = tran;
                        scom.ExecuteNonQuery();
                        tran.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("插入vip删除增量数据出现异常==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        tran.Rollback();
                        //string operateSql = "use " + ClientApp.localBase + " insert into operateRecord values('" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','门店定时数据下载. 状态:失败[" + ex.Message.Replace("\"", "").Replace("'", "").Replace("\r\n", "") + "].')";
                        //失败，写操作日志
                        string operateSql = " insert into operateRecord values('" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','vip删除增量下载. 状态:失败[" + ex.Message.Replace("\"", "").Replace("'", "").Replace("\r\n", "") + "].')";
                        scom.CommandText = operateSql;
                        scom.ExecuteNonQuery();
                        return false;
                    }
                    finally
                    {
                        scon.Close();
                    }
                }
                else
                {
                    Console.WriteLine("没有vip删除增量的数据==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                    return false;
                }

            }
            else
            {
                Console.WriteLine("没有vip删除增量的数据==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                return false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using MySQLDriverCS;


namespace WindowsFormsApplication3
{
    class ClientData
    {
        SqlConnection con;
        ClientMsg[] Remsg;
        int rowcount = 0;
        public int RowCount
        {
            get
            {
                return rowcount;
            }
            set
            {
                rowcount = value;
            }
        }
        public ClientMsg[] getClientMsg()
        {
            try
            {

                con = new SqlConnection(ClientApp.Basecon);
                //查询上传数据
                string str = "select top 10 autoid,sessionid,data,dataBag from tempstor where flag=0 and companyid='" + ClientApp.id + "'";
                SqlDataAdapter ad = new SqlDataAdapter(str, con);
                DataTable Tb = new DataTable();
                con.Open();
                ad.Fill(Tb);
                con.Close();

                rowcount = Tb.Rows.Count;
                Remsg = new ClientMsg[rowcount];
                for (int i = 0; i < rowcount; i++)
                {

                    if (Tb.Rows[i]["data"].ToString().Trim() == "0x24")
                    {
                        Remsg[i] = new ClientMsg(Convert.ToInt32(Tb.Rows[i]["autoid"]),
                                  Tb.Rows[i]["sessionid"].ToString(), DataOperation.VIPRegTempStor, (byte[])Tb.Rows[i]["dataBag"]);
                    }


                    if (Tb.Rows[i]["data"].ToString().Trim() == "0x30")//VIP基本资料下载
                    {
                        Remsg[i] = new ClientMsg(Convert.ToInt32(Tb.Rows[i]["autoid"]),
                                  Tb.Rows[i]["sessionid"].ToString(), DataOperation.VIPDownland);
                    }
                    //新增手动下载vip制卡信息资料 0x61
                    #region 新增手动下载vip制卡信息资料0x61
                    if (Tb.Rows[i]["data"].ToString().Trim() == "0x61")//VIP只制卡基本资料下载
                    {
                        Remsg[i] = new ClientMsg(Convert.ToInt32(Tb.Rows[i]["autoid"]),
                                  Tb.Rows[i]["sessionid"].ToString(), DataOperation.VIPCardDownload);
                    }
                    #endregion

                    //if (Tb.Rows[i]["data"].ToString().Trim() == "0x31")
                    //{
                    //    Remsg[i] = new ClientMsg(Convert.ToInt32(Tb.Rows[i]["autoid"]),
                    //                                     Tb.Rows[i]["sessionid"].ToString(), DataOperation.VIPConfig);
                    //}

                    if (Tb.Rows[i]["data"].ToString().Trim() == "0x34")//VIP基本资料批量修改
                    {
                        //byte[] ms = (byte[])Tb.Rows[i]["dataBag"];
                        //string id = Encoding.Default.GetString(ms);
                        //string[] ArrId = id.Split((char)21);
                        //string ss = "";
                        //for (int i = 0; i < ArrId; i++)
                        //{
                        //    ss = ss + ArrId[i] + (char)21;
                        //    if (i % 100 != 0)
                        //    {

                        Remsg[i] = new ClientMsg(Convert.ToInt32(Tb.Rows[i]["autoid"]),
                                                        Tb.Rows[i]["sessionid"].ToString(), DataOperation.VIPBatchModify, (byte[])Tb.Rows[i]["dataBag"]);
                        //    ss = "";
                        //}
                        //}



                    }
                    if (Tb.Rows[i]["data"].ToString().Trim() == "0x35")//VIP基本资料导入
                    {
                        Remsg[i] = new ClientMsg(Convert.ToInt32(Tb.Rows[i]["autoid"]),
                                                       Tb.Rows[i]["sessionid"].ToString(), DataOperation.VIPUpLoad, (byte[])Tb.Rows[i]["dataBag"]);
                    }
                    if (Tb.Rows[i]["data"].ToString().Trim() == "0x36")//营业员资料下载
                    {
                        Remsg[i] = new ClientMsg(Convert.ToInt32(Tb.Rows[i]["autoid"]),
                                  Tb.Rows[i]["sessionid"].ToString(), DataOperation.VIPSalesDown);
                    }
                    if (Tb.Rows[i]["data"].ToString().Trim() == "0x38")//门店资料下载
                    {
                        Remsg[i] = new ClientMsg(Convert.ToInt32(Tb.Rows[i]["autoid"]),
                                  Tb.Rows[i]["sessionid"].ToString(), DataOperation.VIPShopsDown);
                    }
                }
                return Remsg;
            }
            catch (SqlException ex)
            {
                //Console.WriteLine(ex.Message.ToString());
                ErrInfo.WriterErrInfo("ClientData", "getClientMsg", ex);
                Console.WriteLine("类ClientData方法getClientMsg出现异常==={0}",ex.Message);
            }
            return null;

        }
        /// <summary>
        /// 修改表tempstor处理过的数据的状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool ChangeClientMsgFlag(int id)
        {
            try
            {
                SqlConnection Mcon = new SqlConnection(ClientApp.Basecon);
                string strupdata = "update tempstor set flag=1 where autoid=" + id + "";
                Console.WriteLine("修改表tempstor中处理过的数据状态");
                SqlCommand cmd = new SqlCommand(strupdata);
                cmd.Connection = Mcon;
                Mcon.Open();
                int j = cmd.ExecuteNonQuery();
                Mcon.Close();
                if (j > 0)
                {
                    return true;
                }
                else { return false; }
            }
            catch (SqlException ex)
            {
                // Console.WriteLine(ex.Message.ToString());
                ErrInfo.WriterErrInfo("ClientData", "ChangeClientMsgFlag", ex);
                Console.WriteLine("修改表tempstor中数据状态出现异常==={0}", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 将请求的结果插入到表Tempreturn中
        /// </summary>
        /// <param name="sessionid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool InsertTempReturn(string sessionid, byte[] data)
        {
            try
            {
                string ee = Encoding.GetEncoding("GB2312").GetString(data);
                con = new SqlConnection(ClientApp.Basecon);
                string str = "insert into Tempreturn(sessionid,data,companyid) values('" + sessionid.Trim() + "',@data,'" + ClientApp.id + "'); delete from tempstor where Sessionid='" + sessionid.Trim() + "'";//autoid=" + autoid.Trim() + "
                Console.WriteLine("请求数据返回结果插入表Tempreturn==={0}", str);
                SqlParameter mydata = new SqlParameter("@data", SqlDbType.Binary);
                mydata.Value = data;
                SqlCommand smd1 = new SqlCommand(str);
                smd1.Parameters.Add(mydata);

                smd1.Connection = con;
                con.Open();
                int j = smd1.ExecuteNonQuery();
                con.Close();
                if (j > 0)
                {
                    return true;
                }
                else return false;
            }
            catch (SqlException err)
            {
                ErrInfo.WriterErrInfo("ClientData", "InsertTempReturn", err);
                Console.WriteLine("请求数据返回结果插入表Tempreturn出现异常==={0}", err.Message);
                return false;
            }
        }
        /// <summary>
        /// 删除表TempStor中的数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sessionid"></param>
        /// <returns></returns>
        public bool DeleteTempStor(int id, string sessionid)
        {
            try
            {
                con = new SqlConnection(ClientApp.Basecon);
                string str = "delete from TempStor where autoid=" + id + " and sessionid='" + sessionid + "'";
                Console.WriteLine("删除表TempStor中的数据==={0}", str);
                SqlCommand smd1 = new SqlCommand(str);
                smd1.Connection = con;
                con.Open();
                int j = smd1.ExecuteNonQuery();
                con.Close();
                if (j > 0)
                {
                    return true;
                }
                else return false;
            }
            catch (SqlException err)
            {
                // Console.WriteLine("ClientData-->DeleteTempStor Err:" + err.Message);
                ErrInfo.WriterErrInfo("ClientData", "DeleteTempStor", err);
                Console.WriteLine("删除表TempStor中的数据出现异常==={0}", err.Message);
                return false;
            }
        }
        /// <summary>
        /// 将处理完成的表TempStor中的请求，删除掉
        /// </summary>
        /// <param name="sessionid">会话id</param>
        /// <returns></returns>
        public bool DeleteTempStor(string sessionid)
        {
            try
            {
                con = new SqlConnection(ClientApp.Basecon);
                string str = "delete from TempStor where sessionid='" + sessionid + "'";
                Console.WriteLine("删除表TempStor中的数据==={0}", str);
                SqlCommand smd1 = new SqlCommand(str);
                smd1.Connection = con;
                con.Open();
                int j = smd1.ExecuteNonQuery();
                con.Close();
                if (j > 0)
                {
                    return true;
                }
                else return false;
            }
            catch (SqlException err)
            {
                // Console.WriteLine("ClientData-->DeleteTempStor Err:" + err.Message);
                ErrInfo.WriterErrInfo("ClientData", "DeleteTempStor", err);
                Console.WriteLine("删除表TempStor中的数据出现异常==={0}",err.Message);
                return false;
            }
        }
        public byte[] DataInfoByte()
        {
            try
            {
                byte[] rebyte = null;
                string strline = "";
                con = new SqlConnection(ClientApp.Basecon);
                string str = "select a.[name],b.[name],a.length from sysobjects m left join syscolumns a on m.id=a.id  left join systypes b on a.xtype=b.xtype where m.name='datainfo'";
                //string str = "select * From datainfo";
                SqlDataAdapter adp = new SqlDataAdapter(str, con);
                DataTable mt = new DataTable();
                con.Open();
                adp.Fill(mt);
                con.Close();
                if (mt.Rows.Count > 0)
                {
                    for (int i = 0; i < mt.Rows.Count; i++)
                    {
                        for (int j = 0; j < mt.Columns.Count; j++)
                        {
                            strline = strline + mt.Rows[i][j] + (char)21;
                        }
                        strline = strline.Substring(0, strline.LastIndexOf((char)21)) + (char)22;
                    }
                    strline = strline.Substring(0, strline.LastIndexOf((char)22)) + (char)23;
                }

                //str = "select * From datainfo where DBname='" + ClientApp.localBase + "'";
                str = "select * From datainfo ";
                adp = new SqlDataAdapter(str, con);
                mt = new DataTable();
                con.Open();
                adp.Fill(mt);
                con.Close();
                if (mt.Rows.Count > 0)
                {
                    for (int i = 0; i < mt.Rows.Count; i++)
                    {
                        for (int j = 0; j < mt.Columns.Count; j++)
                        {
                            strline = strline + mt.Rows[i][j] + (char)21;
                        }
                        strline = strline.Substring(0, strline.LastIndexOf((char)21)) + (char)22;
                    }
                    strline = strline.Substring(0, strline.LastIndexOf((char)22));

                    rebyte = Encoding.Default.GetBytes(strline);
                    return rebyte;
                }

                return null;

            }
            catch (SqlException err)
            {
                ErrInfo.WriterErrInfo("ClientData", "DataInfoByte", err);
                return null;
            }
        }

        public byte[] DataInfoByte(byte[] msg)
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(ClientApp.Basecon);
                SqlDataAdapter adp;
                DataTable mt;
                byte[] data = null;
                string sql = "";
                string str = "";
                string strline = "";
                string WhereIn = "";
                string strframe = "";
                bool Bfra = false;

                MemoryStream ms = new MemoryStream();

                string cardid = Encoding.Default.GetString(msg);//points -1000 0001

                char sp = (char)22;
                string tempPoints = cardid.Split(sp)[1];

                //string[] arrCard = cardid.Split((char)21);


                // str = "select ENname From datainfo where DBname='" + ClientApp.localBase + "' and udefined=0 order by ENname";
                //查询cardinfo表的字段名
                str = "select column_name from INFORMATION_SCHEMA.COLUMNS where table_name='cardinfo' and ordinal_position<=20";
                adp = new SqlDataAdapter(str, new SqlConnection(ClientApp.Basecon));
                mt = new DataTable();
                adp.Fill(mt);
                str = "";


                //for(int j=0;j<mt.Rows.Count;j++)
                //{
                //    //str = str + "[" + mt.Rows[j]["ENname"] + "],";
                //    str = str + "isnull([" + mt.Rows[j]["ENname"] + "],'') as " + mt.Rows[j]["ENname"] + ",";
                //}
                for (int j = 0; j < mt.Rows.Count; j++)
                {
                    str = str + "[" + mt.Rows[j]["column_name"] + "],";
                }
                //str存放cardinfo表字段名
                str = str.Substring(0, str.LastIndexOf(","));

                //if (cardid.IndexOf((char)21) > 0)
                //{
                //string[] arrid = cardid.Split((char)21);
                string[] arrvalue = cardid.Split((char)22);
                string[] arrid = arrvalue[2].Split((char)21);

                for (int i = 0; i < arrid.Length; i++)
                {
                    //根据网页发来的iden_id字段查询cardinfo表中的记录
                    if (i % 100 != 0 && i > 0 && i < arrid.Length - 1)
                    {
                        WhereIn = WhereIn + "'" + arrid[i] + "',";
                    }
                    else if (i % 100 == 0 && i == 0 && i < arrid.Length - 1)
                    {
                        WhereIn = WhereIn + "'" + arrid[i] + "',";
                    }
                    else
                    {
                        WhereIn = WhereIn + "'" + arrid[i] + "',";
                        WhereIn = WhereIn.Substring(0, WhereIn.LastIndexOf(","));
                        //sql = "select " + str + " from cardinfo where Iden_id in (" + WhereIn + ")";
                        sql = "select " + str + " from cardinfo where card_id in (" + WhereIn + ")";

                        //adp = new SqlDataAdapter(sql, sqlcon);
                        //mt = new DataTable();
                        //adp.Fill(mt);
                        //select [card_Id],[card_Type],[card_Discount],[userName],[userSex],[userTitle],[userBirthday],[userPhone],[userMobile],[userEmail],[userCode],[userPost],[userAddress],[sendClient],[sendMan],[beginDate],[endDate],[points],[pwd],[remark] from cardinfo where card_id in ('0001')
                        SqlCommand command = new SqlCommand(sql, sqlcon);
                        sqlcon.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            //组织数据
                            DynamicObj obj = new DynamicObj();
                            int count = reader.FieldCount;
                            for (int k = 0; k < count; k++)
                            {
                                if (!Bfra)
                                {
                                    //columname = columname + reader.GetName[i] + getAsciiString(25);
                                    string ss = obj.addSpecialCharactere(str) + getAsciiString(27);
                                    byte[] bss = Encoding.Default.GetBytes(ss);
                                    ms.Write(bss, 0, bss.Length);
                                    Bfra = true;

                                }
                                string columname = reader.GetName(k);
                                string columnameVales = string.Empty;
                                if (columname == "points")
                                {
                                    //FileStream fs = new FileStream(@"c:/lkspoints/points.txt", FileMode.Open, FileAccess.Read);
                                    //BinaryReader br = new BinaryReader(fs);
                                    //columnameVales = br.ReadString();
                                    //fs.Close();
                                    //br.Close();
                                    columnameVales = tempPoints;
                                }
                                else
                                {
                                    columnameVales = reader.GetValue(k).ToString();
                                    if (columnameVales.Length == 0)
                                    {
                                        columnameVales = " ";
                                    }
                                }
                                obj.addNewAttribute(columname, columnameVales);
                            }
                            string Stemp = obj.addSpecialCharactere() + getAsciiString(26);
                            byte[] btemp = Encoding.Default.GetBytes(Stemp);
                            ms.Write(btemp, 0, btemp.Length);
                        }
                        sqlcon.Close();
                        reader.Close();

                        WhereIn = "";
                        sql = "";
                        #region "Delete"
                        //if (mt.Rows.Count > 0)
                        //{
                        //    if (!Bfra)
                        //    {
                        //        for (int col = 0; col < mt.Columns.Count; col++)
                        //        {
                        //            strframe = strframe + mt.Columns[col].ColumnName + getAsciiString(25);
                        //        }

                        //        Bfra = true;
                        //        strframe = strframe.Substring(0, strframe.LastIndexOf(getAsciiString(25))) + getAsciiString(27);
                        //    }

                        //    for (int k = 0; k < mt.Rows.Count; k++)
                        //    {
                        //        for (int c = 0; c < mt.Columns.Count; c++)
                        //        {
                        //            if (mt.Rows[k][c].ToString().Length == 0)
                        //            {
                        //                strline = strline + " " + getAsciiString(25);
                        //            }
                        //            else
                        //            {
                        //                strline = strline + mt.Rows[k][c] + getAsciiString(25);
                        //            }
                        //        }
                        //        strline = strline.Substring(0, strline.LastIndexOf(getAsciiString(25))) + getAsciiString(26);
                        //    }
                        //    // strline = strline.Substring(0, strline.LastIndexOf(getAsciiString(26)));

                        //}
                        #endregion

                    }

                    //WhereIn = WhereIn + "'" + arrid[i] + "',";
                    //WhereIn = WhereIn.Substring(0, WhereIn.LastIndexOf(","));
                    //sql = "select " + str + " from cardinfo where Iden_id in (" + WhereIn + ")";
                }


                //}
                //else
                //{
                //    sql = "select " + str + " from cardinfo where " + cardid;
                //}



                //    strline = strframe + strline.Substring(0, strline.LastIndexOf(getAsciiString(26)));

                //data = Encoding.Default.GetBytes(strline);
                //return data;
                return ms.ToArray();
            }
            catch (Exception err)
            {
                ErrInfo.WriterErrInfo("ClientData", "DataInfoByte22222", err);
                return null;
            }

        }

        private string MyBatchUPdate(string cmd)
        {

            try
            {
                SqlConnection sqlcon = new SqlConnection(ClientApp.Basecon);
                SqlDataAdapter adp;
                DataTable mt;
                string strline = "";

                adp = new SqlDataAdapter(cmd, sqlcon);
                mt = new DataTable();
                adp.Fill(mt);

                if (mt.Rows.Count > 0)
                {
                    for (int col = 0; col < mt.Columns.Count; col++)
                    {
                        strline = strline + mt.Columns[col].ColumnName + getAsciiString(25);
                    }
                    strline = strline.Substring(0, strline.LastIndexOf(getAsciiString(25))) + getAsciiString(27);

                    for (int k = 0; k < mt.Rows.Count; k++)
                    {
                        for (int c = 0; c < mt.Columns.Count; c++)
                        {
                            if (mt.Rows[k][c].ToString().Length == 0)
                            {
                                strline = strline + " " + getAsciiString(25);
                            }
                            else
                            {
                                strline = strline + mt.Rows[k][c] + getAsciiString(25);
                            }
                        }
                        strline = strline.Substring(0, strline.LastIndexOf(getAsciiString(25))) + getAsciiString(26);
                    }
                    strline = strline.Substring(0, strline.LastIndexOf(getAsciiString(26)));
                    return strline;
                }
                return "";
            }
            catch (Exception err)
            {
                ErrInfo.WriterErrInfo("ClientData", "MyBatchUPdate", err);
                return "";
            }
        }

        public byte[] DataInfoByteUPload(byte[] msg)
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(ClientApp.Basecon);
                // MySQLConnection DBConn = null;
                SqlDataAdapter adp;
                DataTable mt;
                byte[] data = null;
                string sql = "";
                string str = "";
                string strline = "";
                string WhereIn = "";
                string strframe = "";
                bool Bfra = false;

                string cardid = Encoding.Default.GetString(msg);


                // str = "select ENname From datainfo where DBname='" + ClientApp.localBase + "' and udefined=0 order by ENname";
                //查询cardinfo表的字段名
                str = "select column_name from INFORMATION_SCHEMA.COLUMNS where table_name='cardinfo' and ordinal_position<=20";
                adp = new SqlDataAdapter(str, new SqlConnection(ClientApp.Basecon));
                mt = new DataTable();
                adp.Fill(mt);
                str = "";
                for (int j = 0; j < mt.Rows.Count; j++)
                {
                    str = str + "isnull([" + mt.Rows[j]["column_name"] + "],'') as [" + mt.Rows[j]["column_name"] + "],";
                }
                str = str.Substring(0, str.LastIndexOf(","));

                //if (cardid.IndexOf((char)21) > 0)
                //{

                string[] arrid = cardid.Split((char)21);
                for (int i = 0; i < arrid.Length; i++)
                {
                    if (i % 100 != 0 && i > 0 && i < arrid.Length - 1)
                    {
                        WhereIn = WhereIn + "'" + arrid[i] + "',";
                    }
                    else if (i % 100 == 0 && i == 0 && i < arrid.Length - 1)
                    {
                        WhereIn = WhereIn + "'" + arrid[i] + "',";
                    }
                    else
                    {
                        //根据网页发来的iden_id字段查询cardinfo表中的记录
                        WhereIn = WhereIn + "'" + arrid[i] + "',";
                        WhereIn = WhereIn.Substring(0, WhereIn.LastIndexOf(","));
                        sql = "select " + str + " from cardinfo where card_id in (" + WhereIn + ")";

                        adp = new SqlDataAdapter(sql, sqlcon);
                        mt = new DataTable();
                        adp.Fill(mt);
                        //组织数据
                        if (mt.Rows.Count > 0)
                        {
                            if (!Bfra)
                            {
                                for (int col = 0; col < mt.Columns.Count; col++)
                                {
                                    strframe = strframe + mt.Columns[col].ColumnName + getAsciiString(25);
                                }

                                Bfra = true;
                                strframe = strframe.Substring(0, strframe.LastIndexOf(getAsciiString(25))) + getAsciiString(27);
                            }

                            for (int k = 0; k < mt.Rows.Count; k++)
                            {
                                for (int c = 0; c < mt.Columns.Count; c++)
                                {
                                    if (mt.Rows[k][c].ToString().Length == 0)
                                    {
                                        strline = strline + " " + getAsciiString(25);
                                    }
                                    else
                                    {
                                        strline = strline + mt.Rows[k][c] + getAsciiString(25);
                                    }
                                }
                                strline = strline.Substring(0, strline.LastIndexOf(getAsciiString(25))) + getAsciiString(26);
                            }
                            // strline = strline.Substring(0, strline.LastIndexOf(getAsciiString(26)));

                        }

                        WhereIn = "";
                        sql = "";
                    }

                }
                #region "以前处理方式。现暂不用。2009-06-11注销"

                //    string[] id = cardid.Split((char)21);
                //    for (int i = 0; i < id.Length; i++)
                //    {
                //        WhereIn = WhereIn + "'" + id[i] + "',";
                //    }

                //    WhereIn = WhereIn.Substring(0, WhereIn.LastIndexOf(","));


                ////    sql = "select card_id as customerid,isnull(use_nm,'') as customername,isnull(pwd,'') as password,isnull(e_mail,'') as email,getdate() as create_time,isnull(bir_dt,'') as birthday,isnull(tel_no,'') as mobile,'' as nickname,1 as type,1 as status   from cardinfo where Iden_id in (" + WhereIn + ")";
                ////adp = new SqlDataAdapter(sql, sqlcon);
                ////mt = new DataTable();
                ////adp.Fill(mt);

                ////string insertMySql = "";


                ////if (mt.Rows.Count > 0)
                ////{
                //    //for (int c = 0; c < mt.Rows.Count; c++)
                //    //{
                //    //  //  insertMySql = insertMySql + "insert into customer(customerid,customername,password,email,create_time,birthday,mobile,nickname,type,status)" +
                //    //  //"values ('" + mt.Rows[c]["card_id"] + "','" + Encoding.GetEncoding("gb2312").GetString(Encoding.Default.GetBytes(mt.Rows[c]["use_nm"].ToString())) + "','" + mt.Rows[c]["pwd"] + "','" + mt.Rows[c]["e_mail"] + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','" + mt.Rows[c]["bir_dt"] + "','" + mt.Rows[c]["tel_no"] + "','',1,1) ; ";
                //    //    insertMySql = "insert into customer(customerid,customername,password,email,create_time,birthday,mobile,nickname,type,status)" +
                //    //     "values ('" + mt.Rows[c]["card_id"] + "','" + Encoding.GetEncoding("gb2312").GetString(Encoding.Default.GetBytes(mt.Rows[c]["use_nm"].ToString())) + "','" + mt.Rows[c]["pwd"] + "','" + mt.Rows[c]["e_mail"] + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','" + mt.Rows[c]["bir_dt"] + "','" + mt.Rows[c]["tel_no"] + "','',1,1) ; ";
                //    //    MySQLCommand DBComm = new MySQLCommand();
                //    //    MySQLParameter sd = new MySQLParameter();

                //    //    DBComm.CommandText = insertMySql;//Encoding.GetEncoding("gb2312").GetString(Encoding.GetEncoding("gb2312").GetBytes(insertMySql));
                //    //    DBComm.Connection = DBConn;
                //    //    //DBConn.Open();
                //    //    int iexec = DBComm.ExecuteNonQuery();

                //    //    if (iexec < 0)//插入失败
                //    //    {
                //    //        //当插入mysql数据库，应该写一些处理．目前没有写．2009-04-22。
                //    //    }
                //    //}
                //    // DBConn.Close();

                //    //DBConn = new MySQLConnection(new MySQLConnectionString("127.0.0.1", "fuzhuang", MysqlSource.User, MysqlSource.Pwd, 3333).AsString);
                //    //MySQLCommand cmd = new MySQLCommand("set   charset   gb2312", DBConn);
                //    //DBConn.Open();
                //    //cmd.ExecuteNonQuery();
                //    //cmd.Dispose();

                //    //MySQLDataAdapter sd = new MySQLDataAdapter();
                //    //sd.SelectCommand = new MySQLCommand();
                //    //sd.SelectCommand.CommandText = "select customerid,customername,password,email,create_time,birthday,mobile,nickname,type,status from customer where 1=0";
                //    //sd.SelectCommand.Connection = DBConn;
                //    //sd.InsertCommand = new MySQLCommand("insert into customer(customerid,customername,password,email,create_time,birthday,mobile,nickname,type,status)"
                //    //                + " values (@customerid,@customername,@password,@email,@create_time,@birthday,@mobile,@nickname,@type,@status);", DBConn);
                //    //sd.InsertCommand.Parameters.Add("@customerid", DbType.String, "customerid");
                //    //sd.InsertCommand.Parameters.Add("@customername", DbType.String, "customername");
                //    //sd.InsertCommand.Parameters.Add("@password", DbType.String, "password");
                //    //sd.InsertCommand.Parameters.Add("@email", DbType.String, "email");
                //    //sd.InsertCommand.Parameters.Add("@create_time", DbType.String, "create_time");
                //    //sd.InsertCommand.Parameters.Add("@birthday", DbType.String, "birthday");
                //    //sd.InsertCommand.Parameters.Add("@mobile", DbType.String, "mobile");
                //    //sd.InsertCommand.Parameters.Add("@nickname", DbType.String, "nickname");
                //    //sd.InsertCommand.Parameters.Add("@type", DbType.UInt16, "type");
                //    //sd.InsertCommand.Parameters.Add("@status", DbType.UInt16, "status");

                //    //sd.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
                //    ////sd.UpdateBatchSize = 0;

                //    //DataSet ds = new DataSet();

                //    //sd.Fill(ds);

                //    //for (int i = 0; i < mt.Rows.Count; i++)
                //    //{
                //    //    ds.Tables[0].Rows.Add(mt.Rows[i].ItemArray);
                //    //}


                //    ////DataSet dataset = new DataSet();
                //    //sd.Update(ds.Tables[0]);
                //    //mt.Clear();
                //    //ds.Tables[0].Clear();
                //    //sd.Dispose();
                //    //mt.Dispose();
                //    //DBConn.Close();
                ////}             

                //    sql = "select " + str + " from cardinfo where Iden_id in (" + WhereIn + ")";

                //    adp = new SqlDataAdapter(sql, sqlcon);
                //    mt = new DataTable();
                //    adp.Fill(mt);

                //    if (mt.Rows.Count > 0)
                //    {
                //        for (int col = 0; col < mt.Columns.Count; col++)
                //        {
                //            strline = strline + mt.Columns[col].ColumnName + getAsciiString(25);
                //        }
                //        strline = strline.Substring(0, strline.LastIndexOf(getAsciiString(25))) + getAsciiString(27);

                //        for (int k = 0; k < mt.Rows.Count; k++)
                //        {
                //            for (int c = 0; c < mt.Columns.Count; c++)
                //            {

                //                if (mt.Rows[k][c].ToString().Length == 0)
                //                {
                //                    strline = strline + " " + getAsciiString(25);
                //                }
                //                else
                //                {
                //                    strline = strline + mt.Rows[k][c] + getAsciiString(25);
                //                }
                //            }
                //            strline = strline.Substring(0, strline.LastIndexOf(getAsciiString(25))) + getAsciiString(26);
                //        }
                //        strline = strline.Substring(0, strline.LastIndexOf(getAsciiString(26)));

                //   // }
                //}
                #endregion

                strline = strframe + strline.Substring(0, strline.LastIndexOf(getAsciiString(26)));

                data = Encoding.Default.GetBytes(strline);
                return data;
            }
            catch (Exception err)
            {
                ErrInfo.WriterErrInfo("ClientData", "DataInfoByte33333", err);
                return null;

            }
        }

        public static String getAsciiString(int i)
        {
            char[] a = {
        (char) i};
            return new String(a);
        }
        /// <summary>
        /// 修改表TempStor中的请求信息的状态
        /// </summary>
        /// <param name="type">请求的类型</param>
        /// <param name="flag">状态的标志</param>
        /// <param name="sessionid">会话的id</param>
        /// <returns></returns>
        public bool ChangeCardInfoFlag(byte type, int flag, string sessionid)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClientApp.Basecon);
                string strwhere = "";
                string sql = "select * from TempStor where sessionid='" + sessionid + "'";
                Console.WriteLine("获取表TempStor中的数据，进行状态的改变==={0}", sql);
                SqlDataAdapter adp = new SqlDataAdapter(sql, con);
                DataTable mt = new DataTable();
                con.Open();
                adp.Fill(mt);
                con.Close();
                if (mt.Rows.Count > 0)
                {
                    byte[] data = (byte[])mt.Rows[0]["dataBag"];
                    string str = Encoding.Default.GetString(data);
                    string[] sp = str.Split((char)21);
                    for (int i = 0; i < sp.Length; i++)
                    {
                        strwhere = strwhere + sp[i] + " ,";
                    }

                    strwhere = strwhere.Substring(0, strwhere.LastIndexOf(","));
                    string strCmd = "";
                    if (type == DataOperation.VIPRegTempStor)
                    {
                        strCmd = "update cardinfo set is_upload=" + flag + " where is_upload=4 and iden_id in (" + strwhere + " )";
                        Console.WriteLine("Vip注册信息请求修改状态==={0}", strCmd);
                    }
                    if (type == DataOperation.VIPUpLoad)
                    {
                        strCmd = "update cardinfo set is_upload=" + flag + " where is_upload=2 and iden_id in (" + strwhere + " )";
                        Console.WriteLine("Vip批量上传请求修改状态==={0}", strCmd);
                    }


                    SqlCommand cmd = new SqlCommand(strCmd);
                    SqlConnection sqlcon = new SqlConnection(ClientApp.Basecon);
                    cmd.Connection = sqlcon;
                    sqlcon.Open();

                    int k = cmd.ExecuteNonQuery();
                    sqlcon.Close();

                    if (k > 0)
                    {

                    }
                    return true;
                }
                else
                    return false;
            }
            catch (SqlException err)
            {
                ErrInfo.WriterErrInfo("ClientData", "DeleteTempStor", err);
                Console.WriteLine("修改表TempStor中的请求的状态出现异常==={0}", err.Message);
                return false;
            }


        }

        public void VIPRegisterTempStor()
        {
            SqlCommand cmd;
            SqlDataAdapter adp;

            SqlConnection con;
            SqlConnection mcon;
            DataTable dt;

            string strwhere = "";
            string strSelect = "";
            string ideID = "";


            try
            {
                con = new SqlConnection(ClientApp.Basecon);
                string str = "select top 100 Iden_id from cardinfo where is_upload=3";
                adp = new SqlDataAdapter(str, con);
                adp.SelectCommand.CommandTimeout = 60 * 1000 * 5;
                dt = new DataTable();
                con.Open();
                adp.Fill(dt);
                con.Close();


                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        strwhere = strwhere + dt.Rows[i]["Iden_id"] + ",";
                        ideID = ideID + dt.Rows[i]["Iden_id"] + (char)21;
                    }


                    strwhere = strwhere.Substring(0, strwhere.LastIndexOf(","));
                    ideID = ideID.Substring(0, ideID.LastIndexOf((char)21));

                    //mcon = new SqlConnection(ClientApp.Basecon);
                    //str = "insert into TempTable values('temp');SELECT SCOPE_IDENTITY() from TempTable";
                    //cmd = new SqlCommand(str, mcon);
                    //mcon.Open();
                    //string tp = Convert.ToString(cmd.ExecuteScalar());

                    DateTime newDT = new DateTime(2010, 1, 1);
                    TimeSpan sp = DateTime.Now - newDT;
                    double li = sp.TotalMilliseconds;


                    byte[] t = Encoding.Default.GetBytes(ideID);
                    str = "insert into TempStor values('" + li.ToString() + "','0x24',0,'','" + ClientApp.id + "',@b)";
                    mcon = new SqlConnection(ClientApp.Basecon);
                    cmd = new SqlCommand(str, mcon);
                    cmd.Parameters.Add("@b", SqlDbType.Binary).Value = t;
                    cmd.CommandText = str;
                    cmd.ExecuteNonQuery();
                    mcon.Close();

                    string usql = "update cardinfo set is_upload=4 where Iden_id in (" + strwhere + ")";

                    cmd.CommandText = usql;
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                ErrInfo.WriterErrInfo("ClientData", "VIPRegisterTempStor", e.Message);
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services;
using MySQLDriverCS;
using System.Data;
using System.Threading;

namespace WindowsFormsApplication3
{
    class WEBCoupon
    {
        MySQLConnection DBCon;
        DataTable DT;
        string ss = "";
        string[] temp = null;

         public Thread thread;
         public WEBCoupon()
            {
                thread = new Thread(new ThreadStart(CoupongMatch));
                thread.Name = "CouponExpand";
                thread.Start();
            }

        public void CoupongMatch()
        {
            while (true)
            {
                //long sleepTime = 5*60*1000;
                if (!ClientApp.isServerOpen)
                {
                    try
                    {
                        Thread.Sleep(5 * 60 * 1000);
                    }
                    catch (ThreadInterruptedException ex)
                    {
                        ErrInfo.WriterErrInfo("WEBCoupon", "CoupongMatch", ex);
                    }
                    continue;
                }

                if (ClientApp.isServerOpen)
                {
                    try
                    {
                        string str = "select distinct customername,customerid,card_no from gift_cards a left join customer b on a.applicant=b.customerid" +
                        "where isvalid=0 and flag=0";
                        DBCon = new MySQLConnection(new MySQLConnectionString(MysqlSource.StrCon, MysqlSource.DB, MysqlSource.User, MysqlSource.Pwd, MysqlSource.Port).AsString);

                        MySQLDataAdapter madp = new MySQLDataAdapter(str, DBCon);
                        DT = new DataTable();
                        DBCon.Open();

                        madp.Fill(DT);

                        DBCon.Close();

                        madp.Dispose();

                        if (DT.Rows.Count > 0)
                        {
                            CreateXML cc = new CreateXML();
                            Getcoupon.couponService dd = new Getcoupon.couponService();

                            DBCon.Open();

                            for (int i = 0; i < DT.Rows.Count; i++)
                            {
                                ss = cc.CreateXmlStrCoupon(DT.Rows[i]["card_no"].ToString(), ClientApp.id, "网上商城", "网店", DateTime.Now.Date.ToString());


                              //  CreateXML cc = new CreateXML();
                               // string ss = cc.CreateXmlStr(uname, vip, ClientApp.id, IP, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), ClientApp.CouponValue);
                                string returnSS = dd.ConsumePC(ss);


                                if (returnSS.IndexOf("cid") >= 0)
                                {
                                    ss = cc.Xmlstr(returnSS.Trim());
                                    temp = ss.Split((char)21);
                                }

                                str = "update gift_cards set flag=" + temp[1] + " where card_no='" + DT.Rows[i]["card_no"] + "'";

                                MySQLCommand cmd = new MySQLCommand(str, DBCon);

                                cmd.ExecuteNonQuery();
                                cmd.Dispose();

                                if (i % 100 == 0)
                                {
                                    Thread.Sleep(1000);
                                }
                            }

                            DBCon.Close();
                        }

                    }
                    catch (Exception e)
                    {
                        DBCon.Close();
                        Thread.Sleep(5 * 1000);
                        ErrInfo.WriterErrInfo("WEBCoupon", "CoupongMatch", e.Message);
                    }
                }
            }
            //cmd.ExecuteNonQuery();
            //cmd.Dispose();
            
            //MySQLCommand cmd = new MySQLCommand(str, DBCon);

               
            //    string ss = cc.CreateXmlStr(uname, vip, ClientApp.id, IP, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),ClientApp.CouponValue);
                
               
        }
    }
}

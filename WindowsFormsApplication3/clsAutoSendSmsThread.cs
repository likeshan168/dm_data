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
using WindowsFormsApplication3.Getcoupon;
namespace WindowsFormsApplication3
{
    class clsAutoSendSmsThread
    {
        public Thread autoSendThread;
        public EmailPhone ep;
        public couponService cs;
        public clsAutoSendSmsThread()
        {
            autoSendThread = new Thread(new ThreadStart(run));
            autoSendThread.Start();
        }

        int sleepTime = 1000 * 60 * 60;
        private void run()
        {
            while (true)
            {
                try
                {
                    DateTime currdt = DateTime.Now;
                    DateTime excTimeB = new DateTime(currdt.Year, currdt.Month, currdt.Day, 22, 0, 0);//开始时间
                    DateTime excTimeE = new DateTime(currdt.Year, currdt.Month, currdt.Day, 9, 0, 0);
                    if (currdt > excTimeB && currdt < excTimeE)
                    {
                        Console.WriteLine(currdt.ToString() + " 禁发时间，线程休眠，等待下次循环。");
                        Thread.Sleep(sleepTime);
                        continue;
                    }
                    Console.WriteLine("获取需要发送的自动短信==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                    //获取是否有待发送模板。
                    string sqlStr = "select [id],[type] ,[modelName] ,[modelText] ,	[condition] ,[mainField] ,[sendDate] ,[dateAdd] ,[lastSendDate] ,[presendCoupon] ," +
                                    "[couponModelID] ,[couponEndDate] ,[presendScore] ,isnull([scoreValue],0) as scoreValue ,[usingState] from dbo.autoMessage where usingState=1 and " +
                                    "(type='动态日期' and (DATEDIFF(day,lastsenddate,getdate())>0 or lastsenddate is null) or " +
                                    "type='固定日期' and  CONVERT(varchar(10), GETDATE(), 23) = dateadd(day,[dateadd],senddate) and lastsenddate is null)";
                    Console.WriteLine("sqlstr===" + sqlStr);
                    DataTable dtModle = SqlbyString(sqlStr);
                    int autoMsgCount = dtModle.Rows.Count;
                    if (autoMsgCount > 0)
                    {
                        Console.WriteLine("有需要发送的自动短信==={0}条", autoMsgCount);
                        //****************查询用户名及密码******************
                        sqlStr = "select uid,pwd from smsSysNum";
                        Console.WriteLine("查询短信用户名及密码sqlstr===" + sqlStr);
                        DataTable dt = SqlbyString(sqlStr);

                        string udi = dt.Rows[0]["uid"].ToString();
                        string pwd = dt.Rows[0]["pwd"].ToString();
                        Console.WriteLine("短信用户名:{0} 密码:{1}", udi, pwd);
                        //查询VIP表字段名
                        sqlStr = "select ENname,CNname From datainfo order by ENname";
                        Console.WriteLine("查询VIP表字段名sqlstr===" + sqlStr);
                        DataTable mt = SqlbyString(sqlStr);

                        string str = "";
                        for (int j = 0; j < mt.Rows.Count; j++)
                        {
                            if (mt.Rows[j]["ENname"].ToString().ToUpper() == "CARD_ID")
                            { str = str + "cardinfo.[" + mt.Rows[j]["ENname"] + "],"; }
                            else
                            { str = str + "[" + mt.Rows[j]["ENname"] + "],"; }
                        }
                        str = str.Substring(0, str.LastIndexOf(","));
                        //*******************************
                        DataTable newTb = new DataTable("Tbsms");
                        newTb.Columns.Add("autoid", typeof(int));
                        newTb.Columns.Add("Mobile", typeof(string));
                        newTb.Columns.Add("Content", typeof(string));
                        newTb.Columns.Add("flag", typeof(int));
                        newTb.Columns.Add("channel", typeof(int));
                        newTb.Columns.Add("mdate", typeof(string));
                        newTb.Columns.Add("state", typeof(string));

                        foreach (DataRow dr in dtModle.Rows)
                        {
                            int id = 0;
                            int ok = 1;
                            string errInfo = "";
                            try
                            {
                                ep = new EmailPhone();
                                id = (int)dr["id"];//5
                                bool sendCoupon = (bool)dr["presendCoupon"];//true
                                if (sendCoupon)
                                {
                                    Console.WriteLine("此短息附加优惠券==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                }
                                else
                                {
                                    Console.WriteLine("此短息暂不附加优惠券==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                }

                                bool givePoints = (bool)dr["presendScore"];//true
                                int pointValue = (int)dr["scoreValue"];//100
                                if (givePoints)
                                {
                                    Console.WriteLine("此短息赠送积分==={0}", pointValue);
                                }
                                else
                                {
                                    Console.WriteLine("此短息暂不赠送积分==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                }
                                string whereStr = dr["condition"].ToString();//1=1
                                string Modeltext = dr["modeltext"].ToString();//[%姓名%]您好，谢谢你对本店的支持，特发此优惠券，来本店消费时，可使用。
                                //查询待发短信的VIP卡号
                                Console.WriteLine("自动短信内容==={0}", Modeltext);
                                //sqlStr = "exec p_getAutoMsgVipData " + id + ",'" + str + "'";
                                //Console.WriteLine("sqlstr===========" + sqlStr);
                                // DataTable Tmt = SqlbyString(sqlStr);

                                SqlCommand cmd = new SqlCommand("p_getAutoMsgVipData");
                                Console.WriteLine("调用存储过程p_getAutoMsgVipData获取自动短信");
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@msgId", id));
                                cmd.Parameters.Add(new SqlParameter("@someStr", str));
                                DataTable Tmt = SqlbyCommand(cmd);

                                string getDate = string.Empty;
                                string endDate = string.Empty;
                                int couponModelId = 0;
                                if (Tmt.Rows.Count > 0 && sendCoupon)//看是否附加优惠券
                                {
                                    //根据类型申请优惠券
                                    //获取可申请优惠券数量
                                    sqlStr = " select count(*) as num from dbo.lants_Coupon where isused=0 and userid=0";
                                    Console.WriteLine("获取可用优惠券sqlstr===========" + sqlStr);
                                    DataTable dtsx = SqlbyString(sqlStr);
                                    int kyCon = (int)dtsx.Rows[0]["num"];
                                    if (kyCon > Tmt.Rows.Count)
                                    {
                                        cs = new couponService();
                                        getDate = DateTime.Now.ToString("yyyy-MM-dd");
                                        endDate = DateTime.Now.AddDays((int)dr["couponEndDate"]).ToString("yyyy-MM-dd");
                                        couponModelId = (int)dr["couponModelId"];//优惠券类型编号
                                    }
                                    else
                                    {
                                        errInfo = "优惠券数量不足";
                                        Console.WriteLine(errInfo);
                                        ok = 0;
                                        UpdateAutoMsgState(id, ok, errInfo);
                                        continue;
                                    }
                                }
                                string vipId = string.Empty;
                                StringBuilder vipStrs = new StringBuilder();
                                Console.WriteLine("自动短信的vip数量===" + Tmt.Rows.Count);
                                string phone = string.Empty;
                                for (int k = 0; k < Tmt.Rows.Count; k++)
                                {
                                    string msgModel = Modeltext;
                                    vipId = Tmt.Rows[k]["card_id"].ToString();
                                    Console.WriteLine("自动短信的vip卡号==={0}", vipId);
                                    //验证手机号
                                    phone = Tmt.Rows[k]["userMobile"].ToString();
                                    Console.WriteLine("vip手机号码==={0}", phone);
                                    if (ep.IsChinaUnicomNumber(phone))
                                    {
                                        Console.WriteLine("手机号码{0}验证成功!==={1}", phone, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                        //Console.WriteLine(Tmt.Rows[k]["username"].ToString() + "用户手机号码:" + Tmt.Rows[k]["userMobile"].ToString());
                                        //替换短信内容
                                        for (int l = 0; l < mt.Rows.Count; l++)
                                        {
                                            msgModel = msgModel.Replace("[%" + mt.Rows[l]["CNname"] + "%]", Tmt.Rows[k][mt.Rows[l]["ENname"].ToString()].ToString());
                                        }

                                        Console.WriteLine("自动短信内容Modeltext===" + msgModel);
                                        //<data vid="v00000001" getDate="2009-01-01" endDate="2009-03-03 23:59:59" tid="1" />
                                        if (sendCoupon)
                                        {
                                            string cid = "";
                                            string state = "";
                                            //根据类型申请优惠券
                                            string swPara = "<data vid=\"" + vipId + "\" getDate=\"" + getDate + "\" endDate=\"" + endDate + "\" tid=\"" + couponModelId + "\" />";
                                            Console.WriteLine("申请优惠券==={0}", swPara);
                                            string resXml = cs.GetNewPCwithTid(swPara);//webservices返回值有问题
                                            Console.WriteLine("申请优惠券返回结果==={0}", resXml);
                                            //string resXml = "<data><cType>1</cType><cid>12345678</cid><proposer /><applyTime /><isUse /><isCom /><state /></data>";
                                            XmlDocument xdoc = new XmlDocument();
                                            xdoc.LoadXml(resXml);
                                            XmlNode root = xdoc.SelectSingleNode("data");
                                            XmlNodeList childs = root.ChildNodes;

                                            foreach (XmlNode node in childs)
                                            {
                                                if (node.Name == "cid")
                                                {
                                                    cid = node.InnerText;
                                                }
                                                else if (node.Name == "state")
                                                {
                                                    state = node.InnerText;
                                                }
                                            }
                                            if (cid.Length > 0)
                                            {
                                                msgModel = msgModel.Replace("[%优惠券ID%]", cid);//这里是将优惠券ID换成优惠券编号
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                            //追加优惠券信息
                                        }
                                        if (givePoints)
                                        {
                                            //追加积分信息
                                            msgModel = msgModel.Replace("[%赠送积分%]", pointValue.ToString());
                                            if (k < Tmt.Rows.Count - 1)
                                                vipStrs.Append("'" + vipId + "',");
                                            else
                                                vipStrs.Append("'" + vipId + "'");
                                        }


                                        //if (Modeltext.Length + ClientApp.SMSSuffix.Length > 70)//60Modeltext.Length+ClientApp.SMSSuffix.Length>70
                                        //判断是否需要拆分短信
                                        int msgLength = msgModel.Length + ClientApp.SMSSuffix.Length;
                                        Console.WriteLine("自动短信长度==={0}", msgLength);
                                        if (msgLength > 70)
                                        {
                                            int count = (msgModel.Length) / 59;// 60;
                                            int yacon = (msgModel.Length) % 59;// 60;
                                            if (yacon > 0)
                                            { count++; }
                                            int index = 0;
                                            //拆分短信
                                            for (int ic = 1; ic <= count; ic++)
                                            {
                                                string sendStr = "[" + ic + "/" + count + "]" + (ic != count ? msgModel.Substring(index, 59) + ClientApp.SMSSuffix : msgModel.Substring(index) + ClientApp.SMSSuffix);
                                                index = ic * 59;
                                                newTb.Rows.Add(new object[] { 0, Tmt.Rows[k]["userMobile"].ToString(), sendStr, 1, 1, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), "wait" });
                                            }
                                        }
                                        else
                                        {
                                            newTb.Rows.Add(new object[] { 0, Tmt.Rows[k]["userMobile"].ToString(), msgModel + ClientApp.SMSSuffix, 1, 1, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), "wait" });
                                        }


                                    }
                                    if ((k > 0 && k % 50 == 0) || k == Tmt.Rows.Count - 1)
                                    {
                                        //System.Diagnostics.Stopwatch MyWatch = new System.Diagnostics.Stopwatch();
                                        //每50条短息一提交
                                        try
                                        {
                                         
                                            SMSService.smsService service = new SMSService.smsService();
                                            service.init(ClientApp.DefaultCompanyID);
                                            Console.WriteLine("公司ID==={0}", ClientApp.DefaultCompanyID);
                                            StringBuilder strbd = new StringBuilder();
                                            //序列号数据
                                            XmlWriter writer = XmlWriter.Create(strbd);
                                            XmlSerializer serializer = new XmlSerializer(typeof(DataTable));
                                            serializer.Serialize(writer, newTb);
                                            writer.Close();

                                            // MyWatch.Start();
                                            //Console.WriteLine("MyWatch开始时间：＝＝＝＝" + MyWatch.ElapsedMilliseconds.ToString());
                                            //调用webserivce发送短信的方法。参数（用户名，密码，公司ID,序列化后的数据）
                                            Console.WriteLine("开始发送自动短息==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                                            String Sreturn = service.sendSMS(udi, pwd, ClientApp.DefaultCompanyID, strbd.ToString());
                                            //处理成功后删除待发短信列表
                                            if (Sreturn.ToUpper() == "SUCCESS")
                                            {
                                                Console.WriteLine("发送自动短息SUCCESS***" + newTb.Rows.Count);
                                                newTb.Rows.Clear();
                                            }
                                            else if (Sreturn.ToUpper().IndexOf("ERROR") > 0)
                                            {
                                                Console.WriteLine("发送自动短息ERROR***" + newTb.Rows.Count);
                                                newTb.Rows.Clear();
                                            }

                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine("发送自动短息异常==={0}",e.Message);
                                            errInfo += "短信提交异常";
                                            ok = 0;
                                        }
                                        finally
                                        {
                                            // MyWatch.Stop();
                                            //Console.WriteLine("MyWatch结束时间：＝＝＝＝" + MyWatch.ElapsedMilliseconds.ToString());
                                        }

                                    }

                                }
                                if (givePoints)
                                {
                                    string tempVip = vipStrs.ToString();
                                    if (tempVip.Length > 0 && pointValue > 0)
                                    {
                                        tempVip = tempVip + ((char)21) + pointValue.ToString();
                                        //赠送积分-发起积分调整请求
                                        byte[] vipBytes = Encoding.ASCII.GetBytes(tempVip);
                                        ClientMsg requestMsg = new ClientMsg(DataOperation.VIPGivePoint, vipBytes);
                                        byte[] uploadInfo = new CreateSendData().insertSendMsgTable(CommandCode.FROM_DM_WRITE_CLIENTMSG, requestMsg.sessionId, 1, 0, requestMsg.Data);
                                        byte[] returnInfoArr = new SendAndReceive("SendClientMsg").mySendAndReceive(uploadInfo);
                                        string returnInfo = new ReturnData().isSucceedInsertTableSendMsg(returnInfoArr);

                                        if (returnInfo != null)
                                        {
                                            Console.WriteLine("赠送积分,插入Client_msg表失败！");
                                            errInfo += "赠送积分-插入Client_msg表失败！";
                                            ok = 0;
                                            new ClientData().InsertTempReturn(requestMsg.sessionId, Encoding.Default.GetBytes("插入Client_msg表失败"));
                                        }
                                    }
                                }

                                //插入记录

                            }
                            catch (Exception exx)
                            {
                                Console.WriteLine("发送自动短息异常==={0}",exx.Message);
                                ok = 0;
                                errInfo = exx.Message;
                            }
                            finally
                            {
                                UpdateAutoMsgState(id, ok, errInfo);
                            }
                            //匹配模板内容初始化短信
                        }
                    }
                    else
                    {
                        Console.WriteLine("没有需要发送的自动短信==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        Thread.Sleep(sleepTime);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("发送自动短息出现异常==={0}", ex.Message);
                    Thread.Sleep(1000 * 60 * 10);
                }
            }

        }
        /// <summary>
        /// 更新自动短信的状态和记录发送日志
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isok"></param>
        /// <param name="res"></param>
        private void UpdateAutoMsgState(int id, int isok, string res)
        {
            string sqlStr = "";
            sqlStr = "update autoMessage  set lastsenddate=getdate() where id=" + id;
            Console.WriteLine("更新自动短息表autoMessage中的状态信息==={0}",sqlStr);
            SqlbyString(sqlStr);
            if (isok == 1)
            {
                res = "成功执行";
            }

            sqlStr = "insert into autoMessage_log values(" + id + "," + isok + ",getdate(),'" + res + "')";
            Console.WriteLine("记录自动短息的发送日志==={0}",sqlStr);
            SqlbyString(sqlStr);
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
        public DataTable SqlbyCommand(SqlCommand cmd)
        {
            SqlDataAdapter adp;
            SqlConnection con = new SqlConnection(ClientApp.Basecon);
            try
            {
                cmd.Connection = con;
                adp = new SqlDataAdapter(cmd);
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

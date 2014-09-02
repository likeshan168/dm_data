using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
namespace WindowsFormsApplication3
{
    class readAppconfig
    {
        static SqlCommand scom;
        static SqlConnection scon;
        public static void readConfig()
        {
            ClientApp.username = ConfigurationManager.AppSettings["username"];
            ClientApp.password = ConfigurationManager.AppSettings["password"];
            ClientApp.serverIP = ConfigurationManager.AppSettings["serverIP"];
            ClientApp.PORT = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
            ClientApp.localBase = ConfigurationManager.AppSettings["localBase"];
            // ClientApp.localcon = ConfigurationSettings.AppSettings["localcon"];
            ClientApp.Basecon = ConfigurationManager.AppSettings["Basecon"];
            //ClientApp.localConfig = ConfigurationSettings.AppSettings["localConfig"];
            ClientApp.SmtpServer = ConfigurationManager.AppSettings["SmtpServer"];
            ClientApp.SmtpPort = Convert.ToInt16(ConfigurationManager.AppSettings["SmtpPort"]);
            ClientApp.SmtpUser = ConfigurationManager.AppSettings["SmtpUser"];
            ClientApp.SmtpPwd = ConfigurationManager.AppSettings["SmtpPwd"];
            ClientApp.DefaultSales = ConfigurationManager.AppSettings["DefaultSales"];
            ClientApp.DefaultShop = ConfigurationManager.AppSettings["DefaultShop"];
            ClientApp.CouponValue = ConfigurationManager.AppSettings["CouponValue"];
            //ClientApp.MessageInfo = ConfigurationSettings.AppSettings["MessageInfo"];
            ClientApp.UseDate = ConfigurationManager.AppSettings["UseDate"];
            ClientApp.SMSFlag = Convert.ToInt32(ConfigurationManager.AppSettings["SMSFlag"]);
            ClientApp.DefaultCompanyID = ConfigurationManager.AppSettings["DefaultCompanyID"];
            ClientApp.SMSSuffix = ConfigurationManager.AppSettings["SMSSuffix"];
        }

        public static bool CreateSQLTempTable()
        {
            //建表
            try
            {
                Console.WriteLine("初始化表==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                scon = new SqlConnection(ClientApp.Basecon);
                scom = new SqlCommand();
                scom.Connection = scon;
                scon.Open();
                string DBsql1 = " if not exists(select * from sysobjects where id = object_id(N'[dbo].[TempVipModify]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) CREATE TABLE [dbo].[TempVipModify] ([autoId] [int] IDENTITY (1, 1) NOT NULL ,[sessionid] [varchar] (20) COLLATE Chinese_PRC_CI_AS NULL ,[StrCommand] [image] NULL ,[inputDate] [datetime] default getdate() ,[flag] [bit] default 0 ,[companyid] [varchar] (20) COLLATE Chinese_PRC_CI_AS NULL ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] ";
                scom.CommandText = DBsql1;
                Console.WriteLine("建表:TempVipModify===" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                scom.ExecuteNonQuery();

                string DBsql2 = " if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TempStor]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) CREATE TABLE [dbo].[TempStor] ([autoId] [int] IDENTITY (1, 1) NOT NULL ,[SessionId] [varchar] (20) COLLATE Chinese_PRC_CI_AS NULL ,[data] [varchar] (10) COLLATE Chinese_PRC_CI_AS NULL ,[flag] [bit] default 0 ,[inputDate] [datetime] default getdate(),[companyid] [varchar] (20) COLLATE Chinese_PRC_CI_AS NULL,[databag] [image] NULL ) ON [PRIMARY]  ";
                scom.CommandText = DBsql2;
                Console.WriteLine("建表:TempStor===" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                scom.ExecuteNonQuery();

                string DBsql3 = " if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TempReturn]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) CREATE TABLE [dbo].[TempReturn] ([autoid] [int] IDENTITY (1, 1) NOT NULL ,[SessionId] [varchar] (20) COLLATE Chinese_PRC_CI_AS NULL ,[data] [image] NULL ,[insertDate] [datetime] NULL default getdate() ,[companyid] [varchar] (20) COLLATE Chinese_PRC_CI_AS NULL ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]  ";
                scom.CommandText = DBsql3;
                Console.WriteLine("建表:TempReturn===" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                scom.ExecuteNonQuery();

                string DBsql4 = " if not exists(select * from sysobjects where name='lpc_ActionLog' and type='U') create table lpc_ActionLog(autoId int ,companyID varchar(12) ,lpcID varchar(18)  primary key,vipId varchar(25),mobileNo varchar(20),aId int,state int,point numeric(13,2),par_Value numeric(13,2),isOK int,errInfo varchar(100),sendType int,email varchar(50)) ";
                scom.CommandText = DBsql4;
                Console.WriteLine("建表:lpc_ActionLog===" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                scom.ExecuteNonQuery();

                string DBsql5 = " if not exists(select * from sysobjects where name='lpc_zfsms_table' and type='U') create table lpc_zfsms_table(autoId int identity(1,1),companyID varchar(12) ,insTime datetime,smsType varchar(10),sysContent varchar(3000),state int) ";
                scom.CommandText = DBsql5;
                Console.WriteLine("建表:lpc_zfsms_table===" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                scom.ExecuteNonQuery();

                scon.Close();
                return true;
            }
            catch (SqlException ex)
            {
                ErrInfo.WriterErrInfo("readAppconfig", "CreateSQLTempTable", ex);
                Console.WriteLine("类readAppconfig方法CreateSQLTempTable出现异常==={0}" ,ex.Message);
                return false;
            }
            catch (Exception ex)
            {

                ErrInfo.WriterErrInfo("readAppconfig", "CreateSQLTempTable", ex);
                Console.WriteLine("类readAppconfig方法CreateSQLTempTable出现异常==={0}", ex.Message);
                return false;
            }
            finally
            {
                scon.Close();
            }
        }


        public static bool CreateSmsSqlTable()
        {
            //建表
            try
            {
                int j = 0;
                scon = new SqlConnection(ClientApp.Basecon);
                scom = new SqlCommand();
                scom.Connection = scon;
                scon.Open();
                string DBsql = "if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DM_zfsms_table]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) CREATE TABLE [DM_zfsms_table] ([autoId] [int] IDENTITY (1, 1) NOT NULL ,[companyID] [varchar] (12) COLLATE Chinese_PRC_CI_AS NULL ,[insTime] [datetime] NULL ,	[smsType] [varchar] (10) COLLATE Chinese_PRC_CI_AS NULL ,	[sysContent] [varchar] (3000) COLLATE Chinese_PRC_CI_AS NULL ,	[state] [int] NULL ) ON [PRIMARY]";
                scom.CommandText = DBsql;
                Console.WriteLine("建表:DM_zfsms_table===" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                scom.ExecuteNonQuery();

                string DBsql1 = "if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sms_model]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) CREATE TABLE [dbo].[sms_model] ([serialNumber] [int] NOT NULL primary key ,[messageType] [varchar] (20) COLLATE Chinese_PRC_CI_AS NOT NULL ,[messageContent] [varchar] (400) COLLATE Chinese_PRC_CI_AS NOT NULL ) ON [PRIMARY]";
                scom.CommandText = DBsql1;
                Console.WriteLine("建表:sms_model===" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                scom.ExecuteNonQuery();

                DBsql1 = "select count(*) from sms_model";
                scom.CommandText = DBsql1;
                j = Convert.ToInt32(scom.ExecuteScalar());
                if (j == 0)
                {
                    DBsql1 = "insert into sms_model values(1,'开卡','尊敬的顾客[姓名]您好。您已注册成为我公司会员,卡号为[卡号],初始密码为[密码],谢谢光临。');insert into sms_model values(2,'销售','尊敬的顾客[姓名],您于[消费时间]在[消费地点]消费金额[消费金额],目前积分为[当前积分]。');insert into sms_model values(3,'积分换礼(成功)','尊敬的顾客,您卡号为：[卡号]的VIP卡积分兑换成功，优惠券编号：[优惠券编号]，面值[优惠券面值]，您当前积分：[当前积分]。');insert into sms_model values(4,'积分换礼(失败)','尊敬的顾客您卡号：[卡号]的VIP卡积分兑换失败，您的当前积分：[当前积分]。')";
                }
                scom.CommandText = DBsql1;
                scom.ExecuteNonQuery();

                string DBsql2 = "if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sms_field]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) CREATE TABLE [dbo].[sms_field] ([serialNumber] [int] NOT NULL ,[fieldName] [varchar] (20) COLLATE Chinese_PRC_CI_AS NOT NULL ,[fieldLength] [int] NOT NULL ) ON [PRIMARY]";
                scom.CommandText = DBsql2;
                Console.WriteLine("建表:sms_field===" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                scom.ExecuteNonQuery();

                DBsql2 = "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_sms_field_sms_model]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1) ALTER TABLE [dbo].[sms_field] DROP CONSTRAINT FK_sms_field_sms_model ; ALTER TABLE dbo.sms_field ADD CONSTRAINT FK_sms_field_sms_model foreign key (serialNumber) references dbo.sms_model (serialNumber)";
                scom.CommandText = DBsql2;
                scom.ExecuteNonQuery();

                DBsql2 = "select count(*) from sms_field";
                scom.CommandText = DBsql2;
                j = Convert.ToInt32(scom.ExecuteScalar());
                if (j == 0)
                {
                    DBsql1 = "insert into sms_field values(1,'姓名',4); insert into sms_field values(1,'卡号',10); insert into sms_field values(1,'密码',8) insert into sms_field values(1,'开卡时间',10) insert into sms_field values(1,'优惠券编号',10)  insert into sms_field values(2,'姓名',4) insert into sms_field values(2,'消费地点',8) insert into sms_field values(2,'消费金额',7) insert into sms_field values(2,'消费时间',10) insert into sms_field values(2,'当前积分',8) insert into sms_field values(3,'卡号',10) insert into sms_field values(3,'当前积分',8) insert into sms_field values(3,'优惠券面值',7) insert into sms_field values(3,'优惠券编号',10);insert into sms_field values(4,'卡号',10);insert into sms_field values(4,'当前积分',8);insert into sms_field values(4,'优惠券面值',7);insert into sms_field values(4,'兑换积分',6);insert into sms_field values(4,'返回信息',12) ";
                }
                scom.CommandText = DBsql1;
                scom.ExecuteNonQuery();
                scon.Close();
                return true;
            }
            catch (SqlException ex)
            {
                ErrInfo.WriterErrInfo("readAppconfig", "CreateSmsSqlTable", ex);
                Console.WriteLine("类readAppconfig中方法CreateSmsSqlTable出现sql异常:" + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                ErrInfo.WriterErrInfo("readAppconfig", "CreateSmsSqlTable", ex);
                Console.WriteLine("类readAppconfig中方法CreateSmsSqlTable出现异常:" + ex.Message);
                return false;
            }
            finally
            {
                scon.Close();
            }
        }

        /// <summary>
        /// 建立存储过程BatchInsert（批量插入vip信息批量插入）
        /// </summary>
        /// <returns></returns>
        public static bool procInsert()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("create proc BatchInsert as  insert into cardinfo");
                sb.Append("(card_Id,card_Type,card_Discount,userName,userSex,");
                sb.Append("userTitle,userBirthday,userPhone,userMobile,userEmail, ");
                sb.Append("userCode,userPost,userAddress,sendClient,sendMan,");
                sb.Append("beginDate,endDate,points,pwd,remark) ");
                sb.Append("select card_Id,card_Type,card_Discount,");
                sb.Append("userName,userSex,userTitle,userBirthday,");
                sb.Append("userPhone,userMobile,userEmail,");
                sb.Append("userCode,userPost,userAddress,sendClient,");
                sb.Append("sendMan,beginDate,endDate,points,pwd,remark from cardinf ;");
                sb.Append("insert into UD_Fileds(card_id) select card_id from cardinfo ");

                string sqlStr = string.Format("use {0}  update clientInfo set procText_insert='{1}'", ClientApp.localBase, sb.ToString());

                scon = new SqlConnection(ClientApp.Basecon);
                scom = new SqlCommand();
                scom.Connection = scon;
                scon.Open();
                scom.CommandText = sqlStr;
                Console.WriteLine("更新表:clientInfo中字段procText_insert的值===" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                scom.ExecuteNonQuery();
                return true;
            }
            catch (SqlException ex)
            {
                ErrInfo.WriterErrInfo("procInsert", "procInsert", ex);
                Console.WriteLine(string.Format("更新表:clientInfo中字段procText_insert的值异常:{0}==={1}", ex.Message, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")));
                return false;
            }
            catch (Exception ex)
            {
                ErrInfo.WriterErrInfo("procInsert", "procInsert", ex);
                Console.WriteLine(string.Format("更新表:clientInfo中字段procText_insert的值异常:{0}==={1}", ex.Message, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")));
                return false;
            }
            finally
            {
                scon.Close();
            }
        }
        /// <summary>
        /// 建立存储过程BatchUpdataInsert（vip信息批量更新插入）
        /// </summary>
        /// <returns></returns>
        public static bool procUpdate()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("CREATE proc BatchUpdataInsert as  if exists(select * ");
                sb.Append("from sysobjects where id = object_id(N''[dbo].[tempaa]'') ");
                sb.Append("and OBJECTPROPERTY(id, N''IsUserTable'') = 1)  drop table tempaa ");
                sb.Append("if exists(select * from sysobjects where id = object_id(N''[dbo].[tempbb]'') ");
                sb.Append("and OBJECTPROPERTY(id, N''IsUserTable'') = 1)  drop table tempbb ");
                sb.Append("update cardinfo set card_Id=b.card_Id,card_Type=b.card_Type, ");
                sb.Append("card_Discount=b.card_Discount,userName=b.userName,");
                sb.Append("userSex=b.userSex, userTitle=b.userTitle,userBirthday=b.userBirthday,");
                sb.Append("userPhone=b.userPhone,userMobile=b.userMobile, ");
                sb.Append("userEmail=b.userEmail,userCode=b.userCode,userPost=b.userPost,");
                sb.Append("userAddress=b.userAddress,sendClient=b.sendClient, ");
                sb.Append("sendMan=b.sendMan,beginDate=b.beginDate,endDate=b.endDate, ");
                sb.Append("points=b.points,remark=b.remark  from cardinfo,cardinf b  ");
                sb.Append("where cardinfo.card_Id=b.card_Id;  insert into cardinfo  ");
                sb.Append("select card_Id,card_Type,card_Discount,userName,userSex, ");
                sb.Append("userTitle,userBirthday,userPhone,userMobile,userEmail,");
                sb.Append("userCode,userPost,userAddress,sendClient,sendMan,beginDate,");
                sb.Append("endDate,points,pwd,remark from cardinf card  ");
                sb.Append("where not exists(select * from cardinfo where Card_ID=card.Card_ID) ");
                sb.Append("insert into UD_Fileds (card_id)    select card_Id from cardinf cardb  ");
                sb.Append("where not exists(select * from UD_Fileds where Card_ID=cardb.Card_ID) ");

                string sqlStr = string.Format("use {0}  update clientInfo set procText_update='{1}'", ClientApp.localBase, sb.ToString());
                scon = new SqlConnection(ClientApp.Basecon);
                scom = new SqlCommand();
                scom.Connection = scon;
                scon.Open();
                scom.CommandText = sqlStr;
                //scom.Parameters.Add("@p", System.Data.SqlDbType.NText).Value = sqlStr;
                Console.WriteLine("更新表:clientInfo中字段procText_update的值===" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                scom.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                ErrInfo.WriterErrInfo("procUpdate", "procUpdate", ex);
                Console.WriteLine(string.Format("更新表:clientInfo中字段procText_update的值异常:{0}==={1}", ex.Message, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")));
                return false;
            }
            finally
            {
                scon.Close();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Collections;

namespace WindowsFormsApplication3
{
    class CreateProc
    {
        /// <summary>
        /// 创建存储过程BatchUpdataInsert
        /// </summary>
        /// <param name="constr"></param>
        public static void CreateProcUPdataInsert(string constr)
        {
            SqlConnection con = new SqlConnection(constr);
            try
            {
                Console.WriteLine("创建存储过程BatchUpdataInsert==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                //                string sql = "CREATE proc BatchUpdataInsert\r\n" +
                //"as\r\n" +
                //"if exists(select * from sysobjects where id = object_id(N'[dbo].[tempaa]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\n" +
                //"drop table tempaa\r\n" +
                //"if exists(select * from sysobjects where id = object_id(N'[dbo].[tempbb]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\n" +
                //"drop table tempbb\r\n" +
                //"update cardinfo set card_Id=b.card_Id,card_Type=b.card_Type,card_Discount=b.card_Discount,userName=b.userName,userSex=b.userSex,\r\n" +
                //"userTitle=b.userTitle,userBirthday=b.userBirthday,userPhone=b.userPhone,userMobile=b.userMobile,\r\n" +
                //"userEmail=b.userEmail,userCode=b.userCode,userPost=b.userPost,userAddress=b.userAddress,sendClient=b.sendClient,\r\n" +
                //"sendMan=b.sendMan,beginDate=b.beginDate,endDate=b.endDate,points=b.points,remark=b.remark \r\n" +//pwd=b.pwd,
                //"from cardinfo,cardinf b where cardinfo.card_Id=b.card_Id\r\n" +
                //"insert into cardinfo\r\n" +
                //"select card_Id,card_Type,card_Discount,userName,userSex,userTitle,userBirthday,userPhone,userMobile,userEmail,\r\n" +
                //"userCode,userPost,userAddress,sendClient,sendMan,beginDate,endDate,points,pwd,remark from cardinf card\r\n" +
                //"where not exists(select * from cardinfo where Card_ID=card.Card_ID)\r\n" +
                //"insert into UD_Fileds (card_id)\r\n" +
                //"select card_Id from cardinf cardb\r\n" +
                //"where not exists(select * from cardinfo where Card_ID=cardb.Card_ID)\r\n";

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

                SqlCommand smd = new SqlCommand();
                smd.CommandText = sb.ToString();
                smd.Connection = con;
                con.Open();
                smd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e)
            {
                ErrInfo.WriterErrInfo("CreateProc", "CreateProcUPdataInsert", e);
                Console.WriteLine("创建存储过程BatchUpdataInsert出现异常==={0}", e.Message);
            }
            finally
            {
                con.Close();
            }
        }
        /// <summary>
        /// 这里有可能会超时的情况
        /// </summary>
        /// <param name="conStr"></param>
        public static void CreateProcInsert(string conStr)
        {
            SqlConnection con = new SqlConnection(conStr);
            try
            {
                //                string sql = "create proc BatchInsert\r\n" +
                //"as\r\n" +
                //"insert into cardinfo(card_Id,card_Type,card_Discount,userName,userSex,userTitle,userBirthday,userPhone,userMobile,userEmail,\r\n" +
                //"userCode,userPost,userAddress,sendClient,sendMan,beginDate,endDate,points,pwd,remark)\r\n" +//Mail_code,PosID,
                //"select card_Id,card_Type,card_Discount,userName,userSex,userTitle,userBirthday,userPhone,userMobile,userEmail,\r\n" +
                //"userCode,userPost,userAddress,sendClient,sendMan,beginDate,endDate,points,pwd,remark from cardinf ;\r\n"+
                //"insert into UD_Fileds(card_id) select card_id from cardinfo\r\n";
                Console.WriteLine("创建存储过程BatchInsert==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
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
                SqlCommand smd = new SqlCommand();
                //smd.CommandText = sql;
                smd.CommandText = sb.ToString();

                smd.Connection = con;
                con.Open();
                smd.CommandTimeout = 180;//180秒（以s为单位，默认是30s）
                smd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e)
            {
                ErrInfo.WriterErrInfo("CreateProc", "CreateProcInsert", e);
                Console.WriteLine("创建存储过程BatchInsert出现异常==={0}", e.Message);

            }
            finally
            {
                con.Close();
            }
        }


        /// <summary>
        /// 复制vip卡号的语句
        /// </summary>
        /// <returns></returns>
        #region 新增的，是为添加vip卡号信息的
        public static void CreateVipCardInsertProc(string conStr)
        {
            SqlConnection con = new SqlConnection(conStr);
            try
            {
                Console.WriteLine("创建存储过程BatchVipCardInsert==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                string sql = string.Format("create proc BatchVipCardInsert    as    insert into VipSet select * from TempVipSet");

                SqlCommand smd = new SqlCommand();
                smd.CommandText = sql;
                smd.Connection = con;
                con.Open();
                smd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ErrInfo.WriterErrInfo("CreateProc", "CreateVipCardInsertProc", ex);
                Console.WriteLine("创建存储过程BatchVipCardInsert出现异常==={0}", ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        #endregion
        /// <summary>
        /// 将新增过来的卡号插入到表VipSet中
        /// </summary>
        /// <returns></returns>
        #region 新增的是用来更新vip卡号信息的
        public static void CreateVipCardUpdateInsertProc(string conStr)
        {
            SqlConnection con = new SqlConnection(conStr);
            try
            {
                Console.WriteLine("创建存储过程BatchVipCardUpdate==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                string sql = string.Format("CREATE proc [dbo].[BatchVipCardUpdate]  as  insert into VipSet  select * from TempVipSet as vipcard  where not exists(select * from VipSet where Card_ID=vipcard.Card_ID) ");//将新增的卡号信息插入到表VipSet中，如果已经存在的话，就不需要管了
                SqlCommand smd = new SqlCommand();
                smd.CommandText = sql;
                smd.Connection = con;
                con.Open();
                smd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ErrInfo.WriterErrInfo("CreateProc", "CreateVipCardUpdateInsertProc", ex);
                Console.WriteLine("创建存储过程BatchVipCardUpdate出现异常==={0}", ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        #endregion
    }
}

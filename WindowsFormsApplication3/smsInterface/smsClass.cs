using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace smsInterface
{
    public class smsClass
    {
        SQL_Member sm = new SQL_Member();
        DESencrypt des = new DESencrypt();

        public string smsSend(string uid, string pwd, string cid, DataTable smsTable)
        {
            try
            {
                string validate = "select count(*) from smsClientInfo where uid=@u and pwd=@p";
                DataTable vdt = new DataTable();
                vdt.Rows.Add(new object[] { "@u", des.Encrypt(uid, "empoxsms"), "varchar" });
                vdt.Rows.Add(new object[] { "@p", des.Encrypt(pwd, "empoxsms"), "varchar" });
                if (Convert.ToInt16(sm.sqlExecuteScalar(validate, vdt)) == 1)
                {
                    sendToClient(cid, smsTable);
                    return "success";
                }
                else
                    return "error-用户名或密码不正确";
            }
            catch (Exception ex)
            { return "error-" + ex.Message; }
        }

        private void sendToClient(string cid, DataTable smsTable)
        {
            try
            {
                string csql = "if not exists (select * from dbo.sysobjects where id = " +
                    "object_id(N'[dbo].[smsRecord_" + cid + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)" +
                    "CREATE TABLE [dbo].[smsRecord_" + cid + "] ([idenNum] [int] IDENTITY (1, 1) NOT NULL primary key," +
                    "[mobile] [char] (11) COLLATE Chinese_PRC_CI_AS NOT NULL ," +
                    "[sendText] [varchar] (2000) COLLATE Chinese_PRC_CI_AS NULL ," +
                    "[pri] [int] NOT NULL ,	[channel] [int] NOT NULL ,[senddate] [datetime] NULL ," +
                    "[state] [varchar] (10) COLLATE Chinese_PRC_CI_AS NULL )";
                sm.sqlExcuteNonQuery(csql, false);
                foreach(DataRow dr in smsTable.Rows)
                {
                    dr[5] = dllCollections.SendSMS(SDKmember.SDKnumber, dr[0].ToString(), dr[1].ToString(), "5");
                }
                sm.sqlExcuteBulkCopy("smsRecord_" + cid, smsTable);
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}

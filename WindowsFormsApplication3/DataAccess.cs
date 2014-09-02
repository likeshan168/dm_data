using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using System.Data;

namespace WindowsFormsApplication3
{
    class DataAccess
    {
        private clsDSet clsdset;
        CreateXML cc = new CreateXML();
        
        SendSMSDll sms ;

        public DataAccess(clsDSet _dset)
        {
            this.clsdset = _dset;
            sms = new SendSMSDll();
        }
        /// <summary>
        /// 发送各种pos机短息
        /// </summary>
        /// <returns></returns>
        public bool SendMsg()
        {
            bool success = false;
            try
            {

                switch (clsdset.SmsType)
                {
                    case "开卡":
                        VIP newvip = new VIP();
                        DataTable dt = cc.Xmlstr(clsdset.SysContent.Trim(), newvip);
                        success = sms.SendSMSVIP(dt, newvip);
                        break;
                    case "销售":
                        Sales sales = new Sales();
                        success = cc.Xmlstr(clsdset.SysContent.Trim(), sales);
                        success = sms.SendSMSSale(sales);

                        break;
                    case "积分换礼":
                        Point point = new Point();
                        success = cc.Xmlstr(clsdset.SysContent.Trim(), point);
                        success = sms.SendSMSCoupon(point);
                        break;
                }

            }
            catch (Exception ex)
            {
                ErrInfo.WriterErrInfo("DataAccess", "SendMsg", ex.Message);
                Console.Write("类DataAccess方法SendMsg中出现异常==={0}",ex.Message);
                return false;
            }
            return success;
        }
        /// <summary>
        /// 发送pos机的短信
        /// </summary>
        /// <returns></returns>
        public bool UpdateDll()
        {
            try
            {
                string ss = sms.ImportDLL();
                if (ss == "ERROR")
                {
                    return false;
                }
                else { return true; }
            }
            catch (Exception er)
            {
                Console.WriteLine("pos机短信发送出现异常==={0}", er.Message);
                ErrInfo.WriterErrInfo("DataAccess", "UpdateDll----", er);
                return false;
            }
        }
    }
}

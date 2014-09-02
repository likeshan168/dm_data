using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using System.Data.SqlClient;
namespace WindowsFormsApplication3
{
    class EmailPhone
    {

        public bool IsChinaUnicomNumber(string mobile)
        {
            string sPattern = "(13)[0-9]{1}\\d{8}|(15)[0-9]{1}\\d{8}|(18)[0-9]{1}\\d{8}";

            bool isChinaUnicom = Regex.IsMatch(mobile, sPattern);
            return isChinaUnicom;
        }

        public bool IsEmail(string email)
        {
            string spattern = "^[\\w-]+(\\.[\\w-]+)*@[\\w-]+(\\.[\\w-]+)+$";
            //string spattern = "^[a-zA-Z0-9_\\-]{1,.}@[a-zA-Z0-9_\\-]{1,}\\.[a-zA-Z0-9_\\-.]{1,}";
            bool isemail = Regex.IsMatch(email, spattern);
            return isemail;
        }
        SqlConnection Bcon;
        public bool SendSMS(string phone, string str)
        {
            //SqlConnection testCon = new SqlConnection("server=219.232.48.105;database=dmdemo;uid=sa;pwd=empoxweb!@90zgtyb");
            try
            {
                int icou = 0;
                SqlCommand cmd;

                string sql = "select pri from PRIsetting where ltrim(rtrim(smsType))='开卡'";

                Bcon = new SqlConnection(ClientApp.Basecon);

                cmd = new SqlCommand(sql, Bcon);

                //cmd = new SqlCommand(sql, testCon);
                Bcon.Open();
                //testCon.Open();
                icou = Convert.ToInt16(cmd.ExecuteScalar());

                //string sql = " insert into sendData values('999" + (char)21 + phone + (char)22 + str + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";

                sql = " insert into senddata(Mobile,type,[text],insTime,errorcount,pri) " +
                   " values('" + phone + "','开卡','" + str + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',0," + icou + ")";

                //  sql = "use mansate insert into senddata(Mobile,type,[text],insTime,errorcount,pri) values('" + phone + "','开卡','" + str + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',0," + icou + ")";

                cmd = new SqlCommand(sql, Bcon);
                //cmd = new SqlCommand(sql, testCon);


                icou = cmd.ExecuteNonQuery();
                Bcon.Close();
                //testCon.Close();
                return true;
            }
            catch (Exception e)
            {
                ErrInfo.WriterErrInfo("EmailPhone", "SendSMS", e);
                return false;

            }
            finally
            {
                Bcon.Close();
                //testCon.Close();
                
            }
        }
        
        public bool BsendMail(string STo, string mailbody, int type,string subject)
        {
            bool breturn = false;

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("DMtest@empox.cn", "EMPOX");
            mailMessage.To.Add(STo);
            mailMessage.Subject = subject;
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
            smtpclient.Host = ClientApp.SmtpServer; //*******************************************
            smtpclient.Port = ClientApp.SmtpPort;
            smtpclient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpclient.EnableSsl = false;
            smtpclient.Credentials = new System.Net.NetworkCredential(ClientApp.SmtpUser,ClientApp.SmtpPwd);//*******************************************
            try
            {
                smtpclient.Send(mailMessage);
                breturn = true;
            }
            catch (Exception ex)
            {
                ErrInfo.WriterErrInfo("EmailPhone", "BsendMail", ex);
                breturn = false;
            }
            return breturn;
        }


         public bool ValidatEmail(string email)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("select count(*) from cardinfo where e_mail='" + email + "'");
                Bcon = new SqlConnection(ClientApp.Basecon);
                cmd.Connection = Bcon;
                Bcon.Open();
                int icou = (int)cmd.ExecuteScalar();
                Bcon.Close();
                    if(icou>0)
                        return false;
                    else 
                       return true;

            }
            catch (Exception e)
            {
                ErrInfo.WriterErrInfo("EmailPhone", "ValidatEmail", e.Message);
                Bcon.Close();
                return false;
            }

        }

    }
}
  

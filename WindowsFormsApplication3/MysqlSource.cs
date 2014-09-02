using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySQLDriverCS;

namespace WindowsFormsApplication3
{
    class MysqlSource
    {
        static string _user = "root";
        static string _pwd = "1";
        static string _strcon = "127.0.0.1";
        static string _db = "fuzhuang";
        static int _port = 3306;
        // DBCon = new MySQLConnection(new MySQLConnectionString("219.232.48.113", "fuzhuang", "mstweb", "MSTWEB!@20090531sth", 3306).AsString);
        public static string User
        {
            get
            {
                return _user;
            }
        }
        public static string Pwd
        {
            get
            {
                return _pwd;
            }
        }
        public static string StrCon
        {
            get
            {
                return _strcon;
            }
        }
        public static string DB
        {
            get
            {
                return _db;
            }
        }
        public static int Port
        {
            get
            {
                return _port;
            }
        }
            //DBConn = new MySQLConnection(new MySQLConnectionString("127.0.0.1", "fuzhuang", "test", "test", 3333).AsString);
            //     MySQLCommand cmd = new MySQLCommand("set   charset   gb2312", DBConn);

            //     DBConn.Open();
            //     cmd.ExecuteNonQuery();

            //     cmd.Dispose();   


            //     MySQLCommand DBComm = new MySQLCommand();
            //     DBComm.CommandText = Encoding.GetEncoding("gb2312").GetString(Encoding.GetEncoding("gb2312").GetBytes(insweb));
            //     DBComm.Connection=DBConn;
            //     //DBConn.Open();
            //     int iexec = DBComm.ExecuteNonQuery();
            //     if (iexec > 0)
            //     {
            //         trans.Commit();
            //         Bcon.Close();
            //         DBConn.Close();
            //         return true;
            //     }
 
       
           

    }
}

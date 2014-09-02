using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication3
{
    class clsDSet
    {
        //autoId int identity(1,1), --自增列
        //companyID varchar(12) ,--公司ID
        //insTime datetime,--插入时间
        //smsType varchar(10),--消息类型
        //sysContent varchar(3000),--消息内容（xml）
        //state int--状态
        public clsDSet()
        {
        }

        private int _autoid;
        private string _companyid;
        private DateTime _instime;
        private string _smstype;
        private string _syscontent;
        private int _state;

        /// <summary>
        /// 
        /// </summary>
        public int AutoId
        {
            set { _autoid = value; }
            get { return _autoid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CompanyID
        {
            set { _companyid = value; }
            get { return _companyid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime InsTime
        {
            set { _instime = value; }
            get { return _instime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SmsType
        {
            set { _smstype = value; }
            get { return _smstype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SysContent
        {
            set { _syscontent = value; }
            get { return _syscontent; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Istate
        {
            set { _state = value; }
            get { return _state; }
        }
    }
}

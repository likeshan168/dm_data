using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication3
{
    class clsEntity
    {
        public clsEntity()
        { }
        #region Model
        private int _autoid;
        private string _companyid;
        private string _lpcid;
        private string _vipid;
        private decimal _point;
        private decimal _par_value;
        private int _isok;
        private string _errinfo;
        private string _mobileno;
        private int _sendType;
        private string _email;
        /// <summary>
        /// 
        /// </summary>
        public int autoId
        {
            set { _autoid = value; }
            get { return _autoid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string companyID
        {
            set { _companyid = value; }
            get { return _companyid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string lpcID
        {
            set { _lpcid = value; }
            get { return _lpcid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string vipId
        {
            set { _vipid = value; }
            get { return _vipid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal point
        {
            set { _point = value; }
            get { return _point; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal par_Value
        {
            set { _par_value = value; }
            get { return _par_value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int isOK
        {
            set { _isok = value; }
            get { return _isok; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string errInfo
        {
            set { _errinfo = value; }
            get { return _errinfo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string mobileNo
        {
            set { _mobileno = value; }
            get { return _mobileno; }
        }

        public int sendType
        {
            set { _sendType = value; }
            get { return _sendType; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string email
        {
            set { _email = value; }
            get { return _email; }
        }
        #endregion Model

    }
}

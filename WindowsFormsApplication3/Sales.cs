using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication3
{
    class Sales
    {
        //<Data vipId="vip卡号" name="姓名" sex="性别"
        // points ="积分" telephone="手机号码"  money="消费金额" saleTime="消费时间" clientId="店铺代码" clientName ="店铺名称">
        //</Data>
        private string _vipid;
        private string _name;
        private string _sex;
        private string _points;
        private string _telephone;
        private string _saletime;
        private string _clientid;
        private string _clientname;
        private string _money;

        public string VIPid
        {
            get
            {
                return _vipid;
            }
            set
            {
                _vipid = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public string Sex
        {
            get
            {
                return _sex;
            }
            set
            {
                _sex = value;
            }
        }

        public string Points
        {
            get
            {
                return _points;
            }
            set
            {
                _points = value;
            }
        }

        public string TelPhone
        {
            get
            {
                return _telephone;
            }
            set
            {
                _telephone = value;
            }
        }

        public string SaleTime
        {
            get
            {
                return _saletime;
            }
            set
            {
                _saletime = value;
            }
        }

        public string ClientId
        {
            get
            {
                return _clientid;
            }
            set
            {
                _clientid = value;
            }
        }

        public string ClientName
        {
            get
            {
                return _clientname;
            }
            set
            {
                _clientname = value;
            }
        }

        public string Money
        {
            get
            {
                return _money;
            }
            set
            {
                _money = value;
            }
        }
    }
}

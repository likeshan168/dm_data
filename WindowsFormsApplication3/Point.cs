using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication3
{
    class Point
    {
        //<?xml version='1.0' encoding='gb2312'?>
        //<Data vipId="vip卡号"  points ="积分"  money="优惠券面值" cid ="优惠券ID" linkType="联系方式" backData="返回信息" telephone="手机号码" email="Email">
        //</Data>
        //<?xml version='1.0' encoding='gb2312'?>
        //<Data vipId="vip卡号"  points ="积分"  money="优惠券面值" lpcID=“优惠券ID” isOK=“是否成功” linkType="联系方式" backData="返回信息" telephone="手机号码" email="Email" currPoints="当前积分">
        //</Data>
        private string _vipId;
        private string _points;
        private string _money;
        private string _lpcid;
        private int _isok;
        private string _linktype;
        private string _backData;
        private string _telephone;
        private string _email;
        private string _currPoints;

        public string VIPid
        {
            get
            {
                return _vipId;
            }
            set
            {
                _vipId = value;
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

        public string LpcID
        {
            get
            {
                return _lpcid;
            }
            set
            {
                _lpcid = value;
            }
        }

        public int IsOK
        {
            get { return _isok; }
            set { _isok = value; }
        }

        public string LinkType
        {
            get
            {
                return _linktype;
            }
            set
            {
                _linktype = value;
            }
        }

        public string BackData
        {
            get
            {
                return _backData;
            }
            set
            {
                _backData = value;
            }
        }

        public string Telephone
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

        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
            }
        }

        public string CurrPoints
        {
            get { return _currPoints; }
            set { _currPoints = value; }
        }
    }
}

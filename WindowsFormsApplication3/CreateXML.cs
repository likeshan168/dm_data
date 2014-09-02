using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;

namespace WindowsFormsApplication3
{
    class CreateXML
    {
        public string CreateXmlStr(string perosn, string vip, string companyid, string ip, string date, string money)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                string xmlStr = "<data/>";

                doc.LoadXml(xmlStr);
                XmlNode root = SelectXmlNode(doc, "data");

                //XmlElement elem = doc.CreateElement("row");
                //elem.SetAttribute("proposer", perosn);
                //elem.SetAttribute("vid", vip);
                //elem.SetAttribute("companyId", companyid);
                //elem.SetAttribute("IP", ip);
                //elem.SetAttribute("aDate", date);
                //elem.SetAttribute("money", money);
                //root.AppendChild(elem);

                XmlNode PerNode = doc.CreateNode(XmlNodeType.Attribute, "proposer", null);
                PerNode.InnerText = perosn;
                root.Attributes.SetNamedItem(PerNode);

                XmlNode VipNode = doc.CreateNode(XmlNodeType.Attribute, "vid", null);
                VipNode.InnerText = vip;
                root.Attributes.SetNamedItem(VipNode);

                XmlNode CompanyNode = doc.CreateNode(XmlNodeType.Attribute, "companyId", null);
                CompanyNode.InnerText = companyid;
                root.Attributes.SetNamedItem(CompanyNode);

                XmlNode IPNode = doc.CreateNode(XmlNodeType.Attribute, "IP", null);
                IPNode.InnerText = ip;
                root.Attributes.SetNamedItem(IPNode);

                XmlNode DateNode = doc.CreateNode(XmlNodeType.Attribute, "aDate", null);
                DateNode.InnerText = date;
                root.Attributes.SetNamedItem(DateNode);

                XmlNode MoneyNode = doc.CreateNode(XmlNodeType.Attribute, "money", null);
                MoneyNode.InnerText = money;
                root.Attributes.SetNamedItem(MoneyNode);

            }
            catch (Exception) { }

            return doc.InnerXml;

        }

        public string CreateXmlStrCoupon(string cid, string companyid, string mobileId, string clientId, string cDate)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                string xmlStr = "<data/>";

                doc.LoadXml(xmlStr);
                XmlNode root = SelectXmlNode(doc, "data");

                //XmlElement elem = doc.CreateElement("row");
                //elem.SetAttribute("proposer", perosn);
                //elem.SetAttribute("vid", vip);
                //elem.SetAttribute("companyId", companyid);
                //elem.SetAttribute("IP", ip);
                //elem.SetAttribute("aDate", date);
                //elem.SetAttribute("money", money);
                //root.AppendChild(elem);

                XmlNode PerNode = doc.CreateNode(XmlNodeType.Attribute, "cid", null);
                PerNode.InnerText = cid;
                root.Attributes.SetNamedItem(PerNode);

                XmlNode CompanyNode = doc.CreateNode(XmlNodeType.Attribute, "companyId", null);
                CompanyNode.InnerText = companyid;
                root.Attributes.SetNamedItem(CompanyNode);

                XmlNode IPNode = doc.CreateNode(XmlNodeType.Attribute, "mobileId", null);
                IPNode.InnerText = mobileId;
                root.Attributes.SetNamedItem(IPNode);

                XmlNode MoneyNode = doc.CreateNode(XmlNodeType.Attribute, "clientId", null);
                MoneyNode.InnerText = clientId;
                root.Attributes.SetNamedItem(MoneyNode);

                XmlNode DateNode = doc.CreateNode(XmlNodeType.Attribute, "cDate", null);
                DateNode.InnerText = cDate;
                root.Attributes.SetNamedItem(DateNode);

                
            }
            catch (Exception) { }

            return doc.InnerXml;

        }
        public string Xmlstr(string xmlstr)
        {
            string ss = "";
            try
            {

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlstr);

                XmlNode root = SelectXmlNode(xmlDoc, "NewDataSet");

                ss = ParseXml(root);
                if (ss.Trim().Length > 0)
                {
                    ss = ss.Substring(0, ss.LastIndexOf((char)21));
                    ss = ss.Replace((char)21 + "#text", "");
                }
            }
            catch (Exception) { }

            return ss;
        }
        string s1 = "";
        public string ParseXml(XmlNode root)
        {
            try
            {
                if (root.HasChildNodes)
                {
                    XmlNodeList nodeList = root.ChildNodes;
                    if (nodeList.Count > 0)
                    {
                        foreach (XmlNode node in nodeList)
                        {
                            if (node.Value == null)
                            {
                                s1 = s1 + node.Name + (char)21;
                                ParseXml(node);
                            }
                            else
                            {
                                s1 = s1 + node.Value + (char)21;
                            }
                        }
                    }
                    else
                    {
                        s1 = s1.Substring(0, s1.LastIndexOf((char)21));
                        s1 = s1 + ":" + root.Value + (char)21;
                    }
                }
            }

            catch (Exception) { }
            return s1;

        }


        public XmlNode SelectXmlNode(XmlDocument doc, string xPath)
        {
            XmlNode node = null;
            try
            {
                node = doc.SelectSingleNode(xPath);
            }
            catch (Exception) { }
            return node;
        }


        public string GetNodeAttribute(XmlNode node, String attrName)
        {
            string attrValue = null;
            try
            {
                attrValue = node.Attributes[attrName].InnerText;
            }
            catch (Exception) { }
            return attrValue;
        }


        public DataTable Xmlstr(string xmlstr, VIP _vip)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlstr);

                XmlNode root = SelectXmlNode(xmlDoc, "Data");

                _vip.Defpwd = GetNodeAttribute(root, "pwd");
                _vip.Defmoney = GetNodeAttribute(root, "money");
                _vip.DefCommpany = GetNodeAttribute(root, "companyId");


                XmlNodeList xlist = xmlDoc.SelectNodes("//Data/Row");

                DataTable Dt = new DataTable();
                DataRow Dr;

                for (int i = 0; i < xlist.Count; i++)
                {
                    Dr = Dt.NewRow();
                    XmlElement xe = (XmlElement)xlist.Item(i);
                    for (int j = 0; j < xe.Attributes.Count; j++)
                    {
                        if (!Dt.Columns.Contains("@" + xe.Attributes[j].Name))
                            Dt.Columns.Add("@" + xe.Attributes[j].Name);
                        Dr["@" + xe.Attributes[j].Name] = xe.Attributes[j].Value;
                    }
                    for (int j = 0; j < xe.ChildNodes.Count; j++)
                    {
                        if (!Dt.Columns.Contains(xe.ChildNodes.Item(j).Name))
                            Dt.Columns.Add(xe.ChildNodes.Item(j).Name);
                        Dr[xe.ChildNodes.Item(j).Name] = xe.ChildNodes.Item(j).InnerText;
                    }

                    Dt.Rows.Add(Dr);
                }
                //return Dt;
                // XmlNodeList nodeList = root.ChildNodes;//取得根节点下所有字节点
                //if (nodeList.Count > 0)
                //{
                //    foreach (XmlNode node in nodeList)
                //    {
                //        _vip.DefString = _vip.DefString + GetNodeAttribute(node, "column_name") + (char)21;
                //        _vip.DefString = _vip.DefString + GetNodeAttribute(node, "data_type") + (char)21;
                //        _vip.DefString = _vip.DefString + GetNodeAttribute(node, "character_maximum_length") + (char)21;
                //        _vip.DefString = _vip.DefString + GetNodeAttribute(node, "china_name") + (char)21;
                //        _vip.DefString = _vip.DefString + GetNodeAttribute(node, "value") + (char)21;
                //        _vip.DefString = _vip.DefString + (char)22;
                //    }
                //}
                return Dt;
            }
            catch (Exception e)
            {
                ErrInfo.WriterErrInfo("CreateXML", "Xmlstr---VIP", e.Message.ToString());
                return null;
            }
        }
        public bool Xmlstr(string xmlstr, Point _coupon)
        {
            //<?xml version='1.0' encoding='gb2312'?>
            //<Data vipId="vip卡号"  points ="积分"  money="优惠券面值" cid ="优惠券ID" linkType="联系方式" backData="返回信息" telephone="手机号码" email="Email">
            //</Data>
            //            <?xml version='1.0' encoding='gb2312'?>
            //<Data vipId="vip卡号"  points ="积分"  money="优惠券面值" lpcID=“优惠券ID” isOK=“是否成功” linkType="联系方式" backData="返回信息" telephone="手机号码" email="Email" currPoints="当前积分">
            //</Data>
            try
            {

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlstr);

                XmlNode root = SelectXmlNode(xmlDoc, "Data");

                if (root != null)
                {
                    _coupon.VIPid = GetNodeAttribute(root, "vipId");
                    _coupon.Points = GetNodeAttribute(root, "points");
                    _coupon.Money = GetNodeAttribute(root, "money");
                    _coupon.LpcID = GetNodeAttribute(root, "lpcID");
                    _coupon.IsOK = Convert.ToInt32(GetNodeAttribute(root, "isOK"));
                    _coupon.LinkType = GetNodeAttribute(root, "linkType");
                    _coupon.BackData = GetNodeAttribute(root, "backData");
                    _coupon.Telephone = GetNodeAttribute(root, "telephone");
                    _coupon.Email = GetNodeAttribute(root, "email");
                    _coupon.CurrPoints = GetNodeAttribute(root, "currPoints");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                ErrInfo.WriterErrInfo("CreateXML", "Xmlstr---Point", e.Message.ToString());
                return false;
            }
        }
        public bool Xmlstr(string xmlstr, Sales sale)
        {
            try
            {
                //<Data vipId="vip卡号" name="姓名" sex="性别"
                // points ="积分" telephone="手机号码"  money="消费金额" saleTime="消费时间" clientId="店铺代码" clientName ="店铺名称">
                //</Data>
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlstr);

                XmlNode root = SelectXmlNode(xmlDoc, "Data");

                if (root != null)
                {
                    sale.VIPid = GetNodeAttribute(root, "vipId");
                    sale.Name = GetNodeAttribute(root, "name");
                    sale.Sex = GetNodeAttribute(root, "sex");
                    sale.Points = GetNodeAttribute(root, "points");
                    sale.TelPhone = GetNodeAttribute(root, "telephone");
                    sale.Money = GetNodeAttribute(root, "money");
                    sale.SaleTime = GetNodeAttribute(root, "saleTime");
                    sale.ClientId = GetNodeAttribute(root, "clientId");
                    sale.ClientName = GetNodeAttribute(root, "clientName");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                ErrInfo.WriterErrInfo("CreateXML", "Xmlstr---Sales", e.Message.ToString());
                return false;
            }
        }

        
    }
}

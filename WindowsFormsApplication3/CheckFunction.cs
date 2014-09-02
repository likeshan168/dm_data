using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace WindowsFormsApplication3
{
    class CheckFunction
    {
        public CommonMsg commonMsg;

        public CheckFunction(CommonMsg commonMsg)
        {
            this.commonMsg = commonMsg;
        }

        public CheckMsg checkAll()
        {

            string str = commonMsg.msgBlock.msg; //等待处理的数据
            CheckMsg checkMsg = null;
            string[] allLine = OperateString.getStringCollection(str, MsgMacro.mainDivide1);
            if (allLine.Length < 2)
            {
                allLine = OperateString.getStringCollection(str, MsgMacro.mainDivide2);
            }
            string infoStr = "";
            if (allLine.Length < 2)
            {
                if (allLine.Length == 1)
                {
                    infoStr = "您输入的信息格式错误！error";
                    checkMsg = new CheckMsg(false, DataOperation.UN_KNOW, "", infoStr);
                    Console.WriteLine(infoStr);
                    return checkMsg;
                }
                else
                {
                    infoStr = "您输入的信息格式错误！error";
                    checkMsg = new CheckMsg(false, DataOperation.UN_KNOW, "", infoStr);
                    Console.WriteLine(infoStr);
                    return checkMsg;
                }
               
            }
            allLine[0] = allLine[0].Trim();
            if (allLine[0].CompareTo(MsgMacro.CNPartVipLoginMsgTag) == 0)
            {
                Console.WriteLine(MsgMacro.CNPartVipLoginMsgTag);
                return checkVipLoginWeb(allLine[1]);
            }
            else if (allLine[0].CompareTo(MsgMacro.CNPartVipSelectMsgTag) == 0)
            {
                Console.WriteLine(MsgMacro.CNPartVipSelectMsgTag);
                return checkVipSelectWeb(allLine[1]);
            }
            else if (allLine[0].CompareTo(MsgMacro.CNPartVipModifyMsgTag) == 0)
            {
                Console.WriteLine(MsgMacro.CNPartVipModifyMsgTag);
                return checkVipModifyWeb(allLine[1]);
            }
            else if (allLine[0].CompareTo(MsgMacro.CNPartErpVipModifyMsgTag) == 0)
            {
                Console.WriteLine(MsgMacro.CNPartErpVipModifyMsgTag);
                return checkVipErpVipModify(allLine[1]);
            }
            else if (allLine[0].CompareTo(MsgMacro.CNPartAgentLoginMsgTag) == 0)
            {
                Console.WriteLine(MsgMacro.CNPartAgentLoginMsgTag);
                return checkAgentLoginWeb(allLine[1]);
            }
            else if (allLine[0].CompareTo(MsgMacro.CNPartAgentModifyMsgTag) == 0)
            {
                Console.WriteLine(MsgMacro.CNPartAgentModifyMsgTag);
                return checkAgentModifyWeb(allLine[1]);
            }

            else if (allLine[0].CompareTo(MsgMacro.CNPartAgentSelectMsgTag) == 0)
            {
                Console.WriteLine(MsgMacro.CNPartAgentSelectMsgTag);
                return checkAgentSelectWeb(allLine[1]);
            }

            else if (allLine[0].CompareTo(MsgMacro.CNPartVipRegisterMsgTag) == 0)
            {
                Console.WriteLine(MsgMacro.CNPartVipRegisterMsgTag);
                return checkVipRegisterWeb(allLine[1]);
            }
            else if (allLine[0].CompareTo(MsgMacro.CNPartVipUpdateEmailMsgTag) == 0)
            {
                Console.WriteLine(MsgMacro.CNPartVipUpdateEmailMsgTag);
                return checkVipUpdateEmailWeb(allLine[1]);
            }
            else
            {
                infoStr = "您输入的信息格式错误！error";
                checkMsg = new CheckMsg(false, 0, "", infoStr);
                Console.WriteLine(infoStr);
                return checkMsg;
            }
        }

        public CheckMsg checkVipLoginWeb(string str)
        {
            string infoStr = "";
            CheckMsg checkMsg = new CheckMsg(true, DataOperation.VIPLogin, "", infoStr);
            return checkMsg;

        }
        public CheckMsg checkVipSelectWeb(string str)
        {
            string infoStr = "";
            CheckMsg checkMsg = new CheckMsg(true, DataOperation.VIPSelect, "", infoStr);
            return checkMsg;
        }

        public CheckMsg checkVipModifyWeb(string str)
        {

            string infoStr = "";
            CheckMsg checkMsg = new CheckMsg(true, DataOperation.VIPMod, "", infoStr);
            return checkMsg;
        }
        public CheckMsg checkVipErpVipModify(string str)
        {
            string infoStr = "";
            CheckMsg checkMsg = new CheckMsg(true, DataOperation.VIPEdit, "", infoStr);
            return checkMsg;
        }

        public CheckMsg checkVipErpBatchModify(string str)
        {
            string infoStr = "";
            CheckMsg checkMsg = new CheckMsg(true, DataOperation.VIPBatchModify, "", infoStr);
            return checkMsg;
        }

        public CheckMsg checkAgentLoginWeb(string str)
        {
            string infoStr = "";
            CheckMsg checkMsg = new CheckMsg(true, DataOperation.AgentLogin, "", infoStr);
            return checkMsg;
        }
        public CheckMsg checkAgentModifyWeb(string str)
        {
            string infoStr = "";
            CheckMsg checkMsg = new CheckMsg(true, DataOperation.AgentMod, "", infoStr);
            return checkMsg;
        }

        public CheckMsg checkAgentSelectWeb(string str)
        {
            string infoStr = "";
            CheckMsg checkMsg = new CheckMsg(true, DataOperation.AgentSelect, "", infoStr);
            return checkMsg;
        }

        public CheckMsg checkVipRegisterWeb(string str)
        {
            string infoStr = "";
            CheckMsg checkMsg = new CheckMsg(true, DataOperation.VIPRegister, "", infoStr);
            return checkMsg;
        }
        public CheckMsg checkVipUpdateEmailWeb(string str)
        {
            string infoStr = "";
            CheckMsg checkMsg = new CheckMsg(true, DataOperation.VIPUpdateEmail, "", infoStr);
            return checkMsg;
        }

        #region "以前JAVA平台的代码，在本平台中没有使用"

        //public CheckMsg checkReworkVipCard(string str)
        //{
        //    Console.WriteLine("数据信息:\t" + str);

        //    if (FunctionDefine.ReworkVIPValid == false)
        //    {
        //        Console.WriteLine("vip信息修改功能暂未开通");
        //        return (new CheckMsg(false, DataOperation.Rework_VIP_CARD, "", "vip信息修改功能暂未开通"));
        //    }

        //    CheckMsg checkMsg = null;
        //    string infoStr = "";
        //    string vipID = ""; //vip卡号
        //    string vipPhone = ""; //电话
        //    string salesperson = ""; //营业员
        //    string name = ""; //姓名
        //    string sex = ""; //性别
        //    string paperNo = ""; //证件

        //    string type = ""; //vip卡类型
        //    string birthday = ""; //出生日期
        //    string address = ""; //联系地址
        //    string expireTime = ""; //到期时间
        //    string memo = ""; //备注

        //    string[] allLine = OperateString.getStringCollection(str, MsgMacro.thirdDivide);
        //    Console.WriteLine("-----allLine长度------:" + allLine.Length);
        //    if (allLine.Length < 2)
        //    {
        //        return (new CheckMsg(false, DataOperation.Rework_VIP_CARD, "", "VIP数据不正确"));
        //    }
        //    else
        //    {
        //        if (allLine.Length < 11)
        //        {
        //            return (new CheckMsg(false, DataOperation.Rework_VIP_CARD, "", "VIP数据不正确"));
        //        }

        //        vipID = allLine[1]; //vip卡号
        //        vipPhone = allLine[5]; //电话
        //        salesperson = allLine[0]; //营业员
        //        name = allLine[3]; //姓名
        //        sex = allLine[4]; //性别
        //        paperNo = allLine[6]; //证件

        //        type = allLine[2]; //vip卡类型
        //        birthday = allLine[7]; //出生日期
        //        address = allLine[8]; //联系地址
        //        expireTime = allLine[9]; //到期时间
        //        memo = allLine[10]; //备注
        //    }

        //    if (birthday.Equals("") || birthday == null)
        //    {

        //    }
        //    else
        //    {
        //        string checkBirthday = birthday.Trim();
        //        if (birthday.Length > 0 && checkBirthday.Length == 0)
        //        { //有空格表示改字段修改为空
        //            birthday = "  ";
        //        }
        //        else
        //        {
        //            birthday = birthday.Trim();
        //            if (CheckFunction.isValidDate(birthday))
        //            {
        //                birthday = birthday.Substring(0, 4) + "-" + birthday.Substring(4, 6) + "-"
        //                    + birthday.Substring(6, 8);
        //            }
        //            else
        //            {
        //                return (new CheckMsg(false, DataOperation.Rework_VIP_CARD, "", "出生日期格式不正确"));
        //            }
        //        }


        //    }
        //    checkMsg = new CheckMsg(true, DataOperation.Rework_VIP_CARD, "", infoStr);
        //    return checkMsg;
        //}




        // public static bool isValidDate(string sDate)
        // {
        //     string datePattern1 = "\\d{4}\\d{2}\\d{2}";
        //     string datePattern2 = "^((\\d{2}(([02468][048])|([13579][26]))"
        //         +
        //         "[\\-\\/\\s]?((((0?[13578])|(1[02]))[\\-\\/\\s]?((0?[1-9])|([1-2][0-9])|"
        //         + "(3[01])))|(((0?[469])|(11))[\\-\\/\\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\\-\\/\\s]?"
        //         + "((0?[1-9])|([1-2][0-9])))))|(\\d{2}(([02468][1235679])|([13579][01345789]))[\\-\\/\\s]?("
        //         + "(((0?[13578])|(1[02]))[\\-\\/\\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\\-\\/\\s]?"
        //         + "((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\\-\\/\\s]?((0?[1-9])|(1[0-9])|(2[0-8]))))))";
        //     if ((sDate != null))
        //     {
        //         Regex pattern = new Regex(datePattern1);
        //         if (pattern.IsMatch(sDate))
        //         {
        //             pattern = new Regex(datePattern2);
        //             return pattern.IsMatch(sDate);
        //         }
        //         else
        //         {
        //             return false;
        //         }
        //     }
        //     return false;
        // }
        #endregion
    }
}

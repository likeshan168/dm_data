using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication3
{
    class MsgMacro
    {
        public static string mainDivide1 = ":";
        public static string mainDivide2 = "：";

        public static char Split1 = (char)21;
        public static char Split2 = (char)22;
        public static char Split3 = (char)23;

        /// <summary>
        /// 销售
        /// </summary>
        public static string CNSaleMsgTag = "销售";
        /// <summary>
        /// 操作员
        /// </summary>
        public  static string CNSalerMsgTag = "操作员";

        /// <summary>
        /// VIP登陆验证
        /// </summary>
        public static string CNPartVipLoginMsgTag = "VIP登陆验证";
        /// <summary>
        /// VIP查询
        /// </summary>
        public static string CNPartVipSelectMsgTag = "VIP查询";
        /// <summary>
        /// VIP资料修改
        /// </summary>
        public static string CNPartVipModifyMsgTag = "VIP资料修改";
        /// <summary>
        /// VIP批量修改
        /// </summary>
        public static string CNPartVipBatchModifyMsgTag = "VIP批量修改";
        /// <summary>
        /// VIP注册请求
        /// </summary>
        public static string CNPartVipRegisterMsgTag = "VIP注册请求";
        /// <summary>
        /// VIP修改电子邮箱请求
        /// </summary>
        public static string CNPartVipUpdateEmailMsgTag = "VIP修改电子邮箱请求";

        /// <summary>
        /// Agent登陆验证
        /// </summary>
        public static string CNPartAgentLoginMsgTag = "Agent登陆验证";
        /// <summary>
        /// Agent资料修改
        /// </summary>
        public static string CNPartAgentModifyMsgTag = "Agent资料修改";
        /// <summary>
        /// Agent查询订阅
        /// </summary>
        public static string CNPartAgentSelectMsgTag = "Agent查询订阅";

        /// <summary>
        /// ERPVIP资料修改
        /// </summary>
        public static string CNPartErpVipModifyMsgTag = "ERPVIP资料修改";
        /// <summary>
        /// VIP资料全部下载
        /// </summary>
        public static string CNPartVipInformationAllMsgTag = "VIP资料全部下载";
        /// <summary>
        /// 营业员资料全部下载
        /// </summary>
        public static string CNPartSalesAllMsgTag = "营业员资料全部下载";

        //public static string CNPartVipUpdateMsgTag = "增量更新VIP资料";
        //public static string CNSaleQueryMsgTag = "查销售";
        //public static string CNMODIFYVIPCARDMS = "VIP资料修改";
        //public static string CNAllVIPUpdateNetweb = "VIP资料更新NET";
        //public static string CNAllVipUpdateMsgTag = "更新全部VIP资料";

        //public static string CNCheckOutMsgTag = "结帐";
        //public static string ENCheckOutMsgTag = "JIEZHANG";
        //public static string CNStockAll = "更新全部本地库存";
        //public static string ENStockAll = "BENDIKUCUN";
        //public static string CNReworkVipCardMsgTag = "VIP信息修改";

        public  static string thirdDivide = OperateString.getAsciiString(OperateString.spcialStarChar);
    }
}

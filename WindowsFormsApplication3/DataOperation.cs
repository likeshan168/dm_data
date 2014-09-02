using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication3
{
    class DataOperation
    {
        public static int UN_KNOW = 0;
        public static int ALL_NEW_VIP_UPDATE_NEWWEB = 1;
        public static int VIP_MSG_UPDATE_NETWEB = 2;


        //web
        public static byte VIPLogin = 0x20;
        public static byte VIPSelect = 0x21;
        public static byte VIPMod = 0x22;
        public static byte VIPRegister = 0x23;
        public static byte VIPRegTempStor = 0x24;
        public static byte VIPUpdateEmail = 0x25;
        public static byte VIPGivePoint = 0x26;

        public static byte AgentLogin = 0x40;
        public static byte AgentMod = 0x41;
        public static byte AgentSelect = 0x42;



        /// <summary>
        /// 新增  （口袋通vip信息下载）
        /// author：李克善
        /// 日期：2014-08-18
        /// 用途：获取口袋通的vip信息
        /// </summary>
        public static byte VIP_KDT_Downland = 0x28;

        /// <summary>
        /// 新增  （口袋通vip 销售信息下载）
        /// author：李克善
        /// 日期：2014-08-18
        /// 用途：获取口袋通的vip销售信息
        /// </summary>
        public static byte VIP_KDT_SALL_Downland = 0x29;


        //LAN
        /// <summary>
        /// VIP基本资料下载 0x30
        /// </summary>
        public static byte VIPDownland = 0x30;//VIP基本资料下载
        /// <summary>
        /// 0x31
        /// </summary>
        public static byte VIPConfig = 0x31;
        public static byte VIPEdit = 0x32;
        public static byte VIPTimeing = 0x33;//VIP基本资料定时下载
        public static byte VIPZL = 0x43; //VIP增量资料下载
        public static byte VIPBatchModify = 0x34; //VIP基本资料批量修改

        #region 新增的,vip制卡信息定时下载 0x60
        /// <summary>
        /// vip制卡信息定时下载 0x60
        /// </summary>
        public static byte VIPCardTiming = 0x60;
        /// <summary>
        /// vip制卡信息手动下载 0x61
        /// </summary>
        public static byte VIPCardDownload = 0x61;
        #endregion

        /// <summary>
        /// VIP基本资料导入 0x35
        /// </summary>
        public static byte VIPUpLoad = 0x35;//VIP基本资料导入
        
        /// <summary>
        /// 营业员资料下载0x36
        /// </summary>
        public static byte VIPSalesDown = 0x36;//营业员资料下载
        /// <summary>
        /// 门店资料下载 0x38
        /// </summary>
        public static byte VIPShopsDown = 0x38;//门店资料下载
        /// <summary>
        /// 营业员资料定时下载
        /// </summary>
        public static byte VIPSalesTimingDown = 0x37;//营业员资料定时下载
        /// <summary>
        /// 门店资料定时下载
        /// </summary>
        public static byte VIPShopsTimingDown = 0x39;//门店资料定时下载
        /// <summary>
        /// 销售信息下载
        /// </summary>
        public static byte SalesInfo = 0x44; //销售信息下载
        /// <summary>
        /// 商品资料下载
        /// </summary>
        public static byte ProductInfo = 0x45; //商品资料下载
        /// <summary>
        /// vip删除增量
        /// </summary>
        public static byte VIPDelete = 0x46; //vip删除增量

        public static byte SUCCESS = 1;
        public static byte FAILURE = 2;

    }
}

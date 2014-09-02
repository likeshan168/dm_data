using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SqlClient;

namespace WindowsFormsApplication3
{
    class DelReciveSendMsg
    {
        public CommonMsg commonMsg;
        //private bool isWrite = false;
        WebVipOperator vipOperator = new WebVipOperator();
        public DelReciveSendMsg(CommonMsg commonMsg)
        {
            this.commonMsg = commonMsg;
        }

        public void dealMsg()
        {
            try
            {

                #region  0x30 VIP基本资料下载dm网站手动下载
                //VIP基本资料手动从网页下载，将ERP返回结果存到TempReturn表，等待网页取数据
                if (commonMsg.Data[0] == DataOperation.VIPDownland)
                {
                    Console.WriteLine("进入VIP全部资料下载(数据请求上传到服务端)===========VIPDownland(0x30)");

                    byte[] comData = null;

                    if (commonMsg.Data[1] == 1)
                    {
                        byte[] tmpComMsg = new byte[commonMsg.Data.Length - 2];
                        Array.ConstrainedCopy(commonMsg.Data, 2, tmpComMsg, 0, commonMsg.Data.Length - 2);
                        comData = ZipCompress.DeCompress(tmpComMsg);
                    }
                    else
                    {
                        comData = commonMsg.Data;
                    }

                    if (commonMsg.size == commonMsg.Data.Length)
                    {
                        ClientData mdata = new ClientData();
                        if (mdata.InsertTempReturn(commonMsg.sessionId, comData))
                        {
                            if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id))
                            {
                                File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id);
                                bool isSucceed = true;
                                Console.WriteLine("222222" + isSucceed);
                                Console.WriteLine("手动下载vip基本资料的已经成功发送到服务器上");
                            }
                        }
                    }
                }
                #endregion

                #region  0x33 VIP基本资料定时下载
                if (commonMsg.Data[0] == DataOperation.VIPTimeing)
                {
                    byte[] comData = null;
                    Console.WriteLine("进入VIP定时全部资料下载(接受服务端的数据)===========VIPTimeing(0x33)");

                    //VIP基本资料定时下载                  
                    if (commonMsg.Data[1] == 1)
                    {
                        Console.WriteLine("对接收的数据进行解压缩==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        byte[] tmpComMsg = new byte[commonMsg.Data.Length - 2];
                        Array.ConstrainedCopy(commonMsg.Data, 2, tmpComMsg, 0, commonMsg.Data.Length - 2);
                        comData = ZipCompress.DeCompress(tmpComMsg);
                    }
                    else
                    {
                        Console.WriteLine("接收到的数据没有进行压缩==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        comData = commonMsg.Data;
                    }

                    if (commonMsg.size == commonMsg.Data.Length)
                    {

                        if (vipOperator.TimingVipDownload(comData))//定时下载
                        {

                            if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id))
                            {
                                File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id);
                                bool isSucceed = true;
                                Console.WriteLine("222222" + isSucceed);
                                Console.WriteLine("定时下载vip信息成功==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            }
                        }
                        else
                        {
                            ErrInfo.WriterErrInfo("DelReciveSendMsg", "dealMsg", "没有DataInfo表的数据，无法获得CardId.自动更新失败！请先用网站手动更新VIP数据!");
                            Console.WriteLine("定时下载vip基本资料失败，请用网站手动更新VIP数据!");
                            if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id))
                            {
                                File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id);
                                bool isSucceed = true;
                                Console.WriteLine("222222" + isSucceed);
                            }
                        }
                    }

                }
                #endregion

                //新增的是用于定时下载vip制卡信息操作 0x60
                #region 新增的是用于定时下载vip制卡信息操作 0x60
                #region  0x60 VIP制卡信息基本资料定时下载
                if (commonMsg.Data[0] == DataOperation.VIPCardTiming) //0x60
                {
                    byte[] comData = null;
                    Console.WriteLine("进入VIP制卡信息定时全部资料下载(接受服务端数据)===========VIPCardTiming(0x60)");

                    //VIP制卡基本资料定时下载                  
                    if (commonMsg.Data[1] == 1)
                    {
                        Console.WriteLine("对接收的数据进行解压缩==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        byte[] tmpComMsg = new byte[commonMsg.Data.Length - 2];
                        Array.ConstrainedCopy(commonMsg.Data, 2, tmpComMsg, 0, commonMsg.Data.Length - 2);
                        comData = ZipCompress.DeCompress(tmpComMsg);
                    }
                    else
                    {
                        Console.WriteLine("接收到的数据没有进行压缩==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        comData = commonMsg.Data;
                    }

                    if (commonMsg.size == commonMsg.Data.Length)
                    {

                        if (vipOperator.TimingVipCardDownload(comData))
                        {

                            if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id))
                            {
                                File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id);
                                bool isSucceed = true;
                                Console.WriteLine("222222" + isSucceed);
                                Console.WriteLine("定时下载vip制卡信息失败==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            }
                        }
                        else
                        {
                            ErrInfo.WriterErrInfo("DelReciveSendMsg", "dealMsg", "没有CardDataInfo表的数据，无法获得CardId.自动更新失败！请用网站手动更新VIP制卡信息数据!");
                            Console.WriteLine("定时下载vip制卡信息失败，请用网站手动更新VIP制卡信息数据!");
                            if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id))
                            {
                                File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id);
                                bool isSucceed = true;
                                Console.WriteLine("222222" + isSucceed);
                            }
                        }
                    }

                }
                #endregion
                #endregion

                //新增的，是用于dm网站手动下载vip制卡信息

                #region 新增的，是用于dm网站手动下载vip制卡信息
                #region  0x61 是用于dm网站手动下载vip制卡信息
                //VIP制卡基本资料手动从网页下载，将ERP返回结果存到TempReturn表，等待网页取数据
                if (commonMsg.Data[0] == DataOperation.VIPCardDownload)
                {
                    Console.WriteLine("进入VIP制卡信息全部资料手动下载(请求数据上传到服务器)===========VIPCardDownload");

                    byte[] comData = null;

                    if (commonMsg.Data[1] == 1)
                    {
                        Console.WriteLine("数据进行了压缩，需要解压缩==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        byte[] tmpComMsg = new byte[commonMsg.Data.Length - 2];
                        Array.ConstrainedCopy(commonMsg.Data, 2, tmpComMsg, 0, commonMsg.Data.Length - 2);
                        comData = ZipCompress.DeCompress(tmpComMsg);
                    }
                    else
                    {
                        Console.WriteLine("数据没有进行压缩==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        comData = commonMsg.Data;
                    }

                    if (commonMsg.size == commonMsg.Data.Length)
                    {
                        ClientData mdata = new ClientData();
                        if (mdata.InsertTempReturn(commonMsg.sessionId, comData))
                        {
                            if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id))
                            {
                                File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id);
                                bool isSucceed = true;
                                Console.WriteLine("222222" + isSucceed);
                                Console.WriteLine("请求的手动下在vip制卡信息的请求已经成功上传到服务器上");
                            }
                        }
                    }
                }
                #endregion
                #endregion

                #region  VIPZL = 0x43; //VIP增量资料下载 这和vip定时下载调用相同的处理方法TimingVipDownload
                if (commonMsg.Data[0] == DataOperation.VIPZL)
                {
                    byte[] comData = null;
                    Console.WriteLine("进入VIP资料增量下载(接受服务端数据)===========VIPZL");


                    if (commonMsg.Data[1] == 1)
                    {
                        Console.WriteLine("数据进行了压缩，需要解压缩==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        byte[] tmpComMsg = new byte[commonMsg.Data.Length - 2];
                        Array.ConstrainedCopy(commonMsg.Data, 2, tmpComMsg, 0, commonMsg.Data.Length - 2);
                        comData = ZipCompress.DeCompress(tmpComMsg);
                    }
                    else
                    {
                        Console.WriteLine("数据没有进行压缩==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        comData = commonMsg.Data;
                    }

                    if (commonMsg.size == commonMsg.Data.Length)
                    {
                        if (vipOperator.TimingVipDownload(comData))
                        {

                            if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id))
                            {
                                File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id);
                                bool isSucceed = true;
                                Console.WriteLine("222222" + isSucceed);
                                Console.WriteLine("接受vip增量下载成功==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:dd"));
                            }
                        }
                        else
                        {
                            ErrInfo.WriterErrInfo("DelReciveSendMsg", "VIPZL", "增量下载处理失败");
                            Console.WriteLine("接受vip增量下载失败==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:dd"));
                            if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id))
                            {
                                File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id);
                                bool isSucceed = true;
                                Console.WriteLine("222222" + isSucceed);
                            }
                        }
                    }

                }
                #endregion

                #region  SalesInfo = 0x44; //销售信息下载(提供dm网站vip销售分析用的)
                if (commonMsg.Data[0] == DataOperation.SalesInfo)
                {
                    byte[] comData = null;
                    Console.WriteLine("进入销售数据下载(接受服务端的数据)===========SalesInfo");
                    if (commonMsg.Data[1] == 1)
                    {
                        Console.WriteLine("数据进行了压缩，需要解压缩==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        byte[] tmpComMsg = new byte[commonMsg.Data.Length - 2];
                        Array.ConstrainedCopy(commonMsg.Data, 2, tmpComMsg, 0, commonMsg.Data.Length - 2);
                        comData = ZipCompress.DeCompress(tmpComMsg);
                    }
                    else
                    {
                        Console.WriteLine("数据没有进行压缩==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        comData = commonMsg.Data;
                    }

                    if (commonMsg.size == commonMsg.Data.Length)
                    {

                        if (vipOperator.TimeingSaleInfoDown(comData))
                        {

                            if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id))
                            {
                                File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id);
                                bool isSucceed = true;
                                Console.WriteLine("222222" + isSucceed);
                                Console.WriteLine("接受销售数据成功==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            }
                        }
                        else
                        {
                            ErrInfo.WriterErrInfo("DelReciveSendMsg", "SalesInfo", "下载销售数据处理失败");
                            Console.WriteLine("接受销售数据失败==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id))
                            {
                                File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id);
                                bool isSucceed = true;
                                Console.WriteLine("222222" + isSucceed);
                            }
                        }
                    }

                }
                #endregion

                #region  ProductInfo = 0x45; //商品定时资料下载
                if (commonMsg.Data[0] == DataOperation.ProductInfo)
                {
                    byte[] comData = null;
                    Console.WriteLine("进入商品资料下载(接受服务端的数据)===========ProductInfo(0x45)");


                    if (commonMsg.Data[1] == 1)
                    {
                        Console.WriteLine("数据进行了压缩，需要解压缩==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        byte[] tmpComMsg = new byte[commonMsg.Data.Length - 2];
                        Array.ConstrainedCopy(commonMsg.Data, 2, tmpComMsg, 0, commonMsg.Data.Length - 2);
                        comData = ZipCompress.DeCompress(tmpComMsg);
                    }
                    else
                    {
                        Console.WriteLine("数据没有进行压缩==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        comData = commonMsg.Data;
                    }

                    if (commonMsg.size == commonMsg.Data.Length)
                    {

                        if (vipOperator.TimeingProductInfoDown(comData))
                        {

                            if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id))
                            {
                                File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id);
                                bool isSucceed = true;
                                Console.WriteLine("222222" + isSucceed);
                                Console.WriteLine("接受商品资料成功==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            }
                        }
                        else
                        {
                            ErrInfo.WriterErrInfo("DelReciveSendMsg", "ProductInfo", "下载商品资料处理失败");
                            Console.WriteLine("接受商品资料失败==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id))
                            {
                                File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id);
                                bool isSucceed = true;
                                Console.WriteLine("222222" + isSucceed);
                            }
                        }
                    }

                }
                #endregion

                #region
                //if (commonMsg.Data[0] == DataOperation.VIPConfig)
                //{
                //    Console.WriteLine("进入VIP配置文件更新结果处理===========VIPConfig");

                //    if (commonMsg.size == commonMsg.Data.Length)
                //    {
                //        ClientData mdata = new ClientData();
                //        //上传VIP配置
                //        if (mdata.DeleteTempStor(commonMsg.sessionId))
                //        {
                //            if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id))
                //            {
                //                File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id);
                //                bool isSucceed = true;
                //                Console.WriteLine("222222" + isSucceed);
                //            }
                //        }

                //    }


                //}
                #endregion

                #region  VIPBatchModify = 0x34; //VIP基本资料批量修改
                if (commonMsg.Data[0] == DataOperation.VIPBatchModify)
                {
                    Console.WriteLine("进入VIP批量更新(将vip基本资料批量修改的请求发送服务器)===========VIPBatchModify(0x34)");

                    if (commonMsg.size == commonMsg.Data.Length)
                    {
                        ClientData mdata = new ClientData();
                        //erp返回VIP批量更新处理结果，删除TempStor表中网页申请纪录
                        if (mdata.DeleteTempStor(commonMsg.sessionId))
                        {
                            if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id))
                            {
                                File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id);
                                bool isSucceed = true;
                                Console.WriteLine("VIPBatchModify" + isSucceed);
                                Console.WriteLine("vip批量更新请求发送到服务器成功==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            }
                        }

                    }

                }//
                #endregion


                #region  VIPUpLoad = 0x35;//VIP基本资料导入
                if (commonMsg.Data[0] == DataOperation.VIPUpLoad)
                {
                    Console.WriteLine("进入VIP资料批量上传(上传到服务器上)===========VIPUpLoad(0x35)");

                    if (commonMsg.size == commonMsg.Data.Length)
                    {
                        ClientData mdata = new ClientData();

                        Stream byteArrayInputStream = new MemoryStream(commonMsg.Data);

                        byteArrayInputStream.ReadByte();

                        byte[] timeArray = new byte[4];
                        byteArrayInputStream.Read(timeArray, 0, 4);
                        int lastUpdateTime = ByteConvert.byteArrayToInt(timeArray);


                        int k = (int)byteArrayInputStream.ReadByte();

                        byte[] ByteMsg = null;

                        if (k == 2)
                        {
                            Console.WriteLine("Vip资料批量上传失败==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            //erp返回VIP资料批量上传处理结果失败
                            byteArrayInputStream.ReadByte();
                            ByteMsg = new byte[byteArrayInputStream.Length];
                            byteArrayInputStream.Read(ByteMsg, 0, ByteMsg.Length);
                            string msg = Encoding.Default.GetString(ByteMsg);
                            //修改标志，重新上传
                            mdata.ChangeCardInfoFlag(DataOperation.VIPUpLoad, 1, commonMsg.sessionId);

                        }
                        else if (k == 1)
                        {
                            Console.WriteLine("Vip资料批量上传成功==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            //erp返回VIP资料批量上传处理结果成功
                            byteArrayInputStream.ReadByte();
                            mdata.ChangeCardInfoFlag(DataOperation.VIPUpLoad, 0, commonMsg.sessionId);
                        }

                        //vipOperator.TimingSendCoupon();

                        //删除TempStor表数据
                        if (mdata.DeleteTempStor(commonMsg.sessionId))
                        {
                            if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id))
                            {
                                File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id);
                                bool isSucceed = true;
                                Console.WriteLine("VIPUpLoad" + isSucceed);
                                Console.WriteLine("表TempStor中的处理过的请求已经删除掉了");
                            }
                        }

                    }

                }
                #endregion

                #region  VIPSalesDown = 0x36;//营业员资料下载
                if (commonMsg.Data[0] == DataOperation.VIPSalesDown)
                {
                    //营业员全部资料网页手动下载ERP传回结果，存入TempReturn表中，等待网页获取
                    Console.WriteLine("进入Sales全部资料下载(在网页使用手动下载)===========VIPSalesDown");

                    byte[] comData = null;

                    if (commonMsg.Data[1] == 1)
                    {
                        Console.WriteLine("数据进行了压缩，需要解压==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        byte[] tmpComMsg = new byte[commonMsg.Data.Length - 2];
                        Array.ConstrainedCopy(commonMsg.Data, 2, tmpComMsg, 0, commonMsg.Data.Length - 2);
                        comData = ZipCompress.DeCompress(tmpComMsg);
                    }
                    else
                    {
                        Console.WriteLine("数据没有进行压缩==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        comData = commonMsg.Data;
                    }

                    if (commonMsg.size == commonMsg.Data.Length)
                    {
                        ClientData mdata = new ClientData();
                        //将返回的数据存入TempReturn表
                        if (mdata.InsertTempReturn(commonMsg.sessionId, comData))
                        {
                            if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id))
                            {
                                File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id);
                                bool isSucceed = true;
                                Console.WriteLine("222222" + isSucceed);
                                Console.WriteLine("手动下载营业基本资料成功==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            }
                        }
                    }
                }

                #endregion

                #region VIPSalesTimingDown = 0x37;//营业员资料定时下载
                if (commonMsg.Data[0] == DataOperation.VIPSalesTimingDown)//0x37
                {
                    //营业员定时下载
                    Console.WriteLine("进入Sales全部资料定时下载(接受服务器数据)===========VIPSalesTimingDown(0x37)");

                    byte[] comData = null;

                    if (commonMsg.Data[1] == 1)
                    {
                        Console.WriteLine("数据进行了压缩,需要解压缩==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        byte[] tmpComMsg = new byte[commonMsg.Data.Length - 2];
                        Array.ConstrainedCopy(commonMsg.Data, 2, tmpComMsg, 0, commonMsg.Data.Length - 2);
                        comData = ZipCompress.DeCompress(tmpComMsg);
                    }
                    else
                    {
                        Console.WriteLine("数据没有进行压缩==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        comData = commonMsg.Data;
                    }

                    if (commonMsg.size == commonMsg.Data.Length)
                    {
                        //处理定时下载的主要方法。
                        if (vipOperator.TimingSalesDownload(comData))
                        {
                            if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id))
                            {
                                File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id);
                                bool isSucceed = true;
                                Console.WriteLine("222222" + isSucceed);
                                Console.WriteLine("定时下载营业员基本资料成功==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            }
                        }
                        else
                        {
                            ErrInfo.WriterErrInfo("DelReciveSendMsg", "dealMsg", "VIPSalesTimingDown定时更新失败");
                            Console.WriteLine("定时下载营业员基本资料失败==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id))
                            {
                                File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id);
                                bool isSucceed = true;
                                Console.WriteLine("222222" + isSucceed);
                            }
                        }
                    }
                }
                #endregion

                #region VIPShopsDown = 0x38;//门店资料下载（都只能是通过dm网站进行手动进行下载的）

                if (commonMsg.Data[0] == DataOperation.VIPShopsDown)
                {
                    //门店全部资料网页手动下载ERP传回结果，存入TempReturn表中，等待网页获取
                    Console.WriteLine("进入Shop全部资料下载(在网站上通过手动下载门店资料)===========VIPShopsDown(0x38)");

                    byte[] comData = null;

                    if (commonMsg.Data[1] == 1)
                    {
                        Console.WriteLine("数据进行了压缩，需要进行解压缩==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        byte[] tmpComMsg = new byte[commonMsg.Data.Length - 2];
                        Array.ConstrainedCopy(commonMsg.Data, 2, tmpComMsg, 0, commonMsg.Data.Length - 2);
                        comData = ZipCompress.DeCompress(tmpComMsg);
                    }
                    else
                    {
                        Console.WriteLine("数据没有进行压缩==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        comData = commonMsg.Data;
                    }


                    if (commonMsg.size == commonMsg.Data.Length)
                    {
                        ClientData mdata = new ClientData();
                        if (mdata.InsertTempReturn(commonMsg.sessionId, comData))
                        {
                            if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id))
                            {
                                File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id);
                                bool isSucceed = true;
                                Console.WriteLine("222222" + isSucceed);
                                Console.WriteLine("手动下载门店数据成功==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            }
                        }
                    }
                }
                #endregion

                #region VIPShopsTimingDown = 0x39;//门店资料定时下载
                if (commonMsg.Data[0] == DataOperation.VIPShopsTimingDown)
                {
                    Console.WriteLine("进入Shop资料定时下载(接受服务器的数据)===========VIPShopsTimingDown(0x39)");

                    //门店定时下载
                    byte[] comData = null;

                    if (commonMsg.Data[1] == 1)
                    {
                        Console.WriteLine("数据进行了压缩，需要进行解压缩==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        byte[] tmpComMsg = new byte[commonMsg.Data.Length - 2];
                        Array.ConstrainedCopy(commonMsg.Data, 2, tmpComMsg, 0, commonMsg.Data.Length - 2);
                        comData = ZipCompress.DeCompress(tmpComMsg);
                    }
                    else
                    {
                        Console.WriteLine("数据没有进行压缩==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        comData = commonMsg.Data;
                    }

                    if (commonMsg.size == commonMsg.Data.Length)
                    {

                        if (vipOperator.TimingShopsDownLoad(comData))
                        {
                            if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id))
                            {
                                File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id);
                                bool isSucceed = true;
                                Console.WriteLine("222222" + isSucceed);
                                Console.WriteLine("定时下载门店基本资料成功==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            }
                        }
                        else
                        {
                            ErrInfo.WriterErrInfo("DelReciveSendMsg", "dealMsg", "VIPShopsTimingDown定时更新失败");
                            Console.WriteLine("定时下载门店基本资料失败==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id))
                            {
                                File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id);
                                bool isSucceed = true;
                                Console.WriteLine("222222" + isSucceed);
                            }
                        }
                    }
                }

                #endregion

                #region  VIPDelete = 0x46; //vip删除增量
                //------------------新增处理vip删除增量方法---------------------------
                if (commonMsg.Data[0] == DataOperation.VIPDelete)
                {
                    Console.WriteLine("进入vip删除增量下载(接受服务器的数据)===========VIPDelete(0x46)");

                    //门店定时下载
                    byte[] comData = null;

                    if (commonMsg.Data[1] == 1)
                    {
                        Console.WriteLine("数据进行了压缩，需要解压缩==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        byte[] tmpComMsg = new byte[commonMsg.Data.Length - 2];
                        Array.ConstrainedCopy(commonMsg.Data, 2, tmpComMsg, 0, commonMsg.Data.Length - 2);
                        comData = ZipCompress.DeCompress(tmpComMsg);
                    }
                    else
                    {
                        Console.WriteLine("数据没有进行压缩==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        comData = commonMsg.Data;
                    }

                    if (commonMsg.size == commonMsg.Data.Length)
                    {

                        if (vipOperator.TimingVipDelete(comData))
                        {
                            if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id))
                            {
                                File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id);
                                bool isSucceed = true;
                                Console.WriteLine("222222" + isSucceed);
                                Console.WriteLine("vip删除增量下载成功==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            }
                        }
                        else
                        {
                            ErrInfo.WriterErrInfo("DelReciveSendMsg", "dealMsg", "VIPDelete定时更新失败");
                            Console.WriteLine("vip删除增量下载失败==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id))
                            {
                                File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id);
                                bool isSucceed = true;
                                Console.WriteLine("222222" + isSucceed);
                            }
                        }
                    }
                }

                #endregion

                #region  VIPRegTempStor = 0x24;
                if (commonMsg.Data[0] == DataOperation.VIPRegTempStor)
                {
                    Console.WriteLine("进入VIP注册资料批量上传(将资料上传到服务器上)===========VIPRegTempStor(0x24)");

                    if (commonMsg.size == commonMsg.Data.Length)
                    {
                        ClientData mdata = new ClientData();

                        Stream byteArrayInputStream = new MemoryStream(commonMsg.Data);

                        byteArrayInputStream.ReadByte();

                        byte[] timeArray = new byte[4];
                        byteArrayInputStream.Read(timeArray, 0, 4);
                        int lastUpdateTime = ByteConvert.byteArrayToInt(timeArray);


                        int k = (int)byteArrayInputStream.ReadByte();

                        byte[] ByteMsg = null;
                        if (k == 2)
                        {
                            byteArrayInputStream.ReadByte();
                            ByteMsg = new byte[byteArrayInputStream.Length];
                            byteArrayInputStream.Read(ByteMsg, 0, ByteMsg.Length);
                            string msg = Encoding.Default.GetString(ByteMsg);
                            if (!(msg.IndexOf("已存在") >= 0))
                            {
                                mdata.ChangeCardInfoFlag(DataOperation.VIPRegTempStor, 3, commonMsg.sessionId);
                                //2009-05-06 增加以下删除tempstor内容
                                mdata.DeleteTempStor(commonMsg.sessionId);
                            }

                        }
                        else if (k == 1)
                        {
                            byteArrayInputStream.ReadByte();
                            mdata.ChangeCardInfoFlag(DataOperation.VIPRegTempStor, 0, commonMsg.sessionId);
                        }

                        if (mdata.DeleteTempStor(commonMsg.sessionId))
                        {
                            if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id))
                            {
                                File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id);
                                bool isSucceed = true;
                                Console.WriteLine("VIPUpLoad" + isSucceed);
                                Console.WriteLine("vip注册资料批量上传成功==={0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            }
                        }

                    }

                }

                #endregion

                else
                {
                    if (File.Exists("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id))
                    {
                        File.Delete("C:\\DMService\\" + ClientApp.localBase + "\\TempFileRS\\" + commonMsg.id);
                        bool isSucceed = true;
                        Console.WriteLine("222222" + isSucceed);
                    }

                }

                //vipOperator.TimingSendCoupon();
            }
            catch (Exception err)
            {
                //Console.WriteLine(err.Message.ToString());
                ErrInfo.WriterErrInfo("DelReciveSendMsg", "dealMsg", err);
                Console.WriteLine("类DelReciveSendMsg方法dealMsg出现异常==={0}", err.Message);
            }


        }






    }
}

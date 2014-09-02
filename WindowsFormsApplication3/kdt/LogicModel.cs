using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
//using System.Data.Linq;
using Newtonsoft.Json.Linq;
namespace WindowsFormsApplication3
{
    public class LogicModel
    {
        private KDTApiKit kit = null;
        private Dictionary<String, String> param = null;
        Dictionary<string, object> postParameters = null;

        private string result = string.Empty;
        public LogicModel()
        {
            kit = new KDTApiKit("a698727f1934c56c4a", "6ac1c2d1bf5c6ce40bee0f34fe6ad3a6");
            //param = new Dictionary<String, String>();//参数
            //postParameters = new Dictionary<string, object>();//文件参数
        }
        /// <summary>
        /// 获取所有的口袋通中的vip信息
        /// </summary>
        /// <returns></returns>
        public string GetVipKdtInfo(int page_no, int page_size)
        {
            param = new Dictionary<String, String>();//参数
            param.Add("page_no", page_no.ToString());
            param.Add("page_size", page_size.ToString());


            return kit.get(MethodNameModel.KDT_USERS_WEIXIN_FOLLOWERS_GET, param);

        }


        /// <summary>
        /// 获取所有的口袋通中的vip销售信息
        /// </summary>
        /// <returns></returns>
        public string GetVipSaleKdtInfo(string weixin_user_id, int page_no, int page_size)
        {
            param = new Dictionary<String, String>();//参数
            if (!string.IsNullOrEmpty(weixin_user_id))
            {
                param.Add("weixin_user_id", weixin_user_id);
            }

            param.Add("page_no", page_no.ToString());
            param.Add("page_size", page_size.ToString());
            return kit.get(MethodNameModel.KDT_TRADES_SOLD_GET, param);

        }
        /// <summary>
        /// 根据微信粉丝用户的openid获取用户信息
        /// </summary>
        /// <returns></returns>
        public string GetWeiXinUserInfo(string user_id)
        {
            try
            {
                param = new Dictionary<String, String>();//参数
                param.Add("user_id", user_id.ToString());

                string result = kit.get(MethodNameModel.KDT_USERS_WEIXIN_FOLLOWER_GET, param);
                JObject jo = JObject.Parse(result);
                return jo["response"]["user"]["weixin_openid"].ToString();

            }
            catch (Exception)
            {

                return string.Empty;
            }



        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication3
{
    public class CommandCode
    {
        public static int UnionFlag = 1;

        public static int CLIENTFRAME_CLIENT_MSG_SELECT = 10010; 

        
        public static int TEST_SOCKET = 10000;//10000; //测试连接

      /// <summary>
        /// 100145 删除 RECIVEDMSG_CLIENT_MSG中的数据
      /// </summary>
        public static int DEALRECIVEDMSG_CLIENT_MSG_DELETE = 100145;
        /// <summary>
        /// 100146 删除 RECIVEDMSG_SEND_MSG中的数据
        /// </summary>
        public static int DEALRECIVEDMSG_SEND_MSG_DELETE = 100146;

        public static int LOGON_COMMAND = 10001; //登陆代码

        /// <summary>
        /// DM平台读ClientMsg 200201
        /// </summary>
        public static int GetClientCode = 200201;  //DM平台读ClientMsg
        /// <summary>
        /// DM平台写ClientMsg200202
        /// </summary>
        public static int FROM_DM_WRITE_CLIENTMSG=200202; //DM平台写ClientMsg
        /// <summary>
        /// DM平台读sendmsg200204
        /// </summary>
        public static int GetSendMsgCode = 200203;  //DM平台读sendmsg
        public static int FROM_DM_WRITE_SENDMSG = 200204;  //DM平台写sendmsg

    }
}

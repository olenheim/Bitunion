using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitunion
{
    class BuSetting
    {
        #region 设定资源
        //独立存储配置
        private static IsolatedStorageSettings _setting = IsolatedStorageSettings.ApplicationSettings;
        //页面中的楼层个数
        private static string _pagethreadcount;
        //是否显示图片
        private static string _showphoto;
        //内外网不同的url
        private static string _url;
        //是否显示消息尾巴
        private static string _showtail;
        //消息尾巴的信息
        private static string _tailmsg;
        //是否记住密码
        private static string _rempassword;
        //是否自动登录
        private static string _autologin;
        //用户名及密码
        private static string _id, _password;
        #endregion

        //初始化
        public static void Inital()
        {
            if (!_setting.TryGetValue("pagethreadcount", out _pagethreadcount)) 
            {
                _pagethreadcount = "10";
                _setting.Add("pagethreadcount", _pagethreadcount);
            }

            if( !_setting.TryGetValue("showphoto", out _showphoto))
            {
                _showphoto = "True";
                _setting.Add("showphoto", _showphoto);
            }

            if(!_setting.TryGetValue("network", out _url))
            {
                _url = "http://out.bitunion.org/open_api/";
                _setting.Add("network", _url);
            }

            if(!_setting.TryGetValue("showtail",out _showtail))
            {
                _showtail = "True";
                _setting.Add("showtail", _showtail);
            }

            if(!_setting.TryGetValue("tailmsg", out _tailmsg))
            {
                _tailmsg = "发自联盟WindowsPhone8客户端";   
                _setting.Add("tailmsg", _tailmsg);
            }
            if (!_setting.TryGetValue("rememberpassword", out _rempassword))
            {
                _rempassword = "True";
                _setting.Add("rememberpassword", _rempassword);
            }
            if (!_setting.TryGetValue("autologin", out _autologin))
            {
                _autologin = "True";
                _setting.Add("autologin", _autologin);
            }
            if (!_setting.TryGetValue("id", out _id))
            {
                _id = string.Empty;
                _setting.Add("id", _id);
            }
            if(!_setting.TryGetValue("password",out _password))
            {
                _password = string.Empty;
                _setting.Add("password",_password);
            }
           
        }

        public static bool ShowPhoto
        {
            get
            {
                return (_showphoto == "True");
            }
            set
            {
                if (value != (_showphoto == "True"))
                {
                    _showphoto = value.ToString();
                    _setting["showphoto"] = value.ToString();
                }
            }
        }

        public static string URL
        {
            get
            {
                return _url;
            }
            set
            {
                if (value != _url)
                {
                    _url = value;
                    _setting["network"] = value;
                }
            }
        }

        public static uint PageThreadCount
        {
            get
            {
                return Convert.ToUInt32(_pagethreadcount);
            }
            set
            {
                if (value != Convert.ToUInt32(_pagethreadcount))
                {
                    _pagethreadcount = value.ToString();
                    _setting["pagethreadcount"] = value.ToString();
                }
            }
        }

        public static bool ShowTail
        {
            get
            {
                return (_showtail == "True");
            }
            set
            {
                if (value != (_showtail == "True"))
                {
                    _showtail = value.ToString();
                    _setting["showtail"] = value.ToString();
                }
            }
        }

        public static bool RemPassWord
        {
            get
            {
                return (_rempassword == "True");
            }
            set
            {
                if (value != (_rempassword == "True"))
                {
                    _rempassword = value.ToString();
                    _setting["rememberpassword"] = value.ToString();
                }
            }
        }

        public static bool AutoLogin
        {
            get
            {
                return (_autologin == "True");
            }
            set
            {
                if (value != (_autologin == "True"))
                {
                    _autologin = value.ToString();
                    _setting["autologin"] = value.ToString();
                }
            }
        }

        public static string TailMsg
        {
            get
            {
                return _tailmsg;
            }
            set
            {
                if (value != _tailmsg)
                {
                    _tailmsg = value;
                    _setting["tailmsg"] = value;
                }
            }
        }

        public static string ID
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    _setting["id"] = value;
                }
            }
        }

        public static string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (value != _password)
                {
                    _password = value;
                    _setting["password"] = value;
                }
            }
        }



    }
}

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
            _setting["network"] = "http://out.bitunion.org/open_api/";
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



    }
}

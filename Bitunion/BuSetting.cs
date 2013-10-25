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
        //独立存储配置
        private static IsolatedStorageSettings _setting = IsolatedStorageSettings.ApplicationSettings;
        private static string _pagethreadcount;
        private static string _showphoto;
        private static string _network;
        private static string _showtail;
        private static string _tailmsg;

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

            if(!_setting.TryGetValue("network", out _network))
            {
                _network = "out";
                _setting.Add("network", _network);
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

        public static string NetWork
        {
            get
            {
                return _network;
            }
            set
            {
                if (value != _network)
                {
                    _network = value;
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

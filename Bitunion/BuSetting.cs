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

        public static void Inital()
        {
            if (!_setting.TryGetValue("pagethreadcount", out _pagethreadcount)) 
            {
                _pagethreadcount = "10";
                _showphoto = "ture";
                _network = "out";
                _setting.Add("pagethreadcount", _pagethreadcount);
                _setting.Add("showphoto", _showphoto);
                _setting.Add("network", _network);
            }
            else
            {
                _setting.TryGetValue("showphoto", out _showphoto);
                _setting.TryGetValue("network", out _network);
            }

            PageThreadCount = Convert.ToUInt32(_pagethreadcount);
                
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


    }
}

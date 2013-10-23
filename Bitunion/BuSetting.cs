using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitunion
{
    class BuSetting
    {
        public static void Inital()
        {
            PageThreadCount = 10;
        }
        public static uint PageThreadCount{get;private set;}


    }
}

using System;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading.Tasks;
using System.IO;
using System.Text;

namespace HttpLibrary
{
    public class HttpEngine
    {
        #region Resources
        /// <summary>
        /// 是否对URL进行输入输出
        /// </summary>
        public static bool IsDebug { get; set; }
        /// <summary>
        /// 是否正在请求
        /// </summary>
        public bool IsBusy { get; private set; }
        #endregion

        #region PostUrl
        /// <summary>
        /// post数据
        /// </summary>
        /// <param name="PostUrl">PostUrl</param>
        /// <param name="Context">post的参数</param>
        public virtual async Task<Stream> PostAsync(string RequestUrl, string Context)
        {
            try
            {
                IsBusy = true;
                WriteDebug("PostUrl", RequestUrl);
                WriteDebug("PostContext", Context);

                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(new Uri(RequestUrl, UriKind.Absolute));
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";

                using (Stream stream = await httpWebRequest.GetRequestStreamAsync())
                {
                    byte[] entryBytes = Encoding.UTF8.GetBytes(Context);
                    stream.Write(entryBytes, 0, entryBytes.Length);
                }

                WebResponse response = await httpWebRequest.GetResponseAsync();
                return response.GetResponseStream();
            }
            catch (Exception ex)
            {
                WriteDebug("PostError", ex.Message);
                throw;
            }
        }
        #endregion

        #region 请求数据
        /// <summary>
        /// get数据
        /// </summary>
        /// <param name="RequestUrl">geturl</param>
        /// <param name="Context">get的参数</param>
        protected virtual async Task<Stream> GetAsync(string RequestUrl)
        {
            try
            {
                IsBusy = true;

                WriteDebug("GetUrl", RequestUrl);

                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(new Uri(RequestUrl, UriKind.Absolute));
                httpWebRequest.Method = "GET";
                WebResponse response = await httpWebRequest.GetResponseAsync();
                Stream streamResult = response.GetResponseStream();
                return streamResult;
            }
            catch (Exception ex)
            {
                WriteDebug("GetError", ex.Message);
                throw ex;
            }
        }
        #endregion

        #region WriteDebug
        /// <summary>
        /// WriteMsg
        /// </summary>
        /// <param name="writemsg"></param>
        private void WriteDebug(string flag, string writemsg)
        {
            if (IsDebug)
            {
                Debug.WriteLine(flag + ":" + writemsg);
            }
        }
        #endregion

    }
}

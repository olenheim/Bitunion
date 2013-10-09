using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace Bitunion.ViewModels
{
    public class ImageViewModel : INotifyPropertyChanged
    {
        public ImageViewModel(string path)
        {
            ImageSrc = new BitmapImage(new Uri(path));
        }

        private ImageSource _imagesrc;
        public ImageSource ImageSrc
        {
            get
            {
                return _imagesrc;
            }
            set
            {
                if (value != _imagesrc)
                {
                    _imagesrc = value;
                    NotifyPropertyChanged("ImageSrc");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class QuoteViewModel : INotifyPropertyChanged
    {
        public QuoteViewModel(string str)
        {
            QuoteMsg = str;
        }

        private string _quotemsg;
        /// <summary>
        /// 示例 ViewModel 属性；此属性在视图中用于使用绑定显示它的值。
        /// </summary>
        /// <returns></returns>
        public string QuoteMsg
        {
            get
            {
                return _quotemsg;
            }
            set
            {
                if (value != _quotemsg)
                {
                    _quotemsg = value;
                    NotifyPropertyChanged("QuoteMsg");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
    public class PostViewModel : INotifyPropertyChanged
    {
       public PostViewModel(BuPost post)
        {
            QuoteItems = new ObservableCollection<QuoteViewModel>();
        
           if(post.attachment!= null)
               ImageSrc = new BitmapImage(new Uri(BuAPI._url.Replace("open_api/", "") + HttpUtility.UrlDecode(post.attachment)));

           string message = HttpUtility.UrlDecode(post.message);
           
           //技术原因这里只显示引用的第一个人，以后再改吧……
            List<BuQuote> quotes= BuAPI.parseQuotes(ref message);
            if (quotes.Count != 0)
                QuoteItems.Add(new QuoteViewModel(quotes[0].author + "  " + quotes[0].time  + quotes[0].content));
            

            DateTime dt = BuAPI.DateTimeConvertTime(post.dateline);

            //格式化时间”年-月-日 小时:分钟“
            string strtime = dt.ToString("yyyy-M-d HH:mm");

            Message = HttpUtility.UrlDecode(BuAPI.parseHTML(message));
            AddInfo = HttpUtility.UrlDecode(post.author) + "  " + strtime;
        }

        public PostViewModel(){}

        public string tid { get; private set; }

        //引用视图模型
        public ObservableCollection<QuoteViewModel> QuoteItems { get; private set; }

        //图片附件
        private ImageSource _imagesrc;
        public ImageSource ImageSrc
        {
            get
            {
                return _imagesrc;
            }
            set
            {
                if (value != _imagesrc)
                {
                    _imagesrc = value;
                    NotifyPropertyChanged("ImageSrc");
                }
            }
        }

        private string _message;
        /// <summary>
        /// 示例 ViewModel 属性；此属性在视图中用于使用绑定显示它的值。
        /// </summary>
        /// <returns></returns>
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if (value != _message)
                {
                    _message = value;
                    NotifyPropertyChanged("Message");
                }
            }

        }
        private string _addinfo;
        /// <summary>
        /// 示例 ViewModel 属性；此属性在视图中用于使用绑定显示它的值。
        /// </summary>
        /// <returns></returns>
        public string AddInfo
        {
            get
            {
                return _addinfo;
            }
            set
            {
                if (value != _addinfo)
                {
                   _addinfo= value;
                   NotifyPropertyChanged("AddInfo");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

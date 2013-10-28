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
 
        public class QuoteViewModel : INotifyPropertyChanged
        {
            public QuoteViewModel(string str)
            {
                QuoteMsg = str;
            }

            private string _quotemsg;
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
            public PostViewModel(BuPost post,uint floorno)
            {
                //填写楼层信息
                if (floorno == 1)
                    FloorNo = "楼主";
                else
                    FloorNo = floorno.ToString() + "楼";

                QuoteItems = new ObservableCollection<QuoteViewModel>();
                //加载图片
                if (post.attachment != null)
                    ImageSrc = BuAPI.GetImageSrc(HttpUtility.UrlDecode(post.attachment));

                string message = HttpUtility.UrlDecode(post.message);

                List<BuQuote> quotes = BuAPI.parseQuotes(ref message);
                foreach (var quote in quotes)
                    QuoteItems.Add(new QuoteViewModel(quote.author + "  " + quote.time + quote.content.Trim()));

                DateTime dt = BuAPI.DateTimeConvertTime(post.dateline);

                //格式化时间”年-月-日 小时:分钟“
                string strtime = dt.ToString("yyyy-M-d HH:mm");

                Message = (BuAPI.parseHTML(message)).Trim();
                AddInfo = HttpUtility.UrlDecode(post.author) + "  " + strtime;
                post.dateline = strtime;
                post.message = Message;
                post.author = HttpUtility.UrlDecode(post.author);
                _post = post;
            }

            public PostViewModel() { }
            
            public BuPost _post { get; private set; }

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
                        _addinfo = value;
                        NotifyPropertyChanged("AddInfo");
                    }
                }
            }
            private string _floorno;
            /// <summary>
            /// 示例 ViewModel 属性；此属性在视图中用于使用绑定显示它的值。
            /// </summary>
            /// <returns></returns>
            public string FloorNo
            {
                get
                {
                    return _floorno;
                }
                set
                {
                    if (value != _floorno)
                    {
                        _floorno = value;
                        NotifyPropertyChanged("FloorNo");
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

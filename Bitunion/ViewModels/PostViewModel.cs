using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Bitunion.ViewModels
{
    public class PostViewModel : INotifyPropertyChanged
    {

        public PostViewModel()
        {
        }

        public string tid { get; private set; }

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

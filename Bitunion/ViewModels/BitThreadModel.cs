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
    public class BitThreadModel : INotifyPropertyChanged
    {

        public BitThreadModel(BitLatestThread bt)
        {
            Subject = Uri.UnescapeDataString(bt.pname);
            Author = Uri.UnescapeDataString(bt.author);
            ForumName = Uri.UnescapeDataString(bt.fname);
            Replies = bt.replies;
            this.latestthread = bt;
        }

        public BitThreadModel() { }

        public BitThreadModel(BitThread thread) 
        {
	    Subject = Uri.UnescapeDataString(thread.subject);
            Author = Uri.UnescapeDataString(thread.author);
            Time = thread.dateline;
            Replies = bt.replies;
            this.latestthread = bt;
        }


        public BitLatestThread latestthread { get; private set; }
	public BitThread thread {get; private set;}

        private string _subject;
        /// <summary>
        /// 示例 ViewModel 属性；此属性在视图中用于使用绑定显示它的值。
        /// </summary>
        /// <returns></returns>
        public string Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                if (value != _subject)
                {
                    _subject = value;
                    NotifyPropertyChanged("Subject");
                }
            }
        }

	        private string _author;
        /// <summary>
        /// 示例 ViewModel 属性；此属性在视图中用于使用绑定显示它的值。
        /// </summary>
        /// <returns></returns>
        public string Author
        {
            get
            {
                return _author;
            }
            set
            {
                if (value != _author)
                {
                    _author = value;
                    NotifyPropertyChanged("Author");
                }
            }
        }

	        private string _forumname;
        /// <summary>
        /// 示例 ViewModel 属性；此属性在视图中用于使用绑定显示它的值。
        /// </summary>
        /// <returns></returns>
        public string ForumName
        {
            get
            {
                return _forumname;
            }
            set
            {
                if (value != _forumname)
                {
                    _forumname = value;
                    NotifyPropertyChanged("ForumName");
                }
            }
        }

	        private string _time;
        /// <summary>
        /// 示例 ViewModel 属性；此属性在视图中用于使用绑定显示它的值。
        /// </summary>
        /// <returns></returns>
        public string Time
        {
            get
            {
                return _time;
            }
            set
            {
                if (value != _time)
                {
                    _time = value;
                    NotifyPropertyChanged("Time");
                }
            }
        }

	        private string _replies;
        /// <summary>
        /// 示例 ViewModel 属性；此属性在视图中用于使用绑定显示它的值。
        /// </summary>
        /// <returns></returns>
        public string Replies
        {
            get
            {
                return _replies;
            }
            set
            {
                if (value != _replies)
                {
                    _replies = value;
                    NotifyPropertyChanged("Replies");
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


using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Bitunion.ViewModels
{
    public class ThreadViewModel : INotifyPropertyChanged
    {

        public ThreadViewModel(BuLatestThread bt)
        {
            Subject = bt.pname = BuAPI.parseHTML(HttpUtility.UrlDecode(bt.pname));
            Author = bt.author = HttpUtility.UrlDecode(bt.author);
            ForumName = bt.fname = BuAPI.parseHTML(HttpUtility.UrlDecode(bt.fname));
            Replies = "回复:" + bt.tid_sum;
            this.latestthread = bt;
            PostItems = new ObservableCollection<PostViewModel>();
        }

        public ThreadViewModel() { PostItems = new ObservableCollection<PostViewModel>(); }

        public ThreadViewModel(BuThread thread) 
        {
            Subject = thread.subject = BuAPI.parseHTML(HttpUtility.UrlDecode(thread.subject));
            Author = thread.author = HttpUtility.UrlDecode(thread.author);
            Time = BuAPI.DateTimeConvertTime(thread.dateline).ToString("MM-dd HH:mm");
            PostItems = new ObservableCollection<PostViewModel>();
            Replies = "回复:" + thread.replies;
            this.thread = thread;
        }

        public ObservableCollection<PostViewModel> PostItems { get; private set; }
        public BuLatestThread latestthread { get; private set; }
    	public BuThread thread {get; private set;}

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


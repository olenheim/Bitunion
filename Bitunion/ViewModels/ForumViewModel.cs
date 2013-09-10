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
    public class ForumViewModel : INotifyPropertyChanged
    {

        public ForumViewModel(BuForum forum)
        {
            Name = Uri.UnescapeDataString(forum.name);
            Description = Uri.UnescapeDataString(forum.description);
            Moderator = Uri.UnescapeDataString(forum.moderator);
            Onlines = forum.onlines;
            this.forum= forum;
            this.ThreadItems = new ObservableCollection<ThreadViewModel>();
        }

        public ForumViewModel() {this.ThreadItems = new ObservableCollection<ThreadViewModel>(); }
        
        //论坛vm下的帖子vm列表
        public ObservableCollection<ThreadViewModel> ThreadItems { get; private set; }

        //论坛逻辑实体对象
        public BuForum forum { get; private set; }

        //论坛名称
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        //论坛描述
        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (value != _description)
                {
                    _description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        //在线人数
        private string _moderator;
        public string Moderator
        {
            get
            {
                return _moderator;
            }
            set
            {
                if (value != _moderator)
                {
                    _moderator = value;
                    NotifyPropertyChanged("Moderator");
                }
            }
        }

        //论坛描述
        private string _onlines;
        public string Onlines
        {
            get
            {
                return _onlines;
            }
            set
            {
                if (value != _onlines)
                {
                    _onlines = value;
                    NotifyPropertyChanged("Onlines");
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

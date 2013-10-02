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
    public class ForumPageViewModel : INotifyPropertyChanged
    {

        public ForumPageViewModel() 
        {
            ThreadItems = new ObservableCollection<ThreadViewModel>();
            SubForumItems = new ObservableCollection<ForumViewModel>();
        }
        
        //帖子视图模型集合
        public ObservableCollection<ThreadViewModel> ThreadItems { get; private set; }

	    //子版块视图模型集合
        public ObservableCollection<ForumViewModel> SubForumItems { get; private set; }

        //论坛对象实体模型
        public BuForum forum { get; private set; }

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


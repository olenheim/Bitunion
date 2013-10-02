using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Bitunion.Resources;
using System.Collections.Generic;

namespace Bitunion.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel() 
        {
            LatestThreadItems = new ObservableCollection<ThreadViewModel>();
            ForumItems = new ObservableCollection<ForumViewModel>();
        }

        //论坛最新帖子VM对象列表
        public ObservableCollection<ThreadViewModel> LatestThreadItems{get;private set;}

        //论坛VM对象列表
        public ObservableCollection<ForumViewModel> ForumItems { get; private set; }
        
        /// <summary>
        /// 返回本地化字符串的示例属性
        /// </summary>
        public string LocalizedSampleProperty
        {
            get
            {
                return AppResources.SampleProperty;
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
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
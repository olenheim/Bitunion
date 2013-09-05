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
            this.LatestThreadItems = new ObservableCollection<BitThreadModel>();
            this.ForumItems = new ObservableCollection<ForumViewModel>();
        }

        //论坛最新帖子VM对象列表
        public ObservableCollection<BitThreadModel> LatestThreadItems { get; private set; }

        //论坛VM对象列表
        public ObservableCollection<ForumViewModel> ForumItems { get; private set; }
        
        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// 示例 ViewModel 属性；此属性在视图中用于使用绑定显示它的值
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

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
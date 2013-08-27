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
            this.Items = new ObservableCollection<BitThreadModel>();
        }

        /// <summary>
        /// ItemViewModel 对象的集合。
        /// </summary>
        public ObservableCollection<BitThreadModel> Items { get; private set; }

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

        /// <summary>
        /// 创建一些 ItemViewModel 对象并将其添加到 Items 集合中。
        /// </summary>
        public async void LoadData()
        {
            bool bl = await BitAPI.Login("泪沸腾", "bitwdazsc");
            List<BitLatestThread> btl = await BitAPI.QueryLatestThreadList();

            foreach (BitLatestThread bt in btl)
                this.Items.Add(new BitThreadModel(bt));


            this.IsDataLoaded = true;
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
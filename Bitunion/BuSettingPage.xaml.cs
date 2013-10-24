using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Bitunion
{
    public partial class BuSettingPage : PhoneApplicationPage
    {
        public BuSettingPage()
        {
            InitializeComponent();
            Init();
        }

        //根据独立储存初始化设置页面
        private void Init()
        {
            tsThreadCount.IsChecked = (BuSetting.PageThreadCount == 20);
        }

        private void tsShowPhoto_Checked(object sender, RoutedEventArgs e)
        {
          //  BuSetting.
        }

        private void tsShowPhoto_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void tsOutNet_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void tsShowTail_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void tsOutNet_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void tsShowTail_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void tsThreadCount_Checked(object sender, RoutedEventArgs e)
        {
            tsThreadCount.Content = "20";
            BuSetting.PageThreadCount = 20;
        }

        private void tsThreadCount_Unchecked(object sender, RoutedEventArgs e)
        {
            tsThreadCount.Content = "10";
            BuSetting.PageThreadCount = 10;
        }
    }
}
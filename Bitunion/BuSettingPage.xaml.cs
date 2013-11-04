using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System.Windows;

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
            tsOutNet.IsChecked = (BuSetting.URL == "http://out.bitunion.org/open_api/");
            tsShowPhoto.IsChecked = BuSetting.ShowPhoto;
            tsShowTail.IsChecked = BuSetting.ShowTail;
            tbMsgTail.Text = BuSetting.TailMsg;

            if (tsThreadCount.IsChecked == true)
                tsThreadCount_Checked(null, null);
            else
                tsThreadCount_Unchecked(null,null);

            if (tsOutNet.IsChecked == true)
                tsOutNet_Checked(null, null);
            else
                tsOutNet_Unchecked(null, null);

            if(tsShowPhoto.IsChecked == true)
                tsShowPhoto_Checked(null, null);
            else
                tsShowPhoto_Unchecked(null, null);

            if(tsShowTail.IsChecked == true)
                tsShowTail_Checked(null, null);
            else
                tsShowTail_Unchecked(null, null);


        }

        private void tsShowPhoto_Checked(object sender, RoutedEventArgs e)
        {
            tsShowPhoto.Content = "显示";
            BuSetting.ShowPhoto = true;
        }

        private void tsShowPhoto_Unchecked(object sender, RoutedEventArgs e)
        {
            tsShowPhoto.Content = "隐藏";
            BuSetting.ShowPhoto = false;
        }

  
        private void tsShowTail_Unchecked(object sender, RoutedEventArgs e)
        {
            tsShowTail.Content = "隐藏";
            BuSetting.ShowTail = false;
            tbMsgTail.IsEnabled = false;
        }

        private void tsShowTail_Checked(object sender, RoutedEventArgs e)
        {
            tsShowTail.Content = "显示";
            BuSetting.ShowTail = true;
            tbMsgTail.IsEnabled = true;
        }

        private void tsOutNet_Checked(object sender, RoutedEventArgs e)
        {
            tsOutNet.Content = "外网";
            BuSetting.URL = "http://out.bitunion.org/open_api/";
        }

        private void tsOutNet_Unchecked(object sender, RoutedEventArgs e)
        {
            tsOutNet.Content = "内网";
            BuSetting.URL = "http://www.bitunion.org/open_api/";
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

        private void tbMsgTail_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            BuSetting.TailMsg = tbMsgTail.Text;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();

            marketplaceReviewTask.Show();
        }
    }
}
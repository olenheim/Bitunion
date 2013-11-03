using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;

namespace Bitunion
{
    public partial class LoginPage : PhoneApplicationPage
    {
        public LoginPage()
        {
            InitializeComponent();

            //对设置类进行初始化
            BuSetting.Inital();

            InitLoginPage();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            bool bl = true;
            PgBar.Visibility = Visibility.Collapsed;

            //可能的登出判断
            string type;
            NavigationContext.QueryString.TryGetValue("type", out type);
            if (type == "logout")
            { 
                NavigationService.RemoveBackEntry();
                SwitchLoading();
                bl = await BuAPI.Logout();
                SwitchLoading(false);
            }
            if (bl == false)
                MessageBox.Show("登出失败，可能与服务器连接已断开");
         
            //填写记录的默认信息
            ID.Text = BuSetting.ID;
            Password.Password = BuSetting.Password;
            isRemPassword.IsChecked = BuSetting.RemPassWord;
            isAutoLogin.IsChecked = BuSetting.AutoLogin;
            tsNetWork.IsChecked = (BuSetting.URL == "http://out.bitunion.org/open_api/");
            if (BuSetting.RemPassWord)
                isRemPassword_Checked(null, null);
            else
                isRemPassword_Unchecked(null, null);

            if (BuSetting.AutoLogin)
                isAutoLogin_Checked(null, null);
            else
                isAutoLogin_Unchecked(null, null);

            if (BuSetting.URL == "http://out.bitunion.org/open_api/")
                NetWork_Checked(null, null);
            else
                NetWork_Unchecked(null, null);

            //自动登录
            if (BuSetting.AutoLogin && BuSetting.RemPassWord && type != "logout" && e.NavigationMode == NavigationMode.New)
                login_click(null, null);
        }

        //从独立存储中提取信息
        private void InitLoginPage()
        {

        }

        //保存账号密码及配置到独立储存
        private void SaveConfig()
        {
            BuSetting.ID = ID.Text;
            if (BuSetting.RemPassWord)
                BuSetting.Password = Password.Password;
            else
                BuSetting.Password = string.Empty;
        }

        //点击登陆事件
        private async void login_click(object sender, EventArgs e)
        {
            if (ID.Text == string.Empty || Password.Password == string.Empty)
                return;
            SwitchLoading();
            bool bl = await BuAPI.Login(ID.Text, Password.Password);
            if (bl)
            {
                SaveConfig();
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            else
                SwitchLoading(false);
        }

        //载入动画效果的切换
        private void SwitchLoading(bool blLoading = true)
        {
            if (blLoading)
            {
                PgBar.Visibility = Visibility.Visible;
                foreach(var ctrl in TitlePanel.Children)
                    ctrl.Visibility = Visibility.Collapsed;
                foreach(var ctrl in GridContent.Children)
                    ctrl.Visibility = Visibility.Collapsed;
                BkPic.Opacity = 1.0;
                ApplicationBar.IsVisible = false;
            }
            else
            {
                PgBar.Visibility = Visibility.Collapsed;
                foreach (var ctrl in TitlePanel.Children)
                    ctrl.Visibility = Visibility.Visible;
                foreach (var ctrl in GridContent.Children)
                    ctrl.Visibility = Visibility.Visible;
                BkPic.Opacity = 0.5;
                ApplicationBar.IsVisible = true;
            }
        }

        //当不保存密码时禁用自动登录
        private void isRemPassword_Click(object sender, RoutedEventArgs e)
        {
            if (isRemPassword == null)
                return;
            if (isRemPassword.IsChecked == true)
                isAutoLogin.IsEnabled = true;
            else
                isAutoLogin.IsEnabled = false;
        }

        private void isRemPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            isRemPassword.Content = "关闭";
            isAutoLogin.IsEnabled = false;
            BuSetting.RemPassWord = false;
        }

        private void isRemPassword_Checked(object sender, RoutedEventArgs e)
        {
            isRemPassword.Content = "开启";
            isAutoLogin.IsEnabled = true;
            BuSetting.RemPassWord = true;
        }

        private void isAutoLogin_Unchecked(object sender, RoutedEventArgs e)
        {
            isAutoLogin.Content = "关闭";
            BuSetting.AutoLogin = false;
        }

        private void isAutoLogin_Checked(object sender, RoutedEventArgs e)
        {
            isAutoLogin.Content = "开启";
            BuSetting.AutoLogin = true;
        }

        private void NetWork_Checked(object sender, RoutedEventArgs e)
        {
            tsNetWork.Content = "外网";
            BuSetting.URL = "http://out.bitunion.org/open_api/";
        }

        private void NetWork_Unchecked(object sender, RoutedEventArgs e)
        {
            tsNetWork.Content = "内网";
            BuSetting.URL = "http://www.bitunion.org/open_api/";
        }

    }
}
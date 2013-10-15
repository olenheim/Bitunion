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
        //独立存储配置与保存用户账号密码
        private IsolatedStorageSettings userinfo = IsolatedStorageSettings.ApplicationSettings;

        //账号密码
        private string _id, _password;

        //是否保存密码与是否自动登陆
        private string _isrempw, _isautologin;

        public LoginPage()
        {
            InitializeComponent();
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
            ID.Text = _id;
            Password.Password = _password;
            if (_isrempw == "false")
                isRemPassword_Unchecked(null, null);
            if (_isautologin == "false")
                isAutoLogin_Unchecked(null, null);

            //自动登录
            if (_isautologin == "true" && _isrempw == "true" && type != "logout")
                login_click(null, null);
        }

        //从独立存储中提取信息
        private void InitLoginPage()
        {
            if (!userinfo.TryGetValue("rememberpassword", out _isrempw))
            {
                _isrempw = "false";
                userinfo.Add("rememberpassword", _isrempw);
            }

            if (!userinfo.TryGetValue("autologin", out _isautologin))
            {
                _isautologin = "false";
                userinfo.Add("autologin", _isrempw);
            }

            if (_isrempw == "true")
            {
                if (!userinfo.TryGetValue("password", out _password))
                {
                    _password = "";
                    userinfo.Add("password", _password);
                }
            }
            else
            {
                _password = "";
            }

            if (!userinfo.TryGetValue("id", out _id))
            {
                _id = "";
                userinfo.Add("id", _id);
            }
        }

        //保存账号密码及配置到独立储存
        private void SaveConfig()
        {
            userinfo["id"] = ID.Text;
            if (isRemPassword.IsChecked == true)
            {
                userinfo["password"] = Password.Password;
                userinfo["rememberpassword"] = "true";
            }
            else
            {
                userinfo["password"] = "";
                userinfo["rememberpassword"] = "false";
            }

            if (isAutoLogin.IsChecked == true)
                userinfo["autologin"] = "true";
            else
                userinfo["autologin"] = "false";
        }

        //点击登陆事件
        private async void login_click(object sender, EventArgs e)
        {
            SwitchLoading();
            bool bl = await BuAPI.Login(ID.Text, Password.Password);
            if (bl)
            {
                SaveConfig();
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            else
            {
                SwitchLoading(false);
                MessageBox.Show("密码输入错误或联盟反思中……");
            }
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
        }

        private void isRemPassword_Checked(object sender, RoutedEventArgs e)
        {
            isRemPassword.Content = "开启";
            isAutoLogin.IsEnabled = true;
        }

        private void isAutoLogin_Unchecked(object sender, RoutedEventArgs e)
        {
            isAutoLogin.Content = "关闭";
        }

        private void isAutoLogin_Checked(object sender, RoutedEventArgs e)
        {
            isAutoLogin.Content = "开启";
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;

namespace Bitunion
{
    public partial class LoginPage : PhoneApplicationPage
    {
        private IsolatedStorageSettings userinfo = IsolatedStorageSettings.ApplicationSettings;

        private string _id, _password;

        public LoginPage()
        {
            InitializeComponent();
            ID.Text = "泪沸腾";
            Password.Password = "bitwdazsc";
        }

        private async void login_click(object sender, EventArgs e)
        {
            //Todo 动画效果
            bool bl = await BuAPI.Login(ID.Text, Password.Password);
            if (bl)
                NavigationService.Navigate(new Uri("/MainPage.xaml",UriKind.Relative));
            else
                //Todo消除动画 重新输入
                ;
        }


    }
}
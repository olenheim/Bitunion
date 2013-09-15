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

        public LoginPage()
        {
            InitializeComponent();
        }

        private void login_click(object sender, EventArgs e)
        {

        }
    }
}
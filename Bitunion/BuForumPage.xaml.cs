using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Bitunion.ViewModels;

namespace Bitunion
{
    public partial class BuForumPage : PhoneApplicationPage
    {
        private static MainViewModel _mainviewmodel = new MainViewModel();

        private string _fid,_forumname;

        public BuForumPage()
        {
            InitializeComponent();
            DataContext = _mainviewmodel;
        }

        protected async override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //获取从父页面传递过来的tid
            NavigationContext.QueryString.TryGetValue("fid", out _fid);
            NavigationContext.QueryString.TryGetValue("fname", out _forumname);

            //ForumName.SetValue(_forumname);

            List<BuThread> threadlist = await BuAPI.QueryThreadList(_fid, "0", "19");

            foreach (BuThread bt in threadlist)
                _mainviewmodel.Items.Add(new BitThreadModel(bt));

        }

        private void LongListSelector_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (ThreadViewList.SelectedItem == null)
                return;

            BitThreadModel item = ThreadViewList.SelectedItem as BitThreadModel;

           ThreadViewList.SelectedItem = null;

            var thread = item.latestthread;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/BitThreadPage.xaml?tid=" + thread.tid
                + "&subject=" + thread.pname
                + "&replies=" + thread.tid_sum
                + "&fid=" + thread.fid
                + "&fname=" + thread.fname
                , UriKind.Relative));
        }
    }
}
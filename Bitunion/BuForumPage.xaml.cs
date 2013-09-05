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
        private static ForumViewModel _forumviewmodel = new ForumViewModel();

        private string _fid,_forumname;
        
        private Dictionary<string,List<BuThread>> _pagecache;

        private uint _page;

        public BuForumPage()
        {
            InitializeComponent();
            DataContext = _forumviewmodel;
            _pagecache = new Dictionary<string,List<BuThread>>();
            _page = 1;
        }

        protected async override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //获取从父页面传递过来的tid
            NavigationContext.QueryString.TryGetValue("fid", out _fid);
            NavigationContext.QueryString.TryGetValue("fname", out _forumname);

            pivotItem1.Text = _forumname;
            LoadThreadList();
        }
        
        private async void LoadThreadList()
        {
            pgbar.Visibility = Visibility.Visible;
            _forumviewmodel.ThreadItems.Clear();
            List<BuThread> threadlist = await BuAPI.QueryThreadList(_fid, ((_page - 1)*20).ToString(),( _page*20 -1).ToString());
            foreach (BuThread bt in threadlist)
                _forumviewmodel.ThreadItems.Add(new BitThreadModel(bt));

            CheckBtnEnable();
            pgbar.Visibility = Visibility.Collapsed;
        }
        

        private void LongListSelector_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (ThreadViewList.SelectedItem == null)
                return;

            BitThreadModel item = ThreadViewList.SelectedItem as BitThreadModel;

           ThreadViewList.SelectedItem = null;

            var thread = item.thread;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/BuThreadPage.xaml?tid=" + thread.tid
                + "&subject=" + thread.subject
                + "&replies=" + thread.replies
                + "&fid=" + _fid
                + "&fname=" + _forumname
                , UriKind.Relative));
        }



        private void refresh_click(object sender, EventArgs e)
        {
            _pagecache.Clear();
            _page = 1;
            LoadThreadList();
        }

        private void Prev_Click(object sender, EventArgs e)
        {
            _page--;
            LoadThreadList();
        }

        private void Next_Click(object sender, EventArgs e)
        {
            _page++;
            LoadThreadList();
        }

        private void CheckBtnEnable()
        {
            //禁用工具栏按钮的方法
            (ApplicationBar.Buttons[1] as ApplicationBarIconButton).IsEnabled = (_page != (uint)1);
        }
    }
}

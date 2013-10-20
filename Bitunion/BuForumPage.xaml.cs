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
        #region 资源文件
        //论坛页面视图模型
        private ForumPageViewModel _forumpageviewmodel = new ForumPageViewModel();

        //该论坛页面的fid以及论坛名称
        private string _fid, _forumname;

        //每一个页面的贴子缓存
        private Dictionary<uint, List<BuThread>> _pagecache = new Dictionary<uint, List<BuThread>>();

        //当前页码
        private uint _pageno = 1;
        #endregion
        private PopupPost pp;
        public BuForumPage()
        {
            InitializeComponent();
            DataContext = _forumpageviewmodel;
            this.ApplicationBar = (Microsoft.Phone.Shell.ApplicationBar)Resources["forum"];
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (_fid != null && _forumname != null)
                return;

            //清空子板数据
            _forumpageviewmodel.SubForumItems.Clear();

            //获取从父页面传递过来的fid与论坛名称
            NavigationContext.QueryString.TryGetValue("fid", out _fid);
            NavigationContext.QueryString.TryGetValue("fname", out _forumname);

            pivotItem1.Text = _forumname;
            List<BuForum> SubForumList;
            if (MainPage.DictFourm.TryGetValue(_fid, out SubForumList) && SubForumList.Count != 0)
            {
                //添加子板相关信息
                foreach (var forum in MainPage.DictFourm[_fid])
                    _forumpageviewmodel.SubForumItems.Add(new ForumViewModel(forum));
            }
            else
            {
                //去除子板标签页
                if(ForumPivot.Items.Count > 1)
                    ForumPivot.Items.RemoveAt(1);
            }

            LoadThreadList();
        }

        //异步加载帖子列表
        private async void LoadThreadList()
        {
            pgbar.Visibility = Visibility.Visible;
            CheckBtnEnable();
            //清除界面数据
            _forumpageviewmodel.ThreadItems.Clear();

            //先从缓存中获取
            List<BuThread> threadlist;
            if (!_pagecache.TryGetValue(_pageno, out threadlist))
                _pagecache[_pageno] = await BuAPI.QueryThreadList(_fid, ((_pageno - 1) * 20).ToString(), (_pageno * 20 - 1).ToString());

            //填写视图模型
            foreach (BuThread bt in _pagecache[_pageno])
                _forumpageviewmodel.ThreadItems.Add(new ThreadViewModel(bt));

            pgbar.Visibility = Visibility.Collapsed;
        }

        //点击某一个帖子
        private void LongListSelector_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (ThreadViewList.SelectedItem == null)
                return;

            ThreadViewModel item = ThreadViewList.SelectedItem as ThreadViewModel;

            ThreadViewList.SelectedItem = null;

            var thread = item.thread;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/BuThreadPage.xaml?tid=" + thread.tid
                + "&subject=" + thread.subject
                + "&replies=" + thread.replies
                + "&fid=" + _fid
                + "&fname=" + _forumname
                , UriKind.Relative));


            //进入页面后应消除后退堆栈
        }

        //切换Tab页面
        private void ForumPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //切换后修改AppBar状态
            switch (((Pivot)sender).SelectedIndex)
            {
                case 0:
                    ApplicationBar.IsVisible = true;
                    break;

                case 1:
                    ApplicationBar.IsVisible = false;
                    break;
            }
        }

        //点击子版块
        private void LongListSelector_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {
            if (SubForumViewList.SelectedItem == null)
                return;

            ForumViewModel item = SubForumViewList.SelectedItem as ForumViewModel;

            SubForumViewList.SelectedItem = null;

            var forum = item.forum;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/BuForumPage.xaml?fid=" + forum.fid
                    + "&fname=" + forum.name
                   , UriKind.Relative));
        }

        //刷新
        private void refresh_click(object sender, EventArgs e)
        {
            _pagecache.Clear();
            _pageno = 1;
            LoadThreadList();
        }

        //前一页
        private void Prev_Click(object sender, EventArgs e)
        {
            _pageno--;
            LoadThreadList();
        }

        //后一页
        private void Next_Click(object sender, EventArgs e)
        {
            _pageno++;
            LoadThreadList();
        }

        private void CheckBtnEnable()
        {
            //禁用工具栏按钮的方法
            (ApplicationBar.Buttons[2] as ApplicationBarIconButton).IsEnabled = (_pageno != (uint)1);
        }

 

        private void newpost_click(object sender, EventArgs e)
        {
            PopupContainer pc = new PopupContainer(this);
            pp = new PopupPost();
            pc.Show(pp);

            ApplicationBar = (Microsoft.Phone.Shell.ApplicationBar)Resources["post"];
        }

        private async void post_click(object sender, EventArgs e)
        {
            if (pp.contentTextBox.Text == string.Empty)
            {
                MessageBox.Show("请输入内容");
                return;
            }
            if (pp.titleTextBox.Text == string.Empty)
            {
                MessageBox.Show("请输入标题");
                return;
            }
            
            pgbar.Visibility = Visibility.Visible;
            bool bl = await BuAPI.PostThread(_fid, pp.titleTextBox.Text, pp.contentTextBox.Text);
            pgbar.Visibility = Visibility.Collapsed;

            if (bl)
            {
                MessageBox.Show("发布成功");
                pp.CloseMeAsPopup();
            }
            else
                MessageBox.Show("发布失败");

        }

        private void cancel_Click(object sender, EventArgs e)
        {
            pp.CloseMeAsPopup();
        }

    }
}

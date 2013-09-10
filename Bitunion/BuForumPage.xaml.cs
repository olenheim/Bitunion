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
        private static ForumViewModel _forumviewmodel = new ForumViewModel();

	//该论坛页面的fid以及论坛名称
        private string _fid,_forumname;
        
	//每一个页面的贴子缓存
        private Dictionary<uint,List<BuThread>> _pagecache;

	//当前页码
        private uint _pageno;
	#endregion

        public BuForumPage()
        {
            InitializeComponent();
            DataContext = _forumviewmodel;
            _pagecache = new Dictionary<uint,List<BuThread>>();
            _pageno = 1;
        }

        protected async override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //获取从父页面传递过来的fid与论坛名称
            NavigationContext.QueryString.TryGetValue("fid", out _fid);
            NavigationContext.QueryString.TryGetValue("fname", out _forumname);

            pivotItem1.Text = _forumname;
            LoadThreadList();
        }
        
	//异步加载帖子列表
        private async void LoadThreadList()
        {
            pgbar.Visibility = Visibility.Visible;
	    CheckBtnEnable();
	    //清除界面数据
            _forumviewmodel.ThreadItems.Clear();

	    //先从缓存中获取
        List<BuThread> threadlist;
	    if(!_pagecache.TryGetValue( _pageno, out threadlist))
            _pagecache[_pageno] = await BuAPI.QueryThreadList(_fid, ((_pageno - 1) * 20).ToString(), (_pageno * 20 - 1).ToString());
           
	    //填写视图模型
        foreach (BuThread bt in _pagecache[_pageno])
                _forumviewmodel.ThreadItems.Add(new ThreadViewModel(bt));

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
            (ApplicationBar.Buttons[1] as ApplicationBarIconButton).IsEnabled = (_pageno != (uint)1);
        }
    }
}

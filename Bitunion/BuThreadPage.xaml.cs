using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using HtmlAgilityPack;
using Bitunion.ViewModels;
using System.Security;

namespace Bitunion
{
    public partial class BitThreadPage : PhoneApplicationPage
    {

        #region 资源文件
        //LLS所绑定的帖子数据模型
        private ThreadViewModel _threadview = new ThreadViewModel();

        //HTML解析类
        private HtmlDocument _htmldoc = new HtmlDocument();

        //帖子的tid,名称,回复数,所在版块fid和fname
        private string _tid, _subject, _replies,_fid,_fname;

        //目前所在的帖子页面,以及最大的页面数
        private uint _currentpage = 1, _maxpage;

        //页面数据缓存
        private Dictionary<uint, List<BuPost>> _pagecache = new Dictionary<uint, List<BuPost>>();
        #endregion

        public BitThreadPage()
        {
            InitializeComponent();
            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;
            //设定数据上下文
            DataContext = _threadview;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //获取从父页面传递过来的tid
            NavigationContext.QueryString.TryGetValue("tid", out _tid);
            NavigationContext.QueryString.TryGetValue("subject", out _subject);
            NavigationContext.QueryString.TryGetValue("replies", out _replies);
            NavigationContext.QueryString.TryGetValue("fid", out _fid);
            NavigationContext.QueryString.TryGetValue("fname", out _fname);

            ThreadName.Text = _subject;
            _maxpage = Convert.ToUInt16(_replies) / (uint)10 + 1;
            ShowViewModel(_currentpage);
        }

        private async void ShowViewModel(uint pageno)
        {
            pgbar.Visibility = Visibility.Visible;
            CheckBtnEnable();
            //清除界面数据
            _threadview.PostItems.Clear();

            //先从缓存中获取
            List<BuPost> postlist;
            if (!_pagecache.TryGetValue(pageno, out postlist))
            {
                postlist = await BuAPI.QueryPost(_tid, ((pageno - 1) * 10).ToString(), (pageno * 10 - 1).ToString());
                _pagecache[pageno] = postlist;
            }

            if (postlist == null || postlist.Count == 0)
                return;
        
            //填写显示模型
            foreach (BuPost post in postlist)
            {
                _htmldoc.LoadHtml(Uri.UnescapeDataString(post.message));
                var node = _htmldoc.DocumentNode;
                DateTime dt = BuAPI.DateTimeConvertTime(post.dateline);

                //格式化时间”年-月-日 小时:分钟“
                string strtime = dt.ToString("yyyy-M-d HH:mm");
                _threadview.PostItems.Add(new PostViewModel() { Message = Uri.UnescapeDataString(node.InnerText), AddInfo = Uri.UnescapeDataString(post.author) + "  " + strtime });
            }
            pgbar.Visibility = Visibility.Collapsed;
        }

        private void reply_click(object sender, EventArgs e)
        {

        }

        private void Prev_Click(object sender, EventArgs e)
        {
            ShowViewModel(--_currentpage);
        }

        //置灰可能的翻页按钮
        private void Next_Click(object sender, EventArgs e)
        {
            ShowViewModel(++_currentpage);
        }

        private void CheckBtnEnable()
        {
            //禁用工具栏按钮的方法
            (ApplicationBar.Buttons[1] as ApplicationBarIconButton).IsEnabled = (_currentpage != (uint)1);
            (ApplicationBar.Buttons[2] as ApplicationBarIconButton).IsEnabled = (_currentpage != _maxpage);
        }

        private void Enter_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/BuForumPage.xaml?fid=" + _fid
           + "&fname=" + _fname, UriKind.Relative));
        }
    }
}

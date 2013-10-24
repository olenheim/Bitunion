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
using System.Windows.Data;
using System.Globalization;

namespace Bitunion
{
    public partial class BuThreadPage : PhoneApplicationPage
    {

        #region 资源文件
        //LLS所绑定的帖子数据模型
        private ThreadViewModel _threadview = new ThreadViewModel();

        //帖子的tid,名称,回复数,所在版块fid和fname
        private string _tid, _subject, _replies,_fid,_fname;

        //目前所在的帖子页面,以及最大的页面数
        private uint _currentpage = 1, _maxpage;

        //页面数据缓存
        private Dictionary<uint, List<BuPost>> _pagecache = new Dictionary<uint, List<BuPost>>();

        //父页面
        private string _parentpage;

        //回复所用的控件对象
        private PopupPost _popupreply;

        //引用、页码、楼层的文字模板
        private const string _quotetemplate = "[quote={0}][b]{1}[/b] {2}\r\n{3}[/quote]";
        private const string _pagetemplate = "({0}/{1})";
        private const string _floortempalte = "{0}楼";
        #endregion

        public BuThreadPage()
        {
            InitializeComponent();
            //设定数据上下文
            DataContext = _threadview;
            ApplicationBar = (Microsoft.Phone.Shell.ApplicationBar)Resources["thread"];
            _popupreply = new PopupPost();
            _popupreply.titleTextBox.Visibility = Visibility.Collapsed;
            //由于隐藏了title，所以缩小控件高度
            _popupreply.Height -= 72;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _parentpage = NavigationService.BackStack.First().Source.OriginalString;
            //获取从父页面传递过来的tid
            NavigationContext.QueryString.TryGetValue("tid", out _tid);
            NavigationContext.QueryString.TryGetValue("subject", out _subject);
            NavigationContext.QueryString.TryGetValue("replies", out _replies);
            NavigationContext.QueryString.TryGetValue("fid", out _fid);
            NavigationContext.QueryString.TryGetValue("fname", out _fname);
            
            _maxpage = Convert.ToUInt16(_replies) / (uint)BuSetting.PageThreadCount + 1;
            ShowViewModel(_currentpage);
        }

        private async void ShowViewModel(uint pageno)
        {
            _currentpage = pageno;
            ThreadName.Text = string.Format(_pagetemplate, pageno.ToString(), _maxpage.ToString()) + _subject;
            pgbar.Visibility = Visibility.Visible;
           // scrollViewer.ScrollToVerticalOffset(0);
            //清除界面数据
            _threadview.PostItems.Clear();

            //先从缓存中获取
            List<BuPost> postlist;
            if (!_pagecache.TryGetValue(pageno, out postlist))
            {
                (ApplicationBar.Buttons[1] as ApplicationBarIconButton).IsEnabled = false;
                (ApplicationBar.Buttons[2] as ApplicationBarIconButton).IsEnabled = false;
                (ApplicationBar.MenuItems[0] as ApplicationBarMenuItem).IsEnabled = false;
                (ApplicationBar.MenuItems[1] as ApplicationBarMenuItem).IsEnabled = false;

                postlist = await BuAPI.QueryPost(_tid, ((pageno - 1) * BuSetting.PageThreadCount).ToString(), 
                    (pageno * BuSetting.PageThreadCount).ToString());
                _pagecache[pageno] = postlist;
                CheckBtnEnable();
            }

            if (postlist == null || postlist.Count == 0)
                return;

            uint floorno = (pageno - 1) * BuSetting.PageThreadCount;
            //填写视图模型
            foreach (BuPost post in postlist)
                _threadview.PostItems.Add(new PostViewModel(post,++floorno));

            pgbar.Visibility = Visibility.Collapsed;
         
        }

        private void reply_click(object sender, EventArgs e)
        {
            PopupContainer pc = new PopupContainer(this);
            pc.Show(_popupreply);
            ApplicationBar = (Microsoft.Phone.Shell.ApplicationBar)Resources["reply"];
        }

        public static void callback_replay(string str)
        {
            MessageBox.Show(str);
        }

        private void Prev_Click(object sender, EventArgs e)
        {
            ShowViewModel(_currentpage-1);
            ThreadName.Text = string.Format(_pagetemplate, _currentpage.ToString(), _maxpage.ToString()) + _subject;
        }

        //置灰可能的翻页按钮
        private void Next_Click(object sender, EventArgs e)
        {
            ShowViewModel(_currentpage+1);
            ThreadName.Text = string.Format(_pagetemplate, _currentpage.ToString(), _maxpage.ToString()) + _subject;
        }

        private void CheckBtnEnable()
        {
            //禁用工具栏按钮的方法
            (ApplicationBar.Buttons[1] as ApplicationBarIconButton).IsEnabled = (_currentpage != (uint)1);
            (ApplicationBar.Buttons[2] as ApplicationBarIconButton).IsEnabled = (_currentpage != _maxpage);
            (ApplicationBar.MenuItems[0] as ApplicationBarMenuItem).IsEnabled = (_currentpage != (uint)1);
            (ApplicationBar.MenuItems[1] as ApplicationBarMenuItem).IsEnabled = (_currentpage != _maxpage);
        }

        private void Enter_Click(object sender, EventArgs e)
        {
            bool bl = NavigationService.CanGoBack;
            if (_parentpage != "/MainPage.xaml" && NavigationService.CanGoBack)
                NavigationService.GoBack();
            else
                NavigationService.Navigate(new Uri("/BuForumPage.xaml?fid=" + _fid
                  + "&fname=" + _fname, UriKind.Relative));
        }

          private void FirstPage_Click(object sender, EventArgs e)
          {
              ShowViewModel(1);
          }

          private void LastPage_Click(object sender, EventArgs e)
          {
              ShowViewModel(_maxpage);
          }

        private void OnImageTap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        private void Image_ImageFailed_1(object sender, ExceptionRoutedEventArgs e)
        {
    
        }

        private void Image_ImageOpened_1(object sender, RoutedEventArgs e)
        {

        }

        private void cancel_Click(object sender, EventArgs e)
        {          
            _popupreply.contentTextBox.Text = string.Empty;
            _popupreply.CloseMeAsPopup();
        }

        private async void post_click(object sender, EventArgs e)
        {
            if (_popupreply.contentTextBox.Text == string.Empty)
            {
                MessageBox.Show("请输入内容");
                return;
            }

            _popupreply.CloseMeAsPopup();
       
            pgbar.Visibility = Visibility.Visible;
            (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = false;
        
            bool bl = await BuAPI.ReplyPost(_tid,_popupreply.contentTextBox.Text);
       
            pgbar.Visibility = Visibility.Collapsed;
            (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = true;
         
            if(bl)
            {
                _popupreply.contentTextBox.Text = string.Empty;
                MessageBox.Show("回复成功");
                //todo 刷新，先简单做一下。
                _pagecache.Remove(_maxpage);
                ShowViewModel(_maxpage);
            }
            else
                MessageBox.Show("回复失败");
        }

        private void ItemsControl_DoubleTap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (PostItemsList.SelectedItem == null)
                return;
            PostViewModel ps = PostItemsList.SelectedItem as PostViewModel;
            BuPost post = ps._post;
            _popupreply.contentTextBox.Text = string.Format(_quotetemplate, post.pid, post.author, post.dateline, post.message);
            reply_click(null, null);
        }

        private void PostItemsList_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (PostItemsList.SelectedItem == null)
                return;
            PostViewModel ps = PostItemsList.SelectedItem as PostViewModel;
            BuPost post = ps._post;
            _popupreply.contentTextBox.Text = string.Format(_quotetemplate, post.pid, post.author, post.dateline, post.message);
            reply_click(null, null);
        }

    }
}

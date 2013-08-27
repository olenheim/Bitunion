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
        private static ThreadViewModel _threadview = new ThreadViewModel();

        //HTML解析类
        private HtmlDocument _htmldoc;

        //帖子的tid,名称以及回复数
        private string _tid,_subject,_replies;

        //目前所在的帖子页面,以及最大的页面数
        private uint _page,_maxpage;
        #endregion

        public BitThreadPage()
        {
            InitializeComponent();
            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;
            //设定数据上下文
            _threadview.PostItems.Clear();
            DataContext = _threadview;
            
            _htmldoc = new HtmlDocument();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //获取从父页面传递过来的tid
            NavigationContext.QueryString.TryGetValue("tid", out _tid);
            NavigationContext.QueryString.TryGetValue("subject", out _subject);
            NavigationContext.QueryString.TryGetValue("replies", out _replies);

            _page = 1;
            _maxpage = Convert.ToUInt16(_replies) / (uint)10 + 1;
            ShowViewModel();
        }

        private async void ShowViewModel()
        {
            _threadview.PostItems.Clear();
            List<BitPost> postlist = await BitAPI.QueryPost(_tid, ((_page-1) * 10).ToString(), (_page * 10 -1).ToString());
            if (postlist == null || postlist.Count == 0)
                return;

            foreach (BitPost post in postlist)
            {
                _htmldoc.LoadHtml(Uri.UnescapeDataString(post.message));
                var node = _htmldoc.DocumentNode;

                _threadview.PostItems.Add(new PostViewModel() { Message = Uri.UnescapeDataString(node.InnerText), AddInfo = Uri.UnescapeDataString(post.author) + "  " + post.dateline });
            }
            CheckBtnEnable();
        }

        private void reply_click(object sender, EventArgs e)
        {

        }

        private void Prev_Click(object sender, EventArgs e)
        {
            _page--;
            ShowViewModel();
        }

        private void Next_Click(object sender, EventArgs e)
        {
            _page++;
            ShowViewModel();
        }

        private void CheckBtnEnable()
        {
                (ApplicationBar.Buttons[1] as ApplicationBarIconButton).IsEnabled = (_page != (uint)1);
                (ApplicationBar.Buttons[2] as ApplicationBarIconButton).IsEnabled = (_page != _maxpage);
        }
    }
}

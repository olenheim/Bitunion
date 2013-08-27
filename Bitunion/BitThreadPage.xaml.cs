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

namespace Bitunion
{
    public partial class BitThreadPage : PhoneApplicationPage
    {

        #region 资源文件
        //LLS所绑定的帖子数据模型
        private static ThreadViewModel _threadview = new ThreadViewModel();

        //HTML解析类
        private HtmlDocument _htmldoc;

        //帖子的tid
        private string _tid;

        //目前存在的帖子页数
        private uint _page;
        #endregion

        public BitThreadPage()
        {
            InitializeComponent();
            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;
            //设定数据上下文
            _threadview.PostItems.Clear();
            DataContext = _threadview;
            
            _htmldoc = new HtmlDocument();
            _page = 0;
        }

        protected async override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //获取从父页面传递过来的tid
            NavigationContext.QueryString.TryGetValue("msg", out _tid);

            await AddViewModel();
            ThreadName.Text = Uri.UnescapeDataString(lp[0].subject);
        }

        private async void AddViewModel()
        {
            List<BitPost> postlist = await BitAPI.QueryPost(_tid, (_page * 10).ToString(), (_page * 10+9).ToString());
            if (postlist == null || postlist.Count == 0)
                return;

            foreach (BitPost post in postlist)
            {
                _htmldoc.LoadHtml(Uri.UnescapeDataString(post.message));
                var node = _htmldoc.DocumentNode;
                _tm.PostItems.Add(new PostViewModel() { Message = Uri.UnescapeDataString(node.InnerText), AddInfo = Uri.UnescapeDataString(post.author) + "  " + Uri.UnescapeDataString(post.dateline) });
            }

            _page++;
        }
    }
}

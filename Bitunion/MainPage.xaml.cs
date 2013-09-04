using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Bitunion.Resources;
using Bitunion.ViewModels;
using System.Collections.ObjectModel;

namespace Bitunion
{
    public partial class MainPage : PhoneApplicationPage
    {
        //主页VM
        private static MainViewModel _mainvm= new MainViewModel();
        
        // 构造函数
        public MainPage()
        {
            InitializeComponent();
            DataContext = _mainvm;
        }

        // 为 ViewModel 项加载数据
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (_mainvm.LatestThreadItems.Count == 0)
                LoadLatestThreadList();
        }

        //响应在最新帖子列表中选择某帖子的事件
        private void LongListSelector_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            // If selected item is null (no selection) do nothing
            if (LatestThreadViewList.SelectedItem == null)
                return;

            BitThreadModel item = LatestThreadViewList.SelectedItem as BitThreadModel;

            LatestThreadViewList.SelectedItem = null;

            var thread  = item.latestthread;
            
            // Navigate to the new page
            NavigationService.Navigate(new Uri("/BuThreadPage.xaml?tid=" + thread.tid
                + "&subject="+ thread.pname 
                + "&replies=" + thread.tid_sum
                + "&fid=" + thread.fid
                + "&fname=" + thread.fname
                , UriKind.Relative));
        }
        
        //异步加载最新帖子列表
        private async void LoadLatestThreadList()
        {
            pgBar.Visibility = Visibility.Visible;
            bool bl = await BuAPI.Login("泪沸腾", "bitwdazsc");
            List<BuLatestThread> btl = await BuAPI.QueryLatestThreadList();

            foreach (BuLatestThread bt in btl)
                _mainvm.LatestThreadItems.Add(new BitThreadModel(bt));
            pgBar.Visibility = Visibility.Collapsed;
        }
        
        //异步加载论坛列表
        private async void LoadForumList()
        {
            pgBar.Visibility = Visibility.Visible;
          List<BuGroupForum> bl = await BuAPI.QueryForumList();

          foreach (BuGroupForum bt in bl)
          {
              if (bt.main == null)
                  continue;
              foreach (BuForum btt in bt.main)
              {
                  _mainvm.ForumItems.Add(new ForumViewModel(btt));
              }
              
          }
            

            pgBar.Visibility = Visibility.Collapsed;
        }
        
        //刷新最新的帖子列表
        private void refresh ()
        {
            _mainvm.LatestThreadItems.Clear();
            LoadLatestThreadList();
        }
        
        //响应论坛列表选择进入某一个论坛的事件
         private void ForumListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         //   if (ThreadViewList.SelectedItem == null)
         //       return;

        //    ForumModelModel item = ForumViewList.SelectedItem as ForumModelModel;

         //   ForumViewList.SelectedItem = null;

           // var forum = item.forum;

            // Navigate to the new page
         //   NavigationService.Navigate(new Uri("/BuForumPage.xaml?fid=" + forum.fid
         //       + "fname=" + forum.naem
          //      , UriKind.Relative));
        }

        //点击刷新按钮
         private void refresh_click(object sender, EventArgs e)
         {
             refresh();
         }

        //切换Tab页面
         private void MainPagePivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {
             //如果切换到论坛分组的页面才加载论坛分组列表
             if (MainPagePivot.SelectedIndex == 1)
             {
                 if (_mainvm.ForumItems.Count() == 0)
                    LoadForumList();
             }
         }

        // 用于生成本地化 ApplicationBar 的示例代码
        //private void BuildLocalizedApplicationBar()
        //{
        //    // 将页面的 ApplicationBar 设置为 ApplicationBar 的新实例。
        //    ApplicationBar = new ApplicationBar();

        //    // 创建新按钮并将文本值设置为 AppResources 中的本地化字符串。
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // 使用 AppResources 中的本地化字符串创建新菜单项。
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}

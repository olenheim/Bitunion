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
        //论坛最新帖子VM对象列表
        private static ObservableCollection<BitThreadModel> LatestThreadItems;
        
        //论坛VM对象列表
        private static ObservableCollection<ForumViewModel> ForumItems;
        
        // 构造函数
        public MainPage()
        {
            InitializeComponent();

            // 将 listbox 控件的数据上下文设置为示例数据
            //DataContext = App.ViewModel;

            // 用于本地化 ApplicationBar 的示例代码
            //BuildLocalizedApplicationBar();
        }

        // 为 ViewModel 项加载数据
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            pgBar.Visibility = Visibility.Visible;
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
            pgBar.Visibility = Visibility.Collapsed;
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
            
        }
        
        //异步加载论坛列表
        private async void LoadForumList()
        {
        
        }
        
        //刷新最新的帖子列表
        private void refresh ()
        {
            
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

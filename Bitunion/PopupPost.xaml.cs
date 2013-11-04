using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Bitunion
{
    public partial class PopupPost : UserControl
    {
        private Popup gPopupControl;
        private string board;
        private int reid = -1;
        public string Title { get; set; }
        public event EventHandler closeEventHander;

        //调用该页面的类
        private const string BUTHREADPAGE = "Bitunion.BuThreadPage";
        private const string BUFORUMPAGE = "Bitunion.BuForumPage";
        

        //内容
        public static string strcontent = null;

        public bool IsOpen
        {
            get
            {
                if (gPopupControl != null)
                {
                    return gPopupControl.IsOpen;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value == true)
                {
                    //pass
                }
                else
                {
                    QuitReply();
                }
            }
        }
        private string _id;
        public PopupPost()
        {     
            InitializeComponent();


       
        }


        public void Close()
        {
            this.CloseMeAsPopup();
        }

        public void ShowReply(string boardName, string article)
        {
            this.board = boardName;
            titleTextBox.Focus();

            this.Title = "发表文章：" + boardName;

            //if (article != null)
            //{
            //    titleTextBox.Text = "Re: " + article.Title;
            //    contentTextBox.Text = "\n【 在 " + article.Id + " 的大作中提到: 】\n: " + GetReplyContent(article.Content);
            //    reid = article.Id;
            //    contentTextBox.Focus();
            //    this.Title = "回复：" + article.Title;
            //}

            this.DataContext = this;

            gPopupControl = new Popup();
            gPopupControl.Child = this;
            gPopupControl.IsOpen = true;
        }

        private string GetReplyContent(string content)
        {
            string newContent = "";

            if (content.Length > 100)
                newContent = content.Substring(0, 100) + "... ...";
            else
                newContent = content;

            newContent = newContent.Replace("\n", "\n: ");

            return newContent;
        }

        private void OnClickCancel(object sender, RoutedEventArgs e)
        {
            QuitReply();
        }

        private void OnClickReply(object sender, RoutedEventArgs e)
        {
      
            string title = titleTextBox.Text;
            
            
            


            this.CloseMeAsPopup();
            //Article.PostArticle(board, title, content, reid, ReplySuccess, ReplyFailure);
        }

        public void ReplySuccess()
        {
            MessageBox.Show("发布成功！");
            QuitReply();
        }

        public void ReplyFailure(string f)
        {
            MessageBox.Show("发布失败！");
        }

        public void QuitReply()
        {
            gPopupControl.IsOpen = false;
            gPopupControl.Child = null;
            this.gPopupControl = null;
            EventHandler tHandler = this.closeEventHander;
            if (tHandler != null)
                tHandler.Invoke(this, null);
        }
    }
}
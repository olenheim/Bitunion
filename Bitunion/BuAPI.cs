﻿using HtmlAgilityPack;
using HttpLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Bitunion
{
    #region 论坛相关自定义对象

    public struct BuGroupForum
    {
        public string type;
        public string fid;
        public string name;
        public List<BuForum> main;
        public List<BuForum> sub;
    }

    public struct BuForum
    {
        public string type;
        public string fid;
        public string fup;
        public string name;
        public string icon;
        public string description;
        public string moderator;
        public string threads;
        public string posts;
        public string onlines;
    }

    public struct BuThread
    {
        public string tid;
        public string author;
        public string authorid;
        public string subject;
        public string dateline;
        public string lastpost;
        public string lastposter;
        public string vies;
        public string replies;
    }

    public struct BuPost
    {
        public string pid;
        public string fid;
        public string tid;
        public string aid;
        public string icon;
        public string author;
        public string authorid;
        public string subject;
        public string dateline;
        public string message;
        public string usesig;
        public string bbcodeoff;
        public string smileyoff;
        public string parseurloff;
        public string score;
        public string rate;
        public string ratetimes;
        public string pstatus;
        public string lastedit;
        public string postsource;
        public string aaid;
        public string creditsrequire;
        public string filetype;
        public string filename;
        public string attachment;
        public string filesize;
        public string downloads;
        public string uid;
        public string username;
        public string avatar;
        public string epid;
        public string maskpost;
    }

    class BuUserProfile
    {
        public string uid;
        public string status;
        public string username;
        public string avatar;
        public string credit;
        public string regdate;
        public string lastvisit;
        public string bday;
        public string signature;
        public string postnum;
        public string threadnum;
        public string email;
        public string site;
        public string icq;
        public string oicq;
        public string yahoo;
        public string msn;
    }

    public struct lastreplay
    {
        public string when;
        public string who;
        public string what;
    }

    public struct BuLatestThread
    {
        public string pname;
        public string fname;
        public string author;
        public string tid;
        public string tid_sum;
        public string fid;
        public string fid_sum;
        public List<lastreplay> lastreplay;
    }

    public struct BuQuote
    {
        public string author;
        public string time;
        public string content;
    }

    #endregion

    class BuAPI
    {
        #region Resources
        //保存登陆后的session
        static string _session;

        //Http库，用于进行Post操作
        static HttpEngine _httphelper = new HttpEngine();

        //保存账号密码，用于登出操作
        static string _name, _password;

        //用于保存获取的论坛列表
        static Dictionary<string, BuForum> _forumList;

        //重登录机制
        static bool relogin = false;

        #endregion

        //登陆，成功后初始化session
        public static async Task<bool> Login(string name, string password)
        {
            JObject staff = new JObject();
            staff.Add(new JProperty("action", "login"));
            staff.Add(new JProperty("username", Uri.EscapeDataString(name)));
            staff.Add(new JProperty("password", password));
            string LoginContext = staff.ToString();

            Stream response = await _httphelper.PostAsync(BuSetting.URL + "bu_logging.php", LoginContext);
            if (response == null || response.Length == 0)
                return false;

            StreamReader reader = new StreamReader(response);
            string strret = reader.ReadToEnd();
            JObject jsonobj = JObject.Parse(strret);
            if (jsonobj["result"].ToString() != "success")
            {
                MessageBox.Show("用户名或密码错误，请重新输入");
                return false;
            }


            _session = jsonobj["session"].ToString();
            if (_session == null)
                return false;

            //保存账户名及密码，用于登出
            _name = name;
            _password = password;

            return true;
        }

        //注销，成功后置空session
        public static async Task<bool> Logout()
        {
            JObject staff = new JObject();
            staff.Add(new JProperty("action", "logout"));
            staff.Add(new JProperty("username", Uri.EscapeDataString(_name)));
            staff.Add(new JProperty("password", _password));
            staff.Add(new JProperty("session", _session));
            string LogoutContext = staff.ToString();

            Stream response = await _httphelper.PostAsync(BuSetting.URL + "bu_logging.php", LogoutContext);
            if (response == null || response.Length == 0)
            {
                _session = string.Empty;
                return false;
            }

            //直接返回是否成功登出的状态
            JObject jsonret = null;
            return StreamToJobjAndCheckState(response, ref  jsonret);
        }

        //将流直接翻译为Json对象，并判断result字段是否为success
        public static bool StreamToJobjAndCheckState(Stream stream, ref JObject jsonobj)
        {
            StreamReader reader = new StreamReader(stream);
            string strret = reader.ReadToEnd();
            jsonobj = JObject.Parse(strret);
            if (jsonobj["result"].ToString() != "success")
                return false;
            return true;
        }

        //查询论坛列表
        public static async Task<List<BuGroupForum>> QueryForumList()
        {
            JObject staff = new JObject();
            staff.Add(new JProperty("action", "forum"));
            staff.Add(new JProperty("username", Uri.EscapeDataString(_name)));
            staff.Add(new JProperty("session", _session));
            string Context = staff.ToString();

            Stream response = await _httphelper.PostAsync(BuSetting.URL + "bu_forum.php", Context);
            if (response == null || response.Length == 0)
                return null;

            JObject jsonret = null;
            if (!StreamToJobjAndCheckState(response, ref jsonret))
            {
                if (jsonret["msg"].ToString() == "IP+logged" && await Login(_name, _password))
                    return await QueryForumList();
                else
                    return null;
            }

            List<BuGroupForum> BuGroupForumList = new List<BuGroupForum>(); int i = 0;
            foreach (JToken forumgroup in JObject.Parse(jsonret["forumslist"].ToString()).Values())
            {
                if (i == 6)
                    break;

                foreach (JToken forum in JObject.Parse(forumgroup.ToString()).Values())
                    BuGroupForumList.Add(JsonConvert.DeserializeObject<BuGroupForum>
                        (JObject.Parse(forum.ToString()).ToString()));
                i++;
            }

            return BuGroupForumList;


        }

        //查询某特定论坛的帖子列表
        public static async Task<List<BuThread>> QueryThreadList(string fid, string start, string end)
        {
            JObject staff = new JObject();
            staff.Add(new JProperty("action", "thread"));
            staff.Add(new JProperty("username", Uri.EscapeDataString(_name)));
            staff.Add(new JProperty("session", _session));
            staff.Add(new JProperty("fid", fid));
            staff.Add(new JProperty("from", start));
            staff.Add(new JProperty("to", end));
            string Context = staff.ToString();

            Stream response = await _httphelper.PostAsync(BuSetting.URL + "bu_thread.php", Context);
            if (response == null || response.Length == 0)
                return null;

            JObject jsonret = null;
            if (!StreamToJobjAndCheckState(response, ref jsonret))
            {
                if (jsonret["msg"].ToString() == "IP+logged" && await Login(_name, _password))
                    return await QueryThreadList(fid, start, end);
                else
                    return null;
            }

            return JsonConvert.DeserializeObject<List<BuThread>>(jsonret["threadlist"].ToString());
        }

        //查询某特定帖子的详情
        public static async Task<List<BuPost>> QueryPost(string tid, string start, string end)
        {
            JObject staff = new JObject();
            staff.Add(new JProperty("action", "post"));
            staff.Add(new JProperty("username", Uri.EscapeDataString(_name)));
            staff.Add(new JProperty("session", _session));
            staff.Add(new JProperty("tid", tid));
            staff.Add(new JProperty("from", start));
            staff.Add(new JProperty("to", end));
            string Context = staff.ToString();

            Stream response = await _httphelper.PostAsync(BuSetting.URL + "bu_post.php", Context);
            if (response == null || response.Length == 0)
                return null;

            JObject jsonret = null;
            if (!StreamToJobjAndCheckState(response, ref jsonret))
            {
                if (jsonret["msg"].ToString() == "IP+logged" && await Login(_name, _password))
                    return await QueryPost(tid, start, end);
                else
                    return null;
            }

            return JsonConvert.DeserializeObject<List<BuPost>>(jsonret["postlist"].ToString());
        }

        //查询特定用户信息
        public static async Task<BuUserProfile> QueryUserProfile(string uid)
        {
            JObject staff = new JObject();
            staff.Add(new JProperty("action", "profile"));
            staff.Add(new JProperty("username", Uri.EscapeDataString(_name)));
            staff.Add(new JProperty("session", _session));
            staff.Add(new JProperty("uid", uid));
            string Context = staff.ToString();

            Stream response = await _httphelper.PostAsync(BuSetting.URL + "bu_profile.php", Context);
            if (response == null || response.Length == 0)
                return null;

            JObject jsonret = null;
            if (!StreamToJobjAndCheckState(response, ref jsonret))
            {
                if (jsonret["msg"].ToString() == "IP+logged" && await Login(_name, _password))
                    return await QueryUserProfile(uid);
                else
                    return null;
            }

            return JsonConvert.DeserializeObject<BuUserProfile>(jsonret["BuUserProfile"].ToString());
        }

        //回复帖子
        public static async Task<bool> ReplyPost(string tid, string msg)
        {
            //增加消息尾巴
            if (BuSetting.ShowTail)
                msg += "\r\n[i]" + BuSetting.TailMsg + "[/i]";

            JObject staff = new JObject();
            staff.Add(new JProperty("action", "newreply"));
            staff.Add(new JProperty("username", Uri.EscapeDataString(_name)));
            staff.Add(new JProperty("session", _session));
            staff.Add(new JProperty("tid", tid));
            staff.Add(new JProperty("message", Uri.EscapeDataString(msg)));
            staff.Add(new JProperty("attachment", 1));
            string Context = staff.ToString();

            Stream response = await _httphelper.PostFormAsync(BuSetting.URL + "bu_newpost.php", Context);
            if (response == null || response.Length == 0)
                return false;

            JObject jsonret = null;
            if (!StreamToJobjAndCheckState(response, ref jsonret))
            {
                if (jsonret["msg"].ToString() == "IP+logged" && await Login(_name, _password))
                    return await ReplyPost(tid, msg);
                else
                    return false;
            }

            return true;
        }

        //发表新帖子
        public static async Task<bool> PostThread(string fid, string subject, string msg)
        {
            //增加消息尾巴
            if (BuSetting.ShowTail)
                msg += "\r\n[i]" + BuSetting.TailMsg + "[/i]";

            JObject staff = new JObject();
            staff.Add(new JProperty("action", "newthread"));
            staff.Add(new JProperty("username", Uri.EscapeDataString(_name)));
            staff.Add(new JProperty("session", _session));
            staff.Add(new JProperty("fid", fid));
            staff.Add(new JProperty("subject", Uri.EscapeDataString(subject)));
            staff.Add(new JProperty("message", Uri.EscapeDataString(msg)));
            staff.Add(new JProperty("attachment", 1));
            string Context = staff.ToString();

            Stream response = await _httphelper.PostFormAsync(BuSetting.URL + "bu_newpost.php", Context);
            if (response == null || response.Length == 0)
                return false;

            JObject jsonret = null;
            if (!StreamToJobjAndCheckState(response, ref jsonret))
            {
                if (jsonret["msg"].ToString() == "IP+logged" && await Login(_name, _password))
                    return await PostThread(fid, subject, msg);
                else
                    return false;
            }

            return true;
        }

        //获取回复字串重的引用对象集合
        public static List<BuQuote> parseQuotes(ref string message)
        {
            Regex rx = new Regex(@"<br><br><center><table border=\""0\"" width=\""90%\"".*?bgcolor=\""ALTBG2\""><b>([\s\S]*?)</b> (\d{4}-\d{1,2}-\d{1,2} \d{1,2}:\d{1,2}[\s\S]*?)<br />([\s\S]*?)</td></tr></table></td></tr></table></center><br>",
                RegexOptions.IgnoreCase);

            List<BuQuote> quotes = new List<BuQuote>();

            MatchCollection ms = rx.Matches(message);

            foreach (Match m in ms)
            {
                // 1: author; 2:time; 3:content
                quotes.Add(new BuQuote() { author = m.Groups[1].Value, time = m.Groups[2].Value, content = parseHTML(m.Groups[3].Value) });
                message = message.Replace(m.Groups[0].Value, "");
            }
            return quotes;
        }

        //解析HTML编码中的字符串
        public static string parseHTML(string htmlstr)
        {
            HtmlDocument htmldoc = new HtmlDocument();
            htmldoc.LoadHtml(htmlstr);
            var node = htmldoc.DocumentNode;
            string ret = HttpUtility.HtmlDecode(node.InnerText);
            ret = ret.Replace("..:: From BIT-Union Open API Project ::..", "\r\n..:: From BIT-Union Open API Project ::..");
            ret = ret.Replace("请仔细阅读发帖须知及本版版规，确认无误后删除本默认内容", "");
            return ret;
        }

        //查询最新的帖子列表
        public static async Task<List<BuLatestThread>> QueryLatestThreadList()
        {
            JObject staff = new JObject();
            staff.Add(new JProperty("username", Uri.EscapeDataString(_name)));
            staff.Add(new JProperty("session", _session));
            string Context = staff.ToString();

            Stream response = await _httphelper.PostAsync(BuSetting.URL + "bu_home.php", Context);
            if (response == null || response.Length == 0)
                return null;

            JObject jsonret = null;
            if (!StreamToJobjAndCheckState(response, ref jsonret))
            {
                if (jsonret["msg"].ToString() == "IP+logged" && await Login(_name, _password))
                    return await QueryLatestThreadList();
                else
                    return null;
            }

            return JsonConvert.DeserializeObject<List<BuLatestThread>>(jsonret["newlist"].ToString());
        }

        //将PHP时间戳格式转换为DateTime格式
        public static DateTime DateTimeConvertTime(string strtime)
        {
            long time = Convert.ToInt64(strtime);
            DateTime timeStamp = new DateTime(1970, 1, 1);  //得到1970年的时间戳
            long t = (time + 8 * 60 * 60) * 10000000 + timeStamp.Ticks;
            DateTime dt = new DateTime(t);
            return dt;
        }

        //
        public static ImageSource GetImageSrc(string pid)
        {
            if (BuSetting.ShowPhoto)
                return new BitmapImage(new Uri(BuSetting.URL.Replace("open_api/", "") + pid));
            else
                return null;
        }


    }
}

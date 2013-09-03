using HttpLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitunion
{
    public struct BuRealForum
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

    struct BuPost
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

   public  struct lastreplay
    {
        public string when;
        public string who;
        public string what;
    }

    public struct  BuLatestThread
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

    class BuAPI
    {
        #region Resources
        //保存登陆后的session
        static string _session;

        //保存访问联盟论坛的前缀地址
        static string _url = "http://out.bitunion.org/open_api/";

        //Http库，用于进行Post操作
        static HttpEngine _httphelper = new HttpEngine();

        //保存账号密码，用于登出操作
        static string _name, _password;

        //用于保存获取的论坛列表
        static Dictionary<string ,BuForum> _forumList;

        #endregion

        //登陆，成功后初始化session
        public static async Task<bool> Login(string name, string password)
        {
            JObject staff = new JObject();
            staff.Add(new JProperty("action", "login"));
            staff.Add(new JProperty("username", Uri.EscapeDataString(name)));
            staff.Add(new JProperty("password", password));
            string LoginContext = staff.ToString();

            Stream response = await _httphelper.PostAsync(_url + "bu_logging.php", LoginContext);
            if (response.Length == 0)
                return false;

            JObject jsonret = null;
            if(!StreamToJobjAndCheckState(response,ref jsonret))
                return false;
      
            _session = jsonret["session"].ToString();
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
            
            Stream response = await _httphelper.PostAsync(_url + "bu_logging.php", LogoutContext);
            if (response.Length == 0)
                return false;

            //直接返回是否成功推出的状态
            JObject jsonret = null;
            return StreamToJobjAndCheckState(response,ref  jsonret);    
        }

        //将流直接翻译为Json对象，并判断result字段是否为success
        public static bool StreamToJobjAndCheckState(Stream stream, ref JObject jsonobj)
        {
            StreamReader reader = new StreamReader(stream);
            string strret = reader.ReadToEnd();
            jsonobj = JObject.Parse(strret);
            return jsonobj["result"].ToString() == "success";
        }

        //查询论坛列表
        public static async Task<Dictionary<string, string>> QueryForumList()
        {
            JObject staff = new JObject();
            staff.Add(new JProperty("action", "forum"));
            staff.Add(new JProperty("username", Uri.EscapeDataString(_name)));
            staff.Add(new JProperty("session", _session));
            string Context = staff.ToString(); 

            Stream response = await _httphelper.PostAsync(_url + "bu_forum.php", Context);
            if (response.Length == 0)
                return null;

            JObject jsonret = null;
            if(!StreamToJobjAndCheckState(response,ref jsonret))
                return null;


            return JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonret["forumslist"].ToString());
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

            Stream response = await  _httphelper.PostAsync(_url + "bu_thread.php", Context);
            if (response.Length == 0)
                return null;
            
            JObject jsonret = null;
            if(!StreamToJobjAndCheckState(response,ref jsonret))
                return null;

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

            Stream response = await _httphelper.PostAsync(_url + "bu_post.php", Context);
            if (response.Length == 0)
                return null;

            JObject jsonret = null;
            if (!StreamToJobjAndCheckState(response, ref jsonret))
                return null;

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

            Stream response = await _httphelper.PostAsync(_url + "bu_profile.php", Context);
            if (response.Length == 0)
                return null;

            JObject jsonret = null;
            if (!StreamToJobjAndCheckState(response, ref jsonret))
                return null;

            return JsonConvert.DeserializeObject<BuUserProfile>(jsonret["BuUserProfile"].ToString());
        }

        //回复帖子
        public static bool ReplyPost(string tid, string msg)
       {
           return true;
       }

        //发表新帖子
        public static bool PostThread(string fid, string subject, string msg)
       {
           return true;
       }

        //查询最新的帖子列表
        public static async Task<List<BuLatestThread>> QueryLatestThreadList()
       {
           JObject staff = new JObject();
           staff.Add(new JProperty("username", Uri.EscapeDataString(_name)));
           staff.Add(new JProperty("session", _session));
           string Context = staff.ToString();

           Stream response = await _httphelper.PostAsync(_url + "bu_home.php", Context);
           if (response.Length == 0)
               return null;

           JObject jsonret = null;
           if (!StreamToJobjAndCheckState(response, ref jsonret))
               return null;

           return JsonConvert.DeserializeObject<List<BuLatestThread>>(jsonret["newlist"].ToString());
       }

        //将PHP时间戳格式转换为DateTime格式
        public static DateTime  DateTimeConvertTime(string  strtime)
        {
            long time = Convert.ToInt64(strtime);
            DateTime timeStamp = new DateTime(1970, 1, 1);  //得到1970年的时间戳
            long t = (time + 8 * 60 * 60) * 10000000 + timeStamp.Ticks;
            DateTime dt = new DateTime(t);
            return dt;
        }


    }
}

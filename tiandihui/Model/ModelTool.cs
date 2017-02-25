using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using PublicHelp;
using System.Text.RegularExpressions;
using System.Web;

namespace Model
{

    #region 表单验证
    public class EmailAttribute : RegularExpressionAttribute
    { 
        public EmailAttribute() :base(@"^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$")
        {
    
        }
    }

    public class MobileAttribute : RegularExpressionAttribute
    {
        public MobileAttribute(): base(@"^1\d{10}$")
        { 
        
        }
    }

    public class LettersAttribute : RegularExpressionAttribute
    {
        public LettersAttribute()
            : base(@"^[a-zA-Z0-9_]+$")
        { 
        
        }
    }

    public class NumberAttribute : RegularExpressionAttribute
    {
        public NumberAttribute()
            : base(@"^[0-9]+(?([.])[.]{1}[0-9]+|[0-9]*)$")
        { 
        
        }
    
    }

    public class PlusOrZeroAttribute : RegularExpressionAttribute
    {
        public PlusOrZeroAttribute()
            : base(@"^([1-9]\d*|[0])$")
        { 
        
        }
    }

    public class PlusAttribute : RegularExpressionAttribute
    {
        public PlusAttribute()
            : base(@"^[1-9]\d*$")
        { 
        
        }
    
    }

    public class DateAttribute : RegularExpressionAttribute
    {
        public DateAttribute()
            : base(@"^\d{4}-\d{2}-\d{2}$")
        { 
        
        }
    }

    #region 

    /// <summary>
    ///Check 的摘要说明
    ///未通过验证返回 false,否则返回true
    /// </summary>
    public class Check
    {

        private static string[] pattern = { 
                         @"^\d{4}-\d{2}-\d{2}$",
                         @"^([0]{0,}[1-9]{1}|[0]{0,}[1]{1}[0-9]{1}|[2]{1}[0-3]{1}):([0-5]{1}[0-9]{0,1})|([6-9]{1})$",
                         @"^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$",
                         @"^[1-9]\d*$",
                         @"^([1-9]\d*|[0])$",
                         @"^[0-9]+(?([.])[.]{1}[0-9]+|[0-9]*)$",                           //@"^([0-9]\d*\.\d+)|([1-9]\d*|0)$", @"^[0-9]+[.]?[0-9]+$"  //此正则有问题，暂时不用
                         @"^[a-zA-Z0-9_]+$",
                         @"^(^\s*)|(\s*$)$",
                         @"^1\d{10}$"
                       };

        public static bool check(CheckType type, params string[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (!Regex.IsMatch(value[i], pattern[(int)type]))
                {
                    return false;
                }

            }
            return true;



        }
    }

    public enum CheckType
    {
        date,
        time,
        eMail,
        plus,   //正数
        plusOrZero,  //非负整数
        number,
        letters,     //字母 数字 下划线
        empty,
        mobile

    }

    #endregion

    #endregion

    public class ModelTool
    {

        
        public static List<string> getErrors(ModelStateDictionary dic)
        {
            if (dic != null)
            {
                List<string> lstStr = new List<string>();
                var keys = dic.Keys;
                foreach(var key in keys)
                {
                    if (dic[key].Errors.Count > 0)
                    {
                        ModelErrorCollection fieldErrors = dic[key].Errors;
                        var emErrors = fieldErrors.GetEnumerator();
                        while (emErrors.MoveNext())
                        {
                            lstStr.Add(emErrors.Current.ErrorMessage);
                        }
                       
                    }
                }

                return lstStr;
            }

            return null;
        }

        #region 获取id
        /// <summary>  
        /// 获取id 
        /// </summary>  
        /// <typeparam name="T">泛型</typeparam>   
        /// <returns>id</returns> 
        public static string GetId<T>()
            where T : class
        {
            TiDiHuiEntities db = new TiDiHuiEntities();
            var id = DateTime.Now.ToString("yyyyMMddhhmmss") + Guid.NewGuid().ToString("N");
            var isRepeat = db.Set<T>().Find(id);
            var isSuccess = true;
            if (isRepeat != null) // id已经存在
            {
                id = DateTime.Now.ToString("yyyyMMddhhmmss") + Guid.NewGuid().ToString("N");
                var i = 0;
                while (db.Set<T>().Find(id) != null) //id存在
                {
                    if (i == 5)
                    {
                        isSuccess = false;
                        break;
                    }
                    id = DateTime.Now.ToString("yyyyMMddhhmmss") + Guid.NewGuid().ToString("N");
                    i++;
                }
                if (isSuccess)
                {
                    return id;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return id;
            }
        }
        #endregion
    
    }



    public class SendMessage
    {
        RedisDb rdb = new RedisDb();

        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public string getMessageCode(string mobile,string message)
        {
            DateTime now = DateTime.Now;

           string Oldcode = rdb.Get(mobile);
           if (!string.IsNullOrEmpty(Oldcode))
           {
               return "-1"; //验证码未过期
           }

           string code = Assistant.getMessageCode(6);
           message = string.Format(message,code);

           if (true) //短信验证码发送成功
           {
               if (rdb.Set(mobile, code, DateTime.Now.AddMinutes(5)))//记录验证码成功
               {
                   return code;
               }
           }
           return "-2";

        
        }
        /// <summary>
        /// 短信验证码是否有效
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public int isCodeValid(string mobile,string userCode)
        {
            string code = rdb.Get(mobile);
            if (string.IsNullOrEmpty(code))
            {
                return -1; //验证码已过期
            }
            else if (userCode != code)
            {
                return -2; //验证码错误
            }
            return 1;
        }
    }

    public class run
    {

        #region 执行添加、修改、删除的sql，返回模型
        /// <summary>
        /// 执行添加、修改、删除的sql，返回模型
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>PublicResult.pb_execute</returns>
        public PublicResult.pb_execute exac(string sql, DbParameter[] args)
        {
            TiDiHuiEntities dbContext = new TiDiHuiEntities();
            PublicResult.pb_execute rs = new PublicResult.pb_execute();
            int rowCount = 0;
            rowCount = dbContext.Database.ExecuteSqlCommand(sql, args);
            if (rowCount == 0)
            {
                rs.success = false;
                rs.result = "操作失败";
            }
            else
            {
                rs.success = true;
                rs.result = "操作成功";
            }
            return rs;
        }
        #endregion

      

        #region 分页查询 + 条件查询 + 排序
        /// <summary>  
        /// 分页查询 + 条件查询 + 排序  
        /// </summary>  
        /// <typeparam name="Tkey">泛型</typeparam>  
        /// <param name="pageSize">每页大小</param>  
        /// <param name="pageIndex">当前页码</param>  
        /// <param name="total">总数量</param>  
        /// <param name="where">查询条件</param>  
        /// <param name="order">排序条件</param>  
        /// <param name="isAsc">是否升序</param>  
        /// <returns>IQueryable 泛型集合</returns> 
        public List<dynamic> getPageDateW<T, TKey>(Expression<Func<T, dynamic>> select, Expression<Func<T, bool>> where, Expression<Func<T, TKey>> order, int pageIndex, bool isAsc, int pageSize, out int Total)
            where T : class
        {
            if (isAsc)
            {
                TiDiHuiEntities db = new TiDiHuiEntities();
                Total = db.Set<T>().AsNoTracking().Where(where).Count();
                var list = db.Set<T>().AsNoTracking().Where(where).OrderBy(order).Select(select).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                return list.ToList();
            }
            else
            {
                TiDiHuiEntities db = new TiDiHuiEntities();
                Total = db.Set<T>().AsNoTracking().Where(where).Count();
                var list = db.Set<T>().AsNoTracking().Where(where).OrderByDescending(order).Select(select).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                return list.ToList();
            }
        }
        #endregion

        #region 分页查询 + 排序
        /// <summary>  
        /// 分页查询 + 排序  
        /// </summary>  
        /// <typeparam name="Tkey">泛型</typeparam>  
        /// <param name="pageSize">每页大小</param>  
        /// <param name="pageIndex">当前页码</param>  
        /// <param name="total">总数量</param>  
        /// <param name="order">排序条件</param>  
        /// <param name="isAsc">是否升序</param>  
        /// <returns>IQueryable 泛型集合</returns> 
        public List<dynamic> getPageDate<T, TKey>(Expression<Func<T, dynamic>> select, Expression<Func<T, TKey>> order, int pageIndex, bool isAsc, int pageSize, out int Total)
            where T : class
        {
            if (isAsc)
            {
                TiDiHuiEntities db = new TiDiHuiEntities();
                Total = db.Set<T>().AsNoTracking().Count();
                var list = db.Set<T>().AsNoTracking().OrderBy(order).Select(select).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                return list.ToList();
            }
            else
            {
                TiDiHuiEntities db = new TiDiHuiEntities();
                Total = db.Set<T>().AsNoTracking().Count();
                var list = db.Set<T>().AsNoTracking().OrderByDescending(order).Select(select).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                return list.ToList();
            }
        }
        #endregion

     

        #region 获取id
        /// <summary>  
        /// 获取id 
        /// </summary>  
        /// <typeparam name="T">泛型</typeparam>   
        /// <returns>id</returns> 
        public string GetId<T>()
            where T : class
        {
            TiDiHuiEntities db = new TiDiHuiEntities();
            var id = DateTime.Now.ToString("yyyyMMddhhmmss") + Guid.NewGuid().ToString("N");
            var isRepeat = db.Set<T>().Find(id);
            var isSuccess = true;
            if (isRepeat != null) // id已经存在
            {
                id = DateTime.Now.ToString("yyyyMMddhhmmss") + Guid.NewGuid().ToString("N");
                var i = 0;
                while (db.Set<T>().Find(id) != null) //id存在
                {
                    if (i == 5)
                    {
                        isSuccess = false;
                        break;
                    }
                    id = DateTime.Now.ToString("yyyyMMddhhmmss") + Guid.NewGuid().ToString("N");
                    i++;
                }
                if (isSuccess)
                {
                    return id;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return id;
            }
        }
        #endregion
    }


    public class AuthorityManager
    {
        TDHAuthorityEntities athtyDb = new TDHAuthorityEntities();

        /// <summary>
        /// 获取角色下的功能菜单
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public List<string> getMenus(string roleID)
        {
            RedisDb rsdb = new RedisDb();
            List<string> menus = null;
            menus = rsdb.Get<string>(roleID);
            if (menus == null)
            {
                    menus = athtyDb.role_menu.Where((rm) => rm.rol_id == roleID).Select((rm) => rm.menu_id).ToList();
                    rsdb.Set<string>(roleID, menus, DateTime.Now.AddMinutes(10));
            }
            
            
            return menus;

        }

        /// <summary>
        /// 权限验证
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="menusID"></param>
        /// <param name="Request"></param>
        /// <returns></returns>
        public bool checkAuthority(string roleID, string menusID, HttpRequestBase Request)
        {
            if (checkSign(Request))
            {
                List<string> menus = getMenus(roleID);
                if (menus != null && menus.Contains(menusID))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 登录成功后 返回加密sign
        /// </summary>
        /// <param name="random"></param>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public string getSign(string random, string userID, string userName, string roleID,string idtyValue)
        { 
            string key = random + userID + userName + roleID + idtyValue + Assistant.entryKey;
            string sign = Assistant.md5(key);
            return sign;
        }

        public bool checkSign(HttpRequestBase Request)
        {
            if (Request.Cookies["identity"] == null)
            {
                return false;
            }
            HttpCookie cookie = Request.Cookies["identity"];
            if (cookie.Values["userid"] == null || cookie.Values["userName"] == null || cookie.Values["roleid"] == null || cookie.Values["idty"]==null || cookie.Values["random"] == null || cookie.Values["sign"] == null)
            {
                return false;
            }
            string sign1 = cookie.Values["sign"];
            string key = cookie.Values["random"] + cookie.Values["userid"] + cookie.Values["userName"] + cookie.Values["roleid"] + cookie.Values["idty"] + Assistant.entryKey;
            string sign2 = Assistant.md5(key);
            if (sign1 != sign2)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 用户登录成功后将用户身份保存至cookie
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="userName"></param>
        /// <param name="roleid"></param>
        /// <param name="idtyValue">1普通用户,2商户，3代理，4管理员</param>
        /// <param name="random"></param>
        /// <param name="sign"></param>
        public void saveLogin(string userid, string userName, string roleid,string idtyValue, string random, string sign,HttpResponseBase Response)
        {
            HttpCookie cookie = new HttpCookie("identity");
            cookie.Values["userid"] = userid;
            cookie.Values["userName"] = userName;
            cookie.Values["roleid"] = roleid;
            cookie.Values["idty"] = idtyValue;
            cookie.Values["random"] = random;
            cookie.Values["sign"] = sign;

            cookie.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Add(cookie);
        }

        public string getUserIdentity(HttpRequestBase Request,UserIdty idty)
        {
            if (checkSign(Request))
            {
                HttpCookie cookie = Request.Cookies["identity"];

                switch (idty)
                {
                    case UserIdty.userid: return cookie.Values["userid"];
                    case UserIdty.userName: return cookie.Values["userName"];
                    case UserIdty.roleid: return cookie.Values["roleid"];
                    case UserIdty.idty: return cookie.Values["idty"];
                }
               
            }
            return "";
        }
        
    }

    public enum UserIdty
    {
        userid,
        userName,
        roleid,
        idty
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using PublicHelp;
using System.Net.Http;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.Security;

namespace Admins.Controllers
{
    public class AdministratorController : Controller
    {
        TiDiHuiEntities dbcontext = new TiDiHuiEntities();
        private string sql = string.Empty;
        run rn = new run();

        //添加管理员
        [HttpPost]
        public JsonResult Add(Admin item, HttpRequestMessage httprq)
        {
            if (dbcontext.Admin.Any(t => t.Users == item.Users))
            {
                
                return Json(new Model.PublicResult.pb_execute() { success = false, result = "用户名已被注册" });
            }
            else
            {
                var id = rn.GetId<Admin>();
                if (id == null)
                {
                    return Json(new Model.PublicResult.pb_execute() { success = false, result = "id注册失败，请重新注册" });
                }
                else
                {
                    sql = @"insert into Admin(Id, Users,Password,Time,Lasttime,LastIp) values(@Id, @Users,@Password,@Time,@Lasttime,@LastIp)";
                    var args = new DbParameter[] {
                                    new SqlParameter { ParameterName = "Id", Value = id},
                                    new SqlParameter { ParameterName = "Users", Value=item.Users},
                                    new SqlParameter { ParameterName = "Password", Value=FormsAuthentication.HashPasswordForStoringInConfigFile(item.Password, "MD5")},
                                    new SqlParameter { ParameterName = "Time", Value=DateTime.Now},
                                    new SqlParameter { ParameterName = "Lasttime", Value=DateTime.Now},
                                    new SqlParameter { ParameterName = "LastIp", Value= Assistant.GetClientIp(httprq)}
                                };
                    return Json(rn.exac(sql, args));
                }
            }
        }

        //删除管理员
        [HttpPost]
        public JsonResult Del(Admin item)
        {
            var adminlist = dbcontext.Admin.Where(t => t.Id == item.Id);
            var isTrue = true;
            foreach (var a in adminlist.ToList())
            {
                if (a.Users == "admin")
                {
                    isTrue = false;
                }
            }
            if (isTrue)
            {
                if (adminlist != null)
                {
                    dbcontext.Admin.Remove(adminlist.First());
                }
                var i = dbcontext.SaveChanges(); ;
                return Json(new Model.PublicResult.pb_execute() { success = true, result = "成功删除" + i + "条对象" });
            }
            else
            {
                return Json(new Model.PublicResult.pb_execute() { success = false, result = "admin不能删除" });
            }
        }

        //编辑管理员
        [HttpPost]
        public JsonResult Edit(Admin item)
        {
            sql = @"update Admin set Password=@Password where Id=@Id";
            var args = new DbParameter[] {
                    new SqlParameter { ParameterName = "Id", Value=item.Id},
                    new SqlParameter { ParameterName = "Password", Value=FormsAuthentication.HashPasswordForStoringInConfigFile(item.Password, "MD5")}
                };
            return Json(rn.exac(sql, args));
        }
    }
}

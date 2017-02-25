using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using PublicHelp;
using System.Web;
namespace Model.ViewModel
{
    public class BuserRegister
    {
        TiDiHuiEntities dbContext = new TiDiHuiEntities();
        
        public string Id { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [MinLength(6, ErrorMessage = "密码长度不能小于6")]
        public string Password { get; set; }

        [Required(ErrorMessage = "确认密码不能为空")]
        [System.ComponentModel.DataAnnotations.CompareAttribute("Password", ErrorMessage = "密码和确认密码不一致")]
        public string confirmPassword { get; set; }

        [Mobile(ErrorMessage="手机号有误")]
        public string Mobile { get; set; }

        /// <summary>
        /// 短信验证码
        /// </summary>
        [Required(ErrorMessage="验证码不能为空")]
        public string MessCode { get; set; }

        public string ShopId { get; set; }
        public string ParentId { get; set; }
        public string ProxyId { get; set; }
        public string PayId { get; set; }
        public Nullable<int> RolId { get; set; }
        public Nullable<System.DateTime> AddTime { get; set; }

        public BUser getBuser(BuserRegister brModel,string proxyId, ModelStateDictionary ModelState)
        {
            BUser model = new BUser();
            SendMessage smess = new SendMessage();

            int c = dbContext.BUser.Where((buser) => buser.Mobile == brModel.Mobile).Count();
            if (c > 0)
            {
                ModelState["UserName"].Errors.Add("用户名已存在");
            }

            string key =  ModelTool.GetId<BUser>();
            if (!string.IsNullOrEmpty(key))
            {
                model.Id = key;
            }
            else
            {
                ModelState["Id"].Errors.Add("主键生成失败");
            }

            if (string.IsNullOrEmpty(proxyId) && dbContext.ProxyUser.Find(proxyId) != null)
            {
                model.ProxyId = proxyId;
            }

            int state = smess.isCodeValid(brModel.Mobile,brModel.MessCode);
            if (state == -1)
            {
                ModelState["MessCode"].Errors.Add("验证码无效");
            }
            else if (state == -2)
            {
                ModelState["MessCode"].Errors.Add("验证码错误");
            }
            
            model.Id = brModel.Id;
            model.Password =  Assistant.md5(brModel.Password);
            model.Mobile = brModel.Mobile;
            model.RolId = RolsTypeManager.getRolId(RolsType.bsnsAdmin);
            if (!string.IsNullOrEmpty(brModel.ParentId)) //添加子账号
            {
                model.ParentId = brModel.ParentId;
            }

            model.AddTime = DateTime.Now;

            return model;
        
        }

    }

    public class BuserLogin
    {
       TiDiHuiEntities dbContext = new TiDiHuiEntities();
       RedisDb rdb = new RedisDb();
        
        [Required(ErrorMessage="请输入用户名")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }

        [Required(ErrorMessage = "请输入验证码")]
        public string Code { get; set; }

        public BUser getBuserLogin(BuserLogin model, ModelStateDictionary ModelState,HttpRequestBase Request)
        {
            if (Request.Cookies["cusid"] == null)
            {
                ModelState["Code"].Errors.Add("验证码无效");
                
            }
            string key = Request.Cookies["cusid"].Value;
            string code = rdb.Get(key);
            if (string.IsNullOrEmpty(code))
            {
                ModelState["Code"].Errors.Add("验证码无效");
            }
            else if (model.Code != code)
            {
                ModelState["Code"].Errors.Add("验证码错误");
            }
            string password = Assistant.md5(model.Password);

            BUser buserModel = dbContext.BUser.Where((buser) => buser.Mobile == model.Mobile && buser.Password == password).First();
            if (buserModel != null)//登录成功
            {
                return buserModel;
            }
            else
            {
                ModelState["Mobile"].Errors.Add("登录失败");
            }
            return null;

        }

       

    }

}

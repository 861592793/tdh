using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.ViewModel;
using PublicHelp;
using Model;
namespace tiandihui.Controllers
{
    public class BuserController : Controller
    {
        run rn = new run();
        TiDiHuiEntities dbContext = new TiDiHuiEntities();
        //
        // GET: /Buser/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// 生成图片验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult GetValidateCode()
        {
            ImagesTool imeTl = new ImagesTool();
            RedisDb rdb = new RedisDb();

            string code = Assistant.getCode(4);
            string cusid = DateTime.Now.ToString("yyyyMMddhhmmss") + Guid.NewGuid().ToString("N");
            DateTime expireTime = DateTime.Now.AddMinutes(5);
            Response.Cookies["cusid"].Value = cusid;
            Response.Cookies["cusid"].Expires = expireTime;

            rdb.Set(cusid, code, expireTime);

            byte[] bytes = imeTl.CreateCheckCodeImage(code);

            return File(bytes,@"image/jpeg");
        }


        /// <summary>
        /// 获取短信验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public ActionResult getMessageCode(string mobile)
        {
            bool isSucess = false;
            string mess = "验证码发送失败";
            if (!Check.check(CheckType.mobile, mobile))
            {
                mess = "手机号有误";
            }
            SendMessage smess = new SendMessage();

            string content = "欢迎入住天地惠，你的短信验证码为{0},5分钟内有效，请勿泄露";
            string code = smess.getMessageCode(mobile,content);
            if (code != "-1" && code != "-2")
            {
                isSucess = true;
                mess = "短信验证码已成功发送到你的手机上";
            }
            return Json(new { isSucess=isSucess,mess=mess});
        }
        
        
        [HttpPost]
        public ActionResult Register(BuserRegister model,string id)
        {

            bool isSucess = false;
            List<string> lstErrmess = null;
            string mess = "";

            if (ModelState.IsValid)
            {
                BUser buser = model.getBuser(model, id, ModelState);
                lstErrmess = ModelTool.getErrors(ModelState);

                if (lstErrmess.Count == 0) //不存在错误
                {
                    try
                    {
                        dbContext.BUser.Add(buser);
                        dbContext.SaveChanges();

                        isSucess = true;
                        mess = "注册成功";
                    }
                    catch (Exception ex)
                    {
                        mess = ex.Message;

                    }
                }

                //return RedirectToAction("Register");
            }
            else
            {
                lstErrmess = ModelTool.getErrors(ModelState);
            }

            return Json(new { isSucess = isSucess, mess = mess, errors = lstErrmess });
        }

        [HttpPost]
        public ActionResult login(BuserLogin model)
        {
            ModelTool mdtl=new ModelTool();
            BuserLogin blgn = new BuserLogin();
            AuthorityManager atyMng = new AuthorityManager();

            bool isSucess = false;
            List<string> lstErrmess = null;
            string mess = "登录失败";
            if (ModelState.IsValid)
            {
               BUser buser = blgn.getBuserLogin(model,ModelState,Request);
               lstErrmess = ModelTool.getErrors(ModelState);
               if (lstErrmess != null && lstErrmess.Count == 0 && buser!=null) //登陆成功
               {
                   isSucess = true;
                   mess = "登录成功";
                   string random = Assistant.getCode(6);
                   string sign = atyMng.getSign(random, buser.Id, buser.Mobile, buser.RolId,"2");
                   atyMng.saveLogin(buser.Id, buser.Mobile, buser.RolId,"2", random, sign, Response);
                   
                   return Json(new { isSucess=isSucess,mess=mess,userName=buser.Mobile});

               }
            }

            return Json(new { isSucess = isSucess, mess = mess });

            //HttpServerUtilityBase

              
        
        }

       

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using PublicHelp;
namespace tiandihui.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            //RedisDb rdb = new RedisDb();

            //rdb.Set("code","123456");

            //string str = rdb.Get("code");

           string id = ModelTool.GetId<Admin>();

          // return Content(id);
            return View();
        }

        public ActionResult uploadImage(HttpPostedFileBase testUpload)
        {
            string saveUrl = Server.MapPath("~/Content/Image/test/");

            string fileName = UploadFile.saveFile(saveUrl, UploadFile.fileType.image, testUpload);

             if (fileName!="-1"&&fileName!="-2")
             {
                 return Json(new { url = "/Content/Image/test/"+fileName });
             }

             return Json(new { });
        }

    }
}

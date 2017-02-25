using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace tiandihui.Controllers
{
    public class CategoryController : Controller
    {
        Model.TiDiHuiEntities dbContext = new Model.TiDiHuiEntities();
        
        //
        // GET: /Category/

        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult getCategory()
        {
           var lstCategory = dbContext.Category.Where((c) => c.ParentId == null).Select((c) => new { id=c.Id,name=c.Name}).ToList();
           return Json(lstCategory,JsonRequestBehavior.AllowGet);
        }

    }
}

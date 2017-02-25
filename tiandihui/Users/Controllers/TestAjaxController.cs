using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Users.Controllers
{
    public class TestAjaxController : Controller
    {
        //
        // GET: /TestAjax/

        public ActionResult Index()
        {
            //PublicHelp.TiDiHuiEntities1 dbcontext = new PublicHelp.TiDiHuiEntities1();
            //var test = dbcontext.Admin.ToList();
            ////test.result = "test";
            ////test.success = false;
            RedisDb rd = new RedisDb();
            //rd.Set("456", "456",3);
            //var test = rd.Get("456");
            rd.Del("456", 2);
            //rd.Set<PublicHelp.Admin>("123sss", test);
            //var test = rd.Get<PublicHelp.Admin>("testsss");
            //var test2 = test.Where(a => a.Id == "1").First();
            return View();
        }

    }
}

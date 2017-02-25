using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using PublicHelp;

namespace tiandihui.Controllers
{
    public class NaturalController : Controller
    {
        //
        // GET: /Natural/

        TiDiHuiEntities dbContext = new TiDiHuiEntities();
        AuthorityManager atyMng = new AuthorityManager();
      
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult addNatural(Model.ViewModel.NaturalInfo model,List<Model.BrandImprove> lstBrandImprove )
        {
            bool isSucess = false;
            string mess = "";
            List<string> lstErrMess = null;
            if (ModelState.IsValid)
            {
                Model.NaturalInfo ntlInfoModel = model.getNaturalInfo(model, ModelState, Request, Server);
                model.getBrandImprove(ModelState, lstBrandImprove, model.Id);
                if (ntlInfoModel != null && ModelState.IsValid) //ok
                {
                    dbContext.NaturalInfo.Add(ntlInfoModel);
                    dbContext.BrandImprove.AddRange(lstBrandImprove);

                    if (dbContext.SaveChanges() > 0)
                    {
                        isSucess = true;
                        mess = "成功";
                    }
                }
                else
                {
                    lstErrMess = ModelTool.getErrors(ModelState);
                    mess = "提交失败";
                }
            }
            else
            {
                lstErrMess = ModelTool.getErrors(ModelState);
                mess = "提交失败";
            }

            return Json(new { isSucess = isSucess, mess = mess, errors = lstErrMess });
        }

        public ActionResult uploadImage(HttpPostedFileBase naturalImage,string postfix)
        {
            bool isSucess = false;
            string mess = "上传失败";

           string idty = atyMng.getUserIdentity(Request, UserIdty.idty);
           if (idty != "2")
           {
               mess = "没有权限";
               return Json(new { isSucess = isSucess, mess=mess,url="" });
           }
           else if (naturalImage == null)
           {
               mess = "name参数应为naturalImage";
               return Json(new { isSucess = isSucess, mess = mess, url = "" });
           }

           int index = naturalImage.FileName.LastIndexOf('.');
           string extenName = naturalImage.FileName.Substring(index);

           string newFileName = Guid.NewGuid().ToString() + postfix + extenName;

            string saveUrl = Server.MapPath("~/Content/Image/Temp/");

            string fileName = UploadFile.saveFile(saveUrl, UploadFile.fileType.image, naturalImage, newFileName);

            if (fileName != "-1" && fileName != "-2")
            {
                isSucess = true;
                mess = "上传成功";
                return Json(new { isSucess=isSucess,mess=mess, url = "/Content/Image/Temp/" + fileName });
            }

            return Json(new { isSucess = isSucess, mess=mess,url="" });

            
        }

    }
}

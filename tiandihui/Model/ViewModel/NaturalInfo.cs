using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Model.ViewModel
{
    public class NaturalInfo
    {
        public string Id { get; set; }
        public string BUserId { get; set; }

        [Required(ErrorMessage="管理人姓名不能为空")]
        public string MngName { get; set; }

        [Email(ErrorMessage="邮箱格式不正确")]
        public string MngEmail { get; set; }

        [Required(ErrorMessage="身份证号不能为空")]
        public string IDCard { get; set; }

        [Required(ErrorMessage = "紧急联系人姓名不能为空")]
        public string LinkName { get; set; }

        [Mobile(ErrorMessage="手机号有误")]
        public string LinkMobile { get; set; }

        [Required(ErrorMessage="企业法人姓名不能为空")]
        public string LgName { get; set; }

        [Email(ErrorMessage="企业法人邮箱格式有误")]
        public string LgEmail { get; set; }

        [Mobile(ErrorMessage="企业法人手机号有误")]
        public string LgMobile { get; set; }

        [Required(ErrorMessage="公司名称不能为空")]
        public string CompName { get; set; }

        [Required(ErrorMessage="公司地址不能为空")]
        public string Address { get; set; }

        [Required(ErrorMessage="营业执照注册号不能为空")]
        public string LienceCode { get; set; }

        [Required(ErrorMessage="组织机构代码不能为空")]
        public string OrgCode { get; set; }

        [Required(ErrorMessage="纳税人识别码不能为空")]
        public string TaxCode { get; set; }

        [Required(ErrorMessage="统一社会信用代码不能为空")]
        public string CreditCode { get; set; }

        [Required(ErrorMessage = "法人身份证正面不能为空")]
        public string IDFront { get; set; }

        [Required(ErrorMessage = "法人身份证反面不能为空")]
        public string IDBack { get; set; }

        [Date(ErrorMessage="身份证有效期开始日期有误")]
        public Nullable<System.DateTime> IDSDate { get; set; }

         [Date(ErrorMessage = "身份证有效期结束日期有误")]
        public Nullable<System.DateTime> IDEDate { get; set; }

        [Required(ErrorMessage = "营业执照不能为空")]
        public string Licence { get; set; }

        [Date(ErrorMessage = "营业执照开始日期有误")]
        public Nullable<System.DateTime> LceSDate { get; set; }

        [Date(ErrorMessage = "营业执照结束日期有误")]
        public Nullable<System.DateTime> LceEDate { get; set; }

        [Required(ErrorMessage = "税务登记证明不能为空")]
        public string TaxImProve { get; set; }

        [Required(ErrorMessage = "组织机构证明不能为空")]
        public string OrgImProve { get; set; }

        [Date(ErrorMessage = "组织机构开始日期有误")]
        public Nullable<System.DateTime> OrgSDate { get; set; }

        [Date(ErrorMessage = "组织机构结束日期有误")]
        public Nullable<System.DateTime> OrgEDate { get; set; }

        [Required(ErrorMessage = "开户许可证不能为空")]
        public string OpenAllow { get; set; }
        public Nullable<int> ProxyState { get; set; }
        public Nullable<int> State { get; set; }
        public string refuseReason { get; set; }
        public Nullable<System.DateTime> AddTime { get; set; }

        //public virtual ICollection<BrandImprove> BrandImprove { get; set; }
        //public virtual BUser BUser { get; set; }

        public Model.NaturalInfo getNaturalInfo(NaturalInfo model, ModelStateDictionary ModelState,HttpRequestBase Request,HttpServerUtilityBase Server)
        {
            TiDiHuiEntities dbContext = new TiDiHuiEntities();
            run rn = new run();
            AuthorityManager atyMng = new AuthorityManager();

            Model.NaturalInfo ntlInfoModel = new Model.NaturalInfo();
            string userName = "";

           string id = rn.GetId<Model.NaturalInfo>();
           if (!string.IsNullOrEmpty(id))
           {
               ntlInfoModel.Id = id;
           }
           else
           {
               ModelState["Id"].Errors.Add("id生成失败");
           }
           string identity = atyMng.getUserIdentity(Request,UserIdty.idty);
           if (identity == "2")
           {
              // model.BUserId = atyMng.getUserIdentity(Request, UserIdty.userid);
               ntlInfoModel.BUserId = atyMng.getUserIdentity(Request, UserIdty.userid);
               userName = atyMng.getUserIdentity(Request, UserIdty.userName);
               string folder = Server.MapPath("~/Content/Image/"+userName);
               if (!Directory.Exists(folder))
               {
                   Directory.CreateDirectory(folder);
               }

           }
           else
           {
               ModelState["BUserId"].Errors.Add("没有权限");
           }
           ntlInfoModel.MngName = model.MngName;
           ntlInfoModel.MngEmail = model.MngEmail;
           ntlInfoModel.IDCard = model.IDCard;
           ntlInfoModel.LinkName = model.LinkName;
           ntlInfoModel.LinkMobile = model.LinkMobile;
           ntlInfoModel.LgName = model.LgName;
           ntlInfoModel.LgEmail = model.LgEmail;
           ntlInfoModel.LgMobile = model.LgMobile;
           ntlInfoModel.CompName = model.CompName;
           ntlInfoModel.Address = model.Address;
           ntlInfoModel.LienceCode = model.LienceCode;
           ntlInfoModel.OrgCode = model.OrgCode;
           ntlInfoModel.TaxCode = model.TaxCode;
           ntlInfoModel.CreditCode = model.CreditCode;
           ntlInfoModel.IDFront = model.IDFront.Replace("/Temp","/"+userName);
           ntlInfoModel.IDBack = model.IDBack.Replace("/Temp", "/" + userName);
           ntlInfoModel.IDSDate = model.IDSDate;
           ntlInfoModel.IDEDate = model.IDEDate;
           ntlInfoModel.Licence = model.Licence.Replace("/Temp", "/" + userName);
           ntlInfoModel.LceSDate = model.LceSDate;
           ntlInfoModel.LceEDate = model.LceEDate;
           ntlInfoModel.TaxImProve = model.TaxImProve.Replace("/Temp", "/" + userName);
           ntlInfoModel.OrgImProve = model.OrgImProve.Replace("/Temp", "/" + userName);
           ntlInfoModel.OrgSDate = model.OrgSDate;
           ntlInfoModel.OrgEDate = model.OrgEDate;
           ntlInfoModel.OpenAllow = model.OpenAllow.Replace("/Temp", "/" + userName);
           ntlInfoModel.ProxyState = 1;
           ntlInfoModel.State = 1;
           ntlInfoModel.AddTime = DateTime.Now;

            string IDFrontName = Server.MapPath("~"+model.IDFront);
            string IDBackName = Server.MapPath("~"+model.IDBack);
            if (!File.Exists(IDFrontName))
            {
                ModelState["IDFront"].Errors.Add("法人省份证正面不能为空");
            }
            else
            {
                string destFile = Server.MapPath(ntlInfoModel.IDFront);
                File.Move(IDFrontName,destFile);
            }
            if (!File.Exists(IDBackName))
            {
                ModelState["IDBack"].Errors.Add("法人省份证反面不能为空");
            }
            else
            {
                string destFile = Server.MapPath(ntlInfoModel.IDBack);
                File.Move(IDBackName, destFile);
            }
            if (model.IDSDate >= model.IDEDate)
            {
                ModelState["IDEDate"].Errors.Add("身份证结束日期必须大于开始日期");
            }

            string licenceName = Server.MapPath("~"+model.Licence);
            if (!File.Exists(licenceName))
            {
                ModelState["Licence"].Errors.Add("营业执照不能为空");
            }
            else
            {
                string destFile = Server.MapPath(ntlInfoModel.Licence);
                File.Move(licenceName, destFile);
            }
            if (model.LceSDate >= model.LceEDate)
            {
                ModelState["LceEDate"].Errors.Add("营业执照结束日期必须大于开始日期");
            }

            string TaxImProveName = Server.MapPath("~"+model.TaxImProve);
            if (!File.Exists(TaxImProveName))
            {
                ModelState["TaxImProve"].Errors.Add("税务登记证明不能为空");
            }
            else
            {
                string destFile = Server.MapPath(ntlInfoModel.TaxImProve);
                File.Move(TaxImProveName, destFile);
            }

            string OrgImProveName = Server.MapPath("~"+model.OrgImProve);
            if (!File.Exists(OrgImProveName))
            {
                ModelState["OrgImProve"].Errors.Add("组织机构证明不能为空");
            }
            else
            {
                string destFile = Server.MapPath(ntlInfoModel.OrgImProve);
                File.Move(OrgImProveName, destFile);
            }
            if (model.OrgSDate >= model.OrgEDate)
            {
                ModelState["OrgEDate"].Errors.Add("组织机构结束日期必须大于开始日期");
            }

            string OpenAllowName = Server.MapPath("~" + model.OpenAllow);
            if (!File.Exists(OpenAllowName))
            {
                ModelState["OpenAllow"].Errors.Add("开户许可证不能为空");
            }
            else
            {
                string destFile = Server.MapPath(ntlInfoModel.OpenAllow);
                File.Move(OpenAllowName, destFile);
            }
            if (ModelState.IsValid)
            {
                return ntlInfoModel;
            }
            return null;
            
            //return model;
            
        }

        public List<Model.BrandImprove> getBrandImprove( ModelStateDictionary ModelState, List<Model.BrandImprove> lstBrandImprove,string naturlInfoId,HttpRequestBase Request,HttpServerUtilityBase Server)
        {
            if (lstBrandImprove != null && lstBrandImprove.Count > 0 && ModelState.IsValid)
             {
                AuthorityManager atyMng = new AuthorityManager();
                string userName = atyMng.getUserIdentity(Request, UserIdty.userName);
                
                DateTime addTime = DateTime.Now;
                        int c = lstBrandImprove.Count;
                        for (int i = 0; i < c; i++)
                        {
                            string Id = ModelTool.GetId<Model.BrandImprove>();
                            if (!string.IsNullOrEmpty(Id))
                            {
                                string newFilePath = lstBrandImprove[i].Src.Replace("/Temp", "/"+userName);
                                string souceFile = Server.MapPath("~"+lstBrandImprove[i].Src);
                                string destFile = Server.MapPath(newFilePath);
                                File.Move(souceFile, destFile);

                                lstBrandImprove[i].Id = Id;
                                lstBrandImprove[i].NaturalInfoId = naturlInfoId;
                                lstBrandImprove[i].Src = newFilePath;
                                lstBrandImprove[i].AddTime = addTime;
                            }
                            else
                            {
                                ModelState.AddModelError("brandImprove","品牌Id生成失败");
                            }

                            
                        }
              }
             return lstBrandImprove;
        }
       

    }

   
}

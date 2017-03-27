using AspMvcLibrary.Attributes;
using CameraMap.Configs;
using CameraMap.Enums;
using CameraMap.Models.Base;
using CameraMap.Models.Files;
using CameraMap.Models.Master.Organization;
using CameraMap.Models.ResponseModel;
using CameraMap.Sessions;
using PacificSystem.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CameraMap.Controllers
{
    [SessionExpireFilterAttribute(SessionKeyConfig.OrganizationID)]
    [SessionRollFilterAttribute(new SystemRollEnum[] { SystemRollEnum.SysAdmin, SystemRollEnum.Administrator })]
    public class OrganizationController : Controller
    {

        [HttpGet]
        public ActionResult Index()
        {
            var loginInfo = SessionLoginInfo.GetInstance(Session);
            if (loginInfo.SystemRoll == Enums.SystemRollEnum.SysAdmin)
            {
                var modelreg = new OrganizationReg();
                modelreg.SetOrganizationList();
                var model = new OrganizationViewModel()
                {
                    Items = modelreg.Items
                };
                return View(model);
            }
            else
            {
                return RedirectToAction("OrganizationEdit", "Organization", new { OrganizationID = loginInfo.OrganizationID });
            }
        }

        [HttpPost]
        [ActionName("Index")]
        [SubmitCommand("Add")]
        public ActionResult Add()
        {
            var model = new OrganizationModel();
            return RedirectToAction("OrganizationEdit", "Organization", new { OrganizationID = "New" });
        }


        //
        // GET: /Organization/

        public ActionResult OrganizationEdit(string OrganizationID)
        {
            if (string.IsNullOrEmpty(OrganizationID))
            {
                var loginInfo = SessionLoginInfo.GetInstance(Session);
                OrganizationID = loginInfo.OrganizationID;
            }

            var model = OrganizationReg.GetInstance().GetOrganizationModel(OrganizationID);//SessionLoginInfo.GetInstance(Session).OrganizationID);
            Session["OrganizationEdit"] = model;
            return View(model);
        }

        [HttpPost]
        public ActionResult OrganizationEdit(OrganizationModel model)
        {
            if (!ModelState.IsValid)
            {
                model.StateCodeList = OrganizationReg.GetInstance().GetSelectItem();
                return View(model);
            }

            var org = (OrganizationModel)Session["OrganizationEdit"];
            if (!string.IsNullOrEmpty(org.OrganizationID))
            {
                model.OrganizationID = org.OrganizationID;
                model.LastUserID = SessionLoginInfo.GetInstance(Session).LoginID;
                OrganizationReg.GetInstance().UpdateOrganizationModel(model);
            }
            else
            {
                //model.OrganizationID = Guid.NewGuid().ToString();
                model.LastUserID = SessionLoginInfo.GetInstance(Session).LoginID;
                OrganizationReg.GetInstance().AddNewOrganizationModel(model);
            }

            return RedirectToAction("Index", "Organization", new { OrganizationID = org.OrganizationID });
        }

        public ActionResult SetImgageFile()
        {
            try
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(Request.Files["file"].FileName);
                string FileName = fi.Name;

                var bytes = new byte[Request.Files["file"].InputStream.Length];
                Request.Files["file"].InputStream.Read(bytes, 0, bytes.Length);

                var fileID = FileModelReg.Insert(FileName, bytes);

                //string filePath = ParsePath(fileName);
                var sendObj = new ImageResponseModel
                {
                    FileKey = fileID.ToString(),
                    FileName = FileName,
                    FileUrl = Url.RouteUrl("DownloadURL", new { controller = "ImageMng", action = "GetImageFile", FileID = fileID.ToString(), FileName = FileName }),
                    FileUrlThumb = Url.RouteUrl("DownloadURL", new { controller = "ImageMng", action = "GetImageFile", FileID = fileID.ToString(), FileName = FileName }),
                };

                return View("FileUploaded", sendObj);
            }
            catch (Exception)
            {
                var sendObj = new ImageResponseModel
                {
                    IsFail=true
                };

                return View("FileUploaded", sendObj);
            }
        }



    }
}

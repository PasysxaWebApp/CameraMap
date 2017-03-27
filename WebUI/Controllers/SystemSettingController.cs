using CameraMap.Configs;
using CameraMap.Models.SystemSetting;
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
    //[SessionRollFilterAttribute(Enums.SystemRollEnum.Administrator)]
    public class SystemSettingController : Controller
    {
        //
        // GET: /SystemSetting/

        public ActionResult Index()
        {
            var model = new SystemSettingViewModel();
            model.OrganizationID = Converts.ToTryString(Session[SessionKeyConfig.OrganizationID], "");
            model.OrganizationList = SystemSettingManager.GetInstance().GetOrganizationList();
            model.Settings = SystemSettingManager.GetInstance().GetSystemSettingManagerList(model.OrganizationID);
            Session["SystemSettingViewModel"] = model;
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(SystemSettingViewModel model)
        {
            var btnFlag = model.btnName;
            if (btnFlag.Equals("登録"))
            {
                SystemSettingManager.GetInstance().SaveSetting(model.Settings);
            }

            model.OrganizationList = SystemSettingManager.GetInstance().GetOrganizationList();
            model.Settings = SystemSettingManager.GetInstance().GetSystemSettingManagerList(model.OrganizationID);
            Session["SystemSettingViewModel"] = model;
            return View(model);
        }

    }
}

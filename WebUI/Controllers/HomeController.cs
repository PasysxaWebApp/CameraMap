using System;
using System.Diagnostics;
using System.Web.Mvc;
using CameraMap.Configs;
using CameraMap.Enums;
using CameraMap.Models.Home;
using CameraMap.Models.SystemMenus;
using CameraMap.Sessions;
using CameraMap.Models.Notice;
using CameraMap.Models.Layer;
using CameraMap.Models.Device;

namespace CameraMap.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Error()
        {
            if (Session[SessionKeyConfig.OrganizationID] == null)
            {
                return RedirectToAction("Index", "Account", new { OrganizationID = "" });
            }
            //try
            //{

            //    if (Request.UserHostAddress != null)
            //    {
            //        var ex = (Exception)HttpContext.Application[Request.UserHostAddress];

            //        if (ex != null)
            //        {
            //            var loginInfoSession = SessionLoginInfo.GetInstance(Session);

            //            SystemLogManager.GetInstance().SetSystemErrorLog(SystemConfig.SystemTitle, loginInfoSession.OrganizationID, loginInfoSession.LoginID, loginInfoSession.UserName, ex.Message, ex.StackTrace);
            //        }
            //    }
            //}
            //catch (Exception exx)
            //{
            //    Debug.WriteLine(exx.Message);
            //}

            return View("ErrorPage");
        }

        public ActionResult Http404()
        {
            return View("Error");
        }

        public ActionResult TimeoutRedirect(string OrganizationID)
        {
            ViewBag.OrganizationID = string.Format("{0}", OrganizationID);
            return View();
        }

        public ActionResult RolloutRedirect(string OrganizationID)
        {
            ViewBag.OrganizationID = string.Format("{0}", OrganizationID);
            return View();
        }

        public ActionResult Live()
        {
            return new EmptyResult();
        }

        public ActionResult GetMenuHtml()
        {
            // TODO:Fix RollEnum
            var model = SessionLoginInfo.GetInstance(Session);
            var RollEnum = model.SystemRoll;// (SystemRoll)Session[LoginSessionKeys.RollEnum];
            return Content(SystemMenuListModel.GetInstance().GetHtml(RollEnum, Url));
        }

        public ActionResult Index(string key)
        {
            if (Session[SessionKeyConfig.OrganizationID] == null)
            {
                return RedirectToAction("Index", "Account", new { OrganizationID = key });
            }

            var loginInfo = SessionLoginInfo.GetInstance(Session);

            return View();
        }

        public ActionResult GetAllLayersPartial()
        {
            var loginInfo = SessionLoginInfo.GetInstance(Session);
            var allModels = LayerModelReg.GetModels(loginInfo.OrganizationID);
            allModels.ForEach(m => m.SetUrlHelper(Url));
            return PartialView("_AllLayersPartial", allModels);
        }

        public ActionResult GetAllDevicesPartial(long LayerId)
        {
            var loginInfo = SessionLoginInfo.GetInstance(Session);
            var allModels = DeviceModelReg.GetModels(LayerId);
            allModels.ForEach(m => m.SetUrlHelper(Url));
            return PartialView("_AllDevicesPartial", allModels);
        }


        public ActionResult GetLayerEditPartial()
        {
            return PartialView("_EditLayerPartial");
        }


        public ActionResult RefreshSys()
        {
            SystemMenuConfig.RefreshMenu();

            var key = SessionLoginInfo.GetInstance(Session).OrganizationID;
            return RedirectToAction("Index", "Home", new { OrganizationID = key });

        }

        public ActionResult Ver()
        {
            //return new ContentResult() { Content = "2016.04.23.02"  };//
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(asm.Location);
            string versionStr = string.Format(" {0}", fvi.FileVersion);
            return new ContentResult() { Content = versionStr };//"2016.04.23.02"
        }
    }
}

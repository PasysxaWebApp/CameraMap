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
using CameraMap.Models.Master.Organization;
using System.Collections.Generic;

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
            var organization= OrganizationReg.GetInstance().GetOrganizationModel(loginInfo.OrganizationID);
            return View(organization);
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
            var allModels = new List<IDevice>();
            var allDevices= DeviceModelReg.GetModels(LayerId);
            allDevices.ForEach(m => m.SetUrlHelper(Url));
            allModels.AddRange(allDevices);

            var allLines = LineModelReg.GetModels(LayerId);
            allLines.ForEach(m => m.SetUrlHelper(Url));
            allModels.AddRange(allLines);
            
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

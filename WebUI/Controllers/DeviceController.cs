using CameraMap.Models.Device;
using CameraMap.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CameraMap.Controllers
{
    public class DeviceController : Controller
    {
        //
        // GET: /Device/

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetDeviceModel(long Id)
        {
            var model = DeviceModelReg.GetModel(Id);
            model.SetUrlHelper(Url);
            var m = new
            {
                DeviceId = model.DeviceId.ToString(),
                LayerId=model.LayerId.ToString(),
                LayerName=model.LayerName,
                LayerKey=model.LayerKey,
                DeviceKey = model.DeviceKey,
                DeviceName = model.DeviceName,
                IconFile = model.IconFile.ToString(),
                IconFileUrl = model.IconFileUrl,
                IconFileName = model.IconFileName,
                Lat = model.Lat.ToString(),
                Lng = model.Lng.ToString(),
                Note = model.Note,
                DeviceUrl = model.DeviceUrl,
            };
            var r = new
            {
                Model = m
            };
            return Json(r);
        }

        [HttpPost]
        public ActionResult EditDeviceAjax(DeviceModel model)
        {
            var loginInfo = SessionLoginInfo.GetInstance(Session);

            if (!ModelState.IsValid)
            {
                var r = new
                {
                    Success = false
                };
                return Json(r);
            }

            bool bl = false;
            if (model.DeviceId > 0)
            {
                try
                {
                    var oldmodel = DeviceModelReg.GetModel(model.DeviceId);
                    oldmodel.DeviceKey = model.DeviceKey;
                    oldmodel.DeviceName = model.DeviceName;
                    oldmodel.Lat = model.Lat;
                    oldmodel.Lng = model.Lng;
                    oldmodel.Note = model.Note;
                    oldmodel.DeviceUrl = model.DeviceUrl;
                    oldmodel.LastUserID = loginInfo.LoginID;
                    bl = DeviceModelReg.Update(oldmodel);
                }
                catch (Exception) { }
            }
            else
            {
                try
                {
                    model.OrganizationID = loginInfo.OrganizationID;
                    model.LastUserID = loginInfo.LoginID;
                    bl = DeviceModelReg.Add(model);
                }
                catch (Exception) { }
            }
            var jsonObj = new
            {
                Success = bl
            };
            return Json(jsonObj);
        }

        [HttpPost]
        public ActionResult DeleteDeviceAjax(long DeviceId)
        {
            var loginInfo = SessionLoginInfo.GetInstance(Session);
            var model = new DeviceModel()
            {
                DeviceId = DeviceId,
                LastUserID = loginInfo.LoginID,
                OrganizationID = loginInfo.OrganizationID,
            };

            var bl = DeviceModelReg.Delete(model);
            var jsonObj = new
            {
                Success = bl
            };
            return Json(jsonObj);
        }

        public ActionResult GetAllDeviceDatas(long LayerId)
        {
            var loginInfo = SessionLoginInfo.GetInstance(Session);
            var allModels = DeviceModelReg.GetModels(LayerId);
            allModels.ForEach(m => m.SetUrlHelper(Url));
            var r = new { 
                Datas=allModels
            };
            return Json(r);
        }


    }
}

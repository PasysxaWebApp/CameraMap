using CameraMap.Models.Device;
using CameraMap.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CameraMap.Controllers
{
    public class LineController : Controller
    {
        //
        // GET: /Line/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetLineModel(long Id)
        {
            var model = LineModelReg.GetModel(Id);
            model.SetUrlHelper(Url);
            var m = new
            {
                LineId = model.LineId.ToString(),
                LayerId = model.LayerId.ToString(),
                LayerName = model.LayerName,
                LayerKey = model.LayerKey,
                LineKey = model.LineKey,
                LineName = model.LineName,
                IconFile = model.IconFile.ToString(),
                IconFileUrl = model.IconFileUrl,
                IconFileName = model.IconFileName,
                Points = model.Points,
                Note = model.Note,
            };
            var r = new
            {
                Model = m
            };
            return Json(r);
        }

        [HttpPost]
        public ActionResult EditLineAjax(LineModel model)
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
            if (model.LineId > 0)
            {
                try
                {
                    var oldmodel = LineModelReg.GetModel(model.LineId);
                    oldmodel.LineKey = model.LineKey;
                    oldmodel.LineName = model.LineName;
                    oldmodel.Points = model.Points;
                    oldmodel.Note = model.Note;
                    oldmodel.LastUserID = loginInfo.LoginID;
                    bl = LineModelReg.Update(oldmodel);
                }
                catch (Exception) { }
            }
            else
            {
                try
                {
                    model.OrganizationID = loginInfo.OrganizationID;
                    model.LastUserID = loginInfo.LoginID;
                    bl = LineModelReg.Add(model);
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
        public ActionResult DeleteLineAjax(long LineId)
        {
            var loginInfo = SessionLoginInfo.GetInstance(Session);
            var model = new LineModel()
            {
                LineId = LineId,
                LastUserID = loginInfo.LoginID,
                OrganizationID = loginInfo.OrganizationID,
            };

            var bl = LineModelReg.Delete(model);
            var jsonObj = new
            {
                Success = bl
            };
            return Json(jsonObj);
        }

        public ActionResult GetAllLineDatasByLayerId(long LayerId)
        {
            var loginInfo = SessionLoginInfo.GetInstance(Session);
            var allModels = LineModelReg.GetModels(LayerId);
            allModels.ForEach(m => m.SetUrlHelper(Url));
            var r = new
            {
                Datas = allModels
            };
            return Json(r);
        }

        public ActionResult GetAllLineDatas(string LayerIds)
        {
            var loginInfo = SessionLoginInfo.GetInstance(Session);
            var allModels = LineModelReg.GetModels(LayerIds);
            allModels.ForEach(m => m.SetUrlHelper(Url));
            var r = new
            {
                Datas = allModels
            };
            return Json(r);
        }

    }
}

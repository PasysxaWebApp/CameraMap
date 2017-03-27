using System;
using System.Collections.Generic;
using System.Linq;
using CameraMap.Models.Base;
using System.Web;
using System.Web.Mvc; 
using AspMvcLibrary.Attributes;
using CameraMap.Configs;
using PacificSystem.Utility;
using CameraMap.Sessions;
using CameraMap.Models.Master.User;
using CameraMap.Enums;
using CameraMap.App_GlobalResources;
using CameraMap.Models.Master.Organization;
namespace CameraMap.Controllers
{
    [SessionExpireFilterAttribute(SessionKeyConfig.OrganizationID)]
    [SessionRollFilterAttribute(new SystemRollEnum[] { SystemRollEnum.SysAdmin, SystemRollEnum.Administrator })]
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult UserList()
        {
            var loginInfo = SessionLoginInfo.GetInstance(Session);
            var modelreg = new UserModelReg();
            modelreg.SetUserList(loginInfo.SystemRoll, loginInfo.OrganizationID);
            return View(modelreg);
        }

        [HttpPost]
        [ActionName("UserList")]
        [SubmitCommand("Sort")]
        public ActionResult Sort()
        {
            var loginInfo = SessionLoginInfo.GetInstance(Session);
            var modelreg = new UserModelReg();
            modelreg.SetUserList(loginInfo.SystemRoll, loginInfo.OrganizationID);

            Session[SessionKeyConfig.SortTargetModel] = new SortTargetModel
            {
                OrganizationID = SessionLoginInfo.GetInstance(Session).OrganizationID,
                TableName = "M_User",
                IdColumn = "Guid",
                DisplayColumn = "UserName",
                DisplayNoColumn="DisplayNo",
                RedirectController = "User",
                RedirectAction = "UserList",
                SortItems = modelreg.Items
            };
            return RedirectToAction("SortList", "Sorts");
        }

        [HttpPost]
        [ActionName("UserList")]
        [SubmitCommand("Add")]
        public ActionResult Add()
        {
            var model = new UserModel();
            return RedirectToAction("UserEdit", "User", new {id=0 });
        }

        [HttpGet]
        public ActionResult UserEdit(long id)
        {
            var loginInfo = SessionLoginInfo.GetInstance(Session);

            var modelreg = new UserModelReg();
            var model = new UserModel();
            model.DisplayFlag = true;
            if (id > 0)
            {
                if (loginInfo.SystemRoll == SystemRollEnum.SysAdmin)
                {
                    model = modelreg.GetUserModel(id);
                }
                else
                {
                    model = modelreg.GetUserModel(model.OrganizationID, id);
                }
                model.ConfirmPassword = model.Password;
            }
            else {
                modelreg.FillUnionOrganizations(model);
            }
            model.SetAuthrityList(loginInfo.SystemRoll);
            
            var reg = OrganizationReg.GetInstance();
            var items=reg.GetOrganizationList();
            foreach (var item in items)
            {
                model.Organizations.Add(new SelectListItem() { Value = item.OrganizationID, Text = item.OfficeName });
            }
            if (model.Guid == 0)
            {
                model.OrganizationID = loginInfo.OrganizationID;
                model.OfficeName = reg.GetOrganizationModel(model.OrganizationID).OfficeName;
            }

            //var orgModelReg = new OrganizationReg();
            //orgModelReg.SetOrganizationList();
            //model.Organizations = orgModelReg.Items;           

            ViewBag.DeleteCheck = modelreg.DeleteCheck(model.OrganizationID, id);
            return View(model);
        }

        [HttpPost]
        [ActionName("UserEdit")]
        [SubmitCommand("Back")]
        public ActionResult Back()
        {
            return RedirectToAction("UserList", "User");
        }

        

        [HttpPost]
        [ActionName("UserEdit")]
        [SubmitCommand("OK")]
        [ValidateInput(false)]
        public ActionResult Ok(UserModel model)
        {
            var loginInfo = SessionLoginInfo.GetInstance(Session);

            var modelreg = new UserModelReg();
            //if (model.Guid == 0)
            //{
            //    model.OrganizationID = loginInfo.OrganizationID;
            //}

            //ユーザーIDのチェック
            var sameLoginName = false;
            var checkModel= modelreg.GetUserModelByLoginName(model.OrganizationID, model.LoginName);
            if (model.Guid == 0)
            {
                if (checkModel != null && checkModel.Guid>0)
                {
                    sameLoginName = true;
                }
            }
            else
            {
                if (checkModel.Guid>0 && model.Guid != checkModel.Guid)
                {
                    sameLoginName = true;
                }
            }
   

            if (!sameLoginName && ModelState.IsValid)
            {
                modelreg.EditUser(model, loginInfo.SystemRoll);
                return RedirectToAction("UserList", "User");
            }

            if (sameLoginName)
            {
                ModelState.AddModelError("LoginName", LanguageResource.ItemLoginName + "が存在しています");
            }

            model.SetAuthrityList(loginInfo.SystemRoll);

            var reg = OrganizationReg.GetInstance();
            var items= reg.GetOrganizationList();
            foreach (var item in items)
            {
                model.Organizations.Add(new SelectListItem() { Value = item.OrganizationID, Text = item.OfficeName });
            }
            if (model.Guid == 0)
            {
                model.OrganizationID = loginInfo.OrganizationID;
                model.OfficeName = reg.GetOrganizationModel(model.OrganizationID).OfficeName;
            }

            ViewBag.DeleteCheck = modelreg.DeleteCheck(model.OrganizationID, model.Guid);
            return View(model);
        }

        [HttpPost]
        [ActionName("UserEdit")]
        [SubmitCommand("Del")]
        public ActionResult Del(long id) {
            var loginInfo = SessionLoginInfo.GetInstance(Session);
            string OrganizationID = loginInfo.OrganizationID;
            if (loginInfo.SystemRoll == SystemRollEnum.SysAdmin)
            {
                OrganizationID = "";
            }
            var modelreg = new UserModelReg();
            modelreg.DeleteUser(OrganizationID, id);
            return RedirectToAction("UserList", "User");
        }
    }
}

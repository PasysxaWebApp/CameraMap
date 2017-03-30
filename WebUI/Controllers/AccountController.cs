using System;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Security;
using CameraMap.Models.Master.Account;
using CameraMap.Sessions;
using PacificSystem.Utility;
using CameraMap.Models.Account;
using CameraMap.Configs;
using CameraMap.Enums; 

namespace CameraMap.Controllers
{
    //[Authorize]
    //[InitializeSimpleMembership]
    public class AccountController : Controller
    {
        public ActionResult LogOn()
        {
            if (Debugger.IsAttached)
            {
                var model = new LogOnModel();

                return View(model);
            }

            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (_IsValidateUser(model.UserID, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserID, model.RememberMe);

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }

                    var loginInfoSession = SessionLoginInfo.GetInstance(Session);

                    return RedirectToAction("Index", "Home", new { key = loginInfoSession.OrganizationID });
                }

                ModelState.AddModelError("", @"指定されたユーザー名またはパスワードが正しくありません。");
            }

            // ここで問題が発生した場合はフォームを再表示します
            return View(model);
        }

        public ActionResult Index(string OrganizationID)
        {

            if (Session[SessionKeyConfig.OrganizationID] != null)
            {
                return RedirectToAction("Index", "Home", new { OrganizationID = OrganizationID });
            }

            LogOnModel model = new LogOnModel() { 
                OrganizationID=OrganizationID
            };
            return View(model);
        }


        /// <summary>
        /// ログイン処理
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.OrganizationID))
                {
                    model.OrganizationID = "DebugOrganizationID";
                }
                var userModel = AccountModelsReg.GetInstance().GetUserModel(model);

                if (userModel.Guid>0)
                {
                    this.Response.Cookies.Remove("OrganizationID");
                    this.Response.Cookies.Add(new System.Web.HttpCookie("OrganizationID", userModel.OrganizationID));

                    Session[SessionKeyConfig.OrganizationID] = userModel.OrganizationID;
                    //add logs
                    var result = AccountModelsReg.GetInstance().AddAccessLog(userModel);

                    var loginInfoSession = SessionLoginInfo.GetInstance(Session);

                    loginInfoSession.LoginID = userModel.Guid;
                    loginInfoSession.OrganizationID = userModel.OrganizationID;
                    loginInfoSession.UserName = userModel.UserName;
                    loginInfoSession.SystemRoll = (SystemRollEnum)Enum.Parse(typeof(SystemRollEnum), userModel.Authority.ToString());// .Administrator;
                    loginInfoSession.UnionOfficeUser = userModel.UnionOfficeUser;
                    loginInfoSession.UnionOfficeIds = userModel.UnionOfficeIds;
                    loginInfoSession.Save();

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home", new { key = loginInfoSession.OrganizationID });
                }

                ModelState.AddModelError("Err01", @"ユーザー名またはパスワードが確認できませんでした。");
                ModelState.AddModelError("Err02", @"入力内容をご確認ください。");
            }

            // ここで問題が発生した場合はフォームを再表示します
            return View(model);
        }


        private bool _IsValidateUser(string userID, string password)
        {
            //var result = false;

            //var wrapper = new CommunicationWrapper();
            //wrapper.AddInParam("UserID", userID);
            //wrapper.AddInParam("Password", password);
            //wrapper.AddInParam("OrganizationID", Converts.ToTryString(Session[LoginSessionKeys.OrganizationID]));
            //wrapper.Execute(typeof(SelectLoginInfo));

            //var loginUserID = 0;
            //var loginUserName = String.Empty;
            //var ip = HttpContext.Request.GetIpAddress();
            //var agent = HttpContext.Request.GetUserAgent();
            //var operation = String.Format("Login Failure {0}/{1}", userID, password);

            //if (wrapper.OutListParam.Count == 1)
            //{
            //    loginUserID = Converts.ToTryInt(wrapper.OutListParam[0]["LoginID"]);
            //    loginUserName = Converts.ToTryString(wrapper.OutListParam[0]["UserName_ja"]);
            //    operation = "Login Success";

            //    SessionManager.SetKeys(Session, wrapper.OutListParam[0]);
            //    result = true;
            //}

            //SystemLogManager.GetInstance().SetSystemAccessLog(
            //    SystemInfo.SystemTitle,
            //    Converts.ToTryString(Session[LoginSessionKeys.OrganizationID]),
            //    loginUserID,
            //    loginUserName,
            //    operation,
            //    ip,
            //    agent);

            //return result;

            return true;
        }

        public ActionResult LogOff()
        {
            var key = SessionLoginInfo.GetInstance(Session).OrganizationID;
            if (string.IsNullOrEmpty(key))
            {
                key = this.Request.Form["OrganizationID"];
            }
            FormsAuthentication.SignOut();            
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home", new { key =key });

        }

        /// <summary>
        ///     ログインユーザー名取得
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLoginName()
        {
            var loginInfoSession = SessionLoginInfo.GetInstance(Session);

            if (Converts.ToTryInt(loginInfoSession.LoginID) == 0)
            {
                return Content(String.Empty);
            }
            return Content(Converts.ToTryString(loginInfoSession.UserName) + " さんでログイン");
        }
    }
}

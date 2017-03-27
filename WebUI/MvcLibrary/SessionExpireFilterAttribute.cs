using CameraMap.Configs;
using CameraMap.Enums;
using CameraMap.Sessions;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CameraMap.Controllers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {

        private string _LoginSessionKey;
        public SessionExpireFilterAttribute(string loginSessionKey)
        {
            this._LoginSessionKey = string.Empty;
            this._LoginSessionKey = loginSessionKey;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (HttpContext.Current.Session[this._LoginSessionKey] == null)
            {
                var organization = HttpContext.Current.Request.Cookies.Get("OrganizationID");
                if (AjaxRequestExtensions.IsAjaxRequest(filterContext.HttpContext.Request))
                {
                    ActionExecutingContext executingContext = filterContext;
                    JsonResult jsonResult1 = new JsonResult();
                    var dt = new { State = "Timeout", Redirect = UrlHelper.GenerateUrl("default", "TimeoutRedirect", "Home", null, null, filterContext.RequestContext, true) };
                    jsonResult1.Data = dt;// "_Logon_";
                    JsonResult jsonResult2 = jsonResult1;
                    executingContext.Result = (ActionResult)jsonResult2;
                }
                else
                {
                    ActionExecutingContext executingContext = filterContext;
                    RouteValueDictionary routeValues = new RouteValueDictionary();
                    routeValues.Add("Controller", "Home");
                    routeValues.Add("Action", "TimeoutRedirect");
                    if (organization != null)
                    {
                        routeValues.Add("OrganizationID", organization.Value);
                    }
                    RedirectToRouteResult redirectToRouteResult = new RedirectToRouteResult(routeValues);
                    executingContext.Result = (ActionResult)redirectToRouteResult;
                }
            }
            else
            {


                var controller = filterContext.RouteData.Values["controller"].ToString();
                var action = filterContext.RouteData.Values["action"].ToString();
                var Session = HttpContext.Current.Session;
                var loginInfo = SessionLoginInfo.GetInstance(Session);
                var allow = SystemMenuConfig.CheckRoll(controller, action, loginInfo.SystemRoll);
                if (!allow)
                {
                    ActionExecutingContext executingContext = filterContext;
                    RouteValueDictionary routeValues = new RouteValueDictionary();
                    routeValues.Add("Controller", "Home");
                    routeValues.Add("Action", "RolloutRedirect");
                    RedirectToRouteResult redirectToRouteResult = new RedirectToRouteResult(routeValues);
                    executingContext.Result = (ActionResult)redirectToRouteResult;
                }
            }
            base.OnActionExecuting(filterContext);
        }

    }


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class SessionRollFilterAttribute : ActionFilterAttribute
    {

        private SystemRollEnum[] _rolls;
        public SessionRollFilterAttribute(SystemRollEnum[] Rolls)
        {
            this._rolls = Rolls;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var Session = HttpContext.Current.Session;
            var loginInfo = SessionLoginInfo.GetInstance(Session);
            var organization = loginInfo.OrganizationID;// HttpContext.Current.Request.Cookies.Get("OrganizationID");

            if (loginInfo.LoginID == 0 || !_rolls.Contains(loginInfo.SystemRoll))
            {
                if (AjaxRequestExtensions.IsAjaxRequest(filterContext.HttpContext.Request))
                {
                    ActionExecutingContext executingContext = filterContext;
                    JsonResult jsonResult1 = new JsonResult();
                    var dt = new { State = "Timeout", Redirect = UrlHelper.GenerateUrl("default", "RolloutRedirect", "Home", null, null, filterContext.RequestContext, true) };
                    jsonResult1.Data = dt;// "_Logon_";
                    JsonResult jsonResult2 = jsonResult1;
                    executingContext.Result = (ActionResult)jsonResult2;
                }
                else
                {
                    ActionExecutingContext executingContext = filterContext;
                    RouteValueDictionary routeValues = new RouteValueDictionary();
                    routeValues.Add("Controller", "Home");
                    routeValues.Add("Action", "RolloutRedirect");
                    if (string.IsNullOrEmpty(organization))
                    {
                        routeValues.Add("OrganizationID", organization);
                    }
                    RedirectToRouteResult redirectToRouteResult = new RedirectToRouteResult(routeValues);
                    executingContext.Result = (ActionResult)redirectToRouteResult;
                }
            }
            base.OnActionExecuting(filterContext);
        }

    }

}
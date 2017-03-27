using System;
using System.Diagnostics;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using CameraMap.Configs;
using CameraMap.Sessions;
using CameraMap.Controllers;

namespace CameraMap
{
    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        protected void Application_Start()
        {
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AuthConfig.RegisterAuth();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                var ex = Server.GetLastError();

                if (ex == null)
                {
                    return;
                }

                if (ex.Message.IndexOf("favicon.ico", StringComparison.Ordinal) > 0)
                {
                    return;
                }

                //Debug.WriteLine(
                //    @"-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/{0}"
                //    + "DebugInfo ApplicationError:{0}{1}{0}"
                //    + "-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/{0}",
                //    Environment.NewLine, ex.Message);

                //Application[HttpContext.Current.Request.UserHostAddress] = ex;



                var loginInfoSession = SessionLoginInfo.GetInstance(Session);
                var msg = string.Format("{1}{0}{2}", Environment.NewLine, HttpContext.Current.Request.Url.OriginalString, ex.Message);
                SystemLogManager.GetInstance().SetSystemErrorLog(SystemConfig.SystemTitle, loginInfoSession.OrganizationID, loginInfoSession.LoginID, loginInfoSession.UserName, msg, ex.StackTrace);

#if DEBUG
                return;
#endif
                Server.ClearError();

                var url = new UrlHelper(HttpContext.Current.Request.RequestContext);
                Response.Redirect(url.Action("Error", "Home"), false);
                // ReSharper disable once AssignNullToNotNullAttribute
            }
            catch (Exception )
            {
                //Debug.WriteLine(
                //    @"-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/{0}"
                //    + "DebugInfo ApplicationError:{0}{1}{0}"
                //    + "-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/{0}",
                //    Environment.NewLine, ex.Message);
            }
            finally { 
            }
        }

        //protected void Application_Start()
        //{
        //    AreaRegistration.RegisterAllAreas();

        //    WebApiConfig.Register(GlobalConfiguration.Configuration);
        //    FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        //    RouteConfig.RegisterRoutes(RouteTable.Routes);
        //    BundleConfig.RegisterBundles(BundleTable.Bundles);
        //    AuthConfig.RegisterAuth();
        //}
    }
}
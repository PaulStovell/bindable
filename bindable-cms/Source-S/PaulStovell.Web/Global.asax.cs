using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NHibernate;
using PaulStovell.Web.Application.Extensions;
using PaulStovell.Web.Application.Framework;

namespace PaulStovell.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MvcApplication"/> class.
        /// </summary>
        public MvcApplication()
        {
            BeginRequest += Application_BeginRequest;
            EndRequest += Application_EndRequest;
        }

        protected void Application_Start()
        {
            ControllerBuilder.Current.SetControllerFactory(typeof(WindsorControllerFactory));
            RegisterRoutes(RouteTable.Routes);
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("ArchitectureHome", "architecture", controller => "Wiki", action => "Entry", wiki => "Architecture", path => "");
            routes.MapRoute("ArchitectureSubmit", "architecture/{path}/submit", controller => "Wiki", action => "Submit", wiki => "Architecture", path => "");
            routes.MapRoute("ArchitectureEntry", "architecture/{path}", controller => "Wiki", action => "Entry", wiki => "Architecture", path => "");
            routes.MapRoute("Home", "{controller}", controller => "General", action => "Index");
            routes.MapRoute("Default", "{controller}/{action}", controller => "General", action => "Home");
        }

        private void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Items["RenderStartTime"] = DateTime.Now;
        }

        private void Application_EndRequest(object sender, EventArgs e)
        {
            ApplicationService.Current.Resolve<ISession>().Dispose();
        }
    }
}
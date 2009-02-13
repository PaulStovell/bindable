using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Bindable.Cms.Domain.ApplicationModel;
using Bindable.Cms.Domain.Framework;
using Bindable.Cms.Domain.Model;
using Bindable.Cms.Web.Application.Extensions;
using Bindable.Cms.Web.Application.Framework;
using Castle.Core;
using NHibernate;
using NHibernate.Cfg;

namespace Bindable.Cms.Web
{
    public class MvcApplication : HttpApplication
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
            var configuration = new Configuration();
            configuration.SetProperty("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
            configuration.SetProperty("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
            configuration.SetProperty("dialect", "NHibernate.Dialect.MsSql2005Dialect");
            configuration.SetProperty("connection.connection_string", "Server=(local);Data Source=StovellBliki;Trusted_connection=true;");
            configuration.SetProperty("show_sql", "true");
            configuration.AddAssembly(typeof(Wiki).Assembly);

            ApplicationService.Current.RegisterAll<IController>(Assembly.GetExecutingAssembly(), imp => imp.Name.ToLower(), LifestyleType.Transient);
            ApplicationService.Current.RegisterAll<IRepository>(Assembly.GetExecutingAssembly(), LifestyleType.Transient);
            ApplicationService.Current.Register(configuration.BuildSessionFactory());

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
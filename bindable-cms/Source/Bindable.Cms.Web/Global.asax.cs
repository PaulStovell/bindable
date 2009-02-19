using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Bindable.Cms.Database;
using Bindable.Cms.Domain.ApplicationModel;
using Bindable.Cms.Domain.Framework;
using Bindable.Cms.Domain.Infrastructure.Services;
using Bindable.Cms.Domain.Infrastructure.Services.Diagnostics;
using Bindable.Cms.Domain.Repositories.NHibernate;
using Bindable.Cms.Web.Application.Extensions;
using Bindable.Cms.Web.Application.Framework;
using Bindable.Cms.Web.Application.Framework.NVelocitySupport;
using Bindable.Cms.Web.Controllers;
using FluentNHibernate;
using NHibernate;
using NHibernate.Cfg;

namespace Bindable.Cms.Web
{
    /// <summary>
    /// Initial entry point for the application.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MvcApplication"/> class.
        /// </summary>
        public MvcApplication()
        {
            BeginRequest += Application_BeginRequest;
            
        }

        protected void Application_Start()
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Application"].ConnectionString;

            var configuration = new Configuration();
            configuration.SetProperty("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
            configuration.SetProperty("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
            configuration.SetProperty("dialect", "NHibernate.Dialect.MsSql2005Dialect");
            configuration.SetProperty("connection.connection_string", connectionString);
            configuration.SetProperty("show_sql", "true");
            configuration.AddMappingsFromAssembly(Domain.Properties.AssemblyReference.Assembly);
            var sessionFactory = configuration.BuildSessionFactory();

            var htmlExtensionMethods = new[] { 
                typeof(System.Web.Mvc.Html.FormExtensions), 
                typeof(System.Web.Mvc.Html.InputExtensions),
                typeof(System.Web.Mvc.Html.LinkExtensions),
                typeof(System.Web.Mvc.Html.RenderPartialExtensions),
                typeof(System.Web.Mvc.Html.SelectExtensions),
                typeof(System.Web.Mvc.Html.TextAreaExtensions),
                typeof(System.Web.Mvc.Html.ValidationExtensions),
                typeof(NVelocityExtensions),
                typeof(HtmlExtensions)
            };

            ApplicationService.Current.RegisterAll<IController>(typeof(WikiController).Assembly, imp => imp.Name.ToLower(), Lifetime.Transient);
            ApplicationService.Current.RegisterAll<IRepository>(typeof(WikiRepository).Assembly, Lifetime.Transient);
            ApplicationService.Current.RegisterAll<IDiagnosticTest>(typeof(IDiagnosticTest).Assembly, Lifetime.Transient);
            ApplicationService.Current.Register<IApplicationDatabase>(() => new ApplicationDatabase(connectionString), Lifetime.Transient);
            ApplicationService.Current.Register<IDiagnosticService, DiagnosticService>(Lifetime.Transient);
            ApplicationService.Current.Register<ISession>(sessionFactory.OpenSession, Lifetime.PerWebRequest);
            ApplicationService.Current.Register<IViewEngine>(new NVelocityViewEngine(new [] { Properties.AssemblyReference.Assembly }, htmlExtensionMethods));

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(ApplicationService.Current.Resolve<IViewEngine>());
            ControllerBuilder.Current.SetControllerFactory(ApplicationService.Current.RegisterAndResolve<WindsorControllerFactory>(Lifetime.Singleton));
            RegisterRoutes(RouteTable.Routes);
        }

        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");
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
    }
}
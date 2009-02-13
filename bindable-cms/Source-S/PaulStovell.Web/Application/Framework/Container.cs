using System.Reflection;
using System.Web.Mvc;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NHibernate;
using NHibernate.Cfg;
using PaulStovell.Domain.Framework;
using PaulStovell.Domain.Model;

namespace PaulStovell.Web.Application.Framework
{
    public static class ApplicationService
    {
        private static readonly IWindsorContainer _container;

        static ApplicationService()
        {
            _container = new WindsorContainer();
            _container.Register(AllTypes.FromAssembly(Assembly.GetExecutingAssembly()).BasedOn(typeof (IController)).Configure(
                                    comp => comp.Named(comp.ServiceType.Name.ToLower())
                                                .LifeStyle.Is(LifestyleType.Transient)));
            _container.Register(AllTypes.FromAssembly(Assembly.GetExecutingAssembly()).BasedOn(typeof(IRepository)).Configure(
                                    comp => comp.LifeStyle.Is(LifestyleType.Transient)));

            // Configure NHibernate
            var configuration = new Configuration();
            configuration.SetProperty("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
            configuration.SetProperty("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
            configuration.SetProperty("dialect", "NHibernate.Dialect.MsSql2005Dialect");
            configuration.SetProperty("connection.connection_string", "Server=(local);Data Source=StovellBliki;Trusted_connection=true;");
            configuration.SetProperty("show_sql", "true");

            configuration.AddAssembly(typeof(Wiki).Assembly);
            _container.Register(Component.For<ISessionFactory>().Instance(configuration.BuildSessionFactory()).LifeStyle.PerWebRequest);
        }

        public static IWindsorContainer Current
        {
            get { return _container; }
        }
    }
}
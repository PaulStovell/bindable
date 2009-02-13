using System;
using Bindable.Cms.Database;
using Bindable.Cms.Domain.Framework;
using Bindable.DatabaseManagement;
using FluentNHibernate;
using NHibernate;
using NHibernate.Cfg;

namespace Bindable.Cms.Tests.Helpers
{
    public class TestableDatabase : IDisposable
    {
        private readonly TemporarySqlDatabase _database;
        private readonly ISessionFactory _sessionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestableDatabase"/> class.
        /// </summary>
        public TestableDatabase()
        {
            _database = new TemporarySqlDatabase();
            var configuration = new Configuration();
            configuration.SetInterceptor(new PostSaveInterceptor());
            configuration.SetProperty("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
            configuration.SetProperty("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
            configuration.SetProperty("connection.connection_string", _database.ConnectionString);
            configuration.SetProperty("dialect", "NHibernate.Dialect.MsSql2005Dialect");
            configuration.SetProperty("show_sql", "true");
            configuration.AddMappingsFromAssembly(Cms.Domain.Properties.AssemblyReference.Assembly);
            _sessionFactory = configuration.BuildSessionFactory();
            var upgradeManager = new ApplicationDatabase(_database.ConnectionString);
            upgradeManager.PerformUpgrade();
        }

        public ISessionFactory SessionFactory
        {
            get { return _sessionFactory; }
        }

        public void Dispose()
        {
            _sessionFactory.Close();
            _sessionFactory.Dispose();
            _database.Dispose();
        }
    }
}
using System;
using NHibernate;
using NHibernate.Cfg;
using PaulStovell.Database;
using PaulStovell.Database.Management;
using PaulStovell.Domain.Framework;

namespace PaulStovell.Tests.Testing
{
    public class TemporaryDatabase : IDisposable
    {
        private readonly TemporarySqlDatabase _database;
        private readonly ISessionFactory _sessionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemporaryDatabase"/> class.
        /// </summary>
        public TemporaryDatabase()
        {
            _database = new TemporarySqlDatabase();
            var configuration = new Configuration();
            configuration.SetInterceptor(new PostSaveInterceptor());
            configuration.SetProperty("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
            configuration.SetProperty("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
            configuration.SetProperty("connection.connection_string", _database.ConnectionString);
            configuration.SetProperty("dialect", "NHibernate.Dialect.MsSql2005Dialect");
            configuration.SetProperty("show_sql", "true");
            configuration.AddAssembly(typeof(IRepository).Assembly);
            _sessionFactory = configuration.BuildSessionFactory();
            var upgradeManager = new DatabaseManager(_database.ConnectionString);
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
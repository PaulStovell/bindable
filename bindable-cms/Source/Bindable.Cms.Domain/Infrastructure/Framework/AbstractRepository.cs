using NHibernate;

namespace Bindable.Cms.Domain.Framework
{
    public class AbstractRepository : IRepository
    {
        public AbstractRepository(ISession session)
        {
            Session = session;
        }

        protected ISession Session { get; private set; }
    }
}
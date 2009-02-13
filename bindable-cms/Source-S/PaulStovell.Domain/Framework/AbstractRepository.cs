using NHibernate;

namespace PaulStovell.Domain.Framework
{
    public class AbstractRepository
    {
        public AbstractRepository(ISession session)
        {
            Session = session;
        }

        protected ISession Session { get; private set; }
    }
}
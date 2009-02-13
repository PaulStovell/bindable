using System.Data;
using Bindable.Cms.Domain.Framework;
using Bindable.Cms.Domain.Model;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace Bindable.Cms.Domain.Repositories.NHibernate
{
    public class EntryRepository : AbstractRepository, IEntryRepository
    {
        public EntryRepository(ISession sessionFactory) : base(sessionFactory)
        {
        }

        public void SaveEntry(Entry entry)
        {
            using (var transaction = Session.BeginTransaction(IsolationLevel.Serializable))
            {
                Session.SaveOrUpdate(entry);
                transaction.Commit();
            }
        }

        public Entry FindEntry(string wikiName, string entryName)
        {
            return Session.CreateCriteria(typeof (Entry), "entry")
                .Add(Restrictions.Eq("entry.Name", entryName))
                .CreateCriteria("Wiki", "wiki", JoinType.InnerJoin)
                .Add(Restrictions.Eq("wiki.Name", wikiName))
                .SetCacheMode(CacheMode.Refresh)
                .UniqueResult<Entry>();
        }
    }
}
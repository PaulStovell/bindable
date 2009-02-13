using System.Collections.Generic;
using System.Data;
using NHibernate;
using NHibernate.Criterion;
using PaulStovell.Domain.Framework;
using PaulStovell.Domain.Framework.Validation;
using PaulStovell.Domain.Model;

namespace PaulStovell.Domain.Repositories.NHibernate
{
    /// <summary>
    /// Implements the retrieval and storage of wikis using NHibernate.
    /// </summary>
    public class WikiRepository : AbstractRepository, IWikiRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WikiRepository"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public WikiRepository(ISession session) : base(session)
        {
        }

        /// <summary>
        /// Saves a wiki.
        /// </summary>
        /// <param name="wiki">The wiki.</param>
        /// <returns>
        /// A <see cref="ValidationResult"/> with any validation errors.
        /// </returns>
        public ValidationResult SaveWiki(Wiki wiki)
        {
            using (var transaction = Session.BeginTransaction(IsolationLevel.Serializable))
            {
                var validationResult = wiki.Validate();

                // Ensure WikiName is unique amongst active wikis
                if (Session.CreateCriteria(typeof(Wiki))
                    .Add(Restrictions.Like("Name", wiki.Name))
                    .Add(Restrictions.Not(Restrictions.Eq("Id", wiki.Id)))
                    .Add(Restrictions.Eq("IsActive", true))
                    .SetLockMode(LockMode.Upgrade)
                    .UniqueResult() != null)
                {
                    validationResult.FlagRule("OpenId", string.Format("The name '{0}' is already in use amongst active wikis.", wiki.Name));
                }

                // Commit the record
                if (validationResult.Valid)
                {
                    Session.SaveOrUpdate(wiki);
                    transaction.Commit();   
                }
                return validationResult;
            }
        }

        /// <summary>
        /// Finds the wiki given a name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The name of the wiki</returns>
        public Wiki FindActiveWiki(string name)
        {
            return Session.CreateCriteria(typeof (Wiki))
                .Add(Restrictions.Eq("Name", name))
                .Add(Restrictions.Eq("IsActive", true))
                .UniqueResult<Wiki>();
        }

        /// <summary>
        /// Finds all active wikis.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Wiki> FindAllActiveWikis()
        {
            return Session.CreateCriteria(typeof(Wiki))
                .AddOrder(Order.Asc("Name"))
                .List<Wiki>();
        }
    }
}
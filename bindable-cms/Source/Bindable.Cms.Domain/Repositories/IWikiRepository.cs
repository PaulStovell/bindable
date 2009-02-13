using System.Collections.Generic;
using Bindable.Cms.Domain.Framework.Validation;
using Bindable.Cms.Domain.Model;
using Bindable.Cms.Domain.Framework;

namespace Bindable.Cms.Domain.Repositories
{
    /// <summary>
    /// Abstracts the retrieval and storage of Wiki's.
    /// </summary>
    public interface IWikiRepository : IRepository
    {
        /// <summary>
        /// Saves a wiki.
        /// </summary>
        /// <param name="wiki">The wiki.</param>
        /// <returns>A <see cref="ValidationResult"/> with any validation errors.</returns>
        ValidationResult SaveWiki(Wiki wiki);

        /// <summary>
        /// Finds the wiki given a name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The name of the wiki</returns>
        Wiki FindActiveWiki(string name);

        /// <summary>
        /// Finds all active wikis.
        /// </summary>
        IEnumerable<Wiki> FindAllActiveWikis();
    }
}
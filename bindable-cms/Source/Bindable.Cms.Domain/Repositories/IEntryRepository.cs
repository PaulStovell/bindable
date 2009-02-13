using Bindable.Cms.Domain.Framework;
using Bindable.Cms.Domain.Model;

namespace Bindable.Cms.Domain.Repositories
{
    public interface IEntryRepository : IRepository
    {
        void SaveEntry(Entry entry);
        Entry FindEntry(string wikiName, string entryName);
    }
}
using PaulStovell.Domain.Framework;
using PaulStovell.Domain.Model;

namespace PaulStovell.Domain.Repositories
{
    public interface IEntryRepository : IRepository
    {
        void SaveEntry(Entry entry);
        Entry FindEntry(string wikiName, string entryName);
    }
}
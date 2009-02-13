using Bindable.Cms.Domain.Model;
using FluentNHibernate.Mapping;

namespace Bindable.Cms.Domain.Repositories.NHibernate.Mappings
{
    public class EntryMap : ClassMap<Entry>
    {
        public EntryMap()
        {
            SchemaIs("Wiki");

            Id(x => x.Id);
            Map(x => x.Name, "EntryName");
            Map(x => x.Title);
            Map(x => x.Summary);
            Map(x => x.IsActive);
            References(x => x.Wiki, "WikiId");
            HasMany<Revision>(x => x.Revisions).WithKeyColumn("EntryId").AsSet().Cascade.SaveUpdate();
            HasMany<Comment>(x => x.Comments).WithKeyColumn("EntryId").AsSet().Cascade.SaveUpdate();
        }
    }
}
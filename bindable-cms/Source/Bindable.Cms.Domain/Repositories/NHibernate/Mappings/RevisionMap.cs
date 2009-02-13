using Bindable.Cms.Domain.Model;
using FluentNHibernate.Mapping;

namespace Bindable.Cms.Domain.Repositories.NHibernate.Mappings
{
    public class RevisionMap : ClassMap<Revision>
    {
        public RevisionMap()
        {
            SchemaIs("Wiki");

            Id(x => x.Id);
            Map(x => x.Body);
            Map(x => x.RevisionComment);
            Map(x => x.RevisionDateUtc);
            Map(x => x.ModerationStatus);
            Map(x => x.IsActive);
            References(x => x.Entry, "EntryId");
            References(x => x.Member, "MemberId");
        }
    }
}
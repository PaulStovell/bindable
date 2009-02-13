using Bindable.Cms.Domain.Model;
using FluentNHibernate.Mapping;

namespace Bindable.Cms.Domain.Repositories.NHibernate.Mappings
{
    public class MemberMap : ClassMap<Member>
    {
        public MemberMap()
        {
            SchemaIs("Membership");

            Id(x => x.Id);
            Map(x => x.OpenId);
            Map(x => x.EmailAddress);
            Map(x => x.FullName);
            Map(x => x.LastLogin);
            Map(x => x.IsActive);
        }
    }
}
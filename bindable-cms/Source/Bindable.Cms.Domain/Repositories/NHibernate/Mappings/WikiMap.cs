using System.IO;
using Bindable.Cms.Domain.Model;
using FluentNHibernate.Mapping;

namespace Bindable.Cms.Domain.Repositories.NHibernate.Mappings
{
    public class WikiMap : ClassMap<Wiki>
    {
        public WikiMap()
        {
            SchemaIs("Wiki");
            
            Id(x => x.Id);
            Map(x => x.Name, "WikiName");
            Map(x => x.Title);
            Map(x => x.SecurityKey);
            Map(x => x.IsActive);
        }
    }
}
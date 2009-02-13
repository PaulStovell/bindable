using Bindable.Cms.Domain.Model;
using FluentNHibernate.Mapping;

namespace Bindable.Cms.Domain.Repositories.NHibernate.Mappings
{
    public class CommentMap : ClassMap<Comment>
    {
        public CommentMap()
        {
            SchemaIs("Wiki");
            
            Id(x => x.Id);
            Map(x => x.AuthorName);
            Map(x => x.AuthorEmail);
            Map(x => x.AuthorIP);
            Map(x => x.AuthorUrl);
            Map(x => x.CommentBody, "Comment");
            Map(x => x.PostedDateUtc);
            Map(x => x.ModerationStatus);
            Map(x => x.History);
            References(x => x.Entry, "EntryId");
            References(x => x.Member, "MemberId");
        }
    }
}
using PaulStovell.Domain.Model;
using PaulStovell.Web.Application.ContentManagement.Markup;

namespace PaulStovell.Web.Application.ContentManagement.Markup
{
    public class TitleFormatter : AbstractRegexFormatter
    {
        public TitleFormatter() : base(@"^\[Title:\s(?'title'.*?)\]\s*$")
        {
        }

        public override void Apply(Content content, MarkupRendererContext context)
        {
            var match = Expression.Match(content.Html);
            if (match.Success)
            {
                content.Title = match.Groups["title"].Value;
                content.Html = Expression.Replace(content.Html, "");
            }
        }
    }
}
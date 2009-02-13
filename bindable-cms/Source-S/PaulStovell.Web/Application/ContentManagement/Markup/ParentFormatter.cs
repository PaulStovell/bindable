using PaulStovell.Domain.Model;
using PaulStovell.Web.Application.ContentManagement.Markup;

namespace PaulStovell.Web.Application.ContentManagement.Markup
{
    public class ParentFormatter : AbstractRegexFormatter
    {
        public ParentFormatter() : base(@"^\[Parent:\s(?'parent'.*?)\]\s*$")
        {
        }

        public override void Apply(Content content, MarkupRendererContext context)
        {
            var match = Expression.Match(content.Html);
            if (match.Success)
            {
                content.Parent = match.Groups["parent"].Value;
                content.Html = Expression.Replace(content.Html, "");
            }
        }
    }
}

using PaulStovell.Domain.Model;

namespace PaulStovell.Web.Application.ContentManagement.Markup
{
    public class BoldFormatter : AbstractRegexFormatter
    {
        public BoldFormatter() : base(@"([^\\])\*(.*?)\*")
        {
        }

        public override void Apply(Content content, MarkupRendererContext context)
        {
            content.Html = Expression.Replace(content.Html, "$1<strong>$2</strong>");
        }
    }
}
using PaulStovell.Domain.Model;
using PaulStovell.Web.Application.ContentManagement.Markup;

namespace PaulStovell.Web.Application.ContentManagement.Markup
{
    public class HorizontalRuleFormatter : AbstractRegexFormatter
    {
        public HorizontalRuleFormatter() : base(@"^\s*---*")
        {
        }

        public override void Apply(Content content, MarkupRendererContext context)
        {
            content.Html = Expression.Replace(content.Html, "<hr />");
        }
    }
}
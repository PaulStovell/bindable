
namespace Bindable.Cms.Web.Application.ContentManagement.Markup
{
    public class HorizontalRuleFormatter : AbstractRegexFormatter
    {
        public HorizontalRuleFormatter() : base(@"^\s*---*")
        {
        }

        public override void Apply(MarkupContent content, MarkupRendererContext context)
        {
            content.Html = Expression.Replace(content.Html, "<hr />");
        }
    }
}

namespace Bindable.Cms.Web.Application.ContentManagement.Markup
{
    public class QuoteFormatter : AbstractRegexFormatter
    {
        public QuoteFormatter()
            : base(@"\[Quote:\s+(?'Author'.*?)](?'Quote'(.|\W)*?)\[\/Quote\]")
        {
        }

        public override void Apply(MarkupContent content, MarkupRendererContext context)
        {
            content.Html = Expression.Replace(content.Html, "<blockquote><p>${Quote}</p><p><span class='author'>${Author}</span></p></blockquote>");
        }
    }
}
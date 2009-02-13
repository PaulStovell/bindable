using PaulStovell.Domain.Model;

namespace PaulStovell.Web.Application.ContentManagement.Markup
{
    public class SummaryFormatter : AbstractRegexFormatter
    {
        public SummaryFormatter()
            : base(@"\[Summary](?'Summary'(.|\W)*?)\[\/Summary\]")
        {
        }

        public override void Apply(Content content, MarkupRendererContext context)
        {
            var match = Expression.Match(content.Html);
            if (match.Success)
            {
                content.Summary = match.Groups["Summary"].Value;
                content.Html = Expression.Replace(content.Html, "");   
            }
        }
    }
}
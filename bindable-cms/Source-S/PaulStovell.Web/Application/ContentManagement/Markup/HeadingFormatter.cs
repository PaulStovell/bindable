using PaulStovell.Domain.Model;
using PaulStovell.Web.Application.ContentManagement.Markup;

namespace PaulStovell.Web.Application.ContentManagement.Markup
{
    public class HeadingFormatter : AbstractRegexFormatter
    {
        public HeadingFormatter() : base(@"\#\#\#\#\s(\{+)(.*?)(\}+)\s\#\#\#\#")
        {
        }

        public override void Apply(Content content, MarkupRendererContext context)
        {
            var match = Expression.Match(content.Html);
            while (match != null && match.Success)
            {
                var tagname = "h" + (match.Groups[1].Value.Length + 1);
                content.Html = content.Html.Replace(match.Groups[0].Value, string.Format("<{0}>{1}</{0}>\r\n\r\n", tagname, match.Groups[2].Value));
                match = Expression.Match(content.Html);
            }
        }
    }
}
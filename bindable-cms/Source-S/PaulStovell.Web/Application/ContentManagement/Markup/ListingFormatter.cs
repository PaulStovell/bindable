using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web;
using PaulStovell.Domain.Model;

namespace PaulStovell.Web.Application.ContentManagement.Markup
{
    public class ListingFormatter : AbstractRegexFormatter
    {
        public ListingFormatter() : base(@"\[Listing\s+(?'Index'[0-9]+):\s+(?'Language'[a-zA-Z0-9\#]+)\s+\""(?'Caption'.*?)""\](?'Code'(.|\W)*?)\[\/Listing\]")
        {
        }

        public override void Apply(Content content, MarkupRendererContext context)
        {
            content.Html = Expression.Replace(content.Html, Matcher);
        }

        private static string Matcher(Match match)
        {
            var code = (match.Groups["Code"].Value ?? string.Empty).Replace("\t", "    ");
            var lines = code.Split('\n');
            var minIndentation = lines.Select(s => s.TakeWhile(c => c == ' ').Count()).Min();
            if (minIndentation >= 1)
            {
                code = string.Join("\n", lines.Select(line => line.Substring(minIndentation)).ToArray());
            }
            
            return string.Format("<pre class='{0}'>{1}</pre>", match.Groups["Language"].Value.ToLower(), HttpUtility.HtmlEncode(code));
        }
    }
}

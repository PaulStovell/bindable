using System.Linq;
using System.Text.RegularExpressions;
using PaulStovell.Domain.Model;

namespace PaulStovell.Web.Application.ContentManagement.Markup
{
    public class ListFormatter : IMarkupFormatter
    {
        private readonly static Regex _listMatcher = new Regex(@"\n(?'Bullet'\s*[\-|\#]\s+)(?'ListContents'.*?)\n\s*\n", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly static Regex _openingDashes = new Regex(@"^\s*([\-|\#]\s){0,}\-\s+([^\-\#])", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly static Regex _openingHashes = new Regex(@"^\s*([\-|\#]\s){0,}\#\s+([^\-\#])", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly static Regex _cleanLine = new Regex(@"^\s*([\#|\-]\s*){0,}", RegexOptions.Compiled | RegexOptions.Singleline);
        
        public void Apply(Content content, MarkupRendererContext context)
        {
            content.Html = _listMatcher.Replace(content.Html, MatchFormatter);
        }

        private static string MatchFormatter(Match match)
        {
            var writer = new HtmlListWriter();
            var lines = match.Value.Trim().Split('\n');
            for (var lineNumber = 0; lineNumber < lines.Length; lineNumber++)
            {
                var currentLine = lines[lineNumber].TrimEnd();
                var dashes = _openingDashes.Match(currentLine);
                var hashes = _openingHashes.Match(currentLine);
                var currentLineSanitized = _cleanLine.Replace(currentLine, "");
                if (dashes.Success)
                {
                    writer.WriteItem(dashes.Value.Count(c => c == '-' || c == '#'), "ul", currentLineSanitized);
                }
                else if (hashes.Success)
                {
                    writer.WriteItem(hashes.Value.Count(c => c == '-' || c == '#'), "ol", currentLineSanitized);
                }
                else
                {
                    writer.AppendToLastItem(currentLine);
                }
            }
            return writer.ToString();
        }
    }
}
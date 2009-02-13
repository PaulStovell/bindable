using System.Text.RegularExpressions;
using Bindable.Cms.Web.Application.ContentManagement;
using Bindable.Cms.Web.Application.ContentManagement.Markup;

namespace Bindable.Cms.Web.Application.ContentManagement.Markup
{
    public class TableFormatter : IMarkupFormatter
    {
        private readonly static Regex _listMatcher = new Regex(@"\[Table\s+(?'index'[0-9]*):\s+\""(?'caption'.*?)\""\s*\](?'content'.*?)\[\/Table\]", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly static Regex _cellSplitter = new Regex(@"\s{2,}", RegexOptions.Compiled | RegexOptions.Singleline);
        
        public void Apply(MarkupContent content, MarkupRendererContext context)
        {
            content.Html = _listMatcher.Replace(content.Html, MatchFormatter);
        }

        private static string MatchFormatter(Match match)
        {
            var writer = new HtmlTableWriter();
            writer.Caption = match.Groups["caption"].Value;
            writer.Index = match.Groups["index"].Value;
            var lines = match.Groups["content"].Value.Split('\n');
            foreach (var line in lines)
            {
                if (line.Trim().Length == 0) continue;
                
                var cells = _cellSplitter.Split(line.Trim());
                if (cells.Length == 0) continue;

                writer.WriteRow(cells);
            }
            return writer.ToString();
        }
    }
}
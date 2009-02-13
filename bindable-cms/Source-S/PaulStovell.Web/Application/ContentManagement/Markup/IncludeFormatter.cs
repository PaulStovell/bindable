using System.Text;
using System.Text.RegularExpressions;
using PaulStovell.Domain.Model;

namespace PaulStovell.Web.Application.ContentManagement.Markup
{
    public class IncludeFormatter : AbstractRegexFormatter
    {
        public IncludeFormatter() : base(@"\[Include:\s(?'path'.*?)\s*\]")
        {
        }

        public override void Apply(Content content, MarkupRendererContext context)
        {
            content.Html = Expression.Replace(content.Html, 
                delegate (Match match)
                    {
                        var childContent = context.LoadSiblingContent(match.Groups["path"].Value);
                        if (childContent == null) return "";
                        content.SourceFiles.AddRange(childContent.SourceFiles);
                        return childContent.Html;
                    }
                );
        }
    }
}
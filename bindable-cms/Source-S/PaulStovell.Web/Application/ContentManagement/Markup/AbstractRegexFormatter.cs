using System.Text.RegularExpressions;
using PaulStovell.Domain.Model;

namespace PaulStovell.Web.Application.ContentManagement.Markup
{
    public abstract class AbstractRegexFormatter : IMarkupFormatter
    {
        private readonly Regex _regex;

        protected AbstractRegexFormatter(string pattern)
        {
            _regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
        }

        protected Regex Expression { get { return _regex; } }

        public abstract void Apply(Content content, MarkupRendererContext context);
    }
}
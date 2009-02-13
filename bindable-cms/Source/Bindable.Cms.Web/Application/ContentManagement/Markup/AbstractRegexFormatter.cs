using System.Text.RegularExpressions;
using Bindable.Cms.Web.Application.ContentManagement;
using Bindable.Cms.Web.Application.ContentManagement.Markup;

namespace Bindable.Cms.Web.Application.ContentManagement.Markup
{
    public abstract class AbstractRegexFormatter : IMarkupFormatter
    {
        private readonly Regex _regex;

        protected AbstractRegexFormatter(string pattern)
        {
            _regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
        }

        protected Regex Expression { get { return _regex; } }

        public abstract void Apply(MarkupContent content, MarkupRendererContext context);
    }
}
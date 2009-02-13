using System.Text.RegularExpressions;
using Bindable.Cms.Web.Application.ContentManagement;
using Bindable.Cms.Web.Application.ContentManagement.Markup;
using System.Text;
using System.Web;

namespace Bindable.Cms.Web.Application.ContentManagement.Markup
{
    public class InternalLinkFormatter : IMarkupFormatter
    {
        // [Foo]
        private readonly Regex _standardLink = new Regex(@"\[(?'Foo'[a-zA-Z0-9\s\.\/]+)\]", RegexOptions.Compiled);
        // [Foo:Bar]
        private readonly Regex _anchoredLink = new Regex(@"\[(?'Foo'[a-zA-Z0-9]+)\:(?'Bar'[a-zA-Z0-9]+)\]", RegexOptions.Compiled);
        // [Foobar|Foo]
        private readonly Regex _namedLink = new Regex(@"\[(?'Foobar'[a-zA-Z0-9\s\-]+)\|(?'Foo'[a-zA-Z0-9\s\.\/]+)\]", RegexOptions.Compiled);
        // [Foobar|Foo:Bar]
        private readonly Regex _namedAnchoredLink = new Regex(@"\[(?'Foobar'[a-zA-Z0-9\s]+)\|(?'Foo'[a-zA-Z0-9]+)\:(?'Bar'[a-zA-Z0-9]+)\]", RegexOptions.Compiled);

        public void Apply(MarkupContent content, MarkupRendererContext context)
        {
            content.Html = _standardLink.Replace(content.Html, match => BuildLink(context, match.Groups["Foo"].Value, match.Groups["Foo"].Value, null));
            content.Html = _anchoredLink.Replace(content.Html, match => BuildLink(context, match.Groups["Foo"].Value, match.Groups["Foo"].Value, match.Groups["Bar"].Value));
            content.Html = _namedLink.Replace(content.Html, match => BuildLink(context, match.Groups["Foo"].Value, match.Groups["Foobar"].Value, null));
            content.Html = _namedAnchoredLink.Replace(content.Html, match => BuildLink(context, match.Groups["Foo"].Value, match.Groups["Foobar"].Value, match.Groups["Bar"].Value));
        }

        private static string BuildLink(MarkupRendererContext context, string content, string url, string anchor)
        {
            var builder = new StringBuilder();
            builder.Append("<a href='");
            
            if (url != null) builder.Append(context.ResolveInterContentLink(url));
            else builder.Append(context.ResolveInterContentLink(content));
            
            if (anchor != null) builder.Append("#" + anchor);
            
            builder.Append("'>");
            builder.Append(HttpUtility.HtmlEncode(content));
            builder.Append("</a>");
            return builder.ToString();
        }
    }
}
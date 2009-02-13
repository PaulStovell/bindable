using Bindable.Cms.Web.Application.ContentManagement;
using Bindable.Cms.Web.Application.ContentManagement.Markup;

namespace Bindable.Cms.Web.Application.ContentManagement.Markup
{
    public class ParagraphFormatter : IMarkupFormatter
    {
        public void Apply(MarkupContent content, MarkupRendererContext context)
        {
            var lines = content.Html.Split('\n');
            var writer = new HtmlParagraphWriter();
            foreach (var line in lines)
            {
                writer.WriteLine(line);
            }
            content.Html = writer.ToString();
        }
    }
}
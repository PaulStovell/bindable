using PaulStovell.Domain.Model;

namespace PaulStovell.Web.Application.ContentManagement.Markup
{
    public class ParagraphFormatter : IMarkupFormatter
    {
        public void Apply(Content content, MarkupRendererContext context)
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
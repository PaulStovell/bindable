using PaulStovell.Domain.Model;
using PaulStovell.Web.Application.ContentManagement.Markup;

namespace PaulStovell.Web.Application.ContentManagement.Markup
{
    public class BackSlashFormatter : IMarkupFormatter
    {
        public void Apply(Content content, MarkupRendererContext context)
        {
            content.Html = content.Html.Replace("\\", "");
        }
    }
}
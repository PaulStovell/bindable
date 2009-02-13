using Bindable.Cms.Web.Application.ContentManagement;
using Bindable.Cms.Web.Application.ContentManagement.Markup;
using Bindable.Cms.Web.Application.ContentManagement.Markup;

namespace Bindable.Cms.Web.Application.ContentManagement.Markup
{
    public class BackSlashFormatter : IMarkupFormatter
    {
        public void Apply(MarkupContent content, MarkupRendererContext context)
        {
            content.Html = content.Html.Replace("\\", "");
        }
    }
}
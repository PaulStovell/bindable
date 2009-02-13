using Bindable.Cms.Web.Application.ContentManagement.Markup;

namespace Bindable.Cms.Web.Application.ContentManagement.Markup
{
    public interface IMarkupFormatter
    {
        void Apply(MarkupContent content, MarkupRendererContext context);
    }
}
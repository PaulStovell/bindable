using PaulStovell.Domain.Model;
using PaulStovell.Domain.Model;

namespace PaulStovell.Web.Application.ContentManagement.Markup
{
    public interface IMarkupFormatter
    {
        void Apply(Content content, MarkupRendererContext context);
    }
}
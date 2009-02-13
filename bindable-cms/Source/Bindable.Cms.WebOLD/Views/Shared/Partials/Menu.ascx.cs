
namespace Bindable.Cms.Web.Views.Shared.Partials
{
    public partial class Menu : System.Web.Mvc.ViewUserControl
    {
        protected string IsActive(string navigationGroup)
        {
            return (string) ViewData["NavigationGroup"] == navigationGroup ? "class='active'" : "";
        }
    }
}

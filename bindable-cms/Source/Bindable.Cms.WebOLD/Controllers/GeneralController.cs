using System.Web.Mvc;

namespace Bindable.Cms.Web.Controllers
{
    [HandleError]
    public class GeneralController : Controller
    {
        public ActionResult Index()
        {
            ViewData["NavigationGroup"] = "Home";
            return View();
        }

        public ActionResult Projects()
        {
            ViewData["NavigationGroup"] = "Projects";
            return View();
        }

        public ActionResult Presentations()
        {
            ViewData["NavigationGroup"] = "Presentations";
            return View();
        }

        public ActionResult Contact()
        {
            ViewData["NavigationGroup"] = "Contact";
            return View();
        }
    }
}

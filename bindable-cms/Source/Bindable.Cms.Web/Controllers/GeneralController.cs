using System.Web.Mvc;

namespace Bindable.Cms.Web.Controllers
{
    [HandleError]
    public class GeneralController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Projects()
        {
            return View();
        }

        public ActionResult Presentations()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}

using System.Web.Mvc;

namespace Bindable.Cms.Web.Controllers
{
    [HandleError]
    public class BlogController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Archive()
        {
            return View();
        }

        public ActionResult Entry()
        {
            return View();
        }

        public ActionResult SubmitComment()
        {
            return View();
        }

        public ActionResult Feed()
        {
            return View();
        }
    }
}

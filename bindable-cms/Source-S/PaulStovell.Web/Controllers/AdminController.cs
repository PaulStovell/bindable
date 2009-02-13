using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenId;
using DotNetOpenId.Extensions.SimpleRegistration;
using DotNetOpenId.RelyingParty;

namespace PaulStovell.Web.Controllers
{
    [HandleError]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated) Response.Redirect("/Admin/Login?ReturnUrl=/Admin/Index");
            return View();
        }
    }
}

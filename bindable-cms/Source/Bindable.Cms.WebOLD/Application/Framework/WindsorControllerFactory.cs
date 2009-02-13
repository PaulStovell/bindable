using System.Web.Mvc;
using System.Web.Routing;
using Bindable.Cms.Web.Application.Framework;

namespace Bindable.Cms.Web.Application.Framework
{
    public class WindsorControllerFactory : IControllerFactory
    {
        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            return (IController)ApplicationService.Current.Resolve(controllerName.ToLower() + "controller");
        }

        public void ReleaseController(IController controller)
        {
            ApplicationService.Current.Release(controller);
        }
    }
}
using System.Web.Mvc;
using System.Web.Routing;
using Bindable.Cms.Domain.ApplicationModel;

namespace Bindable.Cms.Web.Application.Framework
{
    public class WindsorControllerFactory : IControllerFactory
    {
        private readonly IApplicationService _applicationService;

        public WindsorControllerFactory(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            return (IController)_applicationService.Resolve(controllerName.ToLower() + "controller");
        }

        public void ReleaseController(IController controller)
        {
            _applicationService.Release(controller);
        }
    }
}
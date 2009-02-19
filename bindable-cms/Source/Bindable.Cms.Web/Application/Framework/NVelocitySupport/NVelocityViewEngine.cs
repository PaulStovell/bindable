using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Commons.Collections;
using NVelocity.App;

namespace Bindable.Cms.Web.Application.Framework.NVelocitySupport
{
    /// <summary>
    /// An ASP.NET MVC view engine for NVelocity.
    /// </summary>
    public class NVelocityViewEngine : IViewEngine
    {
        // The following constants could be turned into configuration settings later
        private const string _partialPath = "{0}.Views.Partials.{1}.vm";
        private const string _viewPath = "{0}.Views.{1}.{2}.vm";
        private const string _masterPath = "{0}.Views.Layouts.{1}.vm";
        private const string _defaultMasterPage = "Site";
        private readonly VelocityEngine _engine;
        private readonly DynamicDispatchMethod[] _extensionMethods;

        /// <summary>
        /// Initializes a new instance of the <see cref="NVelocityViewEngine"/> class.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="extensionMethodTypes">The extension method types.</param>
        public NVelocityViewEngine(IEnumerable<Assembly> assemblies, params Type[] extensionMethodTypes)
        {
            var properties = new ExtendedProperties();
            properties.AddProperty("resource.loader", "parials");
            properties.AddProperty("parials.resource.loader.class", typeof(PartialFileResourceLoader).AssemblyQualifiedName.Replace(",", ";"));
            properties.AddProperty("parials.resource.loader.assembly", assemblies.Select(a => a.FullName).ToList());
            _engine = new VelocityEngine();
            _engine.Init(properties);

            _extensionMethods = extensionMethodTypes.SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Static)).Select(method => new DynamicDispatchMethod(method)).ToArray();
        }

        /// <summary>
        /// Finds a partial view.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="partialViewName">Partial name of the view.</param>
        /// <param name="useCache">if set to <c>true</c> [use cache].</param>
        /// <returns></returns>
        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            var viewPath = string.Format(
                _partialPath,
                controllerContext.Controller.GetType().Assembly.GetName().Name,
                partialViewName
                );

            var childView = LocateAndPrepareView(controllerContext, viewPath);
            return new ViewEngineResult(childView, this);
        }

        /// <summary>
        /// Finds a view.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="masterName">Name of the master.</param>
        /// <param name="useCache">if set to <c>true</c> [use cache].</param>
        /// <returns></returns>
        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            if (string.IsNullOrEmpty(masterName)) masterName = _defaultMasterPage;
            
            var viewPath = string.Format(
                _viewPath,
                controllerContext.Controller.GetType().Assembly.GetName().Name,
                controllerContext.Controller.GetType().Name.Replace("Controller", ""),
                viewName
                );
            var masterPath = string.Format(
                _masterPath,
                controllerContext.Controller.GetType().Assembly.GetName().Name,
                masterName
                );

            var masterView = LocateAndPrepareView(controllerContext, masterPath);
            masterView.Context.Put("RequestedViewName", viewPath);
            controllerContext.Controller.ViewData["RequestedViewName"] = viewPath;
            return new ViewEngineResult(masterView, this);
        }

        /// <summary>
        /// Locates, loads, and prepares a view.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="fullPath">The full path.</param>
        /// <returns></returns>
        private NVelocityView LocateAndPrepareView(ControllerContext controllerContext, string fullPath)
        {
            if (!_engine.TemplateExists(fullPath))
            {
                return null;
            }

            var template = _engine.GetTemplate(fullPath);
            var view = new NVelocityView(template, controllerContext.Controller.ViewData, _engine);
            
            // Build the context for the HtmlHelper
            var viewContext = new ViewContext(controllerContext, view, controllerContext.Controller.ViewData, controllerContext.Controller.TempData);
            var htmlHelper = new HtmlHelper(viewContext, view, RouteTable.Routes);
            var htmlHelperWrapper = new ExtensionMethodEnabler(htmlHelper, _extensionMethods);

            // Make all information from the controller's ViewData available to NVelocity, as well as the HtmlHelper
            view.Context.Put("Html", htmlHelperWrapper);
            foreach (var kvp in controllerContext.Controller.ViewData)
            {
                view.Context.Put(kvp.Key, kvp.Value);
            }
            return view;
        }

        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
        }
    }
}
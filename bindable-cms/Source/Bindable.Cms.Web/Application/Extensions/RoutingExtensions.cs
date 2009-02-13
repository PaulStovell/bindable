using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace Bindable.Cms.Web.Application.Extensions
{
    public static class RoutingExtensions
    {
        public static Route MapRoute(this RouteCollection routes, string name, string urlPattern, params Func<string, object>[] defaultsHashes)
        {
            var defaults = new Dictionary<string, object>();
            foreach (var hash in defaultsHashes)
            {
                var key = hash.Method.GetParameters()[0].Name;
                defaults.Add(key, hash(key));
            }
            var route = new LowercaseRoute(urlPattern, new MvcRouteHandler());
            route.Defaults = new RouteValueDictionary(defaults);
            route.Constraints = new RouteValueDictionary((object)null);
            routes.Add(name, route);
            return route;
        }

        public class LowercaseRoute : System.Web.Routing.Route
        {
            public LowercaseRoute(string url, IRouteHandler routeHandler) : base(url, routeHandler) { }
            public LowercaseRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler) : base(url, defaults, routeHandler) { }
            public LowercaseRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler) : base(url, defaults, constraints, routeHandler) { }
            public LowercaseRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler) : base(url, defaults, constraints, dataTokens, routeHandler) { }

            public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
            {
                var path = base.GetVirtualPath(requestContext, values);

                if (path != null)
                    path.VirtualPath = path.VirtualPath.ToLowerInvariant();

                return path;
            }
        }
    }
}
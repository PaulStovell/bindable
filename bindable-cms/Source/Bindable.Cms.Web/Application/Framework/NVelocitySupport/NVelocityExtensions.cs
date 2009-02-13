using System;
using System.Collections;
using System.IO;
using System.Web.Mvc;
using NVelocity;

namespace Bindable.Cms.Web.Application.Framework.NVelocitySupport
{
    public static class NVelocityExtensions
    {
        public static string RenderView(this HtmlHelper html, string viewName)
        {
            return RenderView(html, viewName, null, null);
        }

        public static string RenderView(this HtmlHelper html, string viewName, string sectionName)
        {
            return RenderView(html, viewName, sectionName, null);
        }

        public static string RenderView(this HtmlHelper html, string viewName, IDictionary parameters)
        {
            return RenderView(html, viewName, null, parameters);
        }

        public static string RenderView(this HtmlHelper html, string viewName, string sectionName, IDictionary parameters)
        {
            if (!string.IsNullOrEmpty(sectionName))
            {
                viewName += ":" + sectionName;
            }

            var velocityView = html.ViewContext.View as NVelocityView;
            if (velocityView == null) throw new InvalidOperationException("The RenderView extension can only be used from views that were created using NVelocity.");

            var newContext = velocityView.Context;
            if (parameters != null && parameters.Keys.Count > 0)
            {
                // Clone the existing context and then add the custom parameters
                newContext = new VelocityContext();
                foreach (var key in velocityView.Context.Keys)
                {
                    newContext.Put((string)key, velocityView.Context.Get((string)key));
                }
                foreach (var key in parameters.Keys)
                {
                    newContext.Put((string)key, parameters[key]);
                }
            }

            // Resolve the template and render it. The partial resource loader takes care of validating
            // the view name and extracting the partials.
            var template = velocityView.Engine.GetTemplate(viewName);
            using (var writer = new StringWriter())
            {
                template.Merge(newContext, writer);
                return writer.ToString();
            }
        }

        public static string Format(this HtmlHelper helper, string foo, params string[] bars)
        {
            return string.Format(foo, bars);
        }
    }
}
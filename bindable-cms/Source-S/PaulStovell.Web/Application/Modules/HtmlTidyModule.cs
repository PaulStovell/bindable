using System;
using System.Web;

namespace PaulStovell.Web.Application.Modules
{
    public class HtmlTidyModule : IHttpModule
    {
        private static void Context_BeginRequest(object sender, EventArgs e)
        {
            var context = HttpContext.Current;
            if (!context.Request.Url.PathAndQuery.StartsWith("/Resources"))
            {
                //context.Response.Filter = new HtmlTidyStream(context.Response.Filter);
            }
        }
        
        public void Init(HttpApplication context)
        {
            context.BeginRequest += Context_BeginRequest;
        }
        
        public void Dispose()
        {
        }
    }
}
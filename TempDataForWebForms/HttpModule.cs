namespace TempDataForWebForms
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.UI;

    /// <summary>
    /// HTTP Module implementing tempdata in WebForms pages.
    /// - On PostMapRequestHandler fills the HttpContext.Items["TempDataDictionary"] with the tempdata loaded from configured tempdataprovider.
    /// - On EndRequest clears tempdata if not in a redirection process.
    /// </summary>
    public class HttpModule : IHttpModule
    {
        public const string KEY_TEMPDATA_HTTP_CONTEXT_ITEMS = "TempDataDictionary";

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application </param>
        public void Init(HttpApplication context)
        {
            context.PostMapRequestHandler += WrapHttpContext(this.PostMapRequestHandler);
            context.EndRequest += WrapHttpContext(this.OnEndRequest);
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Raised when request has been mapped to a handler.
        /// </summary>
        /// <param name="httpContext">The current HTTP context.</param>
        internal void PostMapRequestHandler(HttpContextBase httpContext)
        {
            if (httpContext.Handler == null || httpContext.Handler as Page == null)
            {
                return;
            }
            var tempData = new TempDataDictionary();
            var tempDataProvider = DependencyResolver.Current.GetService<ITempDataProvider>();
            tempData.Load(new ControllerContext() { HttpContext = httpContext }, tempDataProvider);
            httpContext.Items[KEY_TEMPDATA_HTTP_CONTEXT_ITEMS] = tempData;
        }

        /// <summary>
        /// Raised when request ends.
        /// </summary>
        /// <param name="httpContext">The current HTTP context.</param>
        internal void OnEndRequest(HttpContextBase httpContext)
        {
            var tempData = httpContext.Items[KEY_TEMPDATA_HTTP_CONTEXT_ITEMS] as TempDataDictionary;
            var tempDataProvider = DependencyResolver.Current.GetService<ITempDataProvider>();
            if (tempData == null)
            {
                return;
            }
            if (!httpContext.Response.IsRequestBeingRedirected && tempData.Count > 0)
            {
                tempData.Clear();
            }
            tempData.Save(new ControllerContext() { HttpContext = httpContext }, tempDataProvider);
        }

        /// <summary>
        /// Provides an event handler which wraps an http context in a httpcontextbase and executes the action provided.
        /// </summary>
        /// <param name="handler">The action to execute on the context.</param>
        /// <returns>The event handler.</returns>
        private static EventHandler WrapHttpContext(Action<HttpContextBase> handler)
        {
            return (sender, e) => handler(new HttpContextWrapper(((HttpApplication)sender).Context));
        }
    }
}
namespace TempDataForWebForms
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.UI;

    /// <summary>
    /// Set of extension methods used to retrieve tempdata.
    /// </summary>
    public static class PageExtensions
    {
        /// <summary>
        /// Gets the tempdata associated with the current HTTP context.
        /// </summary>
        /// <param name="page">The page on which the extension method applies.</param>
        /// <returns>The tempdata associated with the current HTTP context.</returns>
        public static TempDataDictionary GetTempData(this Page page)
        {
            return GetTempDataFromContext(page.Request.RequestContext);
        }

        /// <summary>
        /// Gets the tempdata associated with the current HTTP context.
        /// </summary>
        /// <param name="control">The user control on which the extension method applies.</param>
        /// <returns>The tempdata associated with the current HTTP context.</returns>
        public static TempDataDictionary GetTempData(this UserControl control)
        {
            return GetTempDataFromContext(control.Request.RequestContext);
        }

        /// <summary>
        /// Returns the tempdata from a request context.
        /// </summary>
        /// <param name="context">The request context.</param>
        /// <returns>The tempdata.</returns>
        private static TempDataDictionary GetTempDataFromContext(RequestContext context)
        {
            return context.HttpContext.Items[HttpModule.KEY_TEMPDATA_HTTP_CONTEXT_ITEMS] as TempDataDictionary;
        }
    }
}

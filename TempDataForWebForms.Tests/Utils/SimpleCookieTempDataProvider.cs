namespace TempDataForWebForms.Tests.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// A simple implementation of tempdata provider using cookies.
    /// </summary>
    internal class SimpleCookieTempDataProvider : ITempDataProvider
    {
        internal const string TEMP_DATA_COOKIE_KEY = "__ControllerTempData";

        public IDictionary<string, object> LoadTempData(ControllerContext controllerContext)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            return LoadTempData(controllerContext.HttpContext);
        }

        public IDictionary<string, object> LoadTempData(HttpContextBase context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var cookie = context.Request.Cookies[TEMP_DATA_COOKIE_KEY];
            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                return cookie.Value.DeserializeBase64EncodedString();
            }
            return null;
        }

        public void SaveTempData(ControllerContext controllerContext, IDictionary<string, object> values)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            SaveTempData(controllerContext.HttpContext, values);
        }

        public void SaveTempData(HttpContextBase context, IDictionary<string, object> values)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var cookieValue = string.Empty;

            if (values != null && values.Any())
            {
                cookieValue = values.SerializeToBase64EncodedString();
            }

            if (!string.IsNullOrEmpty(cookieValue))
            {
                context.Response.Cookies.Add(new HttpCookie(TEMP_DATA_COOKIE_KEY, cookieValue) { Path = "/", HttpOnly = true });
            }
            else if (context.Request.Cookies.AllKeys.Contains(TEMP_DATA_COOKIE_KEY))
            {
                context.Response.Cookies.Add(new HttpCookie(TEMP_DATA_COOKIE_KEY, cookieValue) { Path = "/", HttpOnly = true, Expires =  DateTime.UtcNow.AddDays(-1)});
            }
        }
    }
}

namespace TempDataForWebForms.SampleApp
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using TestUtils;

    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            DependencyResolver.SetResolver(new TestDependencyResolver());
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            RouteTable.Routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}
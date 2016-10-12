namespace TempDataForWebForms.TestUtils
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class TestDependencyResolver : IDependencyResolver
    {
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(ITempDataProvider))
            {
                return new SimpleCookieTempDataProvider();
            }
            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (serviceType == typeof(ITempDataProvider))
            {
                return new List<ITempDataProvider>() { new SimpleCookieTempDataProvider() };
            }
            return new List<object>();
        }
    }
}

# TempDataForWebForms
A simple library to provide ASP.Net MVC TempData like support in WebForms pages.

[![Build status](https://ci.appveyor.com/api/projects/status/nysjiju410w0trxm?svg=true)](https://ci.appveyor.com/project/ogaudefroy/tempdataforwebforms) [![NuGet version](https://badge.fury.io/nu/TempDataForWebForms.svg)](https://badge.fury.io/nu/TempDataForWebForms)

This component might help you when mixin ASP.Net WebForms navigation flow with ASP.Net MVC navigation flow in the same application.

Implementation is made with a custom HTTP Module implementing as following:
 - On [PostMapRequestHandler](https://msdn.microsoft.com/en-us/library/system.web.httpapplication.postmaprequesthandler%28v=vs.110%29.aspx) fills the HttpContext.Items[HttpModule.KeyTempDataItems] with the tempdata loaded from the tempdata provider.
 - On [EndRequest](https://msdn.microsoft.com/en-us/library/system.web.httpapplication.endrequest%28v=vs.110%29.aspx) clears tempdata if not in a redirection process.

ITempDataProvider implementation is resolved with ASP.Net MVC [DependencyResolver] () with the following behavior:
 - If an [ITempDataProviderFactory] (https://msdn.microsoft.com/en-us/library/system.web.mvc.itempdataproviderfactory(v=vs.118).aspx) implementation is registered then create a ITempDataProvider via [CreateInstance] (https://msdn.microsoft.com/en-us/library/system.web.mvc.itempdataproviderfactory.createinstance(v=vs.118).aspx#M:System.Web.Mvc.ITempDataProviderFactory.CreateInstance).
 - If an [ITempDataProvider] (https://msdn.microsoft.com/en-us/library/system.web.mvc.itempdataprovider(v=vs.118).aspx) implementation is registered then return the registered implementation.
 - Return a [SessionStateTempDataProvider] (https://msdn.microsoft.com/en-us/library/system.web.mvc.sessionstatetempdataprovider(v=vs.118).aspx)
 
Typical use case: flash messages.

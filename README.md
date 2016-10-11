# TempDataForWebForms
A simple library to provide ASP.Net MVC TempData like support in WebForms pages.

This component might help you when mixin ASP.Net WebForms navigation flow with ASP.Net MVC navigation flow in the same application.

Implementation is made with a custom HTTP Module implementing as following:
 - On [PostMapRequestHandler](https://msdn.microsoft.com/en-us/library/system.web.httpapplication.postmaprequesthandler%28v=vs.110%29.aspx) fills the HttpContext.Items[HttpModule.KeyTempDataItems] with the tempdata loaded from the tempdata provider.
 - On [EndRequest](https://msdn.microsoft.com/en-us/library/system.web.httpapplication.endrequest%28v=vs.110%29.aspx) clears tempdata if not in a redirection process.

Typical use case: sessionless flash messages.

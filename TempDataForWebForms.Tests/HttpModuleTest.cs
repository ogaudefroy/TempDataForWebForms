namespace TempDataForWebForms.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.UI;
    using Moq;
    using NUnit.Framework;
    using TestUtils;

    [TestFixture]
    public class HttpModuleTest
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            DependencyResolver.SetResolver(new TestDependencyResolver());
        }

        [Test]
        public void HttpModule_PostMapRequestHandler_WithNoCookie()
        {
            var tempDataModule = new HttpModule();
            var httpContextBase = this.CreateHttpContextBase();

            tempDataModule.PostMapRequestHandler(httpContextBase);
            var tempData = httpContextBase.Items[HttpModule.KEY_TEMPDATA_HTTP_CONTEXT_ITEMS] as TempDataDictionary;

            Assert.That(tempData, Is.Not.Null);
            Assert.That(tempData.Count, Is.EqualTo(0));
        }

        [Test]
        public void HttpModule_PostMapRequestHandler_WithCookie()
        {
            var tempDataModule = new HttpModule();
            var httpContextBase = this.CreateHttpContextBase();
            var cookie = CreateCookie();
            httpContextBase.Request.Cookies.Add(cookie);

            tempDataModule.PostMapRequestHandler(httpContextBase);
            var tempData = httpContextBase.Items[HttpModule.KEY_TEMPDATA_HTTP_CONTEXT_ITEMS] as TempDataDictionary;

            Assert.That(tempData, Is.Not.Null);
            Assert.That(tempData.Count, Is.EqualTo(1));
            Assert.That(tempData["key"], Is.EqualTo("samplevalue"));
        }

        [Test]
        public void HttpModule_OnEndRequest_NoRedirect()
        {
            var tempDataModule = new HttpModule();
            var httpContextBase = this.CreateHttpContextBase();
            httpContextBase.Items[HttpModule.KEY_TEMPDATA_HTTP_CONTEXT_ITEMS] = new TempDataDictionary { { "key", "samplevalue" } };

            tempDataModule.OnEndRequest(httpContextBase);

            Assert.That(httpContextBase.Response.Cookies.Count, Is.EqualTo(0));
        }

        [Test]
        public void HttpModule_OnEndRequest_NoRedirectWithCookie()
        {
            var tempDataModule = new HttpModule();
            var httpContextBase = this.CreateHttpContextBase();
            var cookie = CreateCookie();
            httpContextBase.Request.Cookies.Add(cookie);
            httpContextBase.Items[HttpModule.KEY_TEMPDATA_HTTP_CONTEXT_ITEMS] = new TempDataDictionary { { "key", "samplevalue" } };

            tempDataModule.OnEndRequest(httpContextBase);

            Assert.That(httpContextBase.Response.Cookies.Count, Is.EqualTo(1));
            var resultCookie = httpContextBase.Response.Cookies[SimpleCookieTempDataProvider.TEMP_DATA_COOKIE_KEY];
            Assert.That(resultCookie, Is.Not.Null);
            Assert.That(resultCookie.Value, Is.Null.Or.Empty);
            Assert.That(resultCookie.Expires, Is.LessThanOrEqualTo(DateTime.Now));
        }

        [Test]
        public void HttpModule_OnEndRequest_Redirect()
        {
            var tempDataModule = new HttpModule();
            var httpContextBase = this.CreateHttpContextBase(true);
            httpContextBase.Items[HttpModule.KEY_TEMPDATA_HTTP_CONTEXT_ITEMS] = new TempDataDictionary { { "key", "samplevalue" } };

            tempDataModule.OnEndRequest(httpContextBase);

            Assert.That(httpContextBase.Response.Cookies.Count, Is.EqualTo(1));
            var cookie = httpContextBase.Response.Cookies[SimpleCookieTempDataProvider.TEMP_DATA_COOKIE_KEY];
            Assert.That(cookie, Is.Not.Null);
            var values = cookie.Value.DeserializeBase64EncodedString();
            Assert.That(values, Is.Not.Null);
            Assert.That(values.Count, Is.EqualTo(1));
            Assert.That(values["key"], Is.Not.Null);
            Assert.That(values["key"], Is.EqualTo("samplevalue"));
        }

        [Test]
        public void HttpModule_OnEndRequest_Redirect_WithReadValue()
        {
            var tempDataModule = new HttpModule();
            var httpContextBase = this.CreateHttpContextBase(true);
            httpContextBase.Items[HttpModule.KEY_TEMPDATA_HTTP_CONTEXT_ITEMS] = new TempDataDictionary { { "key", "samplevalue" }, { "alternateKey", "samplevalue2" } };

            tempDataModule.OnEndRequest(httpContextBase);

            Assert.AreEqual(1, httpContextBase.Response.Cookies.Count);
            var cookie = httpContextBase.Response.Cookies[SimpleCookieTempDataProvider.TEMP_DATA_COOKIE_KEY];
            Assert.IsNotNull(cookie);
            var values = cookie.Value.DeserializeBase64EncodedString();
            Assert.That(values, Is.Not.Null);
            Assert.That(values.Count, Is.EqualTo(2));
            Assert.That(values["alternateKey"], Is.Not.Null);
            Assert.That(values["alternateKey"], Is.EqualTo("samplevalue2"));
        }

        private HttpContextBase CreateHttpContextBase(bool isRedirect = false)
        {
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.SetupGet(r => r.Cookies).Returns(new HttpCookieCollection());

            var mockResponse = new Mock<HttpResponseBase>();
            mockResponse.SetupGet(r => r.Cookies).Returns(new HttpCookieCollection());
            mockResponse.SetupGet(r => r.IsRequestBeingRedirected).Returns(isRedirect);

            var mockCtx = new Mock<HttpContextBase>();
            mockCtx.SetupGet(c => c.Request).Returns(mockRequest.Object);
            mockCtx.SetupGet(c => c.Response).Returns(mockResponse.Object);
            mockCtx.SetupGet(c => c.Items).Returns(new Hashtable());
            mockCtx.SetupGet(c => c.Handler).Returns(new Page());

            return mockCtx.Object;
        }

        private HttpCookie CreateCookie()
        {
            var values = new Dictionary<string, object> { { "key", "samplevalue" } };
            var cookie = new HttpCookie(SimpleCookieTempDataProvider.TEMP_DATA_COOKIE_KEY)
            {
                HttpOnly = true,
                Value = values.SerializeToBase64EncodedString()
            };
            return cookie;
        }
    }
}

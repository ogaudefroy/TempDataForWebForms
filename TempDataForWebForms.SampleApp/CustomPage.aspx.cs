namespace TempDataForWebForms.SampleApp
{
    using System;
    using System.Web.Mvc;

    public partial class CustomPage : System.Web.UI.Page
    {
        public TempDataDictionary TempData
        {
            get { return this.GetTempData(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void OnClick(object sender, EventArgs e)
        {
            this.GetTempData()["Message"] = "Hello from MVC !";
            Response.Redirect("~/Home/Index");
        }
    }
}
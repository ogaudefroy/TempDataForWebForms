namespace TempDataForWebForms.SampleApp
{
    using System;
    using System.Web.Mvc;
    using System.Web.UI.WebControls;

    public partial class CustomPage : System.Web.UI.Page
    {
        public TempDataDictionary TempData
        {
            get { return this.GetTempData(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            foreach (var key in TempData.Keys)
            {
                var content = string.Format("<li>{0}: {1}</li>", key, TempData[key]);
                list.Controls.Add(new Literal() { Text = content});
                
            }
        }

        protected void OnClick(object sender, EventArgs e)
        {
            this.GetTempData()["Message"] = "Hello from WebForms !";
            this.GetTempData()["Time"] = DateTime.UtcNow;
            Response.Redirect("~/Home/Index");
        }
    }
}
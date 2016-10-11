namespace TempDataForWebForms.SampleApp.Controllers
{
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PostToWebForms()
        {
            this.TempData["Message"] = "Hello from MVC !";
            return Redirect("~/CustomPage.aspx");
        }
    }
}
using Microsoft.AspNetCore.Mvc;

namespace OmanChartsWeb.Controllers
{
    public class ProjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

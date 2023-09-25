using Microsoft.AspNetCore.Mvc;

namespace OmanChartsWeb.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

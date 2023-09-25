using Microsoft.AspNetCore.Mvc;

namespace OmanChartsWeb.Controllers
{
    public class StatisticController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

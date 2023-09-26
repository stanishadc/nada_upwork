using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace OmanChartsWeb.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Role") == null)
            {
                var routeValue = new RouteValueDictionary(new { action = "Index", controller = "Home" });
                return RedirectToRoute(routeValue);
            }
            return View();
        }
    }
}

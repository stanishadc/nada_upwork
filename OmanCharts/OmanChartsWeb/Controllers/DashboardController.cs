using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OmanChartsWeb.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OmanChartsWeb.Controllers
{
    public class DashboardController : Controller
    {
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Role") == null)
            {
                var routeValue = new RouteValueDictionary(new { action = "Index", controller = "Home" });
                return RedirectToRoute(routeValue);
            }
            using (var httpClient = new HttpClient())
            {                
                if (HttpContext.Session.GetString("Role") == "Customer")
                {
                    using (var apiresponse = await httpClient.GetAsync("https://localhost:7089/api/Statistic/GetUserDashboard?ZoneId=" + HttpContext.Session.GetString("ZoneId") + "&UserId=" + HttpContext.Session.GetString("UserId") + ""))
                    {
                        var apiData = await apiresponse.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<DashBoardStatistic>(apiData);
                        return View(data);
                    }
                }
                else if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    using (var apiresponse = await httpClient.GetAsync("https://localhost:7089/api/Statistic/GetAdminDashboard"))
                    {
                        var apiData = await apiresponse.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<DashBoardStatistic>(apiData);
                        return View(data);
                    }
                }
            }
            return View();
        }
    }
}

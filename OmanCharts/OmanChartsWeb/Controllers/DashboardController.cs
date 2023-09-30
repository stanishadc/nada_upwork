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
                    using (var apiresponse = await httpClient.GetAsync("https://localhost:7089/api/Statistic/GetDashboard?ZoneId=" + HttpContext.Session.GetString("ZoneId") + "&UserId=" + HttpContext.Session.GetString("UserId") + ""))
                    {
                        var apiData = await apiresponse.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<DashBoardStatistic>(apiData);
                        ViewBag.Labels = JsonConvert.SerializeObject(data.Labels);
                        ViewBag.LabourSeries = JsonConvert.SerializeObject(data.LabourSeries);
                        ViewBag.ProjectSeries = JsonConvert.SerializeObject(data.ProjectSeries);
                        ViewBag.ORateSeries = JsonConvert.SerializeObject(data.ORateSeries);
                        ViewBag.InvestorSeries = JsonConvert.SerializeObject(data.InvestorSeries);
                        return View(data);
                    }
                }
                else if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    using (var apiresponse = await httpClient.GetAsync("https://localhost:7089/api/Statistic/GetDashboard"))
                    {
                        var apiData = await apiresponse.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<DashBoardStatistic>(apiData);
                        ViewBag.Labels = JsonConvert.SerializeObject(data.Labels);
                        ViewBag.LabourSeries = JsonConvert.SerializeObject(data.LabourSeries);
                        ViewBag.ProjectSeries = JsonConvert.SerializeObject(data.ProjectSeries);
                        ViewBag.ORateSeries = JsonConvert.SerializeObject(data.ORateSeries);
                        ViewBag.InvestorSeries = JsonConvert.SerializeObject(data.InvestorSeries);
                        return View(data);
                    }
                }
            }
            return View();
        }
    }
}

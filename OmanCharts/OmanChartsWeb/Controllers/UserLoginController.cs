using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OmanChartsWeb.Models;

namespace OmanChartsWeb.Controllers
{
    public class UserLoginController : Controller
    {
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Role") == null)
            {
                var routeValue = new RouteValueDictionary(new { action = "Index", controller = "Home" });
                return RedirectToRoute(routeValue);
            }
            List<UserLogin> lists = new List<UserLogin>();
            using (var httpClient = new HttpClient())
            {
                using (var apiresponse = await httpClient.GetAsync("https://localhost:7089/api/User/GetLogs"))
                {
                    var apiData = await apiresponse.Content.ReadAsStringAsync();
                    var jObject = JObject.Parse(apiData);
                    var bids = JArray.Parse(jObject["data"].ToString());
                    lists = bids.ToObject<List<UserLogin>>();
                }
            }
            return View(lists);
        }
    }
}

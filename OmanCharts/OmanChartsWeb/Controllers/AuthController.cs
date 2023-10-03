using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using OmanChartsWeb.Models;

namespace OmanChartsWeb.Controllers
{    
    public class AuthController : Controller
    {
        public AuthController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Role");
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("ZoneId");
            return View("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:7089/api/User/login", content))
                {
                    var apiData = await response.Content.ReadAsStringAsync();
                    var jObject = JsonConvert.DeserializeObject<Response>(apiData);
                    var jObject1 = JObject.Parse(apiData);
                    var bids = jObject1["data"].ToString();
                    var data = JsonConvert.DeserializeObject<LoginResponse>(bids);
                    if (data != null)
                    {
                        if (data.Status)
                        {
                            HttpContext.Session.SetString("Role", data.Role);
                            HttpContext.Session.SetString("UserId", data.UserId);
                            HttpContext.Session.SetString("ZoneId", data.ZoneId.ToString());
                            HttpContext.Session.SetString("Name", data.Name);
                            HttpContext.Session.SetString("ZoneName", data.ZoneName);
                            ViewBag.Name = data.Name;
                            var routeValue = new RouteValueDictionary(new { action = "Index", controller = "Dashboard" });
                            return RedirectToRoute(routeValue);
                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
        }
    }
}

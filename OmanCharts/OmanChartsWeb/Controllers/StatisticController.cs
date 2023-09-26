using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using OmanChartsWeb.Models;

namespace OmanChartsWeb.Controllers
{
    public class StatisticController : Controller
    {
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Role") == null)
            {
                var routeValue = new RouteValueDictionary(new { action = "Index", controller = "Home" });
                return RedirectToRoute(routeValue);
            }
            List<Statistic> lists = new List<Statistic>();
            using (var httpClient = new HttpClient())
            {
                if (HttpContext.Session.GetString("Role") == "Customer")
                {
                    using (var apiresponse = await httpClient.GetAsync("https://localhost:7089/api/Statistic/GetByZone?ZoneId=" + HttpContext.Session.GetString("ZoneId") + "&UserId=" + HttpContext.Session.GetString("UserId") + ""))
                    {
                        var apiData = await apiresponse.Content.ReadAsStringAsync();
                        var jObject = JObject.Parse(apiData);
                        var bids = JArray.Parse(jObject["data"].ToString());
                        lists = bids.ToObject<List<Statistic>>();
                    }
                }
                else if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    using (var apiresponse = await httpClient.GetAsync("https://localhost:7089/api/Statistic/Get"))
                    {
                        var apiData = await apiresponse.Content.ReadAsStringAsync();
                        var jObject = JObject.Parse(apiData);
                        var bids = JArray.Parse(jObject["data"].ToString());
                        lists = bids.ToObject<List<Statistic>>();
                    }
                }

            }
            return View(lists);
        }
        private async Task<List<SelectListItem>> GetZones()
        {
            List<Zone> ZonesList = new List<Zone>();
            using (var httpClient = new HttpClient())
            {
                using (var apiresponse = await httpClient.GetAsync("https://localhost:7089/api/Zone/Get"))
                {
                    var apiData = await apiresponse.Content.ReadAsStringAsync();
                    var jObject = JObject.Parse(apiData);
                    var bids = JArray.Parse(jObject["data"].ToString());
                    ZonesList = bids.ToObject<List<Zone>>();
                }
            }
            List<SelectListItem> ListofZones = new List<SelectListItem>();
            for (int i = 0; i < ZonesList.Count; i++)
            {
                SelectListItem selectListItem = new SelectListItem();
                selectListItem.Text = ZonesList[i].ZoneName;
                selectListItem.Value = ZonesList[i].ZoneId.ToString();
                ListofZones.Add(selectListItem);
            }
            return ListofZones;
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            Statistic statistic = new Statistic();
            statistic.ListofZones = await GetZones();
            return View(statistic);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Statistic statistic)
        {
            if (HttpContext.Session.GetString("ZoneId") != null)
            {
                statistic.ZoneId = Guid.Parse(HttpContext.Session.GetString("ZoneId"));
            }
            if (HttpContext.Session.GetString("UserId") != null)
            {
                statistic.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            }
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(statistic), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:7089/api/Statistic/Insert", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Response>(apiResponse);
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(Statistic statistic)
        {
            if (HttpContext.Session.GetString("ZoneId") != null)
            {
                statistic.ZoneId = Guid.Parse(HttpContext.Session.GetString("ZoneId"));
            }
            if (HttpContext.Session.GetString("UserId") != null)
            {
                statistic.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            }
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(statistic), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync("https://localhost:7089/api/Statistic/Update", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Response>(apiResponse);
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(Guid? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            using (var httpClient = new HttpClient())
            {
                using (var apiresponse = await httpClient.GetAsync("https://localhost:7089/api/Statistic/GetById/" + Id))
                {
                    var apiData = await apiresponse.Content.ReadAsStringAsync();
                    var jObject = JsonConvert.DeserializeObject<Response>(apiData);
                    var jObject1 = JObject.Parse(apiData);
                    var bids = jObject1["data"].ToString();
                    var data = JsonConvert.DeserializeObject<Statistic>(bids);
                    return View(data);
                }
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            using (var httpClient = new HttpClient())
            {
                using (var apiresponse = await httpClient.GetAsync("https://localhost:7089/api/Statistic/GetById/" + Id))
                {
                    var apiData = await apiresponse.Content.ReadAsStringAsync();
                    var jObject = JsonConvert.DeserializeObject<Response>(apiData);
                    var jObject1 = JObject.Parse(apiData);
                    var bids = jObject1["data"].ToString();
                    var data = JsonConvert.DeserializeObject<Statistic>(bids);
                    data.ListofZones = await GetZones();
                    return View(data);
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:7089/api/Statistic/Delete?Id=" + Id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Response>(apiResponse);
                }
            }
            return RedirectToAction("Index");
        }
    }
}

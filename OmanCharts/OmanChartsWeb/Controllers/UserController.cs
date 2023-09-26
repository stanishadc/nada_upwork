using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OmanChartsWeb.Models;
using System.Text;

namespace OmanChartsWeb.Controllers
{
    public class UserController : Controller
    {
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Role") == null)
            {
                var routeValue = new RouteValueDictionary(new { action = "Index", controller = "Home" });
                return RedirectToRoute(routeValue);
            }
            List<User> lists = new List<User>();
            using (var httpClient = new HttpClient())
            {
                using (var apiresponse = await httpClient.GetAsync("https://localhost:7089/api/User/GetCustomers"))
                {
                    var apiData = await apiresponse.Content.ReadAsStringAsync();
                    var jObject = JObject.Parse(apiData);
                    var bids = JArray.Parse(jObject["data"].ToString());
                    lists = bids.ToObject<List<User>>();
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
            Register register = new Register();
            register.ListofZones = await GetZones();
            return View(register);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Register register)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(register), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:7089/api/User/create-user", content))
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
                using (var apiresponse = await httpClient.GetAsync("https://localhost:7089/api/User/GetById/"+Id))
                {
                    var apiData = await apiresponse.Content.ReadAsStringAsync();
                    var jObject = JsonConvert.DeserializeObject<Response>(apiData);
                    var jObject1 = JObject.Parse(apiData);
                    var bids = jObject1["data"].ToString();
                    var data = JsonConvert.DeserializeObject<User>(bids);
                    return View(data);
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Guid Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            using (var httpClient = new HttpClient())
            {
                using (var apiresponse = await httpClient.GetAsync("https://localhost:7089/api/User/GetById/" + Id))
                {
                    var apiData = await apiresponse.Content.ReadAsStringAsync();
                    var jObject = JsonConvert.DeserializeObject<Response>(apiData);
                    var jObject1 = JObject.Parse(apiData);
                    var bids = jObject1["data"].ToString();
                    var data = JsonConvert.DeserializeObject<User>(bids);
                    data.ListofZones = await GetZones();
                    return View(data);
                }
            }
        }
        public IActionResult Edit()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:7089/api/User?Id=" + Id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Response>(apiResponse);
                }
            }
            return RedirectToAction("Index");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using OmanChartsWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OmanChartsWeb.Controllers
{
    public class ProjectController : Controller
    {
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Role") == null)
            {
                var routeValue = new RouteValueDictionary(new { action = "Index", controller = "Home" });
                return RedirectToRoute(routeValue);
            }
            List<Project> lists = new List<Project>();
            using (var httpClient = new HttpClient())
            {
                using (var apiresponse = await httpClient.GetAsync("https://localhost:7089/api/Project/Get"))
                {
                    var apiData = await apiresponse.Content.ReadAsStringAsync();
                    var jObject = JObject.Parse(apiData);
                    var bids = JArray.Parse(jObject["data"].ToString());
                    lists = bids.ToObject<List<Project>>();
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
            Project project = new Project();
            project.ListofZones = await GetZones();
            return View(project);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Project project)
        {
            if (HttpContext.Session.GetString("ZoneId") != null)
            {
                project.ZoneId = Guid.Parse(HttpContext.Session.GetString("ZoneId"));
            }
            if (HttpContext.Session.GetString("UserId") != null)
            {
                project.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            }

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(project), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:7089/api/Project/Insert", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Response>(apiResponse);
                }
            }
            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> Update(Project project)
        {
            if (HttpContext.Session.GetString("ZoneId") != null)
            {
                project.ZoneId = Guid.Parse(HttpContext.Session.GetString("ZoneId"));
            }
            if (HttpContext.Session.GetString("UserId") != null)
            {
                project.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            }
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(project), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync("https://localhost:7089/api/Project/Update", content))
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
                using (var apiresponse = await httpClient.GetAsync("https://localhost:7089/api/Project/GetById/" + Id))
                {
                    var apiData = await apiresponse.Content.ReadAsStringAsync();
                    var jObject = JsonConvert.DeserializeObject<Response>(apiData);
                    var jObject1 = JObject.Parse(apiData);
                    var bids = jObject1["data"].ToString();
                    var data = JsonConvert.DeserializeObject<Project>(bids);                    
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
                using (var apiresponse = await httpClient.GetAsync("https://localhost:7089/api/Project/GetById/" + Id))
                {
                    var apiData = await apiresponse.Content.ReadAsStringAsync();
                    var jObject = JsonConvert.DeserializeObject<Response>(apiData);
                    var jObject1 = JObject.Parse(apiData);
                    var bids = jObject1["data"].ToString();
                    var data = JsonConvert.DeserializeObject<Project>(bids);
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
                using (var response = await httpClient.DeleteAsync("https://localhost:7089/api/Project/Delete?Id=" + Id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Response>(apiResponse);
                }
            }
            return RedirectToAction("Index");
        }
    }
}

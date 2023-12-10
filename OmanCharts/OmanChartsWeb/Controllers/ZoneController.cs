using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OmanChartsWeb.Models;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OmanChartsWeb.Controllers
{
    public class ZoneController : Controller
    {
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Role") == null)
            {
                var routeValue = new RouteValueDictionary(new { action = "Index", controller = "Home" });
                return RedirectToRoute(routeValue);
            }
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
            return View(ZonesList);
        }
        public async Task<IActionResult> Update(Zone zone)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(zone), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync("https://localhost:7089/api/Zone/Update", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Response>(apiResponse);
                    if (data != null)
                    {
                        if (data.Status == "Success")
                        {
                            TempData["successMessage"] = "Zone updated successfully";
                        }
                        else
                        {
                            TempData["errorMessage"] = "Error in updating record";
                            return View();
                        }
                    }
                }
            }
            return RedirectToAction("Index");
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
                using (var apiresponse = await httpClient.GetAsync("https://localhost:7089/api/Zone/GetById/" + Id))
                {
                    var apiData = await apiresponse.Content.ReadAsStringAsync();
                    var jObject = JsonConvert.DeserializeObject<Response>(apiData);
                    var jObject1 = JObject.Parse(apiData);
                    var bids = jObject1["data"].ToString();
                    var data = JsonConvert.DeserializeObject<Zone>(bids);
                    return View(data);
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddZone(Zone zone)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(zone), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:7089/api/Zone/Insert", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Response>(apiResponse);
                    if(data != null)
                    {
                        if (data.Status == "Success")
                        {
                            TempData["successMessage"] = "Zone created successfully";
                        }
                        else
                        {
                            TempData["errorMessage"] = "Error in inserting record";
                            return View("Index");
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteZone(Guid ZoneId)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:7089/api/Zone/Delete?id=" + ZoneId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Response>(apiResponse);
                    if (data != null)
                    {
                        if (data.Status == "Success")
                        {
                            TempData["successMessage"] = "Zone deleted successfully";
                        }
                        else
                        {
                            TempData["errorMessage"] = "Error in deleting record";
                            return View("Index");
                        }
                    }
                }
            }

            return RedirectToAction("Index");
        }
    }
}

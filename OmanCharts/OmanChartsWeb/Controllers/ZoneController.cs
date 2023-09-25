using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OmanChartsWeb.Models;
using System.Text;

namespace OmanChartsWeb.Controllers
{
    public class ZoneController : Controller
    {
        public async Task<IActionResult> Index()
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
            return View(ZonesList);
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
                }
            }

            return RedirectToAction("Index");
        }
    }
}

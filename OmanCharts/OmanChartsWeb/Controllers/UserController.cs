using Microsoft.AspNetCore.Mvc;
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
        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:44324/api/User/create-user", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Response>(apiResponse);
                }
            }
            return RedirectToAction("Index");
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
                using (var response = await httpClient.DeleteAsync("https://localhost:7089/api/User/Delete?Id=" + Id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Response>(apiResponse);
                }
            }

            return RedirectToAction("Index");
        }
    }
}

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
        [HttpGet]
        public IActionResult Create()
        {
            return View();
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
        public async Task<IActionResult> Edit(User user)
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

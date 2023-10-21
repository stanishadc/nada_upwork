using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OmanChartsWeb.Models;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OmanChartsWeb.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public async Task<IActionResult> Index()
        {
            using (var httpClient = new HttpClient())
            {
                using (var apiresponse = await httpClient.GetAsync("https://localhost:7089/api/Statistic/GetDashboard"))
                {
                    var apiData = await apiresponse.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<DashBoardStatistic>(apiData);
                    ViewBag.Labels = JsonConvert.SerializeObject(data.Labels);
                    int[] aiIntArray = ConvertDoubleArrayToIntArray(data.LabourSeries);
                    int sum = aiIntArray.Sum();
                    int[] terms = new int[1];
                    terms[0]= sum;
                    ViewBag.LabourSeries = JsonConvert.SerializeObject(terms);
                    ViewBag.ProjectSeries = JsonConvert.SerializeObject(data.ProjectSeries);
                    ViewBag.ORateSeries = JsonConvert.SerializeObject(data.ORateSeries);
                    ViewBag.InvestorSeries = JsonConvert.SerializeObject(data.InvestorSeries);
                    data.StatisticsList = await GetStatisticsList();
                    return View(data);
                }
            }
        }

        private async Task<List<Statistic>> GetStatisticsList()
        {
            List<Statistic> lists = new List<Statistic>();
            using (var httpClient = new HttpClient())
            {
                using (var apiresponse = await httpClient.GetAsync("https://localhost:7089/api/Statistic/Get"))
                {
                    var apiData = await apiresponse.Content.ReadAsStringAsync();
                    var jObject = JObject.Parse(apiData);
                    var bids = JArray.Parse(jObject["data"].ToString());
                    lists = bids.ToObject<List<Statistic>>();
                }
            }
            return lists;
        }

        public static int[] ConvertDoubleArrayToIntArray(double?[] adDoubleArray)
        {
            return adDoubleArray.Select(d => (int)d).ToArray();
        }
        public async Task<IActionResult> Project()
        {
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
        public async Task<IActionResult> ProjectZone(Guid? Id)
        {
            List<Project> lists = new List<Project>();
            using (var httpClient = new HttpClient())
            {
                using (var apiresponse = await httpClient.GetAsync("https://localhost:7089/api/Project/GetByZone?ZoneId=" + Id))
                {
                    var apiData = await apiresponse.Content.ReadAsStringAsync();
                    var jObject = JObject.Parse(apiData);
                    var bids = JArray.Parse(jObject["data"].ToString());
                    lists = bids.ToObject<List<Project>>();
                }
            }
            return View(lists);
        }
        public async Task<IActionResult> ProjectView(Guid? Id)
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
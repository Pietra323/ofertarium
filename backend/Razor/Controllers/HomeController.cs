using System.Diagnostics;
using System.Text;
using backend.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Razor.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Razor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }
        public async Task<IActionResult> AddProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            try
            {
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var client = _clientFactory.CreateClient() ;
                var response = await client.PostAsync("http://localhost:5004/api/users", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("SuccessAction");
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
      


        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            try
            {
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var client = _clientFactory.CreateClient();
                var response = await client.PostAsync("http://localhost:5004/api/users/login", content);

                if (response.IsSuccessStatusCode)
                {
                    HttpContext.Session.SetString("IsLoggedIn", "true");
                    TempData["LoginMessage"] = "Pomyœlnie zalogowano!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7235/api/users/logout");

            if (response.IsSuccessStatusCode)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.Session.Clear(); 
                return RedirectToAction("Index", "Home");
            }
            else
            {

                return View("Error");
            }
        }
        [HttpGet]
        public async Task<List<CategoryViewModel>> GetCategories()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7235/api/category");
            var content = await response.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<List<CategoryViewModel>>(content);
            return categories;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
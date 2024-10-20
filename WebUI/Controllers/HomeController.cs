using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View("Privacy");
        }

        public IActionResult Admin_login()
        {
            return View("Admin_login");
        }

        public IActionResult Blog()
        {
            return View("Blog");
        }


        public IActionResult Checkout()
        {
            return View("Checkout");
        }

        public IActionResult Contact()
        {
            return View("Contact");
        }

        public IActionResult Shop_details()
        {
            return View("Shop_details");
        }

        public IActionResult Shop_grid()
        {
            return View("Shop_grid");
        }

        public IActionResult Shopping_cart()
        {
            return View("Shopping_cart");
        }

        public IActionResult User_login()
        {
            return View("User_login");          // User_login.cshtml sayfasýný döndürüyor
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

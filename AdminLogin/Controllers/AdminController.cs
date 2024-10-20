using Microsoft.AspNetCore.Mvc;

namespace AdminLogin.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult AdminPanel()
        {
            return View();
        }
    }
}

﻿using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}

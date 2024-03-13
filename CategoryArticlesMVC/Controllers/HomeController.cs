using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebFormApp.BLL.DTOs;
using System.Text.Json;

namespace CategoryArticlesMVC.Controllers
{
    public class HomeController : Controller
    {
        // Home/Index
        public IActionResult Index()
        {
            //cek kalau session ada
            if (HttpContext.Session.GetString("user") == null)
            {
                TempData["message"] = @"<div class='alert alert-danger'><strong>Error!</strong> You must login first !</div>";
                return RedirectToAction("Login", "Users");
            }
            else
            {
                var user = JsonSerializer.Deserialize<UserDTO>(HttpContext.Session.GetString("user"));
                ViewBag.message = $"Welcome {user.FirstName} {user.LastName}";
            }

            ViewData["Title"] = "Home Page";
            return View();
        }

        [Route("/Hello/ASP")]
        public IActionResult HelloASP()
        {
            return Content("Hello ASP.NET Core MVC!");
        }

        // Home/About
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return Content("This is the Contact action method...");
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using MyWebFormApp.BLL;
using MyWebFormApp.BLL.DTOs;
using MyWebFormApp.BLL.Interfaces;
using System.Text.Json;

namespace CategoryArticlesMVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserBLL userBLL;
        
        public UsersController()
        {
            userBLL = new UserBLL();
        }

        public IActionResult Login()
        {
			return View();
		}

        [HttpPost]
        public IActionResult Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                var userDTO = userBLL.LoginMVC(loginDTO);

                var useruserDtoSerialize = JsonSerializer.Serialize(userDTO);
                HttpContext.Session.SetString("user", useruserDtoSerialize);

                TempData["Message"] = "Welcome " + userDTO.Username;
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Message = @"<div class=""alert alert-danger"" role=""alert"">" + ex.Message + "</div>";
                return View();
            }

            
            
		}

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("user");
            return RedirectToAction("Login", "Users");
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserCreateDTO user)
        {
			if (ModelState.IsValid)
            {
                // Save to database
                return View();
			}
            try
            {
                userBLL.Insert(user);
                ViewBag.Message = @"<div class=""alert alert-success"" role=""alert"">User has been registered</div>";
            }
            catch(Exception ex)
            {
                ViewBag.Message = @"<div class=""alert alert-danger"" role=""alert"">" + ex.Message + "</div>";
            }

			return View();
		}
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyWebFormApp.BLL;
using MyWebFormApp.BLL.DTOs;
using MyWebFormApp.BLL.Interfaces;
using System.Text;
using System.Text.Json;

namespace CategoryArticlesMVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserBLL _userBLL;
        private readonly IRoleBLL _roleBLL;
        
        public UsersController(IUserBLL userBLL, IRoleBLL roleBLL)
        {
            _roleBLL = roleBLL;
            _userBLL = userBLL;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("user") == null)
            {
                TempData["message"] = @"<div class='alert alert-danger'><strong>Error!</strong> You must login first !</div>";
                return RedirectToAction("Login", "Users");
            }

            var users = _userBLL.GetAll();
            var listUsers = new SelectList(users, "Username", "Username");
            ViewBag.Users = listUsers;

            var roles = _roleBLL.GetAllRoles();
            var listRoles = new SelectList(roles, "RoleID", "RoleName");
            ViewBag.Roles = listRoles;

            var usersWithRoles = _userBLL.GetAllWithRoles();
            return View(usersWithRoles);
        }

        [HttpPost]
        public IActionResult Index(string Username, int RoleID)
        {
            try
            {
                _roleBLL.AddUserToRole(Username, RoleID);
                ViewBag.Message = @"<div class=""alert alert-success"" role=""alert"">User has been added to role</div>";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Message = @"<div class=""alert alert-danger"" role=""alert"">" + ex.Message + "</div>";
                return View();
            }
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
                
                var userDTO = _userBLL.LoginMVC(loginDTO);
                
                var roleString = new StringBuilder();
                foreach (var role in userDTO.Roles)
                {
                    //Build stringbuilder here
                    roleString.Append(role.RoleName + ",");
                }

                HttpContext.Session.SetString("roles", roleString.ToString());

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
                _userBLL.Insert(user);
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

using Microsoft.AspNetCore.Mvc;
using MyWebFormApp.BLL.Interfaces;
using CategoryArticlesMVC.Models;
using NuGet.Protocol.Plugins;
using System.Text.Json;
using CategoryArticlesMVC.Services;
using MyRESTServices.BLL.DTOs;

namespace CategoryArticlesMVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryBLL _categoryBLL;
        private readonly ICategoryServices _categoryServices;
        private const string BaseUrl = "http://localhost:5256/";

        public CategoriesController(ICategoryBLL categoryBLL, ICategoryServices categoryServices)
        {
            _categoryBLL = categoryBLL;
            _categoryServices = categoryServices;

        }


        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5, string name = "", string act = "")
        {
            //pengecekan session username
            if (HttpContext.Session.GetString("user") == null)
            {
                TempData["message"] = @"<div class='alert alert-danger'><strong>Error!</strong> You must login first !</div>";
                return RedirectToAction("Login", "Users");
            }

            if (TempData["message"] != null)
            {
                ViewData["message"] = TempData["message"];
            }

            ViewData["search"] = name;

            var maxsize = await _categoryServices.GetCountCategories(name);
            //return Content($"{pageNumber} - {pageSize} - {search} - {act}");

            if (act == "next")
            {
                if (pageNumber * pageSize < maxsize)
                {
                    pageNumber += 1;
                }
                ViewData["pageNumber"] = pageNumber;
            }
            else if (act == "prev")
            {
                if (pageNumber > 1)
                {
                    pageNumber -= 1;
                }
                ViewData["pageNumber"] = pageNumber;
            }

            if (act == "first")
            {
                pageNumber = 1;
                ViewData["pageNumber"] = pageNumber;
            }
            else if (act == "last")
            {
                if (maxsize == 0)
                {
                    pageNumber= 1;
                }
                else
                {
                    pageNumber = (int)Math.Ceiling((double)maxsize / pageSize);
                    ViewData["pageNumber"] = pageNumber;
                }
                
            }

            

            ViewData["pageNumber"] = pageNumber;
            ViewData["pageSize"] = pageSize;
            //ViewData["action"] = action;
            var models = await _categoryServices.GetWithPaging(pageNumber, pageSize, name);



            return View(models);
        }

        public async Task<IActionResult> GetFromServices()
        {
            var categories = await _categoryServices.GetAll();
            List<Category> categoriesList = new List<Category>();
            foreach (var category in categories)
            {
                categoriesList.Add(new Category
                {
                    CategoryID = category.CategoryID,
                    CategoryName = category.CategoryName
                });
            }
            return View(categoriesList);
        }





        public async Task<IActionResult> Detail(int id)
        {
            var model = await _categoryServices.GetById(id);
            return View(model);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDTO categoryCreate)
        {
            try
            {
                await _categoryServices.Insert(categoryCreate);
                //ViewData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Add Data Category Success !</div>";
                TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Add Data Category Success !</div>";
            }
            catch (Exception ex)
            {
                //ViewData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
                TempData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await _categoryServices.GetById(id);
            if (model == null)
            {
                TempData["message"] = @"<div class='alert alert-danger'><strong>Error!</strong>Category Not Found !</div>";
                return RedirectToAction("GetFromServices");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CategoryUpdateDTO categoryEdit)
        {
            try
            {
                await _categoryServices.Update(categoryEdit);
                TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Edit Data Category Success !</div>";
            }
            catch (Exception ex)
            {
                ViewData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
            }
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Delete(int id)
        {
            var model = await _categoryServices.GetById(id);
            if (model == null)
            {
                TempData["message"] = @"<div class='alert alert-danger'><strong>Error!</strong>Category Not Found !</div>";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, CategoryDTO category)
        {
            try
            {
                await _categoryServices.Delete(id);
                TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Delete Data Category Success !</div>";
            }
            catch (Exception ex)
            {
                TempData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
                return View(category);
            }
            return RedirectToAction("Index");
        }

        public IActionResult DisplayDropdownList()
        {
            var categories = _categoryBLL.GetAll();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        public IActionResult DisplayDropdownList(string CategoryID)
        {
            ViewBag.CategoryID = CategoryID;
            ViewBag.Message = $"You selected {CategoryID}";

            ViewBag.Categories = _categoryBLL.GetAll();

            return View();
        }

    }
}

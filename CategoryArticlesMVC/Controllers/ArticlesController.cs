using Microsoft.AspNetCore.Mvc;
using MyWebFormApp.BLL;
using MyWebFormApp.BLL.DTOs;
using MyWebFormApp.BLL.Interfaces;
using System.Reflection;

namespace CategoryArticlesMVC.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ICategoryBLL _categoryBLL;
        private readonly IArticleBLL _articleBLL;

        public ArticlesController(ICategoryBLL categoryBLL, IArticleBLL articleBLL)
        {
            _categoryBLL = categoryBLL;
            _articleBLL = articleBLL;
        }

        public IActionResult Index()
        {
            var categories = _categoryBLL.GetAll();
            var article = _articleBLL.GetArticleWithCategory();
            ViewBag.Categories = categories;

            return View(article);
        }

        [HttpPost]
        public IActionResult Index(int CategoryID)
        {
            ViewBag.CategoryID = CategoryID;
            ViewBag.Categories = _categoryBLL.GetAll();
            var articles = _articleBLL.GetArticleByCategory(CategoryID);

            return View(articles);
        }

        public IActionResult Create()
        {
            var categories = _categoryBLL.GetAll();
            ViewBag.Categories = categories;
            return View();
        }


        [HttpPost]
        public IActionResult Create(ArticleCreateDTO articleCreateDTO, IFormFile Pic)
        {
            var categories = _categoryBLL.GetAll();
            ViewBag.Categories = categories;
            try
            {
                if (Pic != null)
                {
                    if (Helper.IsImageFile(Pic.FileName))
                    {
                        //random file name based on GUID
                        var fileName = $"{Guid.NewGuid()}_{Pic.FileName}";
                        
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pics", fileName);
                        articleCreateDTO.Pic = fileName;
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            Pic.CopyTo(fileStream);
                        }
                        articleCreateDTO.Pic = fileName;
                        TempData["message2"] = @"<div class='alert alert-success'><strong>Success!&nbsp;</strong>File uploaded successfully !</div>";
                    }
                    else
                    {
                        TempData["message2"] = @"<div class='alert alert-danger'><strong>Error!&nbsp;</strong>File is not an image file !</div>";
                    }
                }

                _articleBLL.Insert(articleCreateDTO);
                TempData["message"] = $@"<div class='alert alert-success'><strong>Success!</strong>Article {articleCreateDTO.Title} has been created !</div>";
                return RedirectToAction("Index", "Articles", new { categoryId = articleCreateDTO.CategoryID });
            }
            catch (System.Exception ex)
            {
                TempData["message"] = $@"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
                return View(articleCreateDTO);
            }
        }

        public IActionResult Edit(int id)
        {
            var categories = _categoryBLL.GetAll();
            
            ViewBag.Categories = categories;
            var article = _articleBLL.GetArticleById(id);
            return View(article);
        }

        [HttpPost]
        public IActionResult Edit(ArticleUpdateDTO articleUpdateDTO, IFormFile Pic, string PicHidden, string CategoryID)
        {
            var categories = _categoryBLL.GetAll();
            ViewBag.Categories = categories;
            try
            {
                if (Pic != null)
                {
                    if (Helper.IsImageFile(Pic.FileName))
                    {
                        //random file name based on GUID
                        var fileName = $"{Guid.NewGuid()}_{Pic.FileName}";
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pics", fileName);
                        articleUpdateDTO.Pic = fileName;
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            Pic.CopyTo(fileStream);
                        }
                        articleUpdateDTO.Pic = fileName;
                        TempData["message2"] = @"<div class='alert alert-success'><strong>Success!&nbsp;</strong>File uploaded successfully !</div>";
                    }
                    else
                    {
                        TempData["message2"] = @"<div class='alert alert-danger'><strong>Error!&nbsp;</strong>File is not an image file !</div>";
                    }
                }
                else
                {
                    articleUpdateDTO.Pic = PicHidden;
                }

                _articleBLL.Update(articleUpdateDTO);
                TempData["message"] = $@"<div class='alert alert-success'><strong>Success!</strong>Article {articleUpdateDTO.Title} has been updated !</div>";
                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                TempData["message"] = $@"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
                return View(articleUpdateDTO);
            }
        }

        public IActionResult Delete(int id)
        {
            _articleBLL.Delete(id);
            return RedirectToAction("Index");
        }




    }
}

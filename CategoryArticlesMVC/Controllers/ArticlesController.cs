using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

		// GET: ArticlesController/Create
		public ActionResult Create()
		{
			ViewData["Title"] = "Create Article";
			ViewBag.Categories = _categoryBLL.GetAll();
			return View();
		}

		// POST: ArticlesController/Create
		[HttpPost]
		public ActionResult Create(ArticleCreateDTO article, IFormFile ImageArticle)
		{
			try
			{
				if (ImageArticle != null)
				{
					if (!Helper.IsImageFile(ImageArticle.FileName))
					{
						TempData["message"] = @"<div class='alert alert-danger'><strong>Error!</strong> File is not an image !</div>";
						return RedirectToAction("Index");
					}
					string fileName = $"{Guid.NewGuid()}_{ImageArticle.FileName}" + Path.GetExtension(ImageArticle.FileName);
					string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Pics", fileName);

					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						ImageArticle.CopyTo(fileStream);
					}

					article.Pic = fileName;


				}

				_articleBLL.Insert(article);

				TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong> Add Article Success !</div>";
			}
			catch (Exception ex)
			{
				TempData["message"] = @"<div class='alert alert-danger'><strong>Error!</strong> " + ex.Message + "</div>";
			}

			return RedirectToAction("Index");
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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebFormApp.BLL.Interfaces;

namespace MyRESTServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleBLL _articleBLL;
        public ArticlesController(IArticleBLL articleBLL)
        {
            _articleBLL = articleBLL;
        }

        

        [HttpGet]
        public IActionResult Get()
        {
            var result = _articleBLL.GetArticleWithCategory();
            if (result == null)
            {
                return NotFound("Article tidak ditemukan");
            }   
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _articleBLL.GetArticleById(id);
            if (result == null)
            {
                return NotFound("Article tidak ditemukan");
            }
            return Ok(result);
        }

        [HttpGet("GetByCategory")]
        public IActionResult GetByCategory(int id)
        {
            var result = _articleBLL.GetArticleByCategory(id);
            if (result == null)
            {
                return NotFound("Article tidak ditemukan");
            }
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody] MyWebFormApp.BLL.DTOs.ArticleCreateDTO articleCreateDTO)
        {
            if (articleCreateDTO == null)
            {
                return BadRequest("Article tidak boleh kosong");
            }
            _articleBLL.Insert(articleCreateDTO);
            return Ok("Article berhasil dibuat");
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] MyWebFormApp.BLL.DTOs.ArticleUpdateDTO articleUpdateDTO)
        {
            var existingArticle = _articleBLL.GetArticleById(id);
            if (existingArticle == null)
            {
                return NotFound("Article tidak ditemukan");
            }
            if (articleUpdateDTO == null)
            {
                return BadRequest("Article tidak boleh kosong");
            }
            articleUpdateDTO.ArticleID = id;
            _articleBLL.Update(articleUpdateDTO);
            return Ok("Article berhasil diupdate");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingArticle = _articleBLL.GetArticleById(id);
            if (existingArticle == null)
            {
                return NotFound("Article tidak ditemukan");
            }
            _articleBLL.Delete(id);
            return Ok("Article berhasil dihapus");
        }
       

    }
}

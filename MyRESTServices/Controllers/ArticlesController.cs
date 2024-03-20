using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyRESTServices.BLL.DTOs;
using MyRESTServices.BLL.Interfaces;

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
        public async Task<IActionResult> Get()
        {
            var result = await _articleBLL.GetArticleWithCategory();
            if (result == null)
            {
                return NotFound("Article tidak ditemukan");
            }   
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _articleBLL.GetArticleById(id);
            if (result == null)
            {
                return NotFound("Article tidak ditemukan");
            }
            return Ok(result);
        }

        [HttpGet("GetByCategory")]
        public async Task<IActionResult> GetByCategory(int id)
        {
            var result = await _articleBLL.GetArticleByCategory(id);
            if (result == null)
            {
                return NotFound("Article tidak ditemukan");
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ArticleCreateDTO articleCreateDTO)
        {
            if (articleCreateDTO == null)
            {
                return BadRequest("Article tidak boleh kosong");
            }
            await _articleBLL.Insert(articleCreateDTO);
            return Ok("Article berhasil dibuat");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ArticleUpdateDTO articleUpdateDTO)
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
            await _articleBLL.Update(articleUpdateDTO);
            return Ok("Article berhasil diupdate");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingArticle = await _articleBLL.GetArticleById(id);
            if (existingArticle == null)
            {
                return NotFound("Article tidak ditemukan");
            }
            await _articleBLL.Delete(id);
            return Ok("Article berhasil dihapus");
        }
       

    }
}

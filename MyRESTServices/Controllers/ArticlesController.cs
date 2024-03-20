using FluentValidation;
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
        private readonly IValidator<ArticleCreateDTO> _validatorArticleCreate;
        private readonly IValidator<ArticleUpdateDTO> _validatorArticleUpdate;

        public ArticlesController(IArticleBLL articleBLL, IValidator<ArticleCreateDTO> validatorArticleCreate, IValidator<ArticleUpdateDTO> validatorArticleUpdate)
        {
            _articleBLL = articleBLL;
            _validatorArticleCreate = validatorArticleCreate;
            _validatorArticleUpdate = validatorArticleUpdate;
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
            var validatorResult = _validatorArticleCreate.Validate(articleCreateDTO);
            if (!validatorResult.IsValid)
            {
                Helpers.Extensions.AddToModelState(validatorResult, ModelState);
                return BadRequest(validatorResult.Errors);
            }
            await _articleBLL.Insert(articleCreateDTO);
            return Ok("Article berhasil dibuat");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ArticleUpdateDTO articleUpdateDTO)
        {
            var existingArticle = await _articleBLL.GetArticleById(id);

            if (existingArticle == null)
            {
                return NotFound("Article tidak ditemukan");
            }
            var validatorResult = _validatorArticleUpdate.Validate(articleUpdateDTO);
            if (!validatorResult.IsValid)
            {
                Helpers.Extensions.AddToModelState(validatorResult, ModelState);
                return BadRequest(validatorResult.Errors);
            }
            articleUpdateDTO.ArticleID = id;
            var updated = await _articleBLL.Update(articleUpdateDTO);
            return Ok(updated);
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

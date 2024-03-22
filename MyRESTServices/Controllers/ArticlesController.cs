using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyRESTServices.BLL.DTOs;
using MyRESTServices.BLL.Interfaces;
using MyRESTServices.Models;

namespace MyRESTServices.Controllers
{
    [Authorize]
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
            try
            {
                var result = await _articleBLL.GetArticleWithCategory();
                if (result == null)
                {
                    return NotFound("Article tidak ditemukan");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await _articleBLL.GetArticleById(id);
                if (result == null)
                {
                    return NotFound("Article tidak ditemukan");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetByCategory")]
        public async Task<IActionResult> GetByCategory(int id)
        {
            try
            {
                var result = await _articleBLL.GetArticleByCategory(id);
                if (result == null)
                {
                    return NotFound("Article tidak ditemukan");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ArticleCreateDTO articleCreateDTO)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //post with upload picture file
        [HttpPost("FileUpload")]
        public async Task<IActionResult> Post([FromForm] ArticleWithFile articleWithFile)
        {
            if (articleWithFile.file == null || articleWithFile.file.Length == 0)
            {
                return BadRequest("File is required");
            }
            var newName = $"{Guid.NewGuid()}_{articleWithFile.file.FileName}";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", newName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await articleWithFile.file.CopyToAsync(stream);
            }

            var articleCreateDTO = new ArticleCreateDTO
            {
                CategoryID = articleWithFile.CategoryId,
                Title = articleWithFile.Title,
                Details = articleWithFile.Details,
                IsApproved = articleWithFile.IsApproved,
                Pic = newName
            };

            var article = await _articleBLL.Insert(articleCreateDTO);

            return CreatedAtAction(nameof(Get), new { id = article.ArticleID }, article);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ArticleUpdateDTO articleUpdateDTO)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existingArticle = await _articleBLL.GetArticleById(id);
                if (existingArticle == null)
                {
                    return NotFound("Article tidak ditemukan");
                }
                await _articleBLL.Delete(id);
                return Ok("Article berhasil dihapus");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

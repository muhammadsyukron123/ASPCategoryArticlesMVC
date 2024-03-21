
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MyRESTServices.BLL.DTOs;
using MyRESTServices.BLL.Interfaces;

namespace MyRESTServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryBLL _categoryBLL;
        private readonly IValidator<CategoryCreateDTO> _validatorCategoryCreate;
        private readonly IValidator<CategoryUpdateDTO> _validatorCategoryUpdate;
        public CategoriesController(ICategoryBLL categoryBLL, IValidator<CategoryCreateDTO> validatorCategoryCreate, IValidator<CategoryUpdateDTO> validatorCategoryUpdate)
        {
            _categoryBLL = categoryBLL;
            _validatorCategoryCreate = validatorCategoryCreate;
            _validatorCategoryUpdate = validatorCategoryUpdate;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _categoryBLL.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Ok(await _categoryBLL.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var result = await _categoryBLL.GetByName(name);
                if (result == null)
                {
                    return NotFound("Category tidak ditemukan");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("GetWithPaging")]
        public async Task<IActionResult> GetWithPaging(int pageNumber, int pageSize, string name = "")
        {
            try
            {
                var result = await _categoryBLL.GetWithPaging(pageNumber, pageSize, name);
                if (result == null)
                {
                    return NotFound("Category tidak ditemukan");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryCreateDTO categoryCreateDTO)
        {
            try
            {
                var validationResult = _validatorCategoryCreate.Validate(categoryCreateDTO);
                if (!validationResult.IsValid)
                {
                    Helpers.Extensions.AddToModelState(validationResult, ModelState);
                    return BadRequest(ModelState);
                }
                await _categoryBLL.Insert(categoryCreateDTO);

                return Ok(categoryCreateDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CategoryUpdateDTO categoryUpdateDTO)
        {
            try
            {
                var existingCategory = await _categoryBLL.GetById(id);
                if (existingCategory == null)
                {
                    return NotFound("Category tidak ditemukan");
                }
                var validationResult = _validatorCategoryUpdate.Validate(categoryUpdateDTO);
                if (!validationResult.IsValid)
                {
                    Helpers.Extensions.AddToModelState(validationResult, ModelState);
                    return BadRequest(ModelState);
                }
                categoryUpdateDTO.CategoryID = id;
                await _categoryBLL.Update(categoryUpdateDTO);
                var updatedCategory = await _categoryBLL.GetById(id);
                return Ok("Category berhasil diupdate");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryBLL.Delete(id);
                return Ok("Category berhasil dihapus");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("GetCategoryCount")]
        public async Task<IActionResult> GetCategoryCount(string name = "")
        {
            try
            {
                return Ok(await _categoryBLL.GetCountCategories(name));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}

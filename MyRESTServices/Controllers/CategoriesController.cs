
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
            return Ok(await _categoryBLL.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _categoryBLL.GetById(id));
        }

        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _categoryBLL.GetByName(name);
            if (result == null)
            {
                return NotFound("Category tidak ditemukan");
            }
            return Ok(result);
        }

        [HttpGet("GetWithPaging")]
        public async Task<IActionResult> GetWithPaging(int pageNumber, int pageSize, string name)
        {
            var result = await _categoryBLL.GetWithPaging(pageNumber, pageSize, name);
            if (result == null)
            {
                return NotFound("Category tidak ditemukan");
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryCreateDTO categoryCreateDTO)
        {
            var validationResult = _validatorCategoryCreate.Validate(categoryCreateDTO);
            if (!validationResult.IsValid)
            {
                Helpers.Extensions.AddToModelState(validationResult, ModelState);
                return BadRequest(ModelState);
            }
            await _categoryBLL.Insert(categoryCreateDTO);
            return Ok("Category berhasil dibuat");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CategoryUpdateDTO categoryUpdateDTO)
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryBLL.Delete(id);
            return Ok("Category berhasil dihapus");
        }
    }
}

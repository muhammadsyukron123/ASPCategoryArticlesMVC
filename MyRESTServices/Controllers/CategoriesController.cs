
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
        public CategoriesController(ICategoryBLL categoryBLL)
        {
            _categoryBLL = categoryBLL;
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
            if (categoryCreateDTO == null)
            {
                   return BadRequest("Category tidak boleh kosong");
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
            if (categoryUpdateDTO == null)
            {
                return BadRequest("Category tidak boleh kosong");
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

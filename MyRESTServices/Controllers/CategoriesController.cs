using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebFormApp.BLL.DTOs;
using MyWebFormApp.BLL.Interfaces;
using MyWebFormApp.BO;

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
        public IActionResult Get()
        {
            return Ok(_categoryBLL.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_categoryBLL.GetById(id));
        }

        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            var result = _categoryBLL.GetByName(name);
            if (result == null)
            {
                return NotFound("Category tidak ditemukan");
            }
            return Ok();
        }

        [HttpPost]
        public IActionResult Post([FromBody] CategoryCreateDTO categoryCreateDTO)
        {
            if (categoryCreateDTO == null)
            {
                   return BadRequest("Category tidak boleh kosong");
            }
            _categoryBLL.Insert(categoryCreateDTO);
            return Ok("Category berhasil dibuat");
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CategoryUpdateDTO categoryUpdateDTO)
        {
            var existingCategory = _categoryBLL.GetById(id);
            if (existingCategory == null)
            {
                return NotFound("Category tidak ditemukan");
            }
            if (categoryUpdateDTO == null)
            {
                return BadRequest("Category tidak boleh kosong");
            }
            categoryUpdateDTO.CategoryID = id;
            _categoryBLL.Update(categoryUpdateDTO);
            return Ok("Category berhasil diupdate");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingCategory = _categoryBLL.GetById(id);
            if (existingCategory == null)
            {
                return NotFound("Category tidak ditemukan");
            }
            _categoryBLL.Delete(id);
            return Ok("Category berhasil dihapus");
        }



    }
}

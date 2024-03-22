using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyRESTServices.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("File not selected");
                }

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok("File uploaded successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

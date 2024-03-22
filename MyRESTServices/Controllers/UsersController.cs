using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyRESTServices.BLL.DTOs;
using MyRESTServices.BLL.Interfaces;
using MyRESTServices.Helpers;
using MyRESTServices.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyRESTServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserBLL _userBLL;
        private IRoleBLL _roleBLL;
        private AppSettings _appSettings;

        public UsersController(IUserBLL userBLL, IRoleBLL roleBLL, IOptions<AppSettings> appSettings)
        {
            _userBLL = userBLL;
            _roleBLL = roleBLL;
            _appSettings = appSettings.Value;

        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _userBLL.GetAll();
                if (result == null)
                {
                    return NotFound("User not found");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{username}")]
        public async Task<IActionResult> Get(string username)
        {
            try
            {
                var result = await _userBLL.GetByUsername(username);
                if (result == null)
                {
                    return NotFound("User not found");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserCreateDTO user)
        {
            try
            {
                var result = await _userBLL.Insert(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole([FromBody] string Username, int RoleId)
        {
            try
            {
                var result = await _roleBLL.AddUserToRole(Username, RoleId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO user)
        {
            try
            {
                var result = await _userBLL.LoginMVC(user);
                if (result == null)
                {
                    return NotFound("User not found");
                }

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, result.Username));
                foreach (var role in result.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
                }
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var userWithToken = new UserWithToken
                {
                    Username = result.Username,
                    Address = result.Address,
                    Email = result.Email,
                    FirstName = result.FirstName,
                    LastName = result.LastName,
                    Roles = result.Roles,
                    Telp = result.Telp,
                    Token = tokenHandler.WriteToken(token)
                };
                return Ok(userWithToken);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(string username, [FromBody]string password)
        {
            try
            {
                var result = await _userBLL.ChangePassword(username, password);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Authorize(Roles="admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string username)
        {
            try
            {
                var result = await _userBLL.Delete(username);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

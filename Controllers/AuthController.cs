using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIAutores.DTO;
using WebAPIAutores.Services;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices authServices;

        public AuthController(IAuthServices authServices)
        {
            this.authServices = authServices;
        }

        [HttpPost("register", Name = "registrarUsuario")]
        public Task<ActionResult<AuthDto>> Register([FromBody] AuthRegisterDto authRegisterDto)
        {
            return authServices.Register(authRegisterDto);
        }

        [HttpPost("login", Name = "loginUsuario")]
        public Task<ActionResult<AuthDto>> Login([FromBody] AuthRegisterDto authRegisterDto)
        {
            return authServices.Login(authRegisterDto);
        }

        [HttpGet("renew", Name = "actualizarToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Task<ActionResult<AuthDto>> Renew()
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            return authServices.Renew(emailClaim);
        }

        [HttpPost("grant/admin", Name = "hacerAdmin")]
        public Task<ActionResult> GrantAdmin([FromBody] UpdateAuthDto updateAuthDto)
        {
            return authServices.GrantAdmin(updateAuthDto);
        }

        [HttpPost("remove/admin", Name = "removerAdmin")]
        public Task<ActionResult> RemoveAdmin([FromBody] UpdateAuthDto updateAuthDto)
        {
            return authServices.RemoveAdmin(updateAuthDto);
        }
    }
}
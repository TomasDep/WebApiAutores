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

        [HttpPost("register")]
        public Task<ActionResult<AuthDto>> Register([FromBody] AuthRegisterDto authRegisterDto)
        {
            return authServices.Register(authRegisterDto);
        }

        [HttpPost("login")]
        public Task<ActionResult<AuthDto>> Login([FromBody] AuthRegisterDto authRegisterDto)
        {
            return authServices.Login(authRegisterDto);
        }
    }
}
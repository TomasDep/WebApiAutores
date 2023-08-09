using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAPIAutores.DTO;

namespace WebAPIAutores.Services
{
    public class AuthServicesImpl : IAuthServices
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signingManager;

        public AuthServicesImpl(
            UserManager<IdentityUser> userManager,
            IConfiguration configuration,
            SignInManager<IdentityUser> signingManager
        )
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signingManager = signingManager;
        }

        public async Task<ActionResult<AuthDto>> Login([FromBody] AuthRegisterDto authRegisterDto)
        {
            var resultado = await signingManager.PasswordSignInAsync(authRegisterDto.Email, authRegisterDto.Password, isPersistent: false, lockoutOnFailure: false);
            if (resultado.Succeeded)
            {
                return JwtTokenBuilder(authRegisterDto);
            }
            else
            {
                return new BadRequestObjectResult("Login failed");
            }
        }

        public async Task<ActionResult<AuthDto>> Register([FromBody] AuthRegisterDto authRegisterDto)
        {
            var usuario = new IdentityUser { UserName = authRegisterDto.Email, Email = authRegisterDto.Email };
            var resultado = await userManager.CreateAsync(usuario, authRegisterDto.Password);
            if (resultado.Succeeded)
            {
                return JwtTokenBuilder(authRegisterDto);
            }
            else
            {
                return new BadRequestObjectResult(resultado.Errors);
            }
        }

        private AuthDto JwtTokenBuilder(AuthRegisterDto authRegisterDto)
        {
            var claims = new List<Claim>() {
                new Claim("email", authRegisterDto.Email),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["keyJwt"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddMinutes(60);
            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion, signingCredentials: creds);
            return new AuthDto() { Token = new JwtSecurityTokenHandler().WriteToken(securityToken), Expiracion = expiracion };
        }
    }
}
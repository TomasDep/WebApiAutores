using Microsoft.AspNetCore.Mvc;
using WebAPIAutores.DTO;

namespace WebAPIAutores.Services
{
    public interface IAuthServices
    {
        Task<ActionResult<AuthDto>> Register(AuthRegisterDto authRegisterDto);
        Task<ActionResult<AuthDto>> Login(AuthRegisterDto authRegisterDto);
    }
}
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WebAPIAutores.DTO;

namespace WebAPIAutores.Services
{
    public interface IAuthServices
    {
        Task<ActionResult<AuthDto>> Register(AuthRegisterDto authRegisterDto);
        Task<ActionResult<AuthDto>> Login(AuthRegisterDto authRegisterDto);
        Task<ActionResult<AuthDto>> Renew(Claim emailClaim);
        Task<ActionResult> GrantAdmin(UpdateAuthDto updateAuthDto);
        Task<ActionResult> RemoveAdmin(UpdateAuthDto updateAuthDto);
    }
}
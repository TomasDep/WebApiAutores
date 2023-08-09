using System.ComponentModel.DataAnnotations;

namespace WebAPIAutores.DTO
{
    public class AuthDto
    {
        public string Token { get; set; }
        public DateTime Expiracion { get; set; }
    }

    public class AuthRegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class UpdateAuthDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
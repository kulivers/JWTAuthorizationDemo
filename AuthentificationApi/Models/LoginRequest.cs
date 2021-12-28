using System.ComponentModel.DataAnnotations;

namespace AuthentificationApi.Models
{
    //6
    public class LoginRequest
    {
        [Required] public string Email { get; set; } 
        [Required]  public string Password { get; set; }
    }
}
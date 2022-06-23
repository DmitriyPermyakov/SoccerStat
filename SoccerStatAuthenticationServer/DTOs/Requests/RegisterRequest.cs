using System.ComponentModel.DataAnnotations;

namespace SoccerStatAuthenticationServer.DTOs.Requests
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Not specified Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Not specified Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password mismatch")]
        public string PasswordConfirm { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace NourhanRageb.G06.PL.ViewModels
{
    public class SignInViewModel
    {
        [Required(ErrorMessage = "Email Is Required !!")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password Is Required !!")]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "Password Min Length is 5")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

    }
}

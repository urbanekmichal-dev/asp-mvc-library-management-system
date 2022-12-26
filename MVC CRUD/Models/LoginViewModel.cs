using System.ComponentModel.DataAnnotations;

namespace MVC_CRUD.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required!")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required!")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}

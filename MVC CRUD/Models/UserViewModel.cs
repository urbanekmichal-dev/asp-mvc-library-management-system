

using System.ComponentModel.DataAnnotations;

namespace MVC_CRUD.Models
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Name is required!")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Last name is required!")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required!")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Birth date is required!")]
        [Display(Name = "Birth date")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Login is required!")]
        [Display(Name = "Login")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Password is required!")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}

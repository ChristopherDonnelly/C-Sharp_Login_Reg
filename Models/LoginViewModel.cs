using System.ComponentModel.DataAnnotations;
 
namespace Login_Register.Models
{
    public class LoginViewModel
    {
        
        [Display(Name = "Email: ")]
        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage="Please ensure that you have entered a valid email address!")]
        public string Email { get; set; }
 
        [Display(Name = "Password: ")]
        [Required(ErrorMessage = "Password is required!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
 
        [Range(0, 0, ErrorMessage = "Password does not match password on record!")]
        public int PasswordConfirmation { get; set; }

        [Range(0, 0, ErrorMessage = "Email does not exist! Enter correct Email or Register.")]
        public int Found { get; set; }
    }
}
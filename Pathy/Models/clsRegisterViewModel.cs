using System.ComponentModel.DataAnnotations;

namespace Pathy.Models
{
    public class clsRegisterViewModel
    {
        [Required(ErrorMessage = "First Name is required")]
        public string? firstName {  get; set; }

        [Required(ErrorMessage = "Second Name is required")]
        public string? secondName { get; set; }


        [Required(ErrorMessage = "Third Name is required")]
        public string? thirdName { get; set; }


        [Required(ErrorMessage = "Last Name is required")]
        public string? lastName { get; set; }


        [Required(ErrorMessage = "Email Name is required")]
        public string? email { get; set; }


        [Required(ErrorMessage = "Phone Number is required")]
        public string? phoneNumber { get; set; }


        [Required(ErrorMessage = "Gedner is required")]
        public short? gender { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string? username { get; set; }


        [Required(ErrorMessage = "Password is required")]
        public string? password { get; set; }

    }
}

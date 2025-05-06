using Pathy_BusinessAccessLayer;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Pathy.Models
{
    public class clsEditUserProfileInfoViewModel
    {

        [Required(ErrorMessage = "First Name is required")]
        public string? firstName { get; set; }

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
        public int? gender { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string? username { get; set; }


        [Required(ErrorMessage = "Password is required")]
        public string? password { get; set; }

        public void loadUserInfo()
        {
            this.username = clsGlobal.logedInUser.username; 
            this.firstName = clsGlobal.logedInUser.person.firstName;
            this.secondName = clsGlobal.logedInUser.person.secondName;
            this.thirdName = clsGlobal.logedInUser.person.thirdName;
            this.lastName = clsGlobal.logedInUser.person.lastName;
            this.phoneNumber = clsGlobal.logedInUser.person.phoneNumber;
            this.email = clsGlobal.logedInUser.person.email;
            this.gender = clsGlobal.logedInUser.person.gender;
        }
    }
}

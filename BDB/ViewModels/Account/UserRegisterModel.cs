using IdentityServer4.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BDB.ViewModels.Account
{
    public class UserRegisterModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required"), StringLength(200, ErrorMessage = "Email must be at most 200 characters"), EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }

       public  List<string> DealerIds { get; set; }
        public bool IsEnabled { get; set; }
    }
    public class RegisterModel
    {
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required"), StringLength(200, ErrorMessage = "Email must be at most 200 characters"), EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }
}

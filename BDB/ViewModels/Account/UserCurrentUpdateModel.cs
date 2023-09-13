using IdentityServer4.Models;
using System.ComponentModel.DataAnnotations;

namespace BDB.ViewModels.Account
{
    public class UserCurrentUpdateModel
    {
        public string FullName { get; set; }
        [Required(ErrorMessage = "Email is required"), StringLength(200, ErrorMessage = "Email must be at most 200 characters"), EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}

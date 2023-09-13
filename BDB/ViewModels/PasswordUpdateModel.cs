using IdentityServer4.Models;
using System.ComponentModel.DataAnnotations;

namespace BDB.ViewModels
{
    public class PasswordUpdateModel
    {
        public string CurrentPassword { get; set; }

        [MinLength(6, ErrorMessage = "New Password must be at least 6 characters")]
        public string NewPassword { get; set; }
    }
}

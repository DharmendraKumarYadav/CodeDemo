using BDB.Helpers;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace BDB.ViewModels.Account
{
    public class UserViewModel : UserBaseViewModel
    {
        public bool IsLockedOut { get; set; }

        public string Role { get; set; }

       
    }



    public class UserEditViewModel : UserBaseViewModel
    {
        public string CurrentPassword { get; set; }

        [MinLength(6, ErrorMessage = "New Password must be at least 6 characters")]
        public string NewPassword { get; set; }
        public string Role { get; set; }
    }



    public class UserPatchViewModel
    {
        public string FullName { get; set; }


        public string PhoneNumber { get; set; }

    }



    public abstract class UserBaseViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Username is required"), StringLength(200, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 200 characters")]
        public string UserName { get; set; }

        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required"), StringLength(200, ErrorMessage = "Email must be at most 200 characters"), EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public List<string> DealerIds { get; set; }

        public bool IsEnabled { get; set; }
    }



    public class MobileVerification
    {
        public string Code { get; set; }
        public string Phone { get; set; }
    }
    public class ResetPassword
    {
        public string Code { get; set; }
        public string Password { get; set; }
        public string PhoneOrEmail { get; set; }
    }
    public class ChangePassword
    {
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }

    }
    public class SetPassword
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }


    public class DealerViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public int LocalOrder { get; set; }
    }
}

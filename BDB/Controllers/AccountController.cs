using AutoMapper;
using BDB.Authorization;
using BDB.Helpers;
using BDB.Helpers.Common;
using BDB.Helpers.MessageService;
using BDB.ViewModels;
using BDB.ViewModels.Account;
using DAL;
using DAL.Core.Interfaces;
using DAL.Models.Entity;
using DAL.Models.Idenity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitechMedical.Helpers;

namespace BDB.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAccountManager _accountManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<AccountController> _logger;
        private const string GetUserByIdActionName = "GetUserById";
        private const string GetRoleByIdActionName = "GetRoleById";
        readonly ISmsService _smsService;
        readonly IEmailService _emailer;
        readonly IWhatsAppService _whatsAppService;
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly DataProtectorTokenProvider<User> _dataProtectorTokenProvider;

        public AccountController(IMapper mapper, DataProtectorTokenProvider<User> dataProtectorTokenProvider, IUnitOfWork unitOfWork, UserManager<User> userManager, IAccountManager accountManager, IAuthorizationService authorizationService, IEmailService emailer, ISmsService smsService, IWhatsAppService whatsAppService,
            ILogger<AccountController> logger)
        {
            _mapper = mapper;
            _accountManager = accountManager;
            _authorizationService = authorizationService;
            _dataProtectorTokenProvider = dataProtectorTokenProvider;
            _logger = logger;
            _smsService = smsService;
            _userManager = userManager;
            _emailer = emailer;
            _unitOfWork = unitOfWork;
            _whatsAppService = whatsAppService;
        }


        #region Register

        [HttpPost("register")]
        [ProducesResponseType(201, Type = typeof(UserViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Register([FromBody] UserRegisterModel user)
        {

            if (ModelState.IsValid)
            {
                if (user == null)
                    return BadRequest($"{nameof(user)} cannot be null");

                User appUser = _mapper.Map<User>(user);
                appUser.CreatedDate = DateTime.Now;
                appUser.CreatedBy = Utilities.GetUserId(this.User);
                appUser.PhoneNumberConfirmed = true;
                appUser.EmailConfirmed = true;

                var result = await _accountManager.CreateUserAsync(appUser, user.Role, user.Password);
                if (result.Succeeded)
                {
                    if (user.Role == Roles.Broker)
                    {
                        //Update Broker:
                        if (user.DealerIds.Count > 0)
                        {
                            List<DealerBroker> brokers = new List<DealerBroker>();
                            foreach (var item in user.DealerIds)
                            {
                                brokers.Add(new DealerBroker { DealerId = new Guid(item), BrokerId = appUser.Id });
                            }
                            _unitOfWork.DealerBrokerService.AddRange(brokers);
                            await _unitOfWork.SaveChangesAsync();
                        }

                    }





                    //Email:-
                    await _emailer.SendEmailAsync(user.FullName, user.Email, "Welcome to the Dekh Bike Dekh", AccountTemplate.SendSignupConfirmationEmailAsync(user.FullName, user.Email, user.Password));




                    return NoContent();
                }

                AddError(result.Errors);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("registeruser")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterModel user)
        {
            if (ModelState.IsValid)
            {
                if (user == null)
                    return BadRequest($"{nameof(user)} cannot be null");

                var entity = new User();
                entity.Email = user.Email;
                entity.FullName = user.Name;
                entity.UserName = user.Email;
                entity.PhoneNumber = user.PhoneNumber;
                entity.CreatedDate = DateTime.Now;
                entity.CreatedBy = null;
                entity.IsEnabled = true;
                var result = await _accountManager.CreateUserAsync(entity, "user", user.Password);
                if (result.Succeeded)
                {
                    //Email:-
                    string message = AccountTemplate.SendSignupConfirmationEmailAsync(user.Name, user.Email, user.Password);
                    await _emailer.SendEmailAsync(entity.FullName, user.Email, "Welcome to the Dekh Bike Dekh", message);

                    //Whatspp
                    var whatsappRequest = new WhatsappRequestModel();
                    whatsappRequest.TemplateName = "customer_welcome_message";
                    whatsappRequest.RecipientPhoneNumber = user.PhoneNumber;
                    await _whatsAppService.SendWhatsAppTemplateMessage(whatsappRequest);

                    //Phone:-
                    var appuser = await _userManager.FindByEmailAsync(user.Email);
                    if (appuser != null && entity.PhoneNumberConfirmed != true)
                    {
                        var code = await _userManager.GenerateChangePhoneNumberTokenAsync(appuser, appuser.PhoneNumber);
                        string phonemessage = AccountTemplate.SendOtpSmsAsync(code, appuser.FullName);
                        (bool success, string errorMsg) phoneResponse = await _smsService.SendAsync(appuser.PhoneNumber, phonemessage);
                    }
                    return NoContent();
                }

                AddError(result.Errors);
            }

            return BadRequest(ModelState);
        }



        #region User

        [HttpGet("users/me")]
        [ProducesResponseType(200, Type = typeof(UserViewModel))]
        public async Task<IActionResult> GetCurrentUser()
        {
            return await GetUserById(Utilities.GetUserId(this.User));
        }


        [HttpGet("users/{id}", Name = GetUserByIdActionName)]
        [ProducesResponseType(200, Type = typeof(UserViewModel))]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUserById(string id)
        {
            UserViewModel userVM = await GetUserViewModelHelper(id);

            if (userVM != null)
                return Ok(userVM);
            else
                return NotFound(id);
        }

        [HttpGet("users/username/{userName}")]
        [ProducesResponseType(200, Type = typeof(UserViewModel))]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUserByUserName(string userName)
        {
            User appUser = await _accountManager.GetUserByUserNameAsync(userName);

            if (appUser == null)
                return NotFound(userName);

            return await GetUserById(appUser.Id.ToString());
        }

        [HttpGet("users")]
        [ProducesResponseType(200, Type = typeof(List<UserViewModel>))]
        public async Task<IActionResult> GetUsers()
        {
            return await GetUsers(-1, -1);
        }

        [HttpGet("dealers")]
        [ProducesResponseType(200, Type = typeof(List<DealerViewModel>))]
        public async Task<IActionResult> GetDealers()
        {
            List<DealerViewModel> dealerViewModels = new List<DealerViewModel>();
            var dealers = await _accountManager.GetDealers();
            foreach (var item in dealers)
            {
                dealerViewModels.Add(new DealerViewModel { Id = item.Id.ToString(), Name = item.FullName, LocalOrder = item.LocalOrder });
            }
            return Ok(dealerViewModels);
        }
        [HttpGet("dealerorderupdate/{id}/{orderId}")]
        [ProducesResponseType(200, Type = typeof(List<DealerViewModel>))]
        public async Task<IActionResult> UpdateDealerOrder(string id, int orderId)
        {
            var dealerList = await _accountManager.GetDealers();
            var sameOrderExit = dealerList.Where(m => m.Id.ToString() != id && m.LocalOrder == orderId).FirstOrDefault();
            if (sameOrderExit != null)
            {
                return BadRequest("Same order exit with " + sameOrderExit.FullName);
            }
            else
            {
                var getDelaer = dealerList.Where(m => m.Id.ToString() == id).FirstOrDefault();
                getDelaer.LocalOrder = orderId;
                await _accountManager.UpdateDealerAsync(getDelaer);
            }
            return NoContent();
        }

        [HttpGet("users/{pageNumber:int}/{pageSize:int}")]
        [ProducesResponseType(200, Type = typeof(List<UserViewModel>))]
        public async Task<IActionResult> GetUsers(int pageNumber, int pageSize)
        {
            var usersAndRoles = await _accountManager.GetUsersAndRolesAsync(pageNumber, pageSize);

            List<UserViewModel> usersVM = new List<UserViewModel>();

            foreach (var item in usersAndRoles)
            {
                var userVM = _mapper.Map<UserViewModel>(item.User);
                userVM.Role = item.Role;

                usersVM.Add(userVM);
            }

            return Ok(usersVM);
        }

        [HttpPut("users/me")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] UserCurrentUpdateModel user)
        {
            User appUser = await _accountManager.GetUserByIdAsync(Utilities.GetUserId(this.User));

            if (ModelState.IsValid)
            {
                if (user == null)
                    return BadRequest($"{nameof(user)} cannot be null");

                if (ModelState.IsValid)
                {

                    _mapper.Map<UserCurrentUpdateModel, User>(user, appUser);
                    appUser.FullName = user.FullName;
                    var result = await _accountManager.UpdateUserAsync(appUser);
                    if (result.Succeeded)
                    {

                        return NoContent();
                    }

                    AddError(result.Errors);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpPut("users/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserUpdateModel user)
        {
            User appUser = await _accountManager.GetUserByIdAsync(id);

            if (ModelState.IsValid)
            {
                if (user == null)
                    return BadRequest($"{nameof(user)} cannot be null");

                if (appUser == null)
                    return NotFound(id);

                if (ModelState.IsValid)
                {
                    _mapper.Map<UserUpdateModel, User>(user, appUser);

                    var result = await _accountManager.UpdateUserAsync(appUser, user.Role);
                    if (result.Succeeded)
                    {


                        if (user.Role == "broker")
                        {
                            //remove
                            var dealerBroker = _unitOfWork.DealerBrokerService.Find(m => m.BrokerId == appUser.Id).ToList();
                            if (dealerBroker.Count > 0)
                            {
                                _unitOfWork.DealerBrokerService.RemoveRange(dealerBroker);
                                await _unitOfWork.SaveChangesAsync();
                            }
                            //Update Broker:
                            if (user.DealerIds.Count > 0)
                            {
                                List<DealerBroker> brokers = new List<DealerBroker>();
                                foreach (var item in user.DealerIds)
                                {
                                    brokers.Add(new DealerBroker { DealerId = new Guid(item), BrokerId = appUser.Id });
                                }
                                _unitOfWork.DealerBrokerService.AddRange(brokers);
                                await _unitOfWork.SaveChangesAsync();
                            }

                        }

                        return NoContent();
                    }

                    AddError(result.Errors);
                }
            }

            return BadRequest(ModelState);
        }


       
        [HttpPut("updateUser/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateResgisteUser(string id, [FromBody] UpdateRegisterUserModel user)
        {
            User appUser = await _accountManager.GetUserByIdAsync(id);

            if (ModelState.IsValid)
            {
                if (user == null)
                    return BadRequest($"{nameof(user)} cannot be null");

                if (appUser == null)
                    return NotFound(id);

                if (ModelState.IsValid)
                {
                    appUser.FullName = user.FullName; appUser.Email = user.Email;
                    appUser.PhoneNumber = user.PhoneNumber;



                    var result = await _accountManager.UpdateUserAsync(appUser);
                    if (result.Succeeded)
                    {

                        return NoContent();
                    }

                    AddError(result.Errors);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("users/{id}")]
        [ProducesResponseType(200, Type = typeof(UserViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(string id)
        {
            User appUser = await _accountManager.GetUserByIdAsync(id);

            if (appUser == null)
                return NotFound(id);


            if (appUser.IsSuperAdmin)
                return BadRequest("Admin User can not be deleted");

            UserViewModel userVM = await GetUserViewModelHelper(appUser.Id.ToString());

            var result = await _accountManager.DeleteUserAsync(appUser);
            if (!result.Succeeded)
                throw new Exception("The following errors occurred whilst deleting user: " + string.Join(", ", result.Errors));


            return Ok(userVM);
        }


        [HttpPut("users/unblock/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UnblockUser(string id)
        {
            User appUser = await _accountManager.GetUserByIdAsync(id);

            if (appUser == null)
                return NotFound(id);

            appUser.LockoutEnd = null;
            var result = await _accountManager.UpdateUserAsync(appUser);
            if (!result.Succeeded)
                throw new Exception("The following errors occurred whilst unblocking user: " + string.Join(", ", result.Errors));


            return NoContent();
        }

        #endregion

        #region Password

        [HttpPost("users/password/change")]
        [ProducesResponseType(201, Type = typeof(UserViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> PasswordChange([FromBody] PasswordUpdateModel user)
        {

            if (ModelState.IsValid)
            {

                if (user == null)
                    return BadRequest($"{nameof(user)} cannot be null");

                User appUser = await _accountManager.GetUserByIdAsync(Utilities.GetUserId(this.User));
                if (appUser == null)
                    return NotFound();

                if (!await _accountManager.CheckPasswordAsync(appUser, user.CurrentPassword))
                {
                    AddError("Current password is invalid.");
                }
                else
                {
                    var result = await _accountManager.UpdatePasswordAsync(appUser, user.CurrentPassword, user.NewPassword);
                    if (result.Succeeded)
                    {
                        return NoContent();
                    }
                    AddError(result.Errors);
                }
            }

            return BadRequest(ModelState);
        }


        [HttpPost("users/password/reset")]
        [ProducesResponseType(201, Type = typeof(UserViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> PasswordReset(string id, [FromBody] UserEditViewModel user)
        {


            if (ModelState.IsValid)
            {

                if (user == null)
                    return BadRequest($"{nameof(user)} cannot be null");

                User appUser = await _accountManager.GetUserByIdAsync(id);
                if (appUser == null)
                    return NotFound(id);



                var result = await _accountManager.ResetPasswordAsync(appUser, user.NewPassword);
                if (result.Succeeded)
                {
                    return NoContent();
                }

                AddError(result.Errors);
            }

            return BadRequest(ModelState);
        }


        #endregion

        #region Role 


        [HttpGet("roles/{id}", Name = GetRoleByIdActionName)]
        [ProducesResponseType(200, Type = typeof(RoleViewModel))]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var appRole = await _accountManager.GetRoleByIdAsync(id);


            if (appRole == null)
                return NotFound(id);

            return await GetRoleByName(appRole.Name);
        }


        [HttpGet("roles/name/{name}")]
        [ProducesResponseType(200, Type = typeof(RoleViewModel))]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetRoleByName(string name)
        {
            RoleViewModel roleVM = await GetRoleViewModelHelper(name);

            if (roleVM == null)
                return NotFound(name);

            return Ok(roleVM);
        }


        [HttpGet("roles")]
        [ProducesResponseType(200, Type = typeof(List<RoleViewModel>))]
        public async Task<IActionResult> GetRoles()
        {
            return await GetRoles(-1, -1);
        }


        [HttpGet("roles/{pageNumber:int}/{pageSize:int}")]
        [ProducesResponseType(200, Type = typeof(List<RoleViewModel>))]
        public async Task<IActionResult> GetRoles(int pageNumber, int pageSize)
        {
            var roles = await _accountManager.GetRolesLoadRelatedAsync(pageNumber, pageSize);
            return Ok(_mapper.Map<List<RoleViewModel>>(roles));
        }


        #endregion


        #region Mobile and Reset Password

        [AllowAnonymous]
        [HttpGet("sendotpreset/{mobile}")]
        public async Task<IActionResult> SendOtpResetCode(string mobile)
        {
            var user = await _accountManager.GetUserByPhoneOrEmailAsync(mobile);
            if (user != null)
            {
                var code = await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultPhoneProvider, "ResetPasswordPurpose");
                await _smsService.SendAsync(user.PhoneNumber, AccountTemplate.SendOtpSmsAsync(code, user.FullName));
                await _emailer.SendEmailAsync(user.FullName, user.Email, "Otp for Bike Dekh Bike account", AccountTemplate.SendOtpEmailAsync(code, user.FullName));
                return NoContent();
            }
            else
            {
                return NotFound("User is not found,Please register your self.");
            }

        }

        [AllowAnonymous]
        [HttpPost("verifyphone")]
        public async Task<IActionResult> VerifyPhone([FromBody] MobileVerification model)
        {
            var user = await _accountManager.GetUserByPhoneOrEmailAsync(model.Phone);
            if (user != null)
            {
                var result = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultPhoneProvider, "ResetPasswordPurpose", model.Code);
                if (result)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest("Invalid code.");
                }

            }
            else
            {
                return NotFound("User is not found with associated phone number.");
            }
        }



        [AllowAnonymous]
        [HttpGet("sendotp/{mobile}")]
        public async Task<IActionResult> SendOtpCode(string mobile)
        {
            var user = await _accountManager.GetUserByPhoneOrEmailAsync(mobile);
            if (user != null)
            {

                var code = await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultPhoneProvider, "ResetPasswordPurpose");

                string phonemessage = AccountTemplate.SendOtpSmsAsync(code, user.FullName);
                await _smsService.SendAsync(user.PhoneNumber, phonemessage);

                return NoContent();
            }
            else
            {
                return NotFound("User is not found,Please register your self.");
            }

        }
        [AllowAnonymous]


        [HttpPost("verify")]
        public async Task<IActionResult> SendOtpCode([FromBody] MobileVerification model)
        {
            var user = await _accountManager.GetUserByPhoneOrEmailAsync(model.Phone);
            if (user != null)
            {
                var result = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultPhoneProvider, "ResetPasswordPurpose", model.Code);
                if (result)
                {
                    user.PhoneNumberConfirmed = true;
                    var resultUpdate = await _accountManager.UpdateUserAsync(user);
                    if (resultUpdate.Succeeded)
                    {

                        return NoContent();
                    }
                    else {
                        return BadRequest(resultUpdate.Errors);
                    }

                   
                    
                }
                else
                {
                    return BadRequest("Invalid code.");
                }

            }
            else
            {
                return NotFound("User is not found with associated phone number.");
            }
        }







        [AllowAnonymous]
        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword model)
        {
            var user = await _accountManager.GetUserByPhoneOrEmailAsync(model.PhoneOrEmail);
            if (user != null)
            {

               
                var result = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultPhoneProvider, "ResetPasswordPurpose", model.Code);
                if (result)
                {
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
                    var res = await _userManager.UpdateAsync(user);
                    if (res.Succeeded)
                    {
                        return NoContent();
                    }
                    else
                    {
                        return BadRequest("Please try after some time.");
                    }
                }
                else
                {
                    return BadRequest("Invalid code.");
                }

            }
            else
            {
                return NotFound("User is not found with associated phone number.");
            }
        }

        [HttpPost("setpassword")]
        public async Task<IActionResult> SetPassword([FromBody] ResetPassword model)
        {
            var user = await _accountManager.GetUserByPhoneOrEmailAsync(model.PhoneOrEmail);
            if (user != null)
            {
                var newPassword = _userManager.PasswordHasher.HashPassword(user, model.Password);
                user.PasswordHash = newPassword;
                var res = await _userManager.UpdateAsync(user);
                if (res.Succeeded)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest("Please try after some time.");
                }
            }
            else
            {
                return NotFound("User is not found..");
            }
        }

        #endregion

        #region Helper Method


        private async Task<UserViewModel> GetUserViewModelHelper(string userId)
        {
            var userAndRoles = await _accountManager.GetUserAndRolesAsync(userId);
            if (userAndRoles == null)
                return null;

            var userVM = _mapper.Map<UserViewModel>(userAndRoles.Value.User);
            userVM.Role = userAndRoles.Value.Role;

            return userVM;
        }


        private async Task<RoleViewModel> GetRoleViewModelHelper(string roleName)
        {
            var role = await _accountManager.GetRoleLoadRelatedAsync(roleName);
            if (role != null)
                return _mapper.Map<RoleViewModel>(role);


            return null;
        }


        private void AddError(IEnumerable<string> errors, string key = "ServerValidation")
        {
            foreach (var error in errors)
            {
                AddError(error, key);
            }
        }

        private void AddError(string error, string key = "ServerValidation")
        {
            ModelState.AddModelError(key, error);
        }

        #endregion
    }
}

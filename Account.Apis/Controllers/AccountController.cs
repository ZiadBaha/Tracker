using Account.Apis.Errors;
using Account.Core.Dtos;
using Account.Core.Dtos.Account;
using Account.Core.Enums;
using Account.Core.Models;
using Account.Core.Models.Account;
using Account.Core.Models.Identity;
using Account.Core.Services;
using Account.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Account.Apis.Controllers
{
    public class AccountController : ApiBaseController
    {
        private readonly IAccountService _accountService; 
        private readonly IFileService _fileService;

        public AccountController(IAccountService accountService , IFileService fileService )
        {
            _accountService = accountService;
            _fileService = fileService;
        }

        //[HttpPost("Register")]
        //public async Task<IActionResult> Register(Register model  )
        //{


        //    // Handle the role assignment
        //    string roleName;
        //    switch (model.Role)
        //    {
        //        case UserRole.User:
        //            roleName = "User";
        //            break;
        //        case UserRole.Admin:
        //            roleName = "Admin";
        //            break;
        //        default:
        //            return BadRequest("Invalid role selected.");
        //    }

        //    // Pass the role name to the account service for registration
        //    var result = await _accountService.RegisterAsync(model, roleName, GenerateCallBackUrl);

        //    if (result.StatusCode == 200)
        //    {
        //        return Ok(result.Message);
        //    }
        //    else
        //    {
        //        return StatusCode(result.StatusCode, result.Message);
        //    }


        //}

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] Register model)
        {
            // Validate the model state
            if (!ModelState.IsValid)
            {
                return BadRequest(new ContentContainer<string>(null, "Please provide valid data"));
            }

            // Handle the role assignment
            string roleName;
            switch (model.Role)
            {
                case UserRole.User:
                    roleName = "User";
                    break;
                case UserRole.Admin:
                    roleName = "Admin";
                    break;
                default:
                    return BadRequest(new ContentContainer<string>(null, "Invalid role selected"));
            }

            // Handle image file upload
            if (model.ImageFile != null)
            {
                var fileResult = _fileService.SaveImage(model.ImageFile);
                if (fileResult.Item1 == 1)
                {
                    model.Image = fileResult.Item2; // getting name of image
                }
                else
                {
                    return BadRequest(new ContentContainer<string>(null, "Error uploading image"));
                }
            }

            try
            {
                // Pass the role name to the account service for registration
                var result = await _accountService.RegisterAsync(model, roleName, GenerateCallBackUrl);

                if (result.StatusCode == 200)
                {
                    return Ok(new ContentContainer<string>(result.Message, "Registration successful"));
                }
                else
                {
                    return StatusCode(result.StatusCode, new ContentContainer<string>(null, result.Message));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ContentContainer<string>(null, "An unexpected error occurred"));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login dto)
        {
           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _accountService.LoginAsync(dto);
            if (result.StatusCode == 400)
            {
                return BadRequest(result.Message); 
            }
            return Ok(result);
        }

        [HttpPost("forgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromHeader][EmailAddress] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email address is required.");
            }
            try
            {
                var result = await _accountService.ForgetPassword(email);

                if (result.StatusCode == 200)
                {
                    return Ok("Password reset email sent successfully.");
                }
                else
                {
                    return StatusCode(result.StatusCode, result.Message);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpPost("verfiyOtp")]
        public IActionResult VerfiyOtp(VerifyOtp dto)
        {
            var result = _accountService.VerfiyOtp(dto);

            if (result.StatusCode == 200)
            {
                return Ok(result.Message);
            }
            else
            {
                return BadRequest(result.Message); // Return the error message directly
            }
        }
        [HttpPut("resetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPassword dto)
        {
            var result = await _accountService.ResetPasswordAsync(dto);

            // Handle different response statuses
            switch (result.StatusCode)
            {
                case 200:
                    return Ok(result.Message);
                case 400:
                    return BadRequest(result.Message);
                case 500:
                    return StatusCode(500, result.Message);
                default:
                    return StatusCode(500, "An unexpected error occurred.");
            }
        }
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmUserEmail(string userId, string confirmationToken)
        {
            var result = await _accountService.ConfirmUserEmailAsync(userId!, confirmationToken!);

            if (result)
            {
                return RedirectPermanent(@"https://www.google.com/webhp?authuser=0");
            }
            else
            {
                return BadRequest("Failed to confirm user email.");
            }
        }

        private string GenerateCallBackUrl(string token, string userId)
        {
            var encodedToken = Uri.EscapeDataString(token);
            var encodedUserId = Uri.EscapeDataString(userId);
            var callBackUrl = $"{Request.Scheme}://{Request.Host}/api/Account/confirm-email?userId={encodedUserId}&confirmationToken={encodedToken}";
            return callBackUrl;
        }


        // Existing methods ...
        // this endpoint Get User Info By Id

        [HttpGet("getUserInfo/{userId}")]
        public async Task<IActionResult> GetUserInfoById(string userId)
        {
            var result = await _accountService.GetUserInfoByIdAsync(userId);
            if (result == null)
            {
                return NotFound(new ContentContainer<string>(null, "User not found."));
            }

            return Ok(new ContentContainer<UserDto>(result, "User information retrieved successfully."));
        }

        //[HttpPut("users/{userId}")]
        //public async Task<IActionResult> UpdateUser(string userId, [FromForm] UpdateUserDto updateUserDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(new ContentContainer<string>(null, "Invalid data provided."));
        //    }

        //    // Handle image file upload
        //    if (updateUserDto.ImageFile != null)
        //    {
        //        var fileResult = _fileService.SaveImage(updateUserDto.ImageFile);
        //        if (fileResult.Item1 == 1)
        //        {
        //            updateUserDto.Image = fileResult.Item2; // getting name of image
        //        }
        //        else
        //        {
        //            return BadRequest(new ContentContainer<string>(null, "Error uploading image"));
        //        }
        //    }

        //    updateUserDto.Id = userId;

        //    try
        //    {
        //        var result = await _accountService.UpdateUserInfoAsync(updateUserDto);
        //        if (result.StatusCode == 200)
        //        {
        //            return Ok(new ContentContainer<string>(result.Message, "User info updated successfully."));
        //        }
        //        else
        //        {
        //            return StatusCode(result.StatusCode, new ContentContainer<string>(null, result.Message));
        //        }
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return NotFound(new ContentContainer<string>(null, $"User not found: {ex.Message}"));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new ContentContainer<string>(null, $"Failed to update user: {ex.Message}"));
        //    }
        //}
    }
}

using Account.Apis.Errors;
using Account.Core.Models.Account;
using Account.Core.Models.Identity;
using Account.Core.Services;
using Account.Reposatory.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MailKit.Security;
using Account.Core.Dtos.Account;
using Account.Core.Errors;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace Account.Reposatory.Reposatories
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly MailSettings _mailSettings;
        private readonly ITokenService _TokenService;
        private readonly IOtpService _otpService;
        private readonly IMemoryCache _cache;

        public AccountService(UserManager<AppUser> userManager,
            IOptionsMonitor<MailSettings> options,
            ITokenService tokenService,
            IOtpService otpService,
            IMemoryCache cache
            )
        {
            _userManager = userManager;
            _mailSettings = options.CurrentValue;
            _TokenService = tokenService;
            _otpService = otpService;
            _cache = cache;
        }

        public async Task<ApiResponse> RegisterAsync(Register dto, string roleName, Func<string, string, string> generateCallBackUrl)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user != null)
            {
                return new ApiResponse(400, "User with this email already exists.");
            }

            user = new AppUser
            {
                UserRole = dto.Role,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Image = dto.Image,
                ImageFile = dto.ImageFile,
             

                //Id = user.Id,
                UserName = dto.Email.Split('@')[0],
                EmailConfirmed = false
            };


            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return new ApiResponse(400, "Something went wrong with the data you entered");
            }

            await _userManager.AddToRoleAsync(user, roleName);

            var emailConfirmation = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callBackUrl = generateCallBackUrl(emailConfirmation, user.Id);
            var emailBody = $"<h1>Dear {user.UserName}! Welcome To Tracker.</h1><p>Please <a href='{callBackUrl}'>Click Here</a> To Confirm Your Email.</p>";

            await SendEmailAsync(user.Email, "Email Confirmation", emailBody);

            return new ApiResponse(200, "Email verification has been sent to your email successfully. Please verify it!");
        }
        public async Task<ApiResponse> LoginAsync(Login dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return new ApiResponse(400, "User not found.");
            }

            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                return new ApiResponse(400, "Incorrect email or password.");
            }

            if (!user.EmailConfirmed)
            {
                return new ApiResponse(400, "Email not confirmed. Please check your email inbox to verify your email address.");
            }

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                //   = user.DisplayName,
                Role = user.UserRole,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Image = user.Image,
               // ImageFile = user.ImageFile,
             
                
                // add id in DTos 
                id = user.Id,
                
                Token = await _TokenService.CreateTokenAsync(user)
            };
        }
        public async Task<ApiResponse> ForgetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email address is required.", nameof(email));
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ApiResponse(400,"User not found.");
            }

            var otp = _otpService.GenerateOtp(email);

            try
            {
                await SendEmailAsync(email,
                    "Verification Code",
                    $"Dear {user.UserName},<br/> Use this code to reset your password: <strong>{otp}</strong>. Keep it safe and do not share it with anyone.");

                return new ApiResponse(200, "Password reset email sent successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500,"An unexpected error occurred while sending the password reset email. Please try again later.");
            }
        }
        public ApiResponse VerfiyOtp(VerifyOtp dto)
        {
            var isValidOtp = _otpService.IsValidOtp(dto.Email, dto.Otp);

            if (!isValidOtp)
            {
                return new ApiResponse(400, "Invalid OTP.");
            }
            return new ApiResponse(200, "Valid");
        }
        public async Task<ApiResponse> ResetPasswordAsync(ResetPassword dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                return new ApiResponse(400, "User not found.");
            }

            // Check if email is verified
            if (!_cache.TryGetValue(dto.Email, out bool validOtp) || !validOtp)
            {
                return new ApiResponse(400, "You have not verified your email addres(OTP).");
            }

            // Generate password reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Reset password
            var result = await _userManager.ResetPasswordAsync(user, token, dto.Password);

            // Check if password reset was successful
            if (result.Succeeded)
            {
                return new ApiResponse(200, "Password reset successfully.");
            }
            else
            {
                // Password reset failed
                return new ApiResponse(500, "Failed to reset password.");
        }
    }
        public async Task<bool> ConfirmUserEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            var confirmed = await _userManager.ConfirmEmailAsync(user, token);

            return confirmed.Succeeded;
        }
        public async Task SendEmailAsync(string To, string Subject, string Body, CancellationToken Cancellation = default)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_mailSettings.DisplayedName, _mailSettings.Email));
            message.To.Add(new MailboxAddress("", To));
            message.Subject = Subject;

            message.Body = new TextPart("html")
            {
                Text = Body
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync(_mailSettings.SmtpServer, _mailSettings.Port,
                    SecureSocketOptions.StartTls, Cancellation);
                await client.AuthenticateAsync(_mailSettings.Email, _mailSettings.Password, Cancellation);
                await client.SendAsync(message, Cancellation);
                await client.DisconnectAsync(true, Cancellation);
            }
        }

        // Existing methods ...

        public async Task<UserDto> GetUserInfoByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                 return null;
            }

            return new UserDto
            {
                
                id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Image = user.Image,
                Role = user.UserRole,
               // Token = await _TokenService.CreateTokenAsync(user)

            };
        }

        public async Task<ApiResponse> UpdateUserInfoAsync( UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.Id);
            if (user == null)
            {
                return new ApiResponse(404, "User not found.");
            }

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Image = dto.Image ?? user.Image;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new ApiResponse(400, "Failed to update user information.");
            }

            return new ApiResponse(200, "User information updated successfully.");
        }
    }
}

using Account.Apis.Errors;
using Account.Core.Dtos.Account;
using Account.Core.Models.Account;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Core.Services
{
    public interface IAccountService
    {
        Task<ApiResponse> RegisterAsync(Register dto, string roleName, Func<string, string, string> generateCallBackUrl);
        //Task<ApiResponse> RegisterAsync(Register user, Func<string, string, string> generateCallBackUrl);
        Task<ApiResponse> LoginAsync(Login dto);
        Task<ApiResponse> ForgetPassword(string email);
        ApiResponse VerfiyOtp(VerifyOtp dto);
        Task SendEmailAsync(string To, string Subject, string Body, CancellationToken Cancellation = default);
        Task<bool> ConfirmUserEmailAsync(string userId, string token);
        Task<ApiResponse> ResetPasswordAsync(ResetPassword dto);

        // add later 
        Task<UserDto> GetUserInfoByIdAsync(string userId);
        Task<ApiResponse> UpdateUserInfoAsync( UpdateUserDto dto);
    }
}

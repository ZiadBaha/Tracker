using Account.Core.Dtos;
using Account.Core.Dtos.Account;
using Account.Core.Models.Account;
using Account.Core.Models.Entites;
using Account.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Core.Services
{
    public interface IAdminRepository
    {
        int GetUserCount();
        int GetItemCount();
        int GetPersonCount();
        //void UpdateUser(string userId, UserDto updateUserDto);
        Task<bool> UpdateUser(string userId, UserDto updateUserDto);

        void DeleteUser(string userId);
        int GetComplaintCount();
        Task<IEnumerable<Complains>> SearchComplaintsByEmailAsync(string email);
        Task<IEnumerable<AppUser>> GetAllUsersAsync();
        Task<IEnumerable<AppUser>> SearchUsersByEmailAsync(string email);


    }
}

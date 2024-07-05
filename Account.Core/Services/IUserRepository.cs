using Account.Core.Dtos;
using Account.Core.Dtos.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Core.Services
{
    public interface IUserRepository
    {
        void UpdateUser(string userId, UserForUserDto updateUserDto);
        void DeleteUser(string userId);

    }
}

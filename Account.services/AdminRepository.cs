using Account.Core.Dtos;
using Account.Core.Dtos.Account;
using Account.Core.Models.Account;
using Account.Core.Models.Entites;
using Account.Core.Models.Identity;
using Account.Core.Services;
using Account.Reposatory.Data;
using Account.Reposatory.Data.Identity;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.services
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppIdentityDbContext _context;
        private readonly StoreContext _storeContext;
        private readonly IMapper _mapper;



        public AdminRepository(AppIdentityDbContext context , StoreContext storeContext , IMapper mapper)
        {
            _context = context;
            _storeContext = storeContext;
            _mapper = mapper;
        }

        public int GetUserCount()
        {
            return _context.Users.Count();
        }

        public int GetItemCount()
        {
            return _storeContext.items.Count();
        }

        public int GetPersonCount()
        {
            return _storeContext.persones.Count();
        }

        //public void UpdateUser(string userId, UserDto updateUserDto)
        //{
        //    var user = _context.Users.Find(userId);
        //    if (user != null)
        //    {
        //        user.FirstName = updateUserDto.FirstName;
        //        user.LastName = updateUserDto.LastName;
        //        user.Email = updateUserDto.Email;
        //        // Update other properties as needed
        //        _context.SaveChanges();
        //    }
        //}

        public async Task<bool> UpdateUser(string userId, UserDto updateUserDto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.FirstName = updateUserDto.FirstName;
                user.LastName = updateUserDto.LastName;
                user.Email = updateUserDto.Email;
                user.ImageFile = updateUserDto.ImageFile;
                user.Image = updateUserDto?.Image;
                user.UserRole = updateUserDto.Role;


                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public void DeleteUser(string userId)
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public int GetComplaintCount()
        {
            return _storeContext.complains.Count();
        }

        public async Task<IEnumerable<Complains>> SearchComplaintsByEmailAsync(string email)
        {
            return await _storeContext.complains
                .Where(c => c.Email.Contains(email))
                .ToListAsync();
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<IEnumerable<AppUser>> SearchUsersByEmailAsync(string email)
        {
            return await _context.Users
                .Where(u => u.Email.Contains(email))
                .ToListAsync();
        }

    }
}

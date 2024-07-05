using Account.Core.Dtos;
using Account.Core.Enums;
using Account.Core.Models.Entites;
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
    public class itemRepository : IitemRepository
    {
        private readonly StoreContext _storeContext;
        private readonly AppIdentityDbContext _identityContext;
        private readonly IMapper _mapper;

        public itemRepository(StoreContext storeContext, AppIdentityDbContext identityContext, IMapper mapper)
        {
            _storeContext = storeContext;
            _identityContext = identityContext;
            _mapper = mapper;
        }

        // Methods with userId
        public async Task<IEnumerable<ItemDto>> GetAllItems(string userId)
        {
            //var user = _identityContext.Users.Find(userId);
            
            var user = _identityContext.Users.Find(userId);
            if (user != null)
            {
                var items = await _storeContext.items./*Where(i => i.UserId == userId).*/ToListAsync();
                return _mapper.Map<IEnumerable<ItemDto>>(items); 
            }
            else
            {
                throw new ArgumentException("User not found");
            }
        }

        public async Task<ItemDto> GetItemById(string userId, int id)
        {
      
            var user = _identityContext.Users.Find(userId);
            if (user != null)
            {
                var item = await _storeContext.items.FirstOrDefaultAsync(i => i.Id == id /*&& i.UserId == userId*/);
                return _mapper.Map<ItemDto>(item);
            }
            else
            {
                throw new ArgumentException("User not found");
            }
        }

        public async Task<IEnumerable<ItemDto>> GetItemByStatus(string userId, ItemStatus itemStatus)
        {
            

            var user = _identityContext.Users.Find(userId);
            if (user != null)
            {
                var items = await _storeContext.items.Where(i => i.Status == itemStatus /*&& i.UserId == userId*/).ToListAsync();
                return _mapper.Map<IEnumerable<ItemDto>>(items);
            }
            else
            {
                throw new ArgumentException("User not found");
            }
        }

        public async Task<int> AddItem(string userId, ItemDto itemDto)
        {
            var user = _identityContext.Users.Find(userId);
            if (user != null)
            {
                var item = _mapper.Map<Item>(itemDto);
                await _storeContext.items.AddAsync(item);
                await _storeContext.SaveChangesAsync();
                return item.Id;
            }
            else
            {
                throw new ArgumentException("User not found");
            }
        }




        public async Task<bool> UpdateItem(string userId, int id, ItemDto itemDto)
        {
            

            var user = _identityContext.Users.Find(userId);
            if (user != null)
            {
                var item = await _storeContext.items.FirstOrDefaultAsync(i => i.Id == id /*&& i.UserId == userId*/);
                if (item == null)
                    return false;

                _mapper.Map(itemDto, item);
                await _storeContext.SaveChangesAsync();
                return true;
            }
            else
            {
                throw new ArgumentException("User not found");
            }
        }

        //public async Task<bool> DeleteItem(string userId, int id)
        //{


        //    var user = _identityContext.Users.Find(userId);
        //    if (user != null)
        //    {
        //        var item = await _storeContext.items.FirstOrDefaultAsync(i => i.Id == id /*&& i.UserId == userId*/);
        //        if (item == null)
        //            return false;

        //        _storeContext.items.Remove(item);
        //        await _storeContext.SaveChangesAsync();
        //        return true;
        //    }
        //    else
        //    {
        //        throw new ArgumentException("User not found");
        //    }
        //}


        public async Task<bool> DeleteItem(string userId, int id)
        {
            using (var transaction = await _storeContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var item = await _storeContext.items
                        .Include(i => i.Comments) // Include related comments
                        .FirstOrDefaultAsync(i => i.Id == id /*&& i.UserId == userId*/);

                    if (item == null)
                    {
                        return false;
                    }

                    // Delete related comments
                    _storeContext.comments.RemoveRange(item.Comments);

                    // Delete the item
                    _storeContext.items.Remove(item);

                    await _storeContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch (DbUpdateException)
                {
                    await transaction.RollbackAsync();
                    throw; // Re-throw the exception to be handled by the controller
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw; // Re-throw the exception to be handled by the controller
                }
            }
        }


        public async Task<IEnumerable<ItemDto>> Search(string userId, string name, string uniqNumber)
        {
            var user = _identityContext.Users.Find(userId);
            if (user != null)
            {
                var query = _storeContext.items.Where(i => i.UserId == userId).AsQueryable();

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(i => i.ItemName.Contains(name));
                }

                if (!string.IsNullOrEmpty(uniqNumber))
                {
                    query = query.Where(i => i.UniqNumber.Contains(uniqNumber));
                }

                var items = await query.ToListAsync();
                return _mapper.Map<IEnumerable<ItemDto>>(items);
            }
            else
            {
                throw new ArgumentException("User not found");
            }
        }

        // Methods without userId
        public async Task<IEnumerable<ItemDto>> GetAllItems()
        {
            var items = await _storeContext.items.ToListAsync();
            return _mapper.Map<IEnumerable<ItemDto>>(items);
        }

        public async Task<ItemDto> GetItemById(int id)
        {
            var item = await _storeContext.items.FindAsync(id);
            return _mapper.Map<ItemDto>(item);
        }

        public async Task<IEnumerable<ItemDto>> GetItemByStatus(ItemStatus itemStatus)
        {
            var items = await _storeContext.items.Where(i => i.Status == itemStatus).ToListAsync();
            return _mapper.Map<IEnumerable<ItemDto>>(items);
        }

        public async Task<IEnumerable<ItemDto>> Search(string name, string uniqNumber)
        {
            var query = _storeContext.items.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(i => i.ItemName.Contains(name));
            }

            if (!string.IsNullOrEmpty(uniqNumber))
            {
                query = query.Where(i => i.UniqNumber.Contains(uniqNumber));
            }

            var items = await query.ToListAsync();
            return _mapper.Map<IEnumerable<ItemDto>>(items);
        }
    }
}

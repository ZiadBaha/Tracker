using Account.Core.Dtos;
using Account.Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Core.Services
{
    public interface IitemRepository
    {
        Task<IEnumerable<ItemDto>> GetAllItems(string userId);
        Task<ItemDto> GetItemById(string userId, int id);
        Task<IEnumerable<ItemDto>> GetItemByStatus(string userId, ItemStatus itemStatus);
        Task<int> AddItem(string userId, ItemDto itemDto);
        Task<bool> UpdateItem(string userId, int id, ItemDto itemDto);
        Task<bool> DeleteItem(string userId, int id);
        Task<IEnumerable<ItemDto>> Search(string userId, string name, string uniqNumber);

        // Methods without userId
        Task<IEnumerable<ItemDto>> GetAllItems();
        Task<ItemDto> GetItemById(int id);
        Task<IEnumerable<ItemDto>> GetItemByStatus(ItemStatus itemStatus);
        Task<IEnumerable<ItemDto>> Search(string name, string uniqNumber);

    }
}




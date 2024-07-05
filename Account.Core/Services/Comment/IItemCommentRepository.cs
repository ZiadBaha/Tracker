using Account.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Core.Services.Comment
{
    public interface IItemCommentRepository
    {
        Task<int> AddComment(string userId, int itemId, commentItemDto commentDto);
        Task<bool> UpdateComment(string userId, int commentId, commentItemDto commentDto);
        Task<bool> DeleteComment(string userId, int commentId);
        Task<IEnumerable<commentItemDto>> GetUserCommentsInUserProfile(string userId);
        Task<IEnumerable<commentItemDto>> GetItemPostComments(int itemId);
        Task<commentItemDto> GetCommentById(int commentId);
     
        // Delete Item Post Comments 
        Task<bool> DeleteItemComments(int itemId);

    }
}

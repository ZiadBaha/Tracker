using Account.Core.Models.Entites;
using Account.Reposatory.Data.Identity;
using Account.Reposatory.Data;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Core.Services;
using Account.Core.Dtos;
using Microsoft.EntityFrameworkCore;
using Account.Core.Services.Comment;

namespace Account.services.Comments
{
    public class ItemCommentRepository : IItemCommentRepository
    {
        private readonly StoreContext _storeContext;
        private readonly AppIdentityDbContext _identityContext;
        private readonly IMapper _mapper;

        public ItemCommentRepository(StoreContext storeContext, AppIdentityDbContext identityContext, IMapper mapper)
        {
            _storeContext = storeContext;
            _identityContext = identityContext;
            _mapper = mapper;
        }

        public async Task<int> AddComment(string userId, int itemId, commentItemDto commentDto)
        {
            var user = _identityContext.Users.Find(userId);
            if (user != null)
            {
                var comment = _mapper.Map<Comment>(commentDto);
                comment.ItemId = itemId; // Assigning the itemId to the comment
                _storeContext.comments.Add(comment);
                await _storeContext.SaveChangesAsync();
                return comment.Id;
            }
            else
            {
                throw new ArgumentException("User not found");
            }
        }

        public async Task<bool> UpdateComment(string userId, int commentId, commentItemDto commentDto)
        {
            var user = _identityContext.Users.Find(userId);
            if (user != null)
            {
                var comment = await _storeContext.comments.FindAsync(commentId);
                if (comment == null)
                    return false;

                _mapper.Map(commentDto, comment);
                await _storeContext.SaveChangesAsync();
                return true;
            }
            else
            {
                throw new ArgumentException("User not found");
            }
        }

        public async Task<bool> DeleteComment(string userId, int commentId)
        {
            var user = _identityContext.Users.Find(userId);
            if (user != null)
            {
                var comment = await _storeContext.comments.FindAsync(commentId);
                if (comment == null)
                    return false;

                _storeContext.comments.Remove(comment);
                await _storeContext.SaveChangesAsync();
                return true;
            }
            else
            {
                throw new ArgumentException("User not found");
            }
        }

        public async Task<IEnumerable<commentItemDto>> GetUserCommentsInUserProfile(string userId)
        {
            ///var user = _identityContext.Users.Find(userId);
            ///if (user != null)
            ///{
            ///    var comments = await _storeContext.comments
            ///                                      .Where(c => c.UserId == userId)
            ///                                      .ToListAsync();
            ///    return _mapper.Map<IEnumerable<commentItemDto>>(comments);
            ///}
            ///else
            ///{
            ///    throw new ArgumentException("User not found");
            ///}

            var user = _identityContext.Users.Find(userId);
            if (user != null)
            {
                var comments = await _storeContext.comments.ToListAsync();
                return _mapper.Map<IEnumerable<commentItemDto>>(comments);
            }
            else
            {
                throw new ArgumentException("User not found");
            }
        }

        public async Task<IEnumerable<commentItemDto>> GetItemPostComments(int itemId)
        {
            var item = _storeContext.items.Find(itemId);
            if (item != null)
            {
                var comments = await _storeContext.comments
                                                  .Where(c => c.ItemId == itemId)
                                                  .ToListAsync();
                return _mapper.Map<IEnumerable<commentItemDto>>(comments);
            }
            else
            {
                throw new ArgumentException("Item not found");
            }
        }

        public async Task<commentItemDto> GetCommentById(int commentId)
        {
            var comment = await _storeContext.comments.FindAsync(commentId);
            return _mapper.Map<commentItemDto>(comment);
        }


        // Delete Item Post comments 
        public async Task<bool> DeleteItemComments(int itemId)
        {
            var comments = _storeContext.comments.Where(c => c.ItemId == itemId);
            if (!comments.Any())
            {
                return false;
            }

            _storeContext.comments.RemoveRange(comments);
            await _storeContext.SaveChangesAsync();
            return true;
        }
    }
}

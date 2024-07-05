using Account.Core.Models.Entites;
using Account.Reposatory.Data.Identity;
using Account.Reposatory.Data;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Core.Services.Comment;
using Account.Core.Dtos;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace Account.services.Comments
{
    public class PersonCommentRepository : IPersonCommentRepository
    {
        private readonly StoreContext _storeContext;
        private readonly AppIdentityDbContext _identityContext;
        private readonly IMapper _mapper;

        public PersonCommentRepository(StoreContext storeContext, AppIdentityDbContext identityContext, IMapper mapper)
        {
            _storeContext = storeContext;
            _identityContext = identityContext;
            _mapper = mapper;
        }

        public async Task<int> AddComment(string userId, int personId, commentpersonDto commentDto)
        {
            var user = _identityContext.Users.Find(userId);
            if (user != null)
            {
                var comment = _mapper.Map<Comment>(commentDto);
                comment.PersonId = personId; // Assigning the personId to the comment
                _storeContext.comments.Add(comment);
                await _storeContext.SaveChangesAsync();
                return comment.Id;
            }
            else
            {
                throw new ArgumentException("User not found");
            }
        }

        public async Task<bool> UpdateComment(string userId, int commentId, commentpersonDto commentDto)
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

        public async Task<IEnumerable<commentpersonDto>> GetUserCommentsInUserProfile(string userId)
        {
            var user = _identityContext.Users.Find(userId);
            if (user != null)
            {
                var comments = await _storeContext.comments.ToListAsync();
                return _mapper.Map<IEnumerable<commentpersonDto>>(comments);
            }
            else
            {
                throw new ArgumentException("User not found");
            }
        }

        public async Task<IEnumerable<commentpersonDto>> GetPersonPostComments(int personId)
        {

            var person = _storeContext.persones.Find(personId);
            if (person != null)
            {
                var comments = await _storeContext.comments
                                                  .Where(c => c.PersonId == personId)
                                                  .ToListAsync();
                return _mapper.Map<IEnumerable<commentpersonDto>>(comments);
            }
            else
            {
                throw new ArgumentException("post not found");
            }
        }

     

        public async Task<commentpersonDto> GetCommentById(int commentId)
        {
            var comment = await _storeContext.comments.FindAsync(commentId);
            return _mapper.Map<commentpersonDto>(comment);
        }


        // Delete person post comments 
        public async Task<bool> DeletePersonComments(int personId)
        {
            var comments = _storeContext.comments.Where(c => c.PersonId == personId);
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

using Account.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Core.Services.Comment
{
    public interface IPersonCommentRepository
    {
        Task<int> AddComment(string userId, int personId, commentpersonDto commentDto);
        Task<bool> UpdateComment(string userId, int commentId, commentpersonDto commentDto);
        Task<bool> DeleteComment(string userId, int commentId);
        Task<IEnumerable<commentpersonDto>> GetUserCommentsInUserProfile(string userId);
        Task<IEnumerable<commentpersonDto>> GetPersonPostComments(int personId);


        Task<commentpersonDto> GetCommentById(int commentId); // Add this method
       
        // Delete Person Post Comments 
        Task<bool> DeletePersonComments(int personId);


        // AddComment 
        // UpdateComment
        // DeleteComment
        // GitUserCommentInUserProfile
        // GitPersonePostComment


    }
}

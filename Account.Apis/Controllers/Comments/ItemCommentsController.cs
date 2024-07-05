using Account.Core.Dtos;
using Account.Core.Models;
using Account.Core.Services.Comment;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Account.Apis.Controllers.Comments
{
   
    public class ItemCommentsController : ApiBaseController
    {
        private readonly IItemCommentRepository _itemCommentRepository;
        private readonly IMapper _mapper;
        

        public ItemCommentsController(IItemCommentRepository itemCommentRepository, IMapper mapper)
        {
            _itemCommentRepository = itemCommentRepository;
            _mapper = mapper;
        }

        [HttpPost("{userId}/items/{itemId}/comments")]
        public async Task<IActionResult> AddComment(string userId, int itemId, [FromBody] commentItemDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ContentContainer<string>(null, "Please provide valid data"));
            }

            try
            {
                var commentId = await _itemCommentRepository.AddComment(userId, itemId, commentDto);
                if (commentId != 0)
                {
                    return CreatedAtAction(nameof(GetComment), new { userId, itemId, commentId }, new ContentContainer<int>(commentId, "Comment added successfully"));
                }
                else
                {
                    return BadRequest(new ContentContainer<string>(null, "Error adding comment"));
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ContentContainer<string>(null, ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ContentContainer<string>(null, "An unexpected error occurred"));
            }
        }

        [HttpPut("{userId}/items/{itemId}/comments/{commentId}")]
        public async Task<IActionResult> UpdateComment(string userId, int commentId, [FromBody] commentItemDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ContentContainer<string>(null, "Please provide valid data"));
            }

            try
            {
                var success = await _itemCommentRepository.UpdateComment(userId, commentId, commentDto);
                if (success)
                {
                    return Ok(new ContentContainer<string>(null, "Comment updated successfully"));
                }
                else
                {
                    return NotFound(new ContentContainer<string>(null, "Comment not found"));
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ContentContainer<string>(null, ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ContentContainer<string>(null, "An unexpected error occurred"));
            }
        }

        [HttpDelete("{userId}/items/{itemId}/comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(string userId, int commentId)
        {
            try
            {
                var success = await _itemCommentRepository.DeleteComment(userId, commentId);
                if (success)
                {
                    return Ok(new ContentContainer<string>(null, "Comment deleted successfully"));
                }
                else
                {
                    return NotFound(new ContentContainer<string>(null, "Comment not found"));
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ContentContainer<string>(null, ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ContentContainer<string>(null, "An unexpected error occurred"));
            }
        }

        [HttpGet("{userId}/items/comments")]
        public async Task<IActionResult> GetUserCommentsInUserProfile(string userId)
        {
            try
            {
                var comments = await _itemCommentRepository.GetUserCommentsInUserProfile(userId);
                return Ok(new ContentContainer<IEnumerable<commentItemDto>>(comments, "Comments retrieved successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ContentContainer<string>(null, ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ContentContainer<string>(null, "An unexpected error occurred"));
            }
        }

        [HttpGet("items/{itemId}/comments")]
        public async Task<IActionResult> GetItemPostComments(int itemId)
        {
            try
            {
                var comments = await _itemCommentRepository.GetItemPostComments(itemId);
                return Ok(new ContentContainer<IEnumerable<commentItemDto>>(comments, "Comments retrieved successfully"));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ContentContainer<string>(null, "An unexpected error occurred"));
            }
        }

        [HttpGet("{userId}/items/{itemId}/comments/{commentId}")]
        public async Task<IActionResult> GetComment(int commentId)
        {
            try
            {
                var comment = await _itemCommentRepository.GetCommentById(commentId);
                if (comment != null)
                {
                    return Ok(new ContentContainer<commentItemDto>(comment, "Comment retrieved successfully"));
                }
                else
                {
                    return NotFound(new ContentContainer<string>(null, "Comment not found"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ContentContainer<string>(null, ex.Message));
            }
        }
    }
}

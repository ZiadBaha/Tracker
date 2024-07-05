using Account.Core.Dtos;
using Account.Core.Models;
using Account.Core.Services.Comment;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Account.Apis.Controllers.Comments
{

    public class PersonCommentsController : ApiBaseController
    {
        private readonly IPersonCommentRepository _personCommentRepository;
        private readonly IMapper _mapper;

        public PersonCommentsController(IPersonCommentRepository personCommentRepository, IMapper mapper)
        {
            _personCommentRepository = personCommentRepository;
            _mapper = mapper;
        }

        [HttpPost("{userId}/persons/{personId}/comments")]
        public async Task<IActionResult> AddComment(string userId, int personId, [FromBody] commentpersonDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ContentContainer<string>(null, "Please provide valid data"));
            }

            try
            {
                var commentId = await _personCommentRepository.AddComment(userId, personId, commentDto);
                if (commentId != 0)
                {
                    return CreatedAtAction(nameof(GetComment), new { userId, personId, commentId }, new ContentContainer<int>(commentId, "Comment added successfully"));
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

        [HttpPut("{userId}/persons/{personId}/comments/{commentId}")]
        public async Task<IActionResult> UpdateComment(string userId,  int commentId, [FromBody] commentpersonDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ContentContainer<string>(null, "Please provide valid data"));
            }

            try
            {
                var success = await _personCommentRepository.UpdateComment(userId, commentId, commentDto);
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

        [HttpDelete("{userId}/persons/{personId}/comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(string userId, int commentId)
        {
            try
            {
                var success = await _personCommentRepository.DeleteComment(userId, commentId);
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

        [HttpGet("{userId}/persons/comments")]
        public async Task<IActionResult> GetUserCommentsInUserProfile(string userId)
        {
            try
            {
                var comments = await _personCommentRepository.GetUserCommentsInUserProfile(userId);
                return Ok(new ContentContainer<IEnumerable<commentpersonDto>>(comments, "Comments retrieved successfully"));
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

        [HttpGet("persons/{personId}/comments")]
        public async Task<IActionResult> GetPersonPostComments(int personId)
        {
            try
            {
                var comments = await _personCommentRepository.GetPersonPostComments(personId);
                return Ok(new ContentContainer<IEnumerable<commentpersonDto>>(comments, "Comments retrieved successfully"));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ContentContainer<string>(null, "An unexpected error occurred"));
            }
        }

        [HttpGet("{userId}/persons/{personId}/comments/{commentId}")]
        public async Task<IActionResult> GetComment(int commentId)
        {
            try
            {
                var comment = await _personCommentRepository.GetCommentById(commentId);
                if (comment != null)
                {
                    return Ok(new ContentContainer<commentpersonDto>(comment, "Comment retrieved successfully"));
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

using Account.Core.Dtos;
using Account.Core.Dtos.Account;
using Account.Core.Models;
using Account.Core.Models.Entites;
using Account.Core.Models.Identity;
using Account.Core.Services;
using Account.Core.Services.Comment;
using Account.services;
using Account.services.Comments;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Account.Apis.Controllers
{
    //[Authorize(Roles = "Admin")]
    //[Authorize]
    public class AdminController : ApiBaseController
    {
        private readonly IAdminRepository _repository;
        private readonly IMapper _mapper;
        private readonly IPersonCommentRepository  _personCommentRepository;
        private readonly IItemCommentRepository _itemCommentRepository;
        private readonly IFileService _fileService;

        public AdminController(IAdminRepository repository, IMapper mapper , 
            IPersonCommentRepository personCommentRepository, IItemCommentRepository itemCommentRepository 
            ,IFileService fileService)
        {
            _repository = repository;
            _mapper = mapper;
            _personCommentRepository = personCommentRepository;
            _itemCommentRepository = itemCommentRepository;
            _fileService = fileService;
        }

        [HttpGet("users/count")]
        public IActionResult GetUserCount()
        {
            try
            {
                int userCount = _repository.GetUserCount();
                return Ok(new ContentContainer<int>(userCount, "User count retrieved successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ContentContainer<string>(null, $"Failed to retrieve user count: {ex.Message}"));
            }
        }

        [HttpGet("items/count")]
        public IActionResult GetItemCount()
        {
            try
            {
                int itemCount = _repository.GetItemCount();
                return Ok(new ContentContainer<int>(itemCount, "Item count retrieved successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ContentContainer<string>(null, $"Failed to retrieve item count: {ex.Message}"));
            }
        }

        [HttpGet("persons/count")]
        public IActionResult GetPersonCount()
        {
            try
            {
                int personCount = _repository.GetPersonCount();
                return Ok(new ContentContainer<int>(personCount, "Person count retrieved successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ContentContainer<string>(null, $"Failed to retrieve person count: {ex.Message}"));
            }
        }

        //[HttpPut("users/{userId}")]
        //public IActionResult UpdateUser(string userId,[FromForm]  UserDto updateUserDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(new ContentContainer<string>(null, "Invalid data provided."));
        //    }

        //    try
        //    {
        //        _repository.UpdateUser(userId, updateUserDto);
        //        return Ok(new ContentContainer<string>(null, "User info updated successfully."));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new ContentContainer<string>(null, $"Failed to update user: {ex.Message}"));
        //    }
        //}


        [HttpPut("users/{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromForm] UserDto updateUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ContentContainer<string>(null, "Invalid data provided."));
            }

            if (updateUserDto.id != userId)
            {
                return BadRequest(new ContentContainer<string>(null, "User ID in URL and form body does not match."));
            }

            if (updateUserDto.ImageFile != null)
            {
                var fileResult = _fileService.SaveImage(updateUserDto.ImageFile);
                if (fileResult.Item1 == 1)
                {
                    updateUserDto.Image = fileResult.Item2; // getting name of image
                }
            }

            try
            {
                var success = await _repository.UpdateUser(userId, updateUserDto);
                if (success)
                {
                    return Ok(new ContentContainer<string>(null, "User info updated successfully."));
                }
                else
                {
                    return NotFound(new ContentContainer<string>(null, "User not found"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ContentContainer<string>(null, $"Failed to update user: {ex.Message}"));
            }
        }


        [HttpDelete("users/{userId}")]
        public IActionResult DeleteUser(string userId)
        {
            try
            {
                _repository.DeleteUser(userId);
                return Ok(new ContentContainer<string>(null, "User deleted successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ContentContainer<string>(null, $"Failed to delete user: {ex.Message}"));
            }
        }

        [HttpGet("complaints/count")]
        public IActionResult GetComplaintCount()
        {
            try
            {
                int complaintCount = _repository.GetComplaintCount();
                return Ok(new ContentContainer<int>(complaintCount, "Complaint count retrieved successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ContentContainer<string>(null, $"Failed to retrieve complaint count: {ex.Message}"));
            }
        }


        [HttpGet("complaints/search")]
        public async Task<IActionResult> SearchComplaintsByEmail([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new ContentContainer<string>(null, "Email query parameter is required."));
            }

            try
            {
                var complaints = await _repository.SearchComplaintsByEmailAsync(email);
                return Ok(new ContentContainer<IEnumerable<Complains>>(complaints, "Complaints retrieved successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ContentContainer<string>(null, $"Failed to search complaints: {ex.Message}"));
            }
        }


        // get all users in website 
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _repository.GetAllUsersAsync();

                if (users != null)
                {
                    return Ok(users);
                }
                else
                {
                    return NotFound("No users found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving users: {ex.Message}");
            }
        }



        // searsh in users by email 
        [HttpGet("users/search")]
        public async Task<IActionResult> SearchUsersByEmail([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new ContentContainer<string>(null, "Email query parameter is required."));
            }

            try
            {
                var users = await _repository.SearchUsersByEmailAsync(email);

                if (users == null || !users.Any())
                {
                    return NotFound(new ContentContainer<string>(null, "No users found with this email."));
                }

                return Ok(new ContentContainer<IEnumerable<AppUser>>(users, "Users retrieved successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ContentContainer<string>(null, $"Failed to search users: {ex.Message}"));
            }
        }



        // Delete Person Comments 
        [HttpDelete("persons/{personId}/comments")]
        public async Task<IActionResult> DeletePersonComments(int personId)
        {
            try
            {
                var success = await _personCommentRepository.DeletePersonComments(personId);
                if (success)
                {
                    return Ok(new ContentContainer<string>(null, "All comments for the person deleted successfully"));
                }
                else
                {
                    return NotFound(new ContentContainer<string>(null, "Comments not found for the specified person"));
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

        // Delete Item Post Comments 
        [HttpDelete("items/{itemId}/comments")]
        public async Task<IActionResult> DeleteItemComments(int itemId)
        {
            try
            {
                var success = await _itemCommentRepository.DeleteItemComments(itemId);
                if (success)
                {
                    return Ok(new ContentContainer<string>(null, "All comments for the item deleted successfully"));
                }
                else
                {
                    return NotFound(new ContentContainer<string>(null, "Comments not found for the specified item"));
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
    }
}




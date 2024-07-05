using Account.Core.Dtos;
using Account.Core.Dtos.Account;
using Account.Core.Models;
using Account.Core.Services;
using Account.Reposatory.Reposatories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Account.Apis.Controllers
{

    [Authorize]
    public class UserController : ApiBaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IFileService _fileService;
        private readonly IAccountService _accountService;


        public UserController(IUserRepository userRepository, IFileService fileService, IAccountService accountService)
        {
            _userRepository = userRepository;
            _fileService = fileService;
            _accountService = accountService;
        }

        //[HttpPut("users/{userId}")]
        //public IActionResult UpdateUser(string userId, [FromForm] [FromBody] UserForUserDto updateUserDto)
        //{
        //    // Handle image file upload
        //    if (updateUserDto.ImageFile != null)
        //    {
        //        var fileResult = _fileService.SaveImage(updateUserDto.ImageFile);
        //        if (fileResult.Item1 == 1)
        //        {
        //            updateUserDto.Image = fileResult.Item2; // getting name of image
        //        }
        //        else
        //        {
        //            return BadRequest(new ContentContainer<string>(null, "Error uploading image"));
        //        }
        //    }
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(new ContentContainer<string>(null, "Invalid data provided."));
        //    }

        //    try
        //    {
        //        _userRepository.UpdateUser(userId, updateUserDto);
        //        return Ok(new ContentContainer<string>(   "User info updated successfully."));
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return NotFound(new ContentContainer<string>(null, $"User not found: {ex.Message}"));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new ContentContainer<string>(null, $"Failed to update user: {ex.Message}"));
        //    }
        //}

        [HttpPut("users/{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromForm] UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ContentContainer<string>(null, "Invalid data provided."));
            }

            // Handle image file upload
            if (updateUserDto.ImageFile != null)
            {
                var fileResult = _fileService.SaveImage(updateUserDto.ImageFile);
                if (fileResult.Item1 == 1)
                {
                    updateUserDto.Image = fileResult.Item2; // getting name of image
                }
                else
                {
                    return BadRequest(new ContentContainer<string>(null, "Error uploading image"));
                }
            }

            updateUserDto.Id = userId;

            try
            {
                var result = await _accountService.UpdateUserInfoAsync(updateUserDto);
                if (result.StatusCode == 200)
                {
                    return Ok(new ContentContainer<string>(result.Message, "User info updated successfully."));
                }
                else
                {
                    return StatusCode(result.StatusCode, new ContentContainer<string>(null, result.Message));
                }
            }
            catch (ArgumentException ex)
            {
                return NotFound(new ContentContainer<string>(null, $"User not found: {ex.Message}"));
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
                _userRepository.DeleteUser(userId);
                return Ok(new ContentContainer<string>(null, "User deleted successfully."));
            }
            catch (ArgumentException ex)
            {
                return NotFound(new ContentContainer<string>(null, $"User not found: {ex.Message}"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ContentContainer<string>(null, $"Failed to delete user: {ex.Message}"));
            }
        }

    
    }
}

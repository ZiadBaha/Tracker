using Account.Core.Dtos;
using Account.Core.Enums;
using Account.Core.Models;
using Account.Core.Services;
using Account.services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Account.Apis.Controllers
{
    //[Authorize]
    public class ItemsController : ApiBaseController
    {
        private readonly IFileService _fileService;
        private readonly IitemRepository _itemRepository;
        private readonly IMapper _mapper;

        public ItemsController(IFileService fileService, IitemRepository itemRepository, IMapper mapper)
        {
            _fileService = fileService;
            _itemRepository = itemRepository;
            _mapper = mapper;
        }

        [HttpGet("{userId}/items")]
        public async Task<IActionResult> GetAllItems(string userId)
        {
            var items = await _itemRepository.GetAllItems(userId);
            return Ok(new ContentContainer<IEnumerable<ItemDto>>(items));
        }

        [HttpGet("{userId}/items/{id}")]
        public async Task<IActionResult> GetItem(string userId, int id)
        {
            var item = await _itemRepository.GetItemById(userId, id);
            if (item == null)
            {
                return NotFound(new ContentContainer<string>(null, "Item not found"));
            }

            return Ok(new ContentContainer<ItemDto>(item, "Item retrieved successfully"));
        }

        [HttpGet("{userId}/items/status/{itemStatus}")]
        public async Task<IActionResult> GetItemsByStatus(string userId, ItemStatus itemStatus)
        {
            var items = await _itemRepository.GetItemByStatus(userId, itemStatus);
            return Ok(new ContentContainer<IEnumerable<ItemDto>>(items, "Items retrieved successfully"));
        }

        [HttpPost("{userId}/items")]
        public async Task<IActionResult> AddItem(string userId, [FromForm] ItemDto itemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ContentContainer<string>(null, "Please provide valid data"));
            }

            if (itemDto.ImageFile != null)
            {
                var fileResult = _fileService.SaveImage(itemDto.ImageFile);
                if (fileResult.Item1 == 1)
                {
                    itemDto.Image = fileResult.Item2; // getting name of image
                }
            }

            try
            {
                var itemId = await _itemRepository.AddItem(userId, itemDto);
                if (itemId != 0)
                {
                    var createdItem = await _itemRepository.GetItemById(userId, itemId);
                    return CreatedAtAction(nameof(GetItem), new { userId, id = itemId }, new ContentContainer<ItemDto>(createdItem, "Added successfully"));
                }
                else
                {
                    return BadRequest(new ContentContainer<string>(null, "Error adding item"));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ContentContainer<string>(null, "An unexpected error occurred"));
            }
        }

        [HttpPut("{userId}/items/{id}")]
        public async Task<IActionResult> UpdateItem(string userId, int id, [FromForm] ItemDto itemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ContentContainer<string>(null, "Please provide valid data"));
            }

            if (id != itemDto.Id)
            {
                return BadRequest(new ContentContainer<string>(null, "Id in URL and form body does not match."));
            }

            if (itemDto.ImageFile != null)
            {
                var fileResult = _fileService.SaveImage(itemDto.ImageFile);
                if (fileResult.Item1 == 1)
                {
                    itemDto.Image = fileResult.Item2; // getting name of image
                }
            }

            try
            {
                var success = await _itemRepository.UpdateItem(userId, id, itemDto);
                if (success)
                {
                    return Ok(new ContentContainer<string>(null, "Updated successfully"));
                }
                else
                {
                    return NotFound(new ContentContainer<string>(null, "Item not found"));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ContentContainer<string>(null, "Error updating item"));
            }
        }


        [HttpDelete("{userId}/items/{id}")]
        public async Task<IActionResult> DeleteItem(string userId, int id)
        {
            var success = await _itemRepository.DeleteItem(userId, id);
            if (success)
            {
                return Ok(new ContentContainer<string>(null, "Deleted successfully"));
            }
            else
            {
                return NotFound(new ContentContainer<string>(null, "Item not found"));
            }
        }


        ////[Authorize(Roles = "Admin,User")]
        //[HttpDelete("{userId}/items/{id}")]
        //public async Task<IActionResult> DeleteItem(string userId, int id)
        //{
        //    try
        //    {
        //        var success = await _itemRepository.DeleteItem(userId, id);
        //        if (success)
        //        {
        //            return Ok(new ContentContainer<string>(null, "Deleted successfully"));
        //        }
        //        else
        //        {
        //            return NotFound(new ContentContainer<string>(null, "Item not found"));
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new ContentContainer<string>(null, "Error deleting item"));
        //    }
        //}

        [HttpGet("{userId}/items/search")]
        public async Task<IActionResult> Search(string userId, [FromQuery] string name, [FromQuery] string uniqNumber)
        {
            var items = await _itemRepository.Search(userId, name, uniqNumber);
            return Ok(new ContentContainer<IEnumerable<ItemDto>>(items, "Items retrieved successfully"));
        }




        // without user id 
        [HttpGet("items")]
        public async Task<IActionResult> GetAllItems()
        {
            var items = await _itemRepository.GetAllItems();
            return Ok(new ContentContainer<IEnumerable<ItemDto>>(items));
        }

        [HttpGet("items/{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            var item = await _itemRepository.GetItemById(id);
            if (item == null)
            {
                return NotFound(new ContentContainer<string>(null, "Item not found"));
            }

            return Ok(new ContentContainer<ItemDto>(item, "Item retrieved successfully"));
        }

        [HttpGet("items/status/{itemStatus}")]
        public async Task<IActionResult> GetItemsByStatus(ItemStatus itemStatus)
        {
            var items = await _itemRepository.GetItemByStatus(itemStatus);
            return Ok(new ContentContainer<IEnumerable<ItemDto>>(items, "Items retrieved successfully"));
        }

        [HttpGet("items/search")]
        public async Task<IActionResult> Search([FromQuery] string? name, [FromQuery] string? uniqNumber)
        {
            var items = await _itemRepository.Search(name, uniqNumber);
            return Ok(new ContentContainer<IEnumerable<ItemDto>>(items, "Items retrieved successfully"));
        }





    }
}

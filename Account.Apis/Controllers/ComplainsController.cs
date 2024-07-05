using Account.Core.Dtos;
using Account.Core.Models;
using Account.Core.Models.Entites;
using Account.Core.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Account.Apis.Controllers
{
    public class ComplainsController : ApiBaseController
    {
        private readonly IComplainsRepository _complainsRepository;
        private readonly IMapper _mapper;

        public ComplainsController(IComplainsRepository complainsRepository, IMapper mapper)
        {
            _complainsRepository = complainsRepository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ContentContainer<List<ComplainsDto>>>> GetAllComplains()
        {
            var complains = await _complainsRepository.GetAllComplains();

            if (complains == null || complains.Count == 0)
            {
                return NotFound(new ContentContainer<string>(null, "No complains found"));
            }

            var complainsDto = _mapper.Map<List<ComplainsDto>>(complains);
            var dataContainer = new ContentContainer<List<ComplainsDto>>(complainsDto, "Complains retrieved successfully");

            return Ok(dataContainer);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ContentContainer<ComplainsDto>>> GetComplainById(int id)
        {
            var complain = await _complainsRepository.GetComplainById(id);
            if (complain == null)
            {
                return NotFound(new ContentContainer<string>(null, "Complain not found"));
            }

            var complainDto = _mapper.Map<ComplainsDto>(complain);
            var dataContainer = new ContentContainer<ComplainsDto>(complainDto, "Complain retrieved successfully");

            return Ok(dataContainer);
        }

        [HttpPost]
        public async Task<ActionResult<ContentContainer<int>>> AddComplain(ComplainsDto complainsDto)
        {
            if (complainsDto == null)
            {
                return BadRequest(new ContentContainer<string>(null, "Complain data is null"));
            }

            try
            {
                var complain = _mapper.Map<ComplainsDto>(complainsDto);
                var complainId = await _complainsRepository.AddComplain(complain);

                if (complainId <= 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ContentContainer<string>(null, "Failed to add complain"));
                }

                return Ok(new ContentContainer<int>(complainId, "Complain added successfully"));
            }
            catch (Exception ex)
            {
                // Log the exception (replace Console.WriteLine with your logging mechanism)
                Console.WriteLine($"An error occurred while adding the complain: {ex}");

                // Return a 500 Internal Server Error with a generic error message
                return StatusCode(StatusCodes.Status500InternalServerError, new ContentContainer<string>(null, "An unexpected error occurred while adding the complain"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ContentContainer<ComplainsDto>>> EditComplain(int id, ComplainsDto updatedComplainDto)
        {
            if (updatedComplainDto == null)
            {
                return BadRequest(new ContentContainer<string>(null, "Complain data is null"));
            }

            try
            {
                var existingComplain = await _complainsRepository.GetComplainById(id);
                if (existingComplain == null)
                {
                    return NotFound(new ContentContainer<string>(null, "Complain not found"));
                }

                var complain = _mapper.Map(updatedComplainDto, existingComplain);
                var success = await _complainsRepository.EditComplain(id, complain);

                if (!success)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ContentContainer<string>(null, "Failed to update complain"));
                }

                var complainDtoResult = _mapper.Map<ComplainsDto>(complain);
                return Ok(new ContentContainer<ComplainsDto>(complainDtoResult, "Complain updated successfully"));
            }
            catch (Exception ex)
            {
                // Log the exception (replace Console.WriteLine with your logging mechanism)
                Console.WriteLine($"An error occurred while updating the complain: {ex}");

                // Return a 500 Internal Server Error with a generic error message
                return StatusCode(StatusCodes.Status500InternalServerError, new ContentContainer<string>(null, "An unexpected error occurred while updating the complain"));
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ContentContainer<string>>> DeleteComplain(int id)
        {
            try
            {
                var existingComplain = await _complainsRepository.GetComplainById(id);
                if (existingComplain == null)
                {
                    return NotFound(new ContentContainer<string>(null, "Complain not found"));
                }

                var success = await _complainsRepository.DeleteComplain(id);
                if (!success)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ContentContainer<string>(null, "Failed to delete complain"));
                }

                return Ok(new ContentContainer<string>(null, "Complain deleted successfully"));
            }
            catch (Exception ex)
            {
                // Log the exception (replace Console.WriteLine with your logging mechanism)
                Console.WriteLine($"An error occurred while deleting the complain: {ex}");

                // Return a 500 Internal Server Error with a generic error message
                return StatusCode(StatusCodes.Status500InternalServerError, new ContentContainer<string>(null, "An unexpected error occurred while deleting the complain"));
            }
        }
    }
}

using Account.Core.Dtos;
using Account.Core.Enums;
using Account.Core.Models;
using Account.Core.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Account.Apis.Controllers
{
    //[Authorize]
    public class PersonsController : ApiBaseController
    {
        private readonly IFileService _fileService;
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        public PersonsController(IFileService fileService, IPersonRepository personRepository, IMapper mapper)
        {
            _fileService = fileService;
            _personRepository = personRepository;
            _mapper = mapper;
        }

        #region MyRegion
        //[HttpGet("{userId}/persons")]
        //public async Task<IActionResult> GetAllPersons(string userId)
        //{
        //    var persons = await _personRepository.GetAllPersons(userId);
        //    return Ok(new ContentContainer<IEnumerable<PersoneDto>>(persons));
        //}

        //[HttpGet("{userId}/persons/{id}")]
        //public async Task<IActionResult> GetPerson(string userId, int id)
        //{
        //    var person = await _personRepository.GetPersonById(userId, id);
        //    if (person == null)
        //    {
        //        return NotFound(new ContentContainer<string>(null, "Person not found"));
        //    }

        //    return Ok(new ContentContainer<PersoneDto>(person, "Person retrieved successfully"));
        //}

        //[HttpGet("{userId}/persons/status/{personStatus}")]
        //public async Task<IActionResult> GetPersonsByStatus(string userId, PersoneStatus personStatus)
        //{
        //    var persons = await _personRepository.GetPersonsByStatus(userId, personStatus);
        //    return Ok(new ContentContainer<IEnumerable<PersoneDto>>(persons, "Persons retrieved successfully"));
        //}

        //[HttpPost("{userId}/persons")]
        //public async Task<IActionResult> AddPerson(string userId, [FromForm] PersoneDto personDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(new ContentContainer<string>(null, "Please provide valid data"));
        //    }

        //    if (personDto.ImageFile != null)
        //    {
        //        var fileResult = _fileService.SaveImage(personDto.ImageFile);
        //        if (fileResult.Item1 == 1)
        //        {
        //            personDto.Image = fileResult.Item2; // getting name of image
        //        }
        //    }
        //    // Your file handling logic here, if needed

        //    try
        //    {
        //        var personId = await _personRepository.AddPerson(userId, personDto);
        //        if (personId != 0)
        //        {
        //            var createdPerson = await _personRepository.GetPersonById(userId, personId);
        //            return CreatedAtAction(nameof(GetPerson), new { userId, id = personId }, new ContentContainer<PersoneDto>(createdPerson, "Added successfully"));
        //        }
        //        else
        //        {
        //            return BadRequest(new ContentContainer<string>(null, "Error adding person"));
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new ContentContainer<string>(null, "An unexpected error occurred"));
        //    }
        //}

        //[HttpPut("{userId}/persons/{id}")]
        //public async Task<IActionResult> UpdatePerson(string userId, int id, [FromForm] PersoneDto personDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(new ContentContainer<string>(null, "Please provide valid data"));
        //    }

        //    if (id != personDto.Id)
        //    {
        //        return BadRequest(new ContentContainer<string>(null, "Id in URL and form body does not match."));
        //    }

        //    try
        //    {
        //        var success = await _personRepository.UpdatePerson(userId, id, personDto);
        //        if (success)
        //        {
        //            return Ok(new ContentContainer<string>(null, "Updated successfully"));
        //        }
        //        else
        //        {
        //            return NotFound(new ContentContainer<string>(null, "Person not found"));
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new ContentContainer<string>(null, "Error updating person"));
        //    }
        //}

        //[HttpDelete("{userId}/persons/{id}")]
        //public async Task<IActionResult> DeletePerson(string userId, int id)
        //{
        //    var success = await _personRepository.DeletePerson(userId, id);
        //    if (success)
        //    {
        //        return Ok(new ContentContainer<string>(null, "Deleted successfully"));
        //    }
        //    else
        //    {
        //        return NotFound(new ContentContainer<string>(null, "Person not found"));
        //    }
        //}

        //[HttpGet("{userId}/persons/search")]
        //public async Task<IActionResult> Search(string userId, [FromQuery] string name)
        //{
        //    var persons = await _personRepository.Search(userId, name);
        //    return Ok(new ContentContainer<IEnumerable<PersoneDto>>(persons, "Persons retrieved successfully"));
        //}

        //// Methods without userId 
        ////[HttpGet("persons")]
        ////public async Task<IActionResult> GetAllPersons()
        ////{
        ////    var persons = await _personRepository.GetAllPersons();
        ////    return Ok(new ContentContainer<IEnumerable<PersoneDto>>(persons));
        ////}

        //[HttpGet]
        //public async Task<IActionResult> GetPersons()
        //{
        //    var persons = await _personRepository.GetAllPersons();
        //    var personList = persons.ToList();

        //    if (personList == null || personList.Count == 0)
        //    {
        //        return NotFound(new ContentContainer<string[]>(null, "No persons found"));
        //    }

        //    return Ok(new ContentContainer<List<PersoneDto>>(personList, "Persons retrieved successfully"));
        //}


        //[HttpGet("persons/{id}")]
        //public async Task<IActionResult> GetPersonById(int id)
        //{
        //    var person = await _personRepository.GetPersonById(id);
        //    if (person == null)
        //    {
        //        return NotFound(new ContentContainer<string>(null, "Person not found"));
        //    }

        //    return Ok(new ContentContainer<PersoneDto>(person, "Person retrieved successfully"));
        //}

        //[HttpGet("persons/status/{personStatus}")]
        //public async Task<IActionResult> GetPersonsByStatus(PersoneStatus personStatus)
        //{
        //    var persons = await _personRepository.GetPersonsByStatus(personStatus);
        //    return Ok(new ContentContainer<IEnumerable<PersoneDto>>(persons, "Persons retrieved successfully"));
        //}

        //[HttpGet("persons/search")]
        //public async Task<IActionResult> Search([FromQuery] string? name)
        //{
        //    var persons = await _personRepository.Search(name);
        //    return Ok(new ContentContainer<IEnumerable<PersoneDto>>(persons, "Persons retrieved successfully"));
        //} 
        #endregion

        [HttpGet("{userId}/persons")]
        public async Task<IActionResult> GetAllPersons(string userId)
        {
            var persons = await _personRepository.GetAllPersons(userId);
            return Ok(new ContentContainer<IEnumerable<PersoneDto>>(persons));
        }

        [HttpGet("{userId}/persons/{id}")]
        public async Task<IActionResult> GetPerson(string userId, int id)
        {
            var person = await _personRepository.GetPersonById(userId, id);
            if (person == null)
            {
                return NotFound(new ContentContainer<string>(null, "Person not found"));
            }

            return Ok(new ContentContainer<PersoneDto>(person, "Person retrieved successfully"));
        }

        [HttpGet("{userId}/persons/status/{personStatus}")]
        public async Task<IActionResult> GetPersonsByStatus(string userId, PersoneStatus personStatus)
        {
            var persons = await _personRepository.GetPersonsByStatus(userId, personStatus);
            return Ok(new ContentContainer<IEnumerable<PersoneDto>>(persons, "Persons retrieved successfully"));
        }

        [HttpPost("{userId}/persons")]
        public async Task<IActionResult> AddPerson(string userId, [FromForm] PersoneDto personDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ContentContainer<string>(null, "Please provide valid data"));
            }

            if (personDto.ImageFile != null)
            {
                var fileResult = _fileService.SaveImage(personDto.ImageFile);
                if (fileResult.Item1 == 1)
                {
                    personDto.Image = fileResult.Item2; // getting name of image
                }
            }

            try
            {
                var personId = await _personRepository.AddPerson(userId, personDto);
                if (personId != 0)
                {
                    var createdPerson = await _personRepository.GetPersonById(userId, personId);
                    return CreatedAtAction(nameof(GetPerson), new { userId, id = personId }, new ContentContainer<PersoneDto>(createdPerson, "Added successfully"));
                }
                else
                {
                    return BadRequest(new ContentContainer<string>(null, "Error adding person"));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ContentContainer<string>(null, "An unexpected error occurred"));
            }
        }

        [HttpPut("{userId}/persons/{id}")]
        public async Task<IActionResult> UpdatePerson(string userId, int id, [FromForm] PersoneDto personDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ContentContainer<string>(null, "Please provide valid data"));
            }

            if (id != personDto.Id)
            {
                return BadRequest(new ContentContainer<string>(null, "Id in URL and form body does not match."));
            }

            if (personDto.ImageFile != null)
            {
                var fileResult = _fileService.SaveImage(personDto.ImageFile);
                if (fileResult.Item1 == 1)
                {
                    personDto.Image = fileResult.Item2; // getting name of image
                }
            }

            try
            {
                var success = await _personRepository.UpdatePerson(userId, id, personDto);
                if (success)
                {
                    return Ok(new ContentContainer<string>(null, "Updated successfully"));
                }
                else
                {
                    return NotFound(new ContentContainer<string>(null, "Person not found"));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ContentContainer<string>(null, "Error updating person"));
            }
        }

        //[HttpDelete("{userId}/persons/{id}")]
        //public async Task<IActionResult> DeletePerson(string userId, int id)
        //{
        //    var success = await _personRepository.DeletePerson(userId, id);
        //    if (success)
        //    {
        //        return Ok(new ContentContainer<string>(null, "Deleted successfully"));
        //    }
        //    else
        //    {
        //        return NotFound(new ContentContainer<string>(null, "Person not found"));
        //    }
        //}
        [HttpDelete("{userId}/persons/{id}")]
        public async Task<IActionResult> DeletePerson(string userId, int id)
        {
            try
            {
                var success = await _personRepository.DeletePerson(userId, id);
                if (success)
                {
                    return Ok(new ContentContainer<string>(null, "Deleted successfully"));
                }
                else
                {
                    return NotFound(new ContentContainer<string>(null, "Person not found"));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ContentContainer<string>(null, "Error deleting person"));
            }
        }


        [HttpGet("{userId}/persons/search")]
        public async Task<IActionResult> Search(string userId, [FromQuery] string name)
        {
            var persons = await _personRepository.Search(userId, name);
            return Ok(new ContentContainer<IEnumerable<PersoneDto>>(persons, "Persons retrieved successfully"));
        }

        // Endpoints without userId
        [HttpGet("persons")]
        public async Task<IActionResult> GetAllPersons()
        {
            var persons = await _personRepository.GetAllPersons();
            return Ok(new ContentContainer<IEnumerable<PersoneDto>>(persons));
        }

        [HttpGet("persons/{id}")]
        public async Task<IActionResult> GetPersonById(int id)
        {
            var person = await _personRepository.GetPersonById(id);
            if (person == null)
            {
                return NotFound(new ContentContainer<string>(null, "Person not found"));
            }

            return Ok(new ContentContainer<PersoneDto>(person, "Person retrieved successfully"));
        }

        [HttpGet("persons/status/{personStatus}")]
        public async Task<IActionResult> GetPersonsByStatus(PersoneStatus personStatus)
        {
            var persons = await _personRepository.GetPersonsByStatus(personStatus);
            return Ok(new ContentContainer<IEnumerable<PersoneDto>>(persons, "Persons retrieved successfully"));
        }

        [HttpGet("persons/search")]
        public async Task<IActionResult> Search([FromQuery] string? name)
        {
            var persons = await _personRepository.Search(name);
            return Ok(new ContentContainer<IEnumerable<PersoneDto>>(persons, "Persons retrieved successfully"));
        }
    }
}

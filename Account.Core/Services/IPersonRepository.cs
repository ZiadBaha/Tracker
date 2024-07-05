using Account.Core.Dtos;
using Account.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Core.Services
{
    public interface IPersonRepository
    {
        Task<IEnumerable<PersoneDto>> GetAllPersons(string userId);
        Task<PersoneDto> GetPersonById(string userId, int id);
        Task<IEnumerable<PersoneDto>> GetPersonsByStatus(string userId, PersoneStatus personStatus);
        Task<int> AddPerson(string userId, PersoneDto personDto);
        Task<bool> UpdatePerson(string userId, int id, PersoneDto personDto);
        Task<bool> DeletePerson(string userId, int id);
        Task<IEnumerable<PersoneDto>> Search(string userId, string name);

        // Methods without userId
        Task<IEnumerable<PersoneDto>> GetAllPersons();
        Task<PersoneDto> GetPersonById(int id);
        Task<IEnumerable<PersoneDto>> GetPersonsByStatus(PersoneStatus personStatus);
        Task<IEnumerable<PersoneDto>> Search(string name);

    }
}

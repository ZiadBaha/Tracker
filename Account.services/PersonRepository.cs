using Account.Core.Dtos;
using Account.Core.Enums;
using Account.Core.Models.Entites;
using Account.Core.Services;
using Account.Reposatory.Data;
using Account.Reposatory.Data.Identity;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.services
{
    public class PersonRepository : IPersonRepository
    {
        private readonly StoreContext _storeContext;
        private readonly AppIdentityDbContext _identityContext;
        private readonly IMapper _mapper;

        public PersonRepository(StoreContext storeContext, AppIdentityDbContext identityContext, IMapper mapper)
        {
            _storeContext = storeContext;
            _identityContext = identityContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PersoneDto>> GetAllPersons(string userId)
        {
            var user = _identityContext.Users.Find(userId);
            if (user != null)
            {
                var persons = await _storeContext.persones.ToListAsync();
                return _mapper.Map<IEnumerable<PersoneDto>>(persons);
            }
            else
            {
                throw new ArgumentException("User not found");
            }
        }

        public async Task<PersoneDto> GetPersonById(string userId, int id)
        {
            var user = _identityContext.Users.Find(userId);
            if (user != null)
            {
                var person = await _storeContext.persones.FindAsync(id);
                return _mapper.Map<PersoneDto>(person);
            }
            else
            {
                throw new ArgumentException("User not found");
            }
        }

        public async Task<IEnumerable<PersoneDto>> GetPersonsByStatus(string userId, PersoneStatus personStatus)
        {
            var user = _identityContext.Users.Find(userId);
            if (user != null)
            {
                var persons = await _storeContext.persones.Where(p => p.Status == personStatus).ToListAsync();
                return _mapper.Map<IEnumerable<PersoneDto>>(persons);
            }
            else
            {
                throw new ArgumentException("User not found");
            }
        }

        public async Task<int> AddPerson(string userId, PersoneDto personDto)
        {
            var user = _identityContext.Users.Find(userId);
            if (user != null)
            {
                var person = _mapper.Map<Persone>(personDto);
                _storeContext.persones.Add(person);
                await _storeContext.SaveChangesAsync();
                return person.Id;
            }
            else
            {
                throw new ArgumentException("User not found");
            }
        }

        public async Task<bool> UpdatePerson(string userId, int id, PersoneDto personDto)
        {
            var user = _identityContext.Users.Find(userId);
            if (user != null)
            {
                var person = await _storeContext.persones.FindAsync(id);
                if (person == null)
                    return false;

                _mapper.Map(personDto, person);
                await _storeContext.SaveChangesAsync();
                return true;
            }
            else
            {
                throw new ArgumentException("User not found");
            }
        }

        public async Task<bool> DeletePerson(string userId, int id)
        {
            var user = _identityContext.Users.Find(userId);
            if (user != null)
            {
                var person = await _storeContext.persones.FindAsync(id);
                if (person == null)
                    return false;

                _storeContext.persones.Remove(person);
                await _storeContext.SaveChangesAsync();
                return true;
            }
            else
            {
                throw new ArgumentException("User not found");
            }
        }

        //public async Task<bool> DeletePerson(string userId, int id)
        //{
        //    using (var transaction = await _storeContext.Database.BeginTransactionAsync())
        //    {
        //        try
        //        {
        //            var person = await _storeContext.persones
        //                .Include(p => p.Comments) // Include related comments
        //                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

        //            if (person == null)
        //            {
        //                return false;
        //            }

        //            // Delete related comments
        //            _storeContext.comments.RemoveRange(person.Comments);

        //            // Delete the person
        //            _storeContext.persones.Remove(person);

        //            await _storeContext.SaveChangesAsync();
        //            await transaction.CommitAsync();
        //            return true;
        //        }
        //        catch (DbUpdateException)
        //        {
        //            await transaction.RollbackAsync();
        //            throw; // Re-throw the exception to be handled by the controller
        //        }
        //        catch (Exception)
        //        {
        //            await transaction.RollbackAsync();
        //            throw; // Re-throw the exception to be handled by the controller
        //        }
        //    }
        //}

        public async Task<IEnumerable<PersoneDto>> Search(string userId, string name)
        {
            var user = _identityContext.Users.Find(userId);
            if (user != null)
            {
                var persons = await _storeContext.persones.Where(p => p.PersonName.Contains(name)).ToListAsync();
                return _mapper.Map<IEnumerable<PersoneDto>>(persons);
            }
            else
            {
                throw new ArgumentException("User not found");
            }
        }

        // Methods without userId
        public async Task<IEnumerable<PersoneDto>> GetAllPersons()
        {
            var persons = await _storeContext.persones.ToListAsync();
            return _mapper.Map<IEnumerable<PersoneDto>>(persons);
        }

        public async Task<PersoneDto> GetPersonById(int id)
        {
            var person = await _storeContext.persones.FindAsync(id);
            return _mapper.Map<PersoneDto>(person);
        }

        public async Task<IEnumerable<PersoneDto>> GetPersonsByStatus(PersoneStatus personStatus)
        {
            var persons = await _storeContext.persones.Where(p => p.Status == personStatus).ToListAsync();
            return _mapper.Map<IEnumerable<PersoneDto>>(persons);
        }

        public async Task<IEnumerable<PersoneDto>> Search(string name)
        {
            var persons = await _storeContext.persones.Where(p => p.PersonName.Contains(name)).ToListAsync();
            return _mapper.Map<IEnumerable<PersoneDto>>(persons);
        }
    }
}

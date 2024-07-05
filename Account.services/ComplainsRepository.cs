using Account.Core.Dtos;
using Account.Core.Models.Entites;
using Account.Core.Services;
using Account.Reposatory.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.services
{
    public class ComplainsRepository : IComplainsRepository
    {
        private readonly StoreContext _dbContext;
        private readonly IMapper _mapper;

        public ComplainsRepository(StoreContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<ComplainsDto>> GetAllComplains()
        {
            var complains = await _dbContext.complains.ToListAsync();
            return _mapper.Map<List<ComplainsDto>>(complains);
        }

        public async Task<ComplainsDto> GetComplainById(int id)
        {
            var complain = await _dbContext.complains.FindAsync(id);
            return _mapper.Map<ComplainsDto>(complain);
        }

        public async Task<int> AddComplain(ComplainsDto complainDto)
        {
            var complain = _mapper.Map<Complains>(complainDto);
            _dbContext.complains.Add(complain);
            await _dbContext.SaveChangesAsync();
            return complain.Id;
        }

        public async Task<bool> EditComplain(int id, ComplainsDto updatedComplainDto)
        {
            var complain = await _dbContext.complains.FindAsync(id);
            if (complain == null)
                return false;

            _mapper.Map(updatedComplainDto, complain);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteComplain(int id)
        {
            var complain = await _dbContext.complains.FindAsync(id);
            if (complain == null)
                return false;

            _dbContext.complains.Remove(complain);
            await _dbContext.SaveChangesAsync();
            return true;
        }

    }
}

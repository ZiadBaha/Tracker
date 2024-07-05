using Account.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Core.Services
{
    public interface IComplainsRepository
    {
        Task<List<ComplainsDto>> GetAllComplains();
        Task<ComplainsDto> GetComplainById(int id);
        Task<int> AddComplain(ComplainsDto complainDto);
        Task<bool> EditComplain(int id, ComplainsDto updatedComplainDto);
        Task<bool> DeleteComplain(int id);
    }
}

using Account.Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Core.Dtos
{
    public class UserForUserDto
    {
        //public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        // public string Email { get; set; }
    }
}

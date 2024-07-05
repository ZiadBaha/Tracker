using Account.Core.Enums;
using Account.Core.Models.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Core.Models.Identity
{
    public class AppUser :IdentityUser
    {
        public UserRole UserRole { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }




        public ICollection<Item> Items { get; set; }
        public ICollection<Persone>  persones { get; set; }
        public ICollection<Comment> comments { get; set; }
    }
}

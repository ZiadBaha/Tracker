using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Core.Enums;
using Account.Core.Models.Identity;

namespace Account.Core.Models.Entites
{
    public class Persone : BaseEntity
    {
        public string PersonName { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }
        public DateTime DateTime { get; set; }
        public string? CommunicationLink { get; set; }
        public string? OtherDetails { get; set; }
        public int Age { get; set; }
        public PersonGender Gender { get; set; }
        public PersoneStatus Status { get; set; }
        // Upload Image
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public string? UserId { get; set; }
        public AppUser? User { get; set; }


        // Relation From Comment Class 
        public ICollection<Comment>? Comments { get; set; } = new List<Comment>();
    }
}

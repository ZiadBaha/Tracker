using Account.Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Core.Dtos
{
    public class PersoneDto
    {
        public int Id { get; set; }
        [Required]
        public string PersonName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Location { get; set; }
        public DateTime DateTime { get; set; }=DateTime.Now;
        public string? CommunicationLink { get; set; }
        public string? OtherDetails { get; set; }
        public int? Age { get; set; }
        public PersonGender Gender { get; set; }
        public PersoneStatus Status { get; set; }
        // Upload Image
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        //public ICollection<CommentDto>? Comments { get; set; }

    }
}

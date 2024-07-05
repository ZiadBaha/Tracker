using Account.Core.Enums;
using Account.Core.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Core.Dtos
{
    public class ItemDto
    { 
        public int Id { get; set; }

        [Required]
        public string ItemName { get; set; }
        [Required]
        public string PhoneNumber { get; set; } // ICone
        public string? CommunicationLink { get; set; }
        [Required]
        public string Location { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        [Required]
        public ItemStatus Status { get; set; }
        public string? OtherDetails { get; set; }

        // Add Image
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [Required]
        public string  UniqNumber { get; set; }

        //public ICollection<CommentDto>? Comments { get; set; }

        //public string UserId { get; set; }


    }

}


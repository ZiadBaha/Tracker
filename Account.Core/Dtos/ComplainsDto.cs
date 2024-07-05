using Account.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Core.Dtos
{
    public class ComplainsDto
    {
        public int Id { get; set; } // Nullable for create operation
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string ComplainText { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}

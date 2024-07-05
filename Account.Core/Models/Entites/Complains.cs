using Account.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Core.Models.Entites
{
    public class Complains : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string ComplainText { get; set; }
        public string Email { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;

    }
}

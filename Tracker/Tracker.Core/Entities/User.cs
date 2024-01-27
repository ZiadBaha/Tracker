using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracker.Core.Entities
{
    public class User
    {
        public int UserID { get; set; } // Primary key
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // Hashed password
                                             // Other relevant user details

        // Navigation properties
        public virtual ICollection<LostItem> LostItems { get; set; }
        public virtual ICollection<FoundItem> FoundItems { get; set; }
        public virtual ICollection<LostPerson> LostPersons { get; set; }
        public virtual ICollection<FoundPerson> FoundPersons { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Core.Dtos
{
    public class commentpersonDto
    {
        public int? Id { get; set; }
        public string Text { get; set; }
        public string PhoneNuamber { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        //public int PersonId { get; set; }
    }
}

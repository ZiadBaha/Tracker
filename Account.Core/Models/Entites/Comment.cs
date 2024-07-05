using Account.Core.Dtos;
using Account.Core.Models.Identity;

namespace Account.Core.Models.Entites
{
    public class Comment : BaseEntity
    {
        public string Text { get; set; }
        public string PhoneNuamber { get; set; }


        public DateTime DateTime { get; set; }


        public string? UserId { get; set; }
        public AppUser? User { get; set; }

        public int? ItemId { get; set; }
        public Item? Item { get; set; }
        public int? PersonId { get; set; }
        public Persone? Person { get; set; }

    }
}



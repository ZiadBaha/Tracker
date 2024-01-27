namespace Tracker.Core.Entities
{
    public class LostItem
    {
        public int ItemID { get; set; } // Primary key
        public int UserID { get; set; } // Foreign key referencing Users table
        public string ItemName { get; set; }
        public string PhoneNumber { get; set; }
        public string ItemDescription { get; set; }
        public string CommunicationLink { get; set; }
        public string Location { get; set; }
        public DateTime DateTimeOfLoss { get; set; }
        public string ImageURL { get; set; }
        // Other relevant item details

        // Navigation properties
        public virtual User User { get; set; }
        public virtual SimilarItem SimilarItem { get; set; }
    }
}
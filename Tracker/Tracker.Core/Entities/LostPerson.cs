namespace Tracker.Core.Entities
{
    public class LostPerson
    {
        public int PersonID { get; set; } // Primary key
        public int UserID { get; set; } // Foreign key referencing Users table
        public string PersonName { get; set; }
        public string PhoneNumber { get; set; }
        public string LastKnownLocation { get; set; }
        public DateTime DateTimeOfDisappearance { get; set; }
        public string CommunicationLink { get; set; }
        public string ImageURL { get; set; }
        // Other relevant person details

        // Navigation properties
        public virtual User User { get; set; }
        public virtual SimilarPerson SimilarPerson { get; set; }
    }
}
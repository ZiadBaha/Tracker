namespace Tracker.Core.Entities
{
    public class FoundPerson
    {
        public int PersonID { get; set; } // Primary key
        public int UserID { get; set; } // Foreign key referencing Users table
        public string PersonName { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }
        public DateTime DateTimeOfFinding { get; set; }
        public string CommunicationLink { get; set; }
        public string ImageURL { get; set; }
        // Other relevant person details

        // Navigation properties
        public virtual User User { get; set; }
        public virtual SimilarPerson SimilarPerson { get; set; }
    }
}
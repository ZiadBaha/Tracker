namespace Tracker.Core.Entities
{
    public class SimilarPerson
    {
        public int SimilarPersonID { get; set; } // Primary key
        public int LostPersonID { get; set; } // Foreign key referencing LostPersons table
        public int FoundPersonID { get; set; } // Foreign key referencing FoundPersons table
        public int ConfidenceScore { get; set; } // Measure of similarity between lost and found persons

        // Navigation properties
        public virtual LostPerson LostPerson { get; set; }
        public virtual FoundPerson FoundPerson { get; set; }
    }
}
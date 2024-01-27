namespace Tracker.Core.Entities
{
    public class SimilarItem
    {
        public int SimilarItemID { get; set; } // Primary key
        public int LostItemID { get; set; } // Foreign key referencing LostItems table
        public int FoundItemID { get; set; } // Foreign key referencing FoundItems table
        public int ConfidenceScore { get; set; } // Measure of similarity between lost and found items

        // Navigation properties
        public virtual LostItem LostItem { get; set; }
        public virtual FoundItem FoundItem { get; set; }
    }
}
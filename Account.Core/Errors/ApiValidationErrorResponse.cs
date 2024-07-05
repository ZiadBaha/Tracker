namespace Account.Apis.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        // Represents a collection of validation errors
        public IEnumerable<string> Errors { get; set; }

        // Initializes the Errors collection as an empty list
        public ApiValidationErrorResponse() : base(400)
        {
            Errors = new List<string>();
        }
    }

}

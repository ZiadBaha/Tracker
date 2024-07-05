using Account.Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Core.Models.Account
{
    public class Register
    {

        //Data anotation
        [Required]
        [RegularExpression(@"^[a-zA-Z\u0600-\u06FF\s]+$", ErrorMessage = " English letters are only allowed.")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z\u0600-\u06FF\s]+$", ErrorMessage = " English letters are only allowed.")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Minimum allowed length is 8 characters")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)*([@$!%*#?&])*[A-Za-z\d@$!%*#?&.]{6,32}$",
            ErrorMessage = "Password must contain at least one uppercase letter," +
            " one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Minimum allowed length is 8 characters")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        // Add Roles 
        public UserRole Role { get; set; } = UserRole.User; // Add this property


        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }

}

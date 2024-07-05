using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Core.Models.Account
{
    public class ResetPassword
    {
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(8, ErrorMessage = "Minimum allowed length is 8 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter," +
            " one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }
        [MinLength(8, ErrorMessage = "Minimum allowed length is 8 characters")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}

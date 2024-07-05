using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Core.Models.Account
{
    public class MailSettings
    {
        public  string Email { get; set; }
        public  string Password { get; set; }
        public  string SmtpServer { get; set; }
        public int Port { get; set; }
        public string DisplayedName { get; set; }
    }
}

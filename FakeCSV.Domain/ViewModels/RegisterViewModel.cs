using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FakeCSV.Domain.ViewModels
{
    public class RegisterViewModel
    {
        [Required (ErrorMessage = "Email address not specified")]
        [EmailAddress(ErrorMessage = "Invalid address")]
        [Remote(action: "CheckEmail", controller: "Account", ErrorMessage = "The address is already in use")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password not specified")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Password confirmation not specified")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password mismatch")]
        public string PasswordConfirmation { get; set; }
    }
}

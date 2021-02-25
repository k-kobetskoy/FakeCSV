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
        [Required (ErrorMessage = "Не указан адрес электронной почты")]
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        [Remote(action: "CheckEmail", controller: "Account", ErrorMessage = "адрес уже используется")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Не указано подтверждение пароля")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirmation { get; set; }
    }
}

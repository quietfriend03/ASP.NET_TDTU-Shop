﻿using System.ComponentModel.DataAnnotations;

namespace TdtuProject.Models.DTO
{
    public class RegistrationModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[#$^+=!*()@%&]).{6,}$",
            ErrorMessage = "Minimum length is 6 and must contain 1 Uppercase, 1 lowecase, 1 special character and 1 digit")]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string PasswordConfirmed { get; set; }
        public string? Role { get; set; }
    }
}

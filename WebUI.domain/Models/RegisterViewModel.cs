using System;
using System.ComponentModel.DataAnnotations;
using OnlineBanking.Domain.Entities;
using OnlineBanking.Domain.Enumerators;

namespace WebUI.domain.Models { 
   public class RegisterViewModel
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [Required(ErrorMessage = "Specify account type")]
        public AccountType AccountType { get; set; }      
        public AccountType AccountTypeTwo { get; set; } = AccountType.Student;        
        
        [Required(ErrorMessage = "Email required, this will serve as your username")]

        public string Email { get; set; }
        [Required(ErrorMessage = "Password required")]

        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm password to proceed")]

        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "First name required")]

        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name required")]

        public string LastName { get; set; }        
        
        public string AccountNumber { get; set; }
        public decimal? Balance { get; set; } = null;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = null;
        public string CreatedBy { get; set; } = "Shola nejo";
        public string UpdatedBy { get; set; } = null;
        public bool IsActive { get; set; } = true;
    }
}

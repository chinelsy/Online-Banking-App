﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlineBanking.Domain.Enumerators;
using OnlineBanking.Domain.Interfaces;

namespace OnlineBanking.Domain.Entities
{
  public class Customer
    {
       
        public int Id { get; set; }

        [MinLength(4), MaxLength(50)]
        public string FirstName { get; set; }
        
        [MinLength(4), MaxLength(50)]
        public string LastName { get; set; }

        public string Email { get; set; }

        public AccountType AccountType { get; set; }

        public DateTime Birthday { get; set; }

        public Gender Gender { get; set; }

        public Guid AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public Account Account { get; set; }

    }
}

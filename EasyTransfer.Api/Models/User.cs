﻿using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace EasyTransfer.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }   
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }

        public virtual List<BankAccount> BankAccounts { get; set; }
    }
}

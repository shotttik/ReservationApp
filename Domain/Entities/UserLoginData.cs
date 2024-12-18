﻿using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class UserLoginData
    {
        public int ID { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public byte [] PasswordHash { get; set; }
        public byte [] PasswordSalt { get; set; }
        public string? ConfirmationToken { get; set; }
        public int UserAccountID { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpirationTime { get; set; }
        public int EmailValidationStatus { get; set; }
        public string? PasswordRecoveryToken { get; set; }
        public DateTime? RecoveryTokenTime { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public UserAccount UserAccount { get; set; } = null!;
    }
}

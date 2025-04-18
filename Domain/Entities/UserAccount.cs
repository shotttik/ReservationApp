﻿namespace Domain.Entities
{
    public class UserAccount
    {
        public int ID { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public int? Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public int? CompanyID { get; set; }
        public required int RoleID { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual UserLoginData? UserLoginData { get; set; }
        public virtual Role? Role { get; set; }
        public virtual Company? Company { get; set; }
        public void UpdateTimestamp() => UpdatedAt = DateTime.Now;

    }
}

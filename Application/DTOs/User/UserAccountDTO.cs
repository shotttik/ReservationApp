﻿namespace Application.DTOs.User
{
    public class UserAccountDTO
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public List<RolesDTO> Roles { get; set; }

    }

    public class RolesDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<PermissionDTO> Permissions { get; set; }
    }
    public class PermissionDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
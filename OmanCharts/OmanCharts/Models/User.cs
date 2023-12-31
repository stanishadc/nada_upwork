﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmanCharts.Models
{
    public class User : IdentityUser
    {
        public Guid? ZoneId { get; set; }
        [Column(TypeName = "varchar(45)")]
        public string? FullName { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string? CompanyName { get; set; }
    }
    public class UserLogin
    {
        public Guid? UserLoginId { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string? UserId { get; set; }
        public DateTime? LoginTime { get; set; }
    }
    public class Login
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
    public class LoginResponse
    {
        public string? Token { get; set; }
        public DateTime? Expiration { get; set; }
        public string? UserId { get; set; }
        public Guid? ZoneId { get; set; }
        public string? Role { get; set; }
        public string? Name { get; set; }
        public string? ZoneName { get; set; }
        public bool Status { get; set; }
    }
    public class Register
    {
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "UserName is required")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
        public string? FullName { get; set; }
        public string? CompanyName { get; set; }
        public string? PhoneNumber { get; set; }
        public Guid? ZoneId { get; set; }
    }
}

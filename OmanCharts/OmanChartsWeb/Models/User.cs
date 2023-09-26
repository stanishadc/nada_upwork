using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace OmanChartsWeb.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? CompanyName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public Guid? RoleId { get; set; }
        public string? RoleName { get; set; }
        public Guid? ZoneId { get; set; }
        public string? ZoneName { get; set; }
        public List<SelectListItem> ListofZones { get; set; }
    }
    public class Login
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
    public class UserLogin
    {
        public string? FullName { get; set; }
        public string? CompanyName { get; set; }
        public string? LoginTime { get; set; }
    }
    public class LoginResponse
    {
        public string? Token { get; set; }
        public DateTime? Expiration { get; set; }
        public string? UserId { get; set; }
        public Guid? ZoneId { get; set; }
        public string? Role { get; set; }
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
        public string? ZoneName { get; set; }
        public List<SelectListItem> ListofZones { get; set; }
    }
}

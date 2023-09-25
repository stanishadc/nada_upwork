﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OmanCharts.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Response = OmanCharts.Models.Response;

namespace OmanCharts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly RepositoryContext _context;
        private readonly IConfiguration _configuration;
        public UserController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager, RepositoryContext context, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
        }
        [HttpGet("GetCustomers")]
        public async Task<IActionResult> GetCustomers()
        {
            var data = await (from u in _context.Users
                              join ur in _context.UserRoles on u.Id equals ur.UserId
                              join r in _context.Roles on ur.RoleId equals r.Id
                              select new
                              {
                                  u.Id,
                                  u.FullName,
                                  u.Email,
                                  u.PhoneNumber,
                                  roleId = r.Id,
                                  roleName = r.Name,
                              }).Where(r => r.roleName == UserRoles.Customer).ToListAsync();

            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Data = data });
        }
        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetById(string UserId)
        {
            var data = await (from u in _context.Users
                              join ur in _context.UserRoles on u.Id equals ur.UserId
                              join r in _context.Roles on ur.RoleId equals r.Id
                              select new
                              {
                                  u.Id,
                                  u.FullName,
                                  u.Email,
                                  u.PhoneNumber,
                                  roleId = r.Id,
                                  roleName = r.Name,
                              }).Where(r => r.roleName == UserRoles.Customer && r.Id == UserId).FirstOrDefaultAsync();

            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Data = data });
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromForm] Login model)
        {
            var user = await _userManager.FindByEmailAsync(model.Username);
            if (user != null)
            {
                var checkpassword = await _userManager.CheckPasswordAsync(user, model.Password);
                if (await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }
                    var token = GetToken(authClaims);

                    var roleName = userRoles.FirstOrDefault();
                    if (roleName == UserRoles.Customer)
                    {
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                            userId = user.Id,
                            role = roleName,
                            zoneId = user.ZoneId
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                            userId = user.Id,
                            role = roleName
                        });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new Response { Status = "Error", Message = "Please check the credentials!" });
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new Response { Status = "Error", Message = "User not exists!" });
            }
        }
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }
        [HttpPost]
        [Route("create-admin")]
        public async Task<IActionResult> CreateAdmin(Register model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Email))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Email is mandatory!" });
                }
                #region Business user creation
                var userExists = await _userManager.FindByNameAsync(model.Email);
                if (userExists != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Email already exists!" });
                }
                else
                {
                    User user = new()
                    {
                        Email = model.Email,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        FullName = model.FullName,
                        ZoneId = model.ZoneId
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (!result.Succeeded)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = DisplayError(result) });
                    }
                    if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                    }
                    await _userManager.AddToRoleAsync(user, UserRoles.Admin);
                }
                #endregion
                return Ok(new Response { Status = "Success", Message = "User Created Successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ex.Message });
            }
        }
        [HttpPost]
        [Route("create-user")]
        public async Task<IActionResult> CreateUser(Register model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Email))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Email is mandatory!" });
                }
                #region user creation
                var userExists = await _userManager.FindByNameAsync(model.Email);
                if (userExists != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Email already exists!" });
                }
                else
                {
                    User user = new()
                    {
                        Email = model.Email,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        FullName = model.FullName,
                        ZoneId = model.ZoneId
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (!result.Succeeded)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = DisplayError(result) });
                    }
                    if (!await _roleManager.RoleExistsAsync(UserRoles.Customer))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(UserRoles.Customer));
                    }
                    await _userManager.AddToRoleAsync(user, UserRoles.Customer);
                }
                #endregion
                return Ok(new Response { Status = "Success", Message = "User Created Successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ex.Message });
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user != null)
            {
                var resetPassResult = await _userManager.DeleteAsync(user);
                if (resetPassResult.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Account Deleted Successfully" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = DisplayError(resetPassResult) });
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not exists!" });
            }
        }
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(string id, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
                if (result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Password changed successfully" });
                }
                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Error", Message = "Failed to change password" });
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Error", Data = "No user error found" });
            }
        }
        private string DisplayError(IdentityResult result)
        {
            List<IdentityError> errorList = result.Errors.ToList();
            var errors = string.Join(", ", errorList.Select(e => e.Description));
            return errors;
        }
    }
}

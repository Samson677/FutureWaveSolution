using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FutureWave.Models.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace FutureWave.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        // Constructor to inject dependencies
        public AuthenticationController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

     
        // Register a new user with a role
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto registerUserDto)
        {
            if (registerUserDto == null || string.IsNullOrEmpty(registerUserDto.Email) || string.IsNullOrEmpty(registerUserDto.Password) || string.IsNullOrEmpty(registerUserDto.ConfirmPassword) || string.IsNullOrEmpty(registerUserDto.Role))
            {
                return BadRequest("All fields are required.");
            }

            if (registerUserDto.Password != registerUserDto.ConfirmPassword)
            {
                return BadRequest("Passwords do not match.");
            }

            var existingUser = await _userManager.FindByEmailAsync(registerUserDto.Email);
            if (existingUser != null)
            {
                return BadRequest("User already exists.");
            }

            // Create the new user
            var user = new IdentityUser
            {
                UserName = registerUserDto.Email,
                Email = registerUserDto.Email
            };  

            var result = await _userManager.CreateAsync(user, registerUserDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest("User registration failed. Please check the provided data.");
            }

            // Ensure the role exists
            var roleExists = await _roleManager.RoleExistsAsync(registerUserDto.Role);
            if (!roleExists)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(registerUserDto.Role));
                if (!roleResult.Succeeded)
                {
                    return BadRequest("Failed to create the specified role.");
                }
            }

            // Assign role to the new user
            var roleAssignResult = await _userManager.AddToRoleAsync(user, registerUserDto.Role);
            if (!roleAssignResult.Succeeded)
            {
                return BadRequest("Failed to assign role to the user.");
            }

            // Optionally sign in the user after registration
            var userRoles = await _userManager.GetRolesAsync(user);

            var token = GenerateJwtToken(user, userRoles);

            return Ok(new { Token = token });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginDto loginUserDto)
        {
            if (loginUserDto == null || string.IsNullOrEmpty(loginUserDto.Email) || string.IsNullOrEmpty(loginUserDto.Password))
            {
                return BadRequest("Invalid request");
            }

            var user = await _userManager.FindByEmailAsync(loginUserDto.Email);
            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);
            if (!isPasswordValid)
            {
                return Unauthorized("Invalid password.");
            }

            // Get user roles
            var userRoles = await _userManager.GetRolesAsync(user);

            // Generate JWT token
            var token = GenerateJwtToken(user, userRoles);

            // Return the token, email, username, and roles
            return Ok(new
            {
                Token = token,               // The JWT token
                Email = user.Email,          // The user's email
                Roles = userRoles            // The user's roles
            });
        }



        // Generate JWT Token for the user
        private string GenerateJwtToken(IdentityUser user, IList<string> userRoles)
        {
            var secretKey = _configuration["Jwt:Secret"];
            if (string.IsNullOrEmpty(secretKey) || secretKey.Length < 32)  // Ensure key is at least 256 bits (32 chars)
            {
                throw new Exception("JWT Secret Key is too short. It must be at least 32 characters long.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Add roles to claims
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:AccessTokenExpirationMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token); 
        }
    }
}

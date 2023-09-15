using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Birjanews.Api.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Birjanews.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController:ControllerBase
	{
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly string _jwtSecretKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9";

        public IdentityController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost("register")]
        [Authorize]

        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Username,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return Ok(new { Message = "User registered successfully." });
                }

                return BadRequest(new { Message = "Failed to register user.", Errors = result.Errors });
            }

            return BadRequest(new { Message = "Invalid registration data.", Errors = ModelState.Values.SelectMany(v => v.Errors) });
        }

        [HttpPost("update")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(UpdateDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);

                if (user != null)
                {
                    // Update username and email
                    user.UserName = model.Username;
                    user.Email = model.Email;

                    // Update password if provided
                    if (!string.IsNullOrEmpty(model.ExsistPassword))
                    {
                        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                        var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

                        if (!result.Succeeded)
                        {
                            return BadRequest(new { Message = "Failed to update password.", Errors = result.Errors });
                        }
                    }

                    var updateResult = await _userManager.UpdateAsync(user);

                    if (updateResult.Succeeded)
                    {
                        return Ok(new { Message = "User updated successfully." });
                    }

                    return BadRequest(new { Message = "Failed to update user.", Errors = updateResult.Errors });
                }

                return NotFound(new { Message = "User not found." });
            }

            return BadRequest(new { Message = "Invalid update data.", Errors = ModelState.Values.SelectMany(v => v.Errors) });
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id),
                }),
                Expires = DateTime.UtcNow.AddDays(365),
                Audience = "https://birjanews.az",
                Issuer= "https://birjanews.az",
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var token = GenerateJwtToken(user);
                    return Ok(new { Token = token });
                }

                return Unauthorized(new { Message = "Invalid login credentials." });
            }

            return BadRequest(new { Message = "Invalid login data." });
        }
        [HttpGet("getallusers")]
        [Authorize]
        public IActionResult GetAllUsers()
        {
            var users = _userManager.Users.Select(user => new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email
            }).ToList();

            return Ok(users);
        }


    }
}


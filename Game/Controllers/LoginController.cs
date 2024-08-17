using Game.Context;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ObjectPool;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Game.Controllers
{
    public class LoginController : Controller
    {

        SignInManager<AppUser> _signInManager;
        UserManager<AppUser> _userManager;
        AuthService _authService;
        TokenHolder _tokenHolder;
        public LoginController(TokenHolder tokenHolder,AuthService authService,SignInManager<AppUser> signInManager,UserManager<AppUser> userManager)
        {   
            _tokenHolder = tokenHolder;
            _authService = authService;
            _signInManager = signInManager;
            _userManager = userManager;

        }
       

        [HttpPost("/UserLogin")]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Invalid login data.");
            }

            // Find user by email
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
             
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (result.Succeeded)
                {
                   
                    var token = GenerateJwtToken(user);
                    _tokenHolder.Token = token;
                    bool dec = await _authService.Login(token);
                    if (dec)
                    {
                        return Ok(token);
                        
                    }
                }
                else
                {
                    return Unauthorized("Invalid credentials.");
                }
            }
            else
            {
                return NotFound("User not found.");
            }

            return BadRequest("Login failed.");
        }


        private string GenerateJwtToken(AppUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("wJnGzW5oGzStbHj2ZxTrkCvJ8xRhfLt2NzDpPv9rZtQ="));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7195/Login",
                audience: "https://localhost:7195/",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

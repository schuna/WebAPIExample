using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApi.Domain;
using WebApi.Domain.Models.Helpers;
using WebApi.Domain.ViewModel;
using WebApi.Persistence;

namespace WebApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private RoleManager<IdentityRole> RoleManager { get; }
        private readonly WebApiDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public AuthenticationController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
            WebApiDbContext context, IConfiguration configuration, TokenValidationParameters tokenValidationParameters)
        {
            _userManager = userManager;
            RoleManager = roleManager;
            _context = context;
            _configuration = configuration;
            _tokenValidationParameters = tokenValidationParameters;
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please, provide all the required fields");
            }

            var userExists = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);
            if (userExists != null)
            {
                return BadRequest($"User {registerViewModel.EmailAddress} already exists");
            }

            User user = new User
            {
                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.LastName,
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);

            if (result.Succeeded)
            {
                switch (registerViewModel.Role)
                {
                    case UserRoles.Admin:
                        await _userManager.AddToRoleAsync(user, UserRoles.Admin);
                        break;
                    case UserRoles.Worker:
                        await _userManager.AddToRoleAsync(user, UserRoles.Worker);
                        break;
                }

                return Ok("User created");
            }

            return BadRequest("User could not be created");
        }

        [HttpPost("login-user")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please, provide all the required fields");
            }

            var userExists = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);
            if (userExists != null && await _userManager.CheckPasswordAsync(userExists, loginViewModel.Password))
            {
                var tokenValue = await GenerateJwtTokenAsync(userExists, null);
                return Ok(tokenValue);
            }

            return Unauthorized();
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequestViewModel tokenRequestViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please, provide all the required fields");
            }

            var result = await VerifyAndGenerateTokenAsync(tokenRequestViewModel);
            return Ok(result);
        }

        private async Task<AuthResultViewModel> VerifyAndGenerateTokenAsync(TokenRequestViewModel tokenRequestViewModel)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var storedToken =
                await _context.RefreshTokens.FirstOrDefaultAsync(x =>
                    x.Token == tokenRequestViewModel.RefreshToken);
            var dbUser = await _userManager.FindByIdAsync(storedToken!.UserId);
            try
            {
                jwtTokenHandler.ValidateToken(tokenRequestViewModel.Token,
                    _tokenValidationParameters, out var validatedToken);
                return await GenerateJwtTokenAsync(dbUser, storedToken);
            }
            catch (SecurityTokenException)
            {
                if (storedToken.DateExpire >= DateTime.UtcNow)
                {
                    return await GenerateJwtTokenAsync(dbUser, storedToken);
                }
                else
                {
                    return await GenerateJwtTokenAsync(dbUser, null);
                }
            }
        }

        private async Task<AuthResultViewModel> GenerateJwtTokenAsync(User user, RefreshToken? rToken)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddMonths(12),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256));

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            if (rToken != null)
            {
                var rTokenResponse = new AuthResultViewModel
                {
                    Token = jwtToken,
                    RefreshToken = rToken.Token,
                    ExpireAt = token.ValidTo
                };
                return rTokenResponse;
            }

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid().ToString(),
                JwtId = token.Id,
                IsRevoked = false,
                UserId = user.Id,
                DateAdded = DateTime.UtcNow,
                DateExpire = DateTime.UtcNow.AddMonths(12),
                Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString()
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            var response = new AuthResultViewModel
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                ExpireAt = token.ValidTo
            };
            return response;
        }
    }
}
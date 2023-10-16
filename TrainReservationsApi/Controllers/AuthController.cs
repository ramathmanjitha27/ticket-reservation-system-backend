/**
 * This is the AuthController class.
 * This class controlls the authentication of the Traveler.
 * It has used JWT for the authentication
 *
 * <p>Bugs: None
 *
 * @author W.A.P.K.V. Wickramasinghe
 */
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrainReservationsApi.Models;
using TrainReservationsApi.Services;

namespace TrainReservationsApi.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IConfiguration _configuration;
        private readonly TravelerServices _travelerServices;
        private readonly StaffProfileService _staffProfileService;

        //Constructor
        public AuthController(IConfiguration configuration, TravelerServices travelerServices, StaffProfileService staffServices)
        {
            this._configuration = configuration;
            _travelerServices = travelerServices;
            _staffProfileService = staffServices;
        }

        [AllowAnonymous]
        [HttpPost]
        //Authentication Controller
        public async Task<IActionResult> Authenticate(string Email, string Password)
        {
            var authenticatedTraveler = await _travelerServices.Login(Email, Password);

            if (authenticatedTraveler != null)
            {
                var existingToken = HttpContext.Request.Headers["Authorization"].ToString();
                if (!string.IsNullOrEmpty(existingToken))
                {
                    // A token already exists, you can return an appropriate response
                    return Ok("You are already logged in.");
                }

                // Authentication successful, return traveler data or a token
                var issuer = _configuration["Jwt:Issuer"];
                var audience = _configuration["Jwt:Audience"];
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
                var signingCredentials = new SigningCredentials(
                                        new SymmetricSecurityKey(key),
                                        SecurityAlgorithms.HmacSha512Signature
                                    );
                var subject = new ClaimsIdentity(new[]
                {
                            new Claim(JwtRegisteredClaimNames.Sub, Email),
                            new Claim(JwtRegisteredClaimNames.Email, Email),
                            });

                var expires = DateTime.UtcNow.AddMinutes(10);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = subject,
                    Expires = DateTime.UtcNow.AddMinutes(10),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = signingCredentials
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);
                return Ok(jwtToken);
            }

            return Unauthorized("Invalid email or password");
        }

        [HttpPost("staff")]
        //Authentication Controller of Staff
        public async Task<IActionResult> AuthenticateStaff([FromBody] LoginModel login)
        {

            if (login == null || string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Email and Password cannot be null");
            }

            string Email = login.Email;
            string Password = login.Password;

            var authenticatedStaff = await _staffProfileService.LoginStaff(Email, Password);

            if (authenticatedStaff != null)
            {
                var existingToken = HttpContext.Request.Headers["Authorization"].ToString();
                if (!string.IsNullOrEmpty(existingToken))
                {
                    // A token already exists, you can return an appropriate 
                    return Ok(new AuthResponseModel { Message = "User is already authenticated!", Status = 200, Success = true, Token = existingToken, User = authenticatedStaff });
                }

                // Authentication successful, return staff data or a token
                var issuer = _configuration["Jwt:Issuer"];
                var audience = _configuration["Jwt:Audience"];
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
                var signingCredentials = new SigningCredentials(
                                        new SymmetricSecurityKey(key),
                                        SecurityAlgorithms.HmacSha512Signature
                                    );
                var subject = new ClaimsIdentity(new[]
                {
                            new Claim(JwtRegisteredClaimNames.Sub, Email),
                            new Claim(JwtRegisteredClaimNames.Email, Email),
                            });

                var expires = DateTime.UtcNow.AddMinutes(10);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = subject,
                    Expires = DateTime.UtcNow.AddMinutes(10),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = signingCredentials
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);
                return Ok(new AuthResponseModel { Message = "User Authenticated Successfully!", Status = 200, Success = true, Token = jwtToken, User = authenticatedStaff });
            }

            return Unauthorized(new AuthResponseModel { Message = "Invalid Email or Passoword", Status = 401, Success = false });
        }
    }
}

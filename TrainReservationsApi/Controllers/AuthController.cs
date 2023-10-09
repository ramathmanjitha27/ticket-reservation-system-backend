using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TrainReservationsApi.Models;
using TrainReservationsApi.Services;

namespace TrainReservationsApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private Staff _staff = new Staff();

        public AuthController(AuthService authService) =>
           _authService = authService;


        // staff login
        [HttpPost("staff/login")]
        public async Task<IActionResult> StaffLogin([FromBody] StaffLogin login)
        {
            _staff = await _authService.StaffLogin(login);

            if(_staff == null)
            {
                return BadRequest("Email or Passoword is incorrect");
            }
            return Ok(_staff);

        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainReservationsApi.Models;
using TrainReservationsApi.Services;

namespace TrainReservationsApi.Controllers
{
    // Exposing the function to be accessed by this url
    [Route("api/staff")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly StaffProfileService _staffService;
        private Staff _staff = new Staff();

        // Constructor initializes the StaffController with a reference to the StaffProfileService,
        public StaffController(StaffProfileService staffService) =>
            _staffService = staffService;


        // get all staff members
        [Authorize]
        [HttpGet]
        public async Task<List<Staff>> GetAllStaff() =>
            await _staffService.GetAllStaffMembers();

        // Get staff details by unique identifier (id).
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStaffDetails(string id)
        {
            _staff = await _staffService.GetStaffMemberById(id);

            if (_staff == null)
            {
                return NotFound();
            }

            return Ok(_staff);
        }


        // create new staff member
        [HttpPost]
        public async Task<IActionResult> CreateNewStaffMember([FromBody] Staff staff)
        {

            try
            {
                await _staffService.CreateStaffMember(staff);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return CreatedAtAction(nameof(GetAllStaff), new { id = staff.Id }, staff);
        }

        // Update staff member details by unique identifier (id).
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStaff(string id, [FromBody] Staff staff)
        {
            try
            {
                await _staffService.UpdateStaffMember(id, staff);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok("Staff member detials updated!");
        }


        // Delete a staff member by unique identifier (id).
        [Authorize]
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteStaff(string id)
        {
            try
            {
                await _staffService.RemoveStaffMember(id);
                return Ok("Staff member is deleted!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

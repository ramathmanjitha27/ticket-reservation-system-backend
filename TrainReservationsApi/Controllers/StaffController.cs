using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainReservationsApi.Models;
using TrainReservationsApi.Services;

namespace TrainReservationsApi.Controllers
{
    [Route("api/staff")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly StaffProfileService _staffService;
        private Staff _staff = new Staff();

        public StaffController(StaffProfileService staffService) =>
            _staffService = staffService;


        // get all staff members
        [HttpGet]
        public async Task<List<Staff>> GetAllStaff() =>
            await _staffService.GetAllStaffMembers();

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

        // update staff member details
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


        // delete staff member
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteStaff(string id)
        {
            try
            {
                await _staffService.RemoveStaffMember(id);
                return Ok("Staff is deleted!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

/**
 * This is the TravelersController class.
 * This class controlls the Traveler GET,POST,DELETE,UPDATE
 * ACCOUNT ACTIVATION, DEACTIVATION
 *
 * <p>Bugs: None
 *
 * @author W.A.P.K.V. Wickramasinghe
 */
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainReservationsApi.Models;
using TrainReservationsApi.Services;

namespace TrainReservationsApi.Controllers
{
    [Route("api/traveler")]
    [ApiController]
    public class TravelersController : ControllerBase
    {
        private readonly TravelerServices _travelerServices;

        //Constructor
        public TravelersController(TravelerServices travelerServices)
        {
            _travelerServices = travelerServices;
        }
        // GET: api/traveler
        [HttpGet]
        public async Task<List<Traveler>> Get() => await _travelerServices.GetTravelersAsync();

        [Authorize]
        // GET api/traveler/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Traveler>> Get(string id)
        {
            var traveler = await _travelerServices.GetTravelerAsync(id);
            if (traveler is null)
            {
                return NotFound();
            }
            return traveler;
        }

        //POST api/traveler
        [HttpPost]
        public async Task<IActionResult> Post(Traveler newtraveler)
        {
            try
            {
                await _travelerServices.CreateTravelerAsync(newtraveler);
                return Ok("Traveler Registered successfully");
            }
            catch (Exception ex)
            {
                // Handle any exceptions or validation errors here
                return BadRequest($"Error creating traveler: {ex.Message}");
            }

        }

        [Authorize]
        // PUT api/traveler/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Traveler updateTraveler)
        {
            var traveler = await _travelerServices.GetTravelerAsync(id);
            if (traveler is null)
            {
                return NotFound("There is no Traveler with this id: " + id);
            }
            updateTraveler.Id = traveler.Id;

            await _travelerServices.UpdateTravelerAsync(id, updateTraveler);

            return Ok("Updated Successfully.");
        }

        // DELETE api/traveler/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var traveler = await _travelerServices.GetTravelerAsync(id);
            if (traveler is null)
            {
                return NotFound("There is no Traveler with this id: " + id);
            }
            await _travelerServices.RemoveTravelerAsync(id);

            return Ok("Deleted Successfully.");
        }

        [HttpPost("{id}/activate")]
        public async Task<IActionResult> ActivateAccount(string id)
        {
            var updatedTraveler = await _travelerServices.ActivateAccount(id);
            if (updatedTraveler == null)
            {
                return NotFound("Traveler not found");
            }
            return Ok(updatedTraveler);
        }

        [HttpPost("{id}/deactivate")]
        public async Task<IActionResult> DeactivateAccount(string id)
        {
            var updatedTraveler = await _travelerServices.DeactivateAccount(id);
            if (updatedTraveler == null)
            {
                return NotFound("Traveler not found");
            }
            return Ok(updatedTraveler);
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Authenticate(Traveler traveler)
        //{
        //    var authenticatedTraveler = await _travelerServices.Login(traveler.Email, traveler.Password);

        //    if (authenticatedTraveler != null)
        //    {
        //        // Authentication successful, return traveler data or a token
        //        return Ok(authenticatedTraveler);
        //    }

        //    return Unauthorized("Invalid email or password");
        //}
    }

}

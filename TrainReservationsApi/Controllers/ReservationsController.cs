/// <summary>
/// The ReservationsController class handles HTTP requests related to reservations.
/// </summary> 
/// <author>IT19051758</author>

using TrainReservationsApi.Models;
using TrainReservationsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace TrainReservationsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly ReservationsService _reservationsService;

    public ReservationsController(ReservationsService reservationsService) =>
        _reservationsService = reservationsService;

    // Get all reservation records from database
    [HttpGet]
    public async Task<List<Reservation>> Get() =>
        await _reservationsService.GetAsync();

    // Get the reservation record of a particular id 
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Reservation>> Get(string id)
    {
        var reservation = await _reservationsService.GetAsync(id);

        if (reservation is null)
        {
            return NotFound();
        }

        return reservation;
    }

    // Get all the reservation records (travel details) of a particular traveler
    [HttpGet("traveler/{travelerId:length(24)}")]
    public async Task<List<Reservation>> GetByTraveler(string travelerId)
    {
        var reservations = await _reservationsService.GetAsync();

        return reservations.Where(r => r.travelerId == travelerId).ToList();
    }

    // Get all the past reservation records (history) of a particular traveler
    [HttpGet("traveler/{travelerId:length(24)}/history")]
    public async Task<List<Reservation>> GetTravelHistory(string travelerId)
    {
        var reservations = await _reservationsService.GetAsync();

        return reservations.Where(r => r.travelerId == travelerId && r.date < DateTime.Now).ToList();
    }

    // Get all the upcoming reservation records of a particular traveler
    [HttpGet("traveler/{travelerId:length(24)}/upcoming")]
    public async Task<List<Reservation>> GetUpcomingReservations(string travelerId)
    {
        var reservations = await _reservationsService.GetAsync();

        return reservations.Where(r => r.travelerId == travelerId && r.date >= DateTime.Now).ToList();
    }

    // Add a new reservation record
    [HttpPost]
    public async Task<IActionResult> Post(Reservation newReservation)
    {
        DateTime bookingDate = DateTime.Now;
        DateTime reservationDate = newReservation.date;

        TimeSpan difference = reservationDate - bookingDate;

        // Check if the reservation date is not past
        if (reservationDate < bookingDate)
        {
            return BadRequest("Reservation date cannot be a past date.");
        }

        // Check if the reservation is made within 30 days of booking date
        if (difference.Days > 30)
        {
            return BadRequest("Reservation date must be within 30 days of booking date.");
        }

        // Check if the number of tickets booked doesn't exceed four
        if (newReservation.ticketCount > 4)
        {
            return BadRequest("Maximum number of tickets per reservation is four.");
        }

        await _reservationsService.CreateAsync(newReservation);

        return CreatedAtAction(nameof(Get), new { id = newReservation.Id }, newReservation);
    }

    // Update an existing reservation record
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Reservation updatedReservation)
    {
        var reservation = await _reservationsService.GetAsync(id);

        if (reservation is null)
        {
            return NotFound();
        }
        

        DateTime oldReservationDate = reservation.date;
        DateTime updateDate = DateTime.Now;


        TimeSpan updateDifference = oldReservationDate - updateDate;
     

        // Check if the update is made at least 5 days before the reserved date
        if (updateDifference.Days < 5)
        {
            return BadRequest("Reservation can only be updated at least 5 days before the reserved date.");
        }

        DateTime bookingDate = DateTime.Now;
        DateTime reservationDate = updatedReservation.date;

        // Check if the reservation date is not past
        if (reservationDate < bookingDate)
        {
            return BadRequest("Reservation date cannot be a past date.");
        }

        TimeSpan difference = reservationDate - bookingDate;

        // Check if the reservation is made within 30 days of booking date
        if (difference.Days > 30)
        {
            return BadRequest("Reservation date must be within 30 days of booking date. You may cancel this reservation and place another later.");
        }

        // Check if the number of tickets booked doesn't exceed four
        if (updatedReservation.ticketCount > 4)
        {
            return BadRequest("Maximum number of tickets per reservation is four.");
        }

        updatedReservation.Id = reservation.Id;

        await _reservationsService.UpdateAsync(id, updatedReservation);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var reservation = await _reservationsService.GetAsync(id);

        if (reservation is null)
        {
            return NotFound();
        }

        DateTime oldReservationDate = reservation.date;
        DateTime deleteDate = DateTime.Now;

        TimeSpan deleteDifference = oldReservationDate - deleteDate;

        // Check if the deletion is made at least 5 days before the reserved date
        if (deleteDifference.Days < 5)
        {
            return BadRequest("Reservation can only be deleted at least 5 days before the reserved date.");
        }

        await _reservationsService.RemoveAsync(id);

        return NoContent();
    }
}

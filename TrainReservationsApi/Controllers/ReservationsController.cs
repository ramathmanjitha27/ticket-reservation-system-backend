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

    [HttpGet]
    public async Task<List<Reservation>> Get() =>
        await _reservationsService.GetAsync();

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

    [HttpPost]
    public async Task<IActionResult> Post(Reservation newReservation)
    {
        DateTime bookingDate = DateTime.Now;
        DateTime reservationDate = newReservation.date;

        TimeSpan difference = reservationDate - bookingDate;

        if (difference.Days > 30)
        {
            return BadRequest("Reservation date must be within 30 days of booking date.");
        }


        if (newReservation.ticketCount > 4)
        {
            return BadRequest("Maximum number of tickets per reservation is four.");
        }

        await _reservationsService.CreateAsync(newReservation);

        return CreatedAtAction(nameof(Get), new { id = newReservation.Id }, newReservation);
    }

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

        if (updateDifference.Days < 5)
        {
            return BadRequest("Reservation can only be updated at least 5 days before the reserved date.");
        }

        DateTime bookingDate = DateTime.Now;
        DateTime reservationDate = updatedReservation.date;

        TimeSpan difference = reservationDate - bookingDate;

        if (difference.Days > 30)
        {
            return BadRequest("Reservation date must be within 30 days of booking date. You may cancel this reservation and place another later.");
        }


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

        if (deleteDifference.Days < 5)
        {
            return BadRequest("Reservation can only be deleted at least 5 days before the reserved date.");
        }

        await _reservationsService.RemoveAsync(id);

        return NoContent();
    }
}

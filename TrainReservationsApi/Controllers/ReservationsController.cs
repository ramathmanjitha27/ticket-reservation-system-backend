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

        await _reservationsService.RemoveAsync(id);

        return NoContent();
    }
}

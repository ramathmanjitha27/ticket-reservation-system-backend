using TrainReservationsApi.Models;
using TrainReservationsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace TrainReservationsApi.Controllers;

[ApiController]
[Route("api/trains")]
public class TrainController : ControllerBase
    {
        private readonly TrainService _trainService;

        public TrainController(TrainService trainService)
        {
            _trainService = trainService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Train>>> GetAll()
        {
            var trains = await _trainService.GetAllAsync();

            return Ok(trains);
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Train>> GetById(string id)
        {
            var train = await _trainService.GetByIdAsync(id);

            if (train == null)
            {
                return NotFound();
            }

            return Ok(train);
        }

        [HttpPost]
        public async Task<ActionResult<Train>> Create(Train newTrain)
        {
            await _trainService.CreateAsync(newTrain);

            return CreatedAtAction(nameof(GetById), new { id = newTrain.id }, newTrain);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult> Update(string id, Train updatedTrain)
        {
            await _trainService.UpdateAsync(id, updatedTrain);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _trainService.DeleteAsync(id);

            return NoContent();
        }

        // Check the availability of trains based on the travler's departure station, arrival station, date, ticket class and count criteria
        [HttpGet("availability")]
        public async Task<List<Train>> GetAvailableTrains(string departure, string arrival, string date, string ticketClass, int ticketCount)
        {
            var trains = await _trainService.GetAllAsync();

            var availableTrains = new List<Train>();

            foreach (var train in trains)
            {
                bool isAvailable = true;

                // Check if the train is published and active for reservations
                if (train.isPublished is false || train.isActive is false)
                {
                    isAvailable = false;           
                }

                // Check if the train departs and arrives at the required stations
                var schedule = train.schedules.FirstOrDefault(s => s.station == departure);
                if (schedule is null || schedule.departureTime.CompareTo(train.schedules.FirstOrDefault(s => s.station == arrival)?.arrivalTime) >= 0)
                {
                    isAvailable = false;
                }

                // Check if the train is available on the chosen date
                var dayOfWeek = DateTime.Parse(date).DayOfWeek.ToString();
                if (!train.availableDates.Contains(dayOfWeek))
                {
                    isAvailable = false;
                }

                // Check if the desired number of tickets are available in the desired ticket class
                var ticketAvailability = train.ticketsAvailability.FirstOrDefault(t => t.trainClass == ticketClass);
                if (ticketAvailability is null || ticketAvailability.tickets - ticketAvailability.reserved < ticketCount)
                {
                    isAvailable = false;
                }

                if (isAvailable)
                {
                    availableTrains.Add(train);
                }
            }
      
            return availableTrains;
    }

}



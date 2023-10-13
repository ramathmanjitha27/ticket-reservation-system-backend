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

        // Retrieves all trains from the service and returns them.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Train>>> GetAll()
        {
           
            var trains = await _trainService.GetAllAsync();

            return Ok(trains);
        }

        // Retrieves a specific train by its ID and returns it.
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

        // Creates a new train and returns it as part of the response.
        [HttpPost]
        public async Task<ActionResult<Train>> Create(Train newTrain)
        {
           
            await _trainService.CreateAsync(newTrain);

            return CreatedAtAction(nameof(GetById), new { id = newTrain.id }, newTrain);
        }

        // Updates an existing train's information based on the provided ID and updated data.
        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult> Update(string id, Train updatedTrain)
        {
           
            await _trainService.UpdateAsync(id, updatedTrain);

            return NoContent();
        }

        // Deletes a train by its ID.
        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> Delete(string id)
        {
           
            await _trainService.DeleteAsync(id);

            return NoContent();
        }


    /// <summary>
    /// Returns a list of available trains based on the user's input of departure station, arrival station, date, ticket class, and number of tickets needed.
    /// </summary>
    /// <returns>A list of available trains.</returns>
    /// <author>IT19051758</author>

    // Check the availability of trains based on the traveler's departure station, arrival station, date, ticket class, and count criteria
    [HttpGet("availability")]
        public async Task<List<Train>> GetAvailableTrains(string departure, string arrival, string date, string ticketClass, int ticketCount)
        {
            var trains = await _trainService.GetAllAsync();

            var availableTrains = new List<Train>();

            foreach (var train in trains)
            {
                bool isAvailable = true;

                // Check if the train is published and active for reservations
                // If not published or not active, it's marked as unavailable.
                if (train.isPublished is false || train.isActive is false)
                {
                    isAvailable = false;
                }

                // Check if the train departs and arrives at the required stations
                // If not departing and arriving correctly, it's marked as unavailable.
                var schedule = train.schedules.FirstOrDefault(s => s.station == departure);
                if (schedule is null || schedule.departureTime.CompareTo(train.schedules.FirstOrDefault(s => s.station == arrival)?.arrivalTime) >= 0)
                {
                    isAvailable = false;
                }

                // Check if the train is available on the chosen date
                // If not available on the chosen date, it's marked as unavailable.
                var dayOfWeek = DateTime.Parse(date).DayOfWeek.ToString();
                if (!train.availableDates.Contains(dayOfWeek))
                {
                    isAvailable = false;
                }

            // Check if the desired number of tickets are available in the desired ticket class
            int? ticketsAvailable = null;
            int? ticketsReserved = null;
            switch (ticketClass)
            {
                case "First":
                    ticketsAvailable = train.firstClassTickets;
                    ticketsReserved = train.firstClassTicketsReserved;
                    break;
                case "Second":
                    ticketsAvailable = train.secondClassTickets;
                    ticketsReserved = train.secondClassTicketsReserved;
                    break;
                case "Third":
                    ticketsAvailable = train.thirdClassTickets;
                    ticketsReserved = train.thirdClassTicketsReserved;
                    break;
                default:
                    isAvailable = false;
                    break;
            }
            if (ticketsAvailable is null || ticketsReserved is null || ticketsAvailable - ticketsReserved < ticketCount)
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


        //Cancel train based on the conditions.
        //if train not reserved can cancelled. otherwise not
        [HttpPut("setActiveStatus/{id:length(24)}")]
        public async Task<ActionResult> SetTrainActiveStatus(string id)
        {
            var existingTrain = await _trainService.GetByIdAsync(id);

            if (existingTrain == null)
            {
                return NotFound();
            }

            // Check if the train should be set as inactive based on ticket reservations.
            if (existingTrain.firstClassTicketsReserved <= 0 &&
                existingTrain.secondClassTicketsReserved <= 0 &&
                existingTrain.thirdClassTicketsReserved <= 0)
            {
                existingTrain.isActive = false; // Mark as inactive.
            }
            else
            {
                existingTrain.isActive = true; // Mark as active.
            }

            await _trainService.UpdateAsync(id, existingTrain);

            return NoContent();
        }
    }


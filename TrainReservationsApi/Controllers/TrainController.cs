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
    }



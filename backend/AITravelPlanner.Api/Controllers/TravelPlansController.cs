using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AITravelPlanner.Domain.DTOs;
using AITravelPlanner.Domain.Interfaces;

namespace AITravelPlanner.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TravelPlansController : ControllerBase
    {
        private readonly ITravelPlanService _travelPlanService;

        public TravelPlansController(ITravelPlanService travelPlanService)
        {
            _travelPlanService = travelPlanService;
        }

        // GET: api/travelplans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TravelPlanResponse>>> GetTravelPlans()
        {
            var travelPlans = await _travelPlanService.GetAllTravelPlansAsync();
            return Ok(travelPlans);
        }

        // GET: api/travelplans/public
        [HttpGet("public")]
        public async Task<ActionResult<IEnumerable<TravelPlanResponse>>> GetPublicTravelPlans()
        {
            var travelPlans = await _travelPlanService.GetPublicTravelPlansAsync();
            return Ok(travelPlans);
        }

        // GET: api/travelplans/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TravelPlanResponse>> GetTravelPlan(int id)
        {
            var travelPlan = await _travelPlanService.GetTravelPlanByIdAsync(id);
            if (travelPlan == null)
            {
                return NotFound();
            }

            return Ok(travelPlan);
        }

        // GET: api/travelplans/destination/{destination}
        [HttpGet("destination/{destination}")]
        public async Task<ActionResult<IEnumerable<TravelPlanResponse>>> GetTravelPlansByDestination(string destination)
        {
            var travelPlans = await _travelPlanService.GetTravelPlansByDestinationAsync(destination);
            return Ok(travelPlans);
        }

        // GET: api/travelplans/style/{travelStyle}
        [HttpGet("style/{travelStyle}")]
        public async Task<ActionResult<IEnumerable<TravelPlanResponse>>> GetTravelPlansByStyle(string travelStyle)
        {
            var travelPlans = await _travelPlanService.GetTravelPlansByTravelStyleAsync(travelStyle);
            return Ok(travelPlans);
        }

        // GET: api/travelplans/date-range
        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<TravelPlanResponse>>> GetTravelPlansByDateRange(
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate)
        {
            var travelPlans = await _travelPlanService.GetTravelPlansByDateRangeAsync(startDate, endDate);
            return Ok(travelPlans);
        }

        // POST: api/travelplans
        [HttpPost]
        public async Task<ActionResult<TravelPlanResponse>> CreateTravelPlan(CreateTravelPlanRequest request)
        {
            try
            {
                var travelPlan = await _travelPlanService.CreateTravelPlanAsync(request);
                return CreatedAtAction(nameof(GetTravelPlan), new { id = travelPlan.Id }, travelPlan);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/travelplans/generate
        [HttpPost("generate")]
        public async Task<ActionResult<TravelPlanResponse>> GenerateTravelPlan(GenerateTravelPlanRequest request)
        {
            try
            {
                var travelPlan = await _travelPlanService.GenerateTravelPlanAsync(request);
                return CreatedAtAction(nameof(GetTravelPlan), new { id = travelPlan.Id }, travelPlan);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/travelplans/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTravelPlan(int id, UpdateTravelPlanRequest request)
        {
            try
            {
                var travelPlan = await _travelPlanService.UpdateTravelPlanAsync(id, request);
                return Ok(travelPlan);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/travelplans/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTravelPlan(int id)
        {
            var result = await _travelPlanService.DeleteTravelPlanAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // Activity endpoints
        // GET: api/travelplans/{id}/activities
        [HttpGet("{id}/activities")]
        public async Task<ActionResult<IEnumerable<ActivityDto>>> GetActivities(int id)
        {
            var activities = await _travelPlanService.GetActivitiesByPlanIdAsync(id);
            return Ok(activities);
        }

        // POST: api/travelplans/{id}/activities
        [HttpPost("{id}/activities")]
        public async Task<ActionResult<ActivityDto>> AddActivity(int id, ActivityDto activityDto)
        {
            try
            {
                var activity = await _travelPlanService.AddActivityAsync(id, activityDto);
                return CreatedAtAction(nameof(GetActivity), new { id = activity.Id }, activity);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/travelplans/activities/{activityId}
        [HttpGet("activities/{activityId}")]
        public async Task<ActionResult<ActivityDto>> GetActivity(int activityId)
        {
            var activity = await _travelPlanService.GetActivityByIdAsync(activityId);
            if (activity == null)
            {
                return NotFound();
            }

            return Ok(activity);
        }

        // PUT: api/travelplans/activities/{activityId}
        [HttpPut("activities/{activityId}")]
        public async Task<IActionResult> UpdateActivity(int activityId, ActivityDto activityDto)
        {
            try
            {
                var activity = await _travelPlanService.UpdateActivityAsync(activityId, activityDto);
                return Ok(activity);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/travelplans/activities/{activityId}
        [HttpDelete("activities/{activityId}")]
        public async Task<IActionResult> DeleteActivity(int activityId)
        {
            var result = await _travelPlanService.DeleteActivityAsync(activityId);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // Accommodation endpoints
        // GET: api/travelplans/{id}/accommodations
        [HttpGet("{id}/accommodations")]
        public async Task<ActionResult<IEnumerable<AccommodationDto>>> GetAccommodations(int id)
        {
            var accommodations = await _travelPlanService.GetAccommodationsByPlanIdAsync(id);
            return Ok(accommodations);
        }

        // POST: api/travelplans/{id}/accommodations
        [HttpPost("{id}/accommodations")]
        public async Task<ActionResult<AccommodationDto>> AddAccommodation(int id, AccommodationDto accommodationDto)
        {
            try
            {
                var accommodation = await _travelPlanService.AddAccommodationAsync(id, accommodationDto);
                return CreatedAtAction(nameof(GetAccommodation), new { id = accommodation.Id }, accommodation);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/travelplans/accommodations/{accommodationId}
        [HttpGet("accommodations/{accommodationId}")]
        public async Task<ActionResult<AccommodationDto>> GetAccommodation(int accommodationId)
        {
            var accommodation = await _travelPlanService.GetAccommodationByIdAsync(accommodationId);
            if (accommodation == null)
            {
                return NotFound();
            }

            return Ok(accommodation);
        }

        // PUT: api/travelplans/accommodations/{accommodationId}
        [HttpPut("accommodations/{accommodationId}")]
        public async Task<IActionResult> UpdateAccommodation(int accommodationId, AccommodationDto accommodationDto)
        {
            try
            {
                var accommodation = await _travelPlanService.UpdateAccommodationAsync(accommodationId, accommodationDto);
                return Ok(accommodation);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/travelplans/accommodations/{accommodationId}
        [HttpDelete("accommodations/{accommodationId}")]
        public async Task<IActionResult> DeleteAccommodation(int accommodationId)
        {
            var result = await _travelPlanService.DeleteAccommodationAsync(accommodationId);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // Transportation endpoints
        // GET: api/travelplans/{id}/transportations
        [HttpGet("{id}/transportations")]
        public async Task<ActionResult<IEnumerable<TransportationDto>>> GetTransportations(int id)
        {
            var transportations = await _travelPlanService.GetTransportationsByPlanIdAsync(id);
            return Ok(transportations);
        }

        // POST: api/travelplans/{id}/transportations
        [HttpPost("{id}/transportations")]
        public async Task<ActionResult<TransportationDto>> AddTransportation(int id, TransportationDto transportationDto)
        {
            try
            {
                var transportation = await _travelPlanService.AddTransportationAsync(id, transportationDto);
                return CreatedAtAction(nameof(GetTransportation), new { id = transportation.Id }, transportation);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/travelplans/transportations/{transportationId}
        [HttpGet("transportations/{transportationId}")]
        public async Task<ActionResult<TransportationDto>> GetTransportation(int transportationId)
        {
            var transportation = await _travelPlanService.GetTransportationByIdAsync(transportationId);
            if (transportation == null)
            {
                return NotFound();
            }

            return Ok(transportation);
        }

        // PUT: api/travelplans/transportations/{transportationId}
        [HttpPut("transportations/{transportationId}")]
        public async Task<IActionResult> UpdateTransportation(int transportationId, TransportationDto transportationDto)
        {
            try
            {
                var transportation = await _travelPlanService.UpdateTransportationAsync(transportationId, transportationDto);
                return Ok(transportation);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/travelplans/transportations/{transportationId}
        [HttpDelete("transportations/{transportationId}")]
        public async Task<IActionResult> DeleteTransportation(int transportationId)
        {
            var result = await _travelPlanService.DeleteTransportationAsync(transportationId);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
} 
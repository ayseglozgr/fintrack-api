using FinTrack.Application.DTOs.Household;
using FinTrack.Application.Interfaces.Factories;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HouseholdController : ControllerBase
    {
        private readonly IUserHouseHoldFactory _userHouseHoldFactory;

        public HouseholdController(IUserHouseHoldFactory userHouseHoldFactory)
        {
            _userHouseHoldFactory = userHouseHoldFactory;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateHouseholdDto request)
        {
            try
            {
                var household = await _userHouseHoldFactory.CreateHouseholdForUserAsync(request);
                return Ok(household);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

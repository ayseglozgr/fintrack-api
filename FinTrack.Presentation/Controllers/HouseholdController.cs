using FinTrack.Application.DTOs.Household;
using FinTrack.Application.Interfaces.Factories;
using FinTrack.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HouseholdController : ControllerBase
    {
        private readonly IUserHouseHoldFactory _userHouseHoldFactory;
        private readonly IHouseholdService _householdService;

        public HouseholdController(IUserHouseHoldFactory userHouseHoldFactory, IHouseholdService householdService)
        {
            _userHouseHoldFactory = userHouseHoldFactory;
            _householdService = householdService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateHouseholdDto request)
        {
            var response = await _userHouseHoldFactory.CreateHouseholdForUserAsync(request);
            if (!response.IsSuccess || response.Data == null)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateHouseholdDto request)
        {
            var response = await _householdService.UpdateHouseholdAsync(request);
            if (!response.IsSuccess)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}

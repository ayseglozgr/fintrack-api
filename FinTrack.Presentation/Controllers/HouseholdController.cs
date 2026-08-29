using FinTrack.Application.DTOs.Household;
using FinTrack.Application.Interfaces.Factories;
using FinTrack.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Presentation.Controllers
{
    [Route("api/household")]
    [ApiController]
    public class HouseholdController : ControllerBase
    {
        private readonly IUserHouseHoldFactory _userHouseHoldFactory;
        private readonly IHouseholdService _householdService;
        private readonly IUserService _userService;

        public HouseholdController(
            IUserHouseHoldFactory userHouseHoldFactory,
            IHouseholdService householdService,
            IUserService userService)
        {
            _userHouseHoldFactory = userHouseHoldFactory;
            _householdService = householdService;
            _userService = userService;
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

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetUserHouseholds(int userId)
        {
            var response = await _userService.GetUserHouseholdsAsync(userId);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut("select-active")]
        public async Task<IActionResult> SelectActive([FromBody] SelectActiveHouseholdRequestDto request)
        {
            var response = await _userService.SwitchActiveHouseholdAsync(request.UserId, request.HouseholdId);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}

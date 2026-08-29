using FinTrack.Application.Common.Helpers;
using FinTrack.Application.Common.Models;
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

        [HttpGet("user/{userUid}")]
        public async Task<IActionResult> GetUserHouseholds(string userUid)
        {
            var userId = CipherHelper.DecryptId(userUid);
            if (userId <= 0)
            {
                return BadRequest(ServiceResponse<object>.Failure("Invalid encrypted id."));
            }

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
            var userId = CipherHelper.DecryptId(request.UserUid);
            var householdId = CipherHelper.DecryptId(request.HouseholdUid);
            if (userId <= 0 || householdId <= 0)
            {
                return BadRequest(ServiceResponse<object>.Failure("Invalid encrypted id."));
            }

            var response = await _userService.SwitchActiveHouseholdAsync(userId, householdId);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}

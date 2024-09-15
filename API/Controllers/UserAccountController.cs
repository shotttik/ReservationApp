using API.Attributes;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class UserAccountController :ControllerBase
    {
        private readonly IUserAccountService userAccountService;

        public UserAccountController(IUserAccountService userAccountService)
        {
            this.userAccountService = userAccountService;
        }

        [HttpPost]
        [Logging(LoggingType.Full)]
        public async Task<IActionResult> AddUserAccount([FromBody] UserAccountDTO userAccountDTO)
        {
            await userAccountService.AddUserAccountAsync(userAccountDTO);
            //return CreatedAtAction(nameof(GetReservationById), new { id = reservationDto.Id }, reservationDto);
            return Ok();
        }
    }
}

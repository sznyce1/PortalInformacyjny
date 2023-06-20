using Microsoft.AspNetCore.Mvc;
using ProjektZaliczeniowy.Models;
using ProjektZaliczeniowy.Services;

namespace ProjektZaliczeniowy.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccacountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccacountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody] RegisterUserDto dto)
        {
            _accountService.ReisterUser(dto);
            return Ok();
        }
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            string token = _accountService.GenerateJwt(dto);
            return Ok(token);
        }
    }
}

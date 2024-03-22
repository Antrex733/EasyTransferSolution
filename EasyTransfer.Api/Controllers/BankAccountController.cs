using Microsoft.AspNetCore.Authorization;

namespace EasyTransfer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BankAccountController: ControllerBase
    {
        private readonly IBankAccountService _bankAccount;
        private readonly IUserContextService _userContextService;

        public BankAccountController(IBankAccountService bankAccount,
            IUserContextService userContextService)
        {
            _bankAccount = bankAccount;
            _userContextService = userContextService;
        }

        [HttpPost("AddAccount")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult AddBankAcccount([FromBody]BankAccountDto bankAccount) 
        {
            if (bankAccount == null) 
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = _userContextService.GetUserId;
            if (!_bankAccount.AddBankAccount(userId, bankAccount))
                return BadRequest(ModelState);

            return Created();
        }
    }
}

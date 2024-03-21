using Microsoft.AspNetCore.Http.HttpResults;

namespace EasyTransfer.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpPost]
        [ProducesResponseType(204)]
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

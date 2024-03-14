
namespace EasyTransfer.Api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly EasyTransferDBContext _dbContext;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(EasyTransferDBContext dbContext, 
            IAccountService accountService, IMapper mapper)
        {
            _dbContext = dbContext;
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult RegisterUser([FromBody] RegisterUserDto dto) 
        {
            if (dto == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_accountService.RegisterUser(dto))
            {
                ModelState.AddModelError("", "Semething went wrong while savin");
                return StatusCode(500, ModelState);
            }

            var logDto = _mapper.Map<LoginUserDto>(dto);

            var token = _accountService.GenerateJwt(logDto);

            return Ok(token);
        }
        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult LoginUser([FromBody] LoginUserDto dto)
        {
            if (dto == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = _accountService.GenerateJwt(dto);

            return Ok(token);
        }
    }
}

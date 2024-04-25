using Microsoft.AspNetCore.Authorization;

namespace EasyTransfer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly EasyTransferDBContext _dbContext;
        private readonly IUserService _accountService;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public UserController(EasyTransferDBContext dbContext, 
            IUserService accountService, IMapper mapper, 
            IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _accountService = accountService;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        [HttpPost("register")]
        [ProducesResponseType(201)]
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
        [HttpGet("login")]
        [ProducesResponseType(201)]
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
        [Authorize]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetUser()
        {
            var userId = _userContextService.GetUserId; 
            var userDto = _accountService.GetUser(userId);
            if (userDto == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            return Ok(userDto);
        }
    }
}

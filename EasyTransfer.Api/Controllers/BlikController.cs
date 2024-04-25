using Microsoft.AspNetCore.Authorization;

namespace EasyTransfer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlikController : ControllerBase
    {
        private readonly IBlikService _blikService;
        private readonly IUserContextService _userContext;

        public BlikController(IBlikService blikService, IUserContextService userContext)
        {
            _blikService = blikService;
            _userContext = userContext;
        }

        [HttpGet("test")]
        public async Task<IActionResult> TestowanaMetoda()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await _blikService.StartConnection(webSocket, _userContext.GetUserId);
                return Created();
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
                await HttpContext.Response.WriteAsync("Bad request");
                return BadRequest(ModelState);
            }
        }

        [HttpGet("GenerateBlik")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task GenerateBlik() 
        {
            //var accessToken = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await _blikService.StartConnection(webSocket, _userContext.GetUserId);
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
                await HttpContext.Response.WriteAsync("Bad request");
            }
        }
        [HttpPost("BlikRequest")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> BlikRequest([FromBody]BlikRequest dto)
        {
            if(dto == null || dto.Amount < 0)
                return BadRequest(ModelState);

            string response = await _blikService
                .BlikTransfer((int)_userContext.GetUserId, dto.Code, dto.Amount);

            return Ok(response);
        }
    }
}

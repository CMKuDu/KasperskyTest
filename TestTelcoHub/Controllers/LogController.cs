using Microsoft.AspNetCore.Mvc;
using TestTelcoHub.Service.Interface;

namespace TestTelcoHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogService _logService;
        public LogController(ILogService logService)
        {
            _logService = logService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllLog()
        {
            var result = await _logService.GetAllLogs();
            return Ok(result);
        }
    }
}

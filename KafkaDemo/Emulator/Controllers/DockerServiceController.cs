using Microsoft.AspNetCore.Mvc;

namespace Emulator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DockerServiceController : ControllerBase
    {
        private readonly ILogger<DockerServiceController> _logger;
        private readonly DockerComposeService _dockerComposeService;

        public DockerServiceController(ILogger<DockerServiceController> logger, DockerComposeService dockerComposeService)
        {
            _logger = logger;
            _dockerComposeService = dockerComposeService;
        }

        [HttpPost("Start")]
        public IActionResult Start()
        {
            _dockerComposeService.Start();
            return Ok("Docker Compose services started.");
        }

        [HttpPost("Stop")]
        public async Task<IActionResult> Stop()
        {
            await _dockerComposeService.StopAsync();
            return Ok("Docker Compose services stopped.");
        }
    }
}

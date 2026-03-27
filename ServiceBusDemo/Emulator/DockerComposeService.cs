using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;

namespace Emulator;

public class DockerComposeService : IAsyncDisposable
{
    private readonly ILogger<DockerComposeService> _logger;
    private ICompositeService? _compositeService;

    public DockerComposeService(ILogger<DockerComposeService> logger)
    {
        _logger = logger;

        Start();
    }

    public async ValueTask DisposeAsync()
    {
        await StopAsync();
    }

    public void Start()
    {
        if (_compositeService == null)
        {
            _logger.LogInformation("Starting Docker Compose services...");
            var file = Path.Combine(Directory.GetCurrentDirectory(), "DockerFiles/docker-compose.yml");
            _compositeService = new Builder()
                                .UseContainer()
                                .UseCompose()
                                .FromFile(file)
                                .RemoveOrphans()
                                .Build()
                                .Start();
        }
    }

    public async Task StopAsync()
    {
        if (_compositeService != null)
        {
            _logger.LogInformation("Stopping Docker Compose services...");
            _compositeService.Dispose();
            await Task.Delay(5000);
            _compositeService = null;
        }
    }
}
namespace Emulator;

public static class IServiceProviderExtensions
{
    public static void EagerLoading(this IServiceProvider services)
    {
        services.GetRequiredService<DockerComposeService>();
    }
}

using Microsoft.Extensions.Logging;

namespace AvaloniaDemonstration.Helpers;

public static class GlobalLogger
{
    public static ILogger Logger { get; private set; } = null!;

    public static void Initialize(ILogger logger)
    {
        Logger = logger;
    }
}
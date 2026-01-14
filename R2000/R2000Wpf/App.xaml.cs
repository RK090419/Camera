using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using R2000Wpf.Hosting;

namespace R2000Wpf;

public partial class App : HostedApplication
{
    public App()
    {
        InitializeComponent();
    }
    public ILogger<App>? Logger { get; private set; }

    public void InitLogger()
    {
        if (Services is null)
        {
            throw new NullReferenceException("Services are not initialized yet");
        }
        Logger = Services.GetRequiredService<ILogger<App>>();
        Logger.LogInformation("Application is starting...");
    }
}


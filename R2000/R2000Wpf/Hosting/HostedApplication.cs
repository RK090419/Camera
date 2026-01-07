using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using R2000Wpf.Helpers;

namespace R2000Wpf.Hosting;
public abstract class HostedApplication : Application, IHostedService
{
    public IServiceProvider? Services { get; internal set; }
    public IHostApplicationLifetime? HostLifetime { get; private set; }

    private IHost? _host;
    public sealed class WpfApplicationBuilder : IHostApplicationBuilder
    {
        private readonly HostApplicationBuilder _hostBuilder;

        public IServiceCollection Services => _hostBuilder.Services;

        public ILoggingBuilder Logging => _hostBuilder.Logging;

        public IConfigurationManager Configuration => _hostBuilder.Configuration;

        IDictionary<object, object> IHostApplicationBuilder.Properties => ((IHostApplicationBuilder)_hostBuilder).Properties;

        IHostEnvironment IHostApplicationBuilder.Environment => _hostBuilder.Environment;

        IMetricsBuilder IHostApplicationBuilder.Metrics => _hostBuilder.Metrics;

        public HostedApplication Build(ServiceProviderOptions? serviceProviderOptions = null)
        {
            serviceProviderOptions ??= new ServiceProviderOptions
            {
                ValidateOnBuild = false
            };

            _hostBuilder.ConfigureContainer(new DefaultServiceProviderFactory(serviceProviderOptions));
            _hostBuilder.Services.AddHostedService((IServiceProvider x) => (HostedApplication)Application.Current);

            //add loggings
            _hostBuilder.Logging.ClearProviders();
            _hostBuilder.Logging.AddConsole();
            _hostBuilder.Logging.SetMinimumLevel(LogLevel.Debug);

            var host = _hostBuilder.Build();
            var app = (HostedApplication)Application.Current;

            app._host = host;
            app.Services = host.Services;
            app.HostLifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
            return (HostedApplication)Application.Current;
        }

        void IHostApplicationBuilder.ConfigureContainer<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory, Action<TContainerBuilder>? configure)
        {
            ((IHostApplicationBuilder)_hostBuilder)
                    .ConfigureContainer(factory, configure);
        }

        internal WpfApplicationBuilder(Func<HostApplicationBuilder>? hostBuilderFactory = null)
        {
            _hostBuilder = (hostBuilderFactory ?? new Func<HostApplicationBuilder>(Host.CreateApplicationBuilder))();
        }
    }

    public new static HostedApplication Current => (HostedApplication)Application.Current;

    public static WpfApplicationBuilder CreateBuilder(Func<HostApplicationBuilder>? hostBuilderFactory = null)
    {
        Thread.CurrentThread.TrySetApartmentState(ApartmentState.Unknown);
        Thread.CurrentThread.TrySetApartmentState(ApartmentState.STA);


        WpfApplicationBuilder wpfApplicationBuilder = new WpfApplicationBuilder(hostBuilderFactory);
        wpfApplicationBuilder.Services.AddSingleton((IServiceProvider x) => (HostedApplication)Application.Current);
        return wpfApplicationBuilder;
    }
    public void RunWithStartingWindow(Window startingWindow)
    {
        UIHelpers.EnsureUIThread();
        startingWindow.Show();
        RunHosted();
    }
    private int RunHosted()
    {
        UIHelpers.EnsureUIThread();

        Startup += async (s, e) =>
        {
            await HostMain();
        };
        Exit += (s, e) =>
        {
            if (_host is null) return;

            try
            {
                _host.StopAsync().GetAwaiter().GetResult();
            }
            finally
            {
                _host.Dispose();
            }
        };
        Application.Current.Run();
        return Environment.ExitCode;
    }
    public virtual Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    public virtual Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public void OnAppStarted(Action action)
    {
        HostLifetime?.ApplicationStarted.Register(action);
    }

    public void OnAppStopping(Action action)
    {
        HostLifetime?.ApplicationStopping.Register(action);
    }

    public void OnAppStopped(Action action)
    {
        HostLifetime?.ApplicationStopped.Register(action);
    }

    private async Task HostMain()
    {
        if (_host == null)
            throw new InvalidOperationException("Host is not initialized.");
        try
        {
            // Start all hosted services
            await _host.StartAsync();

            // Wait until host shutdown is triggered
            await _host.WaitForShutdownAsync().ConfigureAwait(false);
        }
        finally
        {
            if (_host is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync().ConfigureAwait(false);
            }
            else
            {
                _host.Dispose();
            }
        }
    }

    public void UseExceptionHandler(Func<Exception, bool, bool> handler)
    {
        if (Services == null)
        {
            throw new InvalidOperationException("UseExceptionHandler must be called after Build().");
        }
        // handler(Exception ex, bool canHandle) returns true if handled
        var logger = Services?.GetRequiredService<ILogger<HostedApplication>>();

        DispatcherUnhandledException += (s, e) =>
        {
            logger?.LogError(e.Exception, "Unhandled UI exception");

            if (handler(e.Exception, true))
            {
                e.Handled = true;
            }
        };

        AppDomain.CurrentDomain.UnhandledException += (s, e) =>
        {
            if (e.ExceptionObject is Exception ex)
            {
                logger?.LogError(ex, "Unhandled domain exception");

                _ = handler(ex, false);
            }
        };

        TaskScheduler.UnobservedTaskException += (s, e) =>
        {
            logger?.LogError(e.Exception, "Unobserved task exception");

            if (handler(e.Exception, true))
            {
                e.SetObserved();
            }
        };
    }

}

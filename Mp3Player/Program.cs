using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mp3Player.Runners;

var host = CreateHostBuilder(args).Build();
var runner = new ProgramRunner(args, host.Services.GetRequiredService<ILogger<ProgramRunner>>());
await runner.Run();
return;

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.AddEnvironmentVariables();
        })
        .ConfigureLogging((context, logging) =>
        {
            logging.ClearProviders();
            logging.AddConsole();

            var logLevel = context.Configuration.GetValue<string>("LOG_LEVEL");
            if (Enum.TryParse<LogLevel>(logLevel, out var level))
            {
                logging.SetMinimumLevel(level);
            }
            else
            {
                logging.SetMinimumLevel(LogLevel.Information); // Установите уровень по умолчанию
            }
        })
        .ConfigureServices((services) =>
        {
            services.AddLogging(configure => configure.AddConsole())
                .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
        });
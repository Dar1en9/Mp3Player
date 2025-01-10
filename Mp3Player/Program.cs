using System.Data;
using Mp3Player.DataBase;
using Mp3Player.History;
using Mp3Player.Menu.Commands;
using Mp3Player.Menu.Commands.PlayerCommands;
using Mp3Player.Menu.Commands.UserCommands;
using Mp3Player.TrackHandler;
using NetCoreAudio;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var logLevel = builder.Configuration.GetValue<string>("LOG_LEVEL");
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(Enum.TryParse<LogLevel>(logLevel, out var level) ? level : LogLevel.None);

builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddTransient<IDbConnection>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>(); 
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new NpgsqlConnection(connectionString);
});

builder.Services.AddSingleton<IDataBaseService, DataBaseService>();
builder.Services.AddSingleton<IDataBaseReader, DataBaseReader>();
builder.Services.AddSingleton<Player>();
builder.Services.AddSingleton<IHistoryManager, HistoryManager>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var path = configuration["HistorySettings:Path"] ?? Path.Combine(Environment
        .GetFolderPath(Environment.SpecialFolder.MyMusic), "MP3Player");
    var logger = sp.GetRequiredService<ILogger<HistoryManager>>();
    return new HistoryManager(path, logger);
});

builder.Services.AddSingleton<ICommand<List<Track>, string>, FindTracksCommand>();
builder.Services.AddSingleton<IUniCommand<List<Track>>, GetAllTracksCommand>();
builder.Services.AddSingleton<IUniCommand<List<Track>>, GetHistoryCommand>();
builder.Services.AddSingleton<ICommand<bool, Track>, PlayCommand>();
builder.Services.AddSingleton<IUniCommand<bool>, PauseCommand>();
builder.Services.AddSingleton<IUniCommand<bool>, ResumeCommand>();
builder.Services.AddSingleton<IUniCommand<bool>, StopCommand>();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); 
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mp3Player API V1");
    });
}

app.MapGet("/", () => "API works"); 
app.MapControllers(); 
app.Run();
/*
CreateHostBuilder(args).Build().Run(); //предложил copilot со starup.cs
return;

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
        */
/*
var builder = WebApplication.CreateBuilder(args); //стандартный weather

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
*/

/*
var host = CreateHostBuilder(args).Build(); //старый program
var runner = new ProgramRunnerController(args, host.Services.GetRequiredService<ILogger<ProgramRunnerController>>());
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

            logging.SetMinimumLevel(Enum.TryParse<LogLevel>(logLevel, out var level) ? level : LogLevel.None); //уровень по умолчанию
        })
        .ConfigureServices((hostContext, services) =>
        {
            var logLevel = hostContext.Configuration.GetValue<string>("LOG_LEVEL");
            services.AddLogging(configure => configure.AddConsole())
                .Configure<LoggerFilterOptions>(options =>
                {
                    options.MinLevel = Enum.TryParse<LogLevel>(logLevel, out var level) ? level : LogLevel.None; //уровень по умолчанию
                });
        });
        */
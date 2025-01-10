using Mp3Player.Menu;
using Mp3Player.Menu.Pages;
using Mp3Player.Runners;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
var logLevel = builder.Configuration.GetValue<string>("LOG_LEVEL");
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(Enum.TryParse<LogLevel>(logLevel, out var level) ? level : LogLevel.None);

builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddSingleton<IProgramRunner, ProgramRunnerController>();
builder.Services.AddTransient<MenuController>();

builder.Services.AddHttpClient<IMenuNavigator, MenuNavigator>();
builder.Services.AddHttpClient();
builder.Services.AddTransient<NpgsqlConnection>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>(); 
    var connectionString = configuration.GetValue<string>("DefaultConnection"); 
    return new NpgsqlConnection(connectionString);
});

builder.Services.AddSingleton(sp => new UserPages(
    sp.GetRequiredService<NpgsqlConnection>(),
    sp.GetRequiredService<IMenuNavigator>(),
    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "MP3Player"),
    sp.GetRequiredService<ILogger<UserPages>>(),
    sp.GetRequiredService<ILogger<MenuController>>()));

builder.Services.AddSingleton(sp => new AdminPages(
    sp.GetRequiredService<NpgsqlConnection>(),
    sp.GetRequiredService<HttpClient>(),
    sp.GetRequiredService<ILogger<AdminPages>>(),
    sp.GetRequiredService<ILogger<MenuController>>()));



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
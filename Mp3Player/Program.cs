using System.Data;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Mp3Player;
using Mp3Player.DataBase;
using Mp3Player.History;
using Mp3Player.Menu.Commands;
using Mp3Player.Menu.Commands.AdminCommands;
using Mp3Player.Menu.Commands.PlayerCommands;
using Mp3Player.Menu.Commands.UserCommands;
using Mp3Player.RequestHelpers;
using Mp3Player.TrackHandler;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var logLevel = builder.Configuration.GetValue<string>("LOG_LEVEL");
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(Enum.TryParse<LogLevel>(logLevel, out var level) ? level : LogLevel.None);

builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mp3Player API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Введите JWT токен как Bearer <your_token>"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            []
        }
    });
});
builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "Dar1en9", 
            ValidAudience = "Mp3PlayerUser",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("snko%4i5s_pm%otrpt8%9ga4hh$((jad+c%1dkcoo&7@aoj9nw")),
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("admin")); 
});

builder.Services.AddTransient<IDbConnection>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>(); 
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new NpgsqlConnection(connectionString);
});

builder.Services.AddSingleton<TokenGenerator>();
builder.Services.AddSingleton<IDataBaseService, DataBaseService>();
builder.Services.AddSingleton<IDataBaseReader, DataBaseReader>();
builder.Services.AddSingleton<IDataBaseWriter, DataBaseWriter>();
builder.Services.AddSingleton<IDataBaseDeleter, DataBaseDeleter>();
builder.Services.AddSingleton<IHistoryManager, HistoryManager>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var path = configuration["HistorySettings:Path"] ?? Path.Combine(Environment
        .GetFolderPath(Environment.SpecialFolder.MyMusic), "MP3Player");
    var logger = sp.GetRequiredService<ILogger<HistoryManager>>();
    return new HistoryManager(path, logger);
});

builder.Services.AddKeyedSingleton<ICommand<List<Track>, string>, FindTracksCommand>("find");
builder.Services.AddKeyedSingleton<IUniCommand<List<Track>>, GetHistoryCommand>("history");
builder.Services.AddKeyedSingleton<IUniCommand<List<Track>>, GetAllTracksCommand>("all");
builder.Services.AddKeyedSingleton<ICommand<bool, string>, DeleteTrackCommand>("delete");
builder.Services.AddKeyedSingleton<ICommand<bool, TrackCreatorDto>, AddTrackCommand>("add");
builder.Services.AddKeyedSingleton<ICommand<FileStreamResult, string>, StreamTrackCommand>("play");


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); 
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mp3Player API V1");
    });
}

app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/", () => "API works").AllowAnonymous(); 
app.MapControllers(); 
app.Run();
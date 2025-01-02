using Microsoft.Extensions.Configuration;

namespace Mp3Player;

public sealed class ConfigBuilder
{
    private static readonly IConfigurationRoot Config = new ConfigurationBuilder() 
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build();
    public static readonly ConfigBuilder AppConfigSettings = Config.GetRequiredSection("ConnectionStrings")
        .Get<ConfigBuilder>()!;
    public required string DefaultConnection { get; init; }
}
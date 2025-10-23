using System.Text.Json;
using DisBot_Helper_Bot.Config;

namespace DisBot_Helper_Bot.Services;

public class ConfigService
{
    private AppConfig Config { get; set; }

    public ConfigService()
    {
    }

    public async Task CreateConfig()
    {
        var configFile = File.Exists("config.json");
        if (!configFile)
        {
            var defaultConfig = JsonSerializer.Serialize(new AppConfig
            {
                DiscordBotToken = ""
            }, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            });
            await File.WriteAllTextAsync("config.json", defaultConfig);
        }

        var jsontext = await File.ReadAllTextAsync("config.json");
        Config = JsonSerializer.Deserialize<AppConfig>(jsontext)!;
    }

    public AppConfig Get()
    {
        return Config;
    }
}
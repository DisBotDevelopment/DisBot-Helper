using Microsoft.Extensions.Hosting;
using NetCord.Hosting.Gateway;

namespace DisBot_Helper_Bot;

public static class Startup
{
    public static async Task Start(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services
            .AddDiscordGateway();

        var host = builder.Build();

        await host.RunAsync();
    }
}
using Microsoft.Extensions.Hosting;
using NetCord;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Hosting.Services.ComponentInteractions;
using NetCord.Services.ComponentInteractions;

namespace DisBot_Helper_Bot;

public static class Startup
{
    public static async Task Start(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services
            .AddApplicationCommands()
            .AddComponentInteractions<ButtonInteraction, ButtonInteractionContext>()
            .AddComponentInteractions<StringMenuInteraction, StringMenuInteractionContext>()
            .AddComponentInteractions<UserMenuInteraction, UserMenuInteractionContext>()
            .AddComponentInteractions<RoleMenuInteraction, RoleMenuInteractionContext>()
            .AddComponentInteractions<MentionableMenuInteraction, MentionableMenuInteractionContext>()
            .AddComponentInteractions<ChannelMenuInteraction, ChannelMenuInteractionContext>()
            .AddComponentInteractions<ModalInteraction, ModalInteractionContext>()
            .AddGatewayHandlers(typeof(Program).Assembly)
            .AddDiscordGateway(options =>
            {
                options.Intents = GatewayIntents.GuildMessages | GatewayIntents.DirectMessages |
                                  GatewayIntents.MessageContent;
            });

        var host = builder.Build();

        host.AddModules(typeof(Program).Assembly);


        await host.RunAsync();
    }
}
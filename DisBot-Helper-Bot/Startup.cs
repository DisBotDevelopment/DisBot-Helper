using DisBot_Helper_Bot.Config;
using DisBot_Helper_Bot.Services;
using Microsoft.Extensions.DependencyInjection;
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

        // Services
        var configService = new ConfigService();
        await configService.CreateConfig();
        builder.Services.AddSingleton(configService);
        builder.Services.AddSingleton<SuggestionStateService>();
        builder.Services.AddSingleton<DocsService>();
        builder.Services.AddHostedService<OldThreadWatcher>();

        builder.Services
            .AddApplicationCommands()
            .AddComponentInteractions<ButtonInteraction, ButtonInteractionContext>()
            .AddComponentInteractions<StringMenuInteraction, StringMenuInteractionContext>()
            .AddComponentInteractions<UserMenuInteraction, UserMenuInteractionContext>()
            .AddComponentInteractions<RoleMenuInteraction, RoleMenuInteractionContext>()
            .AddComponentInteractions<MentionableMenuInteraction, MentionableMenuInteractionContext>()
            .AddComponentInteractions<ChannelMenuInteraction, ChannelMenuInteractionContext>()
            .AddComponentInteractions<ModalInteraction, ModalInteractionContext>()
            .AddDiscordGateway(options =>
                {
                    options.Token = configService.Get().DiscordBotToken;
                    options.Presence = new PresenceProperties(UserStatusType.Online)
                    {
                        Activities =
                        [
                            new UserActivityProperties("helping Jesper to develop DisBot", UserActivityType.Playing)
                        ],
                        StatusType = UserStatusType.Online,
                        Afk = false
                    };
                    options.Intents = GatewayIntents.All
                                      | GatewayIntents.DirectMessages
                                      | GatewayIntents.MessageContent
                                      | GatewayIntents.DirectMessageReactions
                                      | GatewayIntents.GuildMessageReactions
                                      | GatewayIntents.Guilds
                                      | GatewayIntents.GuildVoiceStates;
                }
            )
            .AddGatewayHandlers(typeof(Program).Assembly);

        var host = builder.Build();

        host.AddModules(typeof(Program).Assembly);

        await host.RunAsync();
    }
}
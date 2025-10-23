using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace DisBot_Helper_Bot.Commands;

public class CloseThreadCommand : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("close-thread", "Close a Thread",
        DefaultGuildPermissions = Permissions.MentionEveryone,
        Contexts = [InteractionContextType.Guild]
    )]
    public async Task Message()
    {
        var forumThread = await ((Context.Channel as PublicGuildThread)!).GetAsync();
        await forumThread.ModifyAsync(options => options.Locked = true);

        await Context.Interaction.SendResponseAsync(
            InteractionCallback.Message(
                "-# <:reply:1430577881205182555> **Thread has beed Closed from an Moderator!**"));
    }
}
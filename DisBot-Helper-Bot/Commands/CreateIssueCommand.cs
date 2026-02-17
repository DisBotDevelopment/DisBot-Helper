using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using DisBot_Helper_Bot.Models;
using DisBot_Helper_Bot.Services;
using DisBot_Helper_Bot.Templates;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace DisBot_Helper_Bot.Commands;

public class CreateIssueCommand : ApplicationCommandModule<ApplicationCommandContext>
{
    private readonly ConfigService ConfigService;
    private readonly RestClient RestClient;

    public CreateIssueCommand(ConfigService configService, RestClient restClient)
    {
        ConfigService = configService;
        RestClient = restClient;
    }

    [SlashCommand("create-issue", "Creates an issue on github...",
        DefaultGuildPermissions = Permissions.UseApplicationCommands,
        Contexts = [InteractionContextType.Guild]
    )]
    public async Task Message(
        [SlashCommandParameter(Name = "title", Description = "Issue title", MaxLength = 100)]
        string? title = null,
        [SlashCommandParameter(Name = "description", Description = "Issue description")]
        string? description = null)
    {
        if (Context.Channel is GuildThread)
        {
            var thread = Context.Channel as GuildThread;

            if (
                thread != null &&
                ConfigService.Get().SuggestionHelper.SuggestionChannelId != thread.ParentId &&
                ConfigService.Get().SuggestionHelper.SuggestionChannelId != thread.ParentId)
            {
                await Context.Interaction.SendResponseAsync(
                    InteractionCallback.Message(Messages.ErrorMessage));
                return;
            }

            if (thread.OwnerId == Context.User.Id)
            {
                var threadMessages = await thread.GetMessagesAsync().ToArrayAsync();
                var data = new GitHubApiIssueModel(
                    thread.Name,
                    threadMessages[0].Content,
                    ["xyzjesper"]
                );

                await CreateIssueAsync(data);
            }
            else
            {
                await Context.Interaction.SendResponseAsync(
                    InteractionCallback.Message(new InteractionMessageProperties()
                    {
                        Flags = MessageFlags.Ephemeral,
                        Content = "-# You are not the owner of the Thread"
                    }));
            }
        }
        else
        {
            var restGuild = await Context.Guild!.GetAsync();
            var user = await restGuild.GetUserAsync(Context.User.Id);
            var permissions = user.GetPermissions(restGuild);

            if ((permissions & Permissions.ManageGuild) != 0)
            {
                if (title == null && description == null)
                {
                    await Context.Interaction.SendResponseAsync(
                        InteractionCallback.Message(new InteractionMessageProperties()
                        {
                            Flags = MessageFlags.Ephemeral,
                            Content = "-# You need to set a title and description..."
                        }));
                }
                else
                {
                    var data = new GitHubApiIssueModel(
                        title!,
                        description!,
                        ["xyzjesper"]
                    );

                    await CreateIssueAsync(data);
                }
            }
            else
            {
                await Context.Interaction.SendResponseAsync(
                    InteractionCallback.Message(new InteractionMessageProperties()
                    {
                        Flags = MessageFlags.Ephemeral,
                        Content = $"-# Only Moderators can use this command here..."
                    }));
            }
        }
    }

    private async Task CreateIssueAsync(GitHubApiIssueModel data)
    {
        try
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization",
                $"Bearer {ConfigService.Get().GitHubAuthToken}");
            httpClient.DefaultRequestHeaders.Add("User-Agent",
                "DisBot Helper - Add Issue...");

            var issue = await httpClient.PostAsJsonAsync(
                "https://api.github.com/repos/DisBotDevelopment/DisBot-Bot/issues",
                data
            );

            await Context.Interaction.SendResponseAsync(
                InteractionCallback.Message(new InteractionMessageProperties()
                {
                    Flags = MessageFlags.Ephemeral,
                    Content =
                        $"-# Created DisBot issue on github...\n Info: {issue.StatusCode}"
                }));
        }
        catch (Exception e)
        {
            await Context.Interaction.SendResponseAsync(
                InteractionCallback.Message(new InteractionMessageProperties()
                {
                    Flags = MessageFlags.Ephemeral,
                    Content = $"-# Failed to create an issue with error: {e.Message}"
                }));
        }
    }
}
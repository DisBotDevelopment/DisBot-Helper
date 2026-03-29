using DisBot_Helper_Bot.Enums;
using DisBot_Helper_Bot.Services;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace DisBot_Helper_Bot.Commands;

public class StatusCommand : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("status", "Status Update Message",
        DefaultGuildPermissions = Permissions.MentionEveryone,
        Contexts = [InteractionContextType.Guild]
    )]
    public async Task Message()
    {
        await Context.Interaction.SendResponseAsync(InteractionCallback.Modal(
                new ModalProperties("openStatusIncidence", "Create Incidence")
                {
                    new LabelProperties("Select your type for the status incidence",
                        new StringMenuProperties("type", [
                                new StringMenuSelectOptionProperties(nameof(StatusType.New),
                                    nameof(StatusType.New))
                                {
                                    Emoji = EmojiProperties.Custom(1430128828801487000)
                                },
                                new StringMenuSelectOptionProperties(nameof(StatusType.Resolved),
                                    nameof(StatusType.Resolved))
                                {
                                    Emoji = EmojiProperties.Custom(1430128825865474111)
                                },
                                new StringMenuSelectOptionProperties(nameof(StatusType.Update),
                                    nameof(StatusType.Update))
                                {
                                    Emoji = EmojiProperties.Custom(1430647258990379008)
                                }
                            ]
                        )
                    ),

                    new LabelProperties("Write a message for the incidence",
                        new TextInputProperties("message", TextInputStyle.Paragraph)
                    ),

                    new LabelProperties("Title for the thread (New)",
                        new TextInputProperties("title", TextInputStyle.Short)
                        {
                            Required = false,
                        }
                    ),

                    new LabelProperties("Select the thread (Update/Resolved)",
                        new ChannelMenuProperties("thread")
                        {
                            Required = false,
                            ChannelTypes = [ChannelType.PublicGuildThread, ChannelType.AnnouncementGuildThread]
                        }
                    )
                }
            )
        );
    }
}
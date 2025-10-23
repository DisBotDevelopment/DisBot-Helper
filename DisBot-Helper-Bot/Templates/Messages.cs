using NetCord;
using NetCord.Rest;

namespace DisBot_Helper_Bot.Templates;

public class Messages
{
    public static readonly InteractionMessageProperties ErrorMessage = new InteractionMessageProperties()
    {
        Flags = MessageFlags.Ephemeral,
        Content = "-# <:reply:1430577881205182555> Sorry, you can't use this command here!"
    };
}
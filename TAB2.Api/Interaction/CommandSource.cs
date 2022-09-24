using Discord.WebSocket;

namespace TAB2.Api.Interaction;

public class CommandSource
{
    public SocketMessage Message { get; }

    public CommandSource(SocketMessage message)
    {
        Message = message;
    }
}
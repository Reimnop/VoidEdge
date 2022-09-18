using Discord.WebSocket;

namespace TAB2.Api.Command;

public class CommandSource
{
    public SocketMessage Message { get; }

    public CommandSource(SocketMessage message)
    {
        Message = message;
    }
}
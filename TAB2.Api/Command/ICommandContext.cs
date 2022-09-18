using Discord;
using Discord.WebSocket;

namespace TAB2.Api.Command;

public interface ICommandContext
{
    SocketUser User { get; }
    ISocketMessageChannel Channel { get; }

    bool GetArgument<T>(string name, out T? value);
    Task DeferAsync();
    Task RespondAsync(string message = "", Embed? embed = null);
}
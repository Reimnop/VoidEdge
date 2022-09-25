using Discord;
using Discord.WebSocket;

namespace TAB2.Api.Interaction;

public interface ICommandContext
{
    SocketUser User { get; }
    ISocketMessageChannel Channel { get; }

    bool GetArgument<T>(string name, out T? value);
    object? GetArgument(string name);
    Task DeferAsync();
    Task RespondAsync(string message = "", Embed? embed = null);
    MultiStringHash GetCommandNameHash();
}
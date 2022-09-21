using Discord;
using Discord.WebSocket;
using TAB2.Api.Command;

namespace TAB2.Command;

public class SlashCommandContext : ICommandContext
{
    public SocketUser User { get; }
    public ISocketMessageChannel Channel { get; }
    
    private readonly SocketSlashCommand slashCommand;
    private readonly Dictionary<string, object> arguments;

    private bool deferred = false;

    public SlashCommandContext(SocketSlashCommand slashCommand)
    {
        this.slashCommand = slashCommand;
        User = slashCommand.User;
        Channel = slashCommand.Channel;
        arguments = slashCommand.Data.Options.ToDictionary(x => x.Name, x => x.Value);
    }

    public bool GetArgument<T>(string name, out T? value)
    {
        if (arguments.TryGetValue(name, out object? uncastedValue))
        {
            if (uncastedValue is T castedValue)
            {
                value = castedValue;
                return true;
            }
        }

        value = default;
        return false;
    }

    public object? GetArgument(string name)
    {
        if (arguments.TryGetValue(name, out object? value))
        {
            return value;
        }

        return null;
    }

    public async Task DeferAsync()
    {
        deferred = true;
        await slashCommand.DeferAsync();
    }

    public async Task RespondAsync(string message, Embed? embed)
    {
        Embed[]? embeds = embed == null ? null : new[] {embed};
        
        if (!deferred) 
        {
            await slashCommand.RespondAsync(message, embeds);
        }
        else
        {
            await slashCommand.FollowupAsync(message, embeds);
        }
    }
}
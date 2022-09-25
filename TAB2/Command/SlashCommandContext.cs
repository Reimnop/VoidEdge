using Discord;
using Discord.WebSocket;
using TAB2.Api.Interaction;

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
        arguments = slashCommand.Data.Options
            .Flatten(x => x.Options)
            .Where(x => x.Type != ApplicationCommandOptionType.SubCommand && x.Type != ApplicationCommandOptionType.SubCommandGroup)
            .ToDictionary(x => x.Name, x => x.Value);
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

    public MultiStringHash GetCommandNameHash()
    {
        List<string> subCommandNames = new List<string>();
        subCommandNames.Add(slashCommand.CommandName);

        SocketSlashCommandDataOption? second = slashCommand
            .Data.Options
            .FirstOrDefault(x => 
                x.Type == ApplicationCommandOptionType.SubCommandGroup || 
                x.Type == ApplicationCommandOptionType.SubCommand);
        
        SocketSlashCommandDataOption? third = second
            ?.Options
            .FirstOrDefault(x => 
                x.Type == ApplicationCommandOptionType.SubCommand);

        if (second != null)
        {
            subCommandNames.Add(second.Name);
        }
        
        if (third != null)
        {
            subCommandNames.Add(third.Name);
        }

        return new MultiStringHash(subCommandNames.ToArray());
    }
}
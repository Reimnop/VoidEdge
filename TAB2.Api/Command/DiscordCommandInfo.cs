namespace TAB2.Api.Command;

public delegate Task CommandExecutesTaskDelegate(ICommandContext commandContext);

public class DiscordCommandInfo
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<ArgumentInfo> Arguments { get; } = new List<ArgumentInfo>();
    public CommandExecutesTaskDelegate? ExecutesTaskDelegate { get; set; }

    public DiscordCommandInfo WithName(string name)
    {
        Name = name;
        return this;
    }

    public DiscordCommandInfo WithDescription(string description)
    {
        Description = description;
        return this;
    }

    public DiscordCommandInfo AddArgument(ArgumentInfo argumentInfo)
    {
        Arguments.Add(argumentInfo);
        return this;
    }

    public DiscordCommandInfo Executes(CommandExecutesTaskDelegate executesTaskDelegate)
    {
        ExecutesTaskDelegate = executesTaskDelegate;
        return this;
    }
}
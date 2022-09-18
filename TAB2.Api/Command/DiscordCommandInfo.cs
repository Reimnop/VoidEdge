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

    public DiscordCommandInfo AddIntArgument(string name, string description, bool isRequired = true)
    {
        Arguments.Add(new IntArgumentInfo(name, description, isRequired));
        return this;
    }
    
    public DiscordCommandInfo AddDoubleArgument(string name, string description, bool isRequired = true)
    {
        Arguments.Add(new DoubleArgumentInfo(name, description, isRequired));
        return this;
    }
    
    public DiscordCommandInfo AddStringArgument(string name, string description, bool isRequired = true)
    {
        Arguments.Add(new StringArgumentInfo(name, description, isRequired));
        return this;
    }
    
    public DiscordCommandInfo AddUserArgument(string name, string description, bool isRequired = true)
    {
        Arguments.Add(new UserArgumentInfo(name, description, isRequired));
        return this;
    }

    public DiscordCommandInfo AddEnumArgument(string name, string description, Action<EnumArgumentInfo> addFunc, bool isRequired = true)
    {
        EnumArgumentInfo argumentInfo = new EnumArgumentInfo(name, description, isRequired);
        addFunc(argumentInfo);
        Arguments.Add(argumentInfo);
        return this;
    }

    public DiscordCommandInfo Executes(CommandExecutesTaskDelegate executesTaskDelegate)
    {
        ExecutesTaskDelegate = executesTaskDelegate;
        return this;
    }
}
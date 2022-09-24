namespace TAB2.Api.Interaction;

public class SubCommand
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<Argument> Arguments { get; } = new List<Argument>();
    public CommandExecutesTaskDelegate? ExecutesTaskDelegate { get; set; }

    public SubCommand WithName(string name)
    {
        Name = name;
        return this;
    }

    public SubCommand WithDescription(string description)
    {
        Description = description;
        return this;
    }

    public SubCommand AddArgument(Argument argument)
    {
        Arguments.Add(argument);
        return this;
    }

    public SubCommand Executes(CommandExecutesTaskDelegate executesTaskDelegate)
    {
        ExecutesTaskDelegate = executesTaskDelegate;
        return this;
    }
}
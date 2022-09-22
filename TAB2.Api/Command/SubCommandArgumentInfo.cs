namespace TAB2.Api.Command;

public class SubCommandArgumentInfo : ArgumentInfo
{
    public List<ArgumentInfo> Arguments { get; } = new List<ArgumentInfo>();
    public CommandExecutesTaskDelegate? ExecutesTaskDelegate { get; set; }
    
    public SubCommandArgumentInfo(string name, string description, bool isRequired = true) : base(name, description, isRequired)
    {
    }
    
    public SubCommandArgumentInfo AddArgument(ArgumentInfo argumentInfo)
    {
        Arguments.Add(argumentInfo);
        return this;
    }

    public SubCommandArgumentInfo Executes(CommandExecutesTaskDelegate executesTaskDelegate)
    {
        ExecutesTaskDelegate = executesTaskDelegate;
        return this;
    }
}
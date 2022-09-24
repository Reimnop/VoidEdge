namespace TAB2.Api.Interaction;

public class StringArgument : Argument
{
    public StringArgument(string name, string description, bool isRequired = true) : base(name, description, isRequired)
    {
    }
}
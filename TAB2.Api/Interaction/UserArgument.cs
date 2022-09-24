namespace TAB2.Api.Interaction;

public class UserArgument : Argument
{
    public UserArgument(string name, string description, bool isRequired = true) : base(name, description, isRequired)
    {
    }
}
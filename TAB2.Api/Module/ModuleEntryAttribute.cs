namespace TAB2.Api.Module;

[AttributeUsage(AttributeTargets.Class)]
public class ModuleEntryAttribute : Attribute
{
    public string Name { get; }
    public string Id { get; }
    public string Version { get; }

    public ModuleEntryAttribute(string name, string id, string version)
    {
        Name = name;
        Id = id;
        Version = version;
    }
}
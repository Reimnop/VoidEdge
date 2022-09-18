using TAB2.Api.Module;

namespace TAB2.Module;

public class Module
{
    public BaseModule BaseModule { get; set; }
    public ModuleEntryAttribute Attribute { get; set; }

    public Module(BaseModule baseModule, ModuleEntryAttribute attribute)
    {
        BaseModule = baseModule;
        Attribute = attribute;
    }
}
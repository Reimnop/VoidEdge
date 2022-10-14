using System.Reflection;
using TAB2.Api.Module;

namespace TAB2.Module;

public class ModuleEntry
{
    public BaseModule BaseModule { get; }
    public ModuleEntryAttribute Attribute { get; }

    public ModuleEntry(BaseModule baseModule, ModuleEntryAttribute attribute)
    {
        BaseModule = baseModule;
        Attribute = attribute;
    }
}

public static class ModuleLoader
{
    public static IEnumerable<ModuleEntry> GetModuleEntries(Assembly assembly)
    {
        foreach (Type type in assembly.GetTypes())
        {
            if (type.IsInterface || type.IsAbstract || !typeof(BaseModule).IsAssignableFrom(type) || type.GetCustomAttribute(typeof(ModuleEntryAttribute)) != null)
            {
                continue;
            }

            BaseModule? baseModule = (BaseModule?) Activator.CreateInstance(type);
            ModuleEntryAttribute? attribute = (ModuleEntryAttribute?) Attribute.GetCustomAttribute(type, typeof(ModuleEntryAttribute));

            if (baseModule == null || attribute == null)
            {
                continue;
            }
            
            yield return new ModuleEntry(baseModule, attribute);
        }
    }
}
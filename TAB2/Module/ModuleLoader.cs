using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Mono.Cecil;
using TAB2.Api.Module;

#pragma warning disable CS8600
#pragma warning disable CS8762

namespace TAB2.Module;

public static class ModuleLoader
{
    public static bool TryLoadModule(
        string path,
        [MaybeNullWhen(false)] out BaseModule baseModule, 
        [MaybeNullWhen(false)] out ModuleEntryAttribute attribute)
    {
        baseModule = null;
        attribute = null;
        
        // Check if assembly is valid without loading
        int entryCount = 0;
        
        ModuleDefinition moduleDefinition = ModuleDefinition.ReadModule(path);
        foreach (TypeDefinition typeDefinition in moduleDefinition.Types)
        {
            if (!typeDefinition.HasCustomAttributes)
            {
                continue;
            }

            if (TryGetCustomAttribute(typeDefinition, typeof(ModuleEntryAttribute), out _))
            {
                entryCount++;
            }
        }

        if (entryCount != 1)
        {
            return false;
        }
        
        // Loads the assembly
        Assembly assembly = Assembly.LoadFile(path);
        
        // Guaranteed to be non-null
        Type entryType = assembly
            .GetTypes()
            .First(x => !x.IsInterface && !x.IsAbstract && typeof(BaseModule).IsAssignableFrom(x) && x.GetCustomAttribute(typeof(ModuleEntryAttribute)) != null);

        baseModule = (BaseModule) Activator.CreateInstance(entryType);
        attribute = (ModuleEntryAttribute) Attribute.GetCustomAttribute(entryType, typeof(ModuleEntryAttribute));
        
        return true;
    }
    
    private static bool TryGetCustomAttribute(TypeDefinition type, Type attributeType, [MaybeNullWhen(false)] out CustomAttribute result)
    {
        result = null;
        
        if (!type.HasCustomAttributes)
        {
            return false;
        }

        foreach (CustomAttribute attribute in type.CustomAttributes) 
        {
            if (attribute.AttributeType.FullName != attributeType.FullName)
            {
                continue;
            }

            result = attribute;
            return true;
        }

        return false;
    }
}
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Loader;
using Mono.Cecil;
using TAB2.Api.Module;

#pragma warning disable CS8600
#pragma warning disable CS8762

namespace TAB2.Module;

public static class ModuleLoader
{
    public static bool TryLoadModule(string path, [MaybeNullWhen(false)] out Assembly assembly)
    {
        assembly = null;
        
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
        assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
        return true;
    }

    public static void ModuleToAttributes(
        Assembly assembly,
        out BaseModule baseModule,
        out ModuleEntryAttribute attribute)
    {
        Type entryType = assembly
            .GetTypes()
            .First(x => !x.IsInterface && !x.IsAbstract && typeof(BaseModule).IsAssignableFrom(x) && x.GetCustomAttribute(typeof(ModuleEntryAttribute)) != null);

        baseModule = (BaseModule) Activator.CreateInstance(entryType);
        attribute = (ModuleEntryAttribute) Attribute.GetCustomAttribute(entryType, typeof(ModuleEntryAttribute));
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
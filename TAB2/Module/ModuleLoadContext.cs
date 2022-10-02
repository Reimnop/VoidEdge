using System.Reflection;
using System.Runtime.Loader;

namespace TAB2.Module;

public class ModuleLoadContext : AssemblyLoadContext
{
    private AssemblyDependencyResolver resolver;

    public ModuleLoadContext(string modulePath)
    {
        resolver = new AssemblyDependencyResolver(modulePath);
    }
    
    protected override Assembly? Load(AssemblyName assemblyName)
    {
        string? assemblyPath = resolver.ResolveAssemblyToPath(assemblyName);
        if (assemblyPath != null)
        {
            return LoadFromAssemblyPath(assemblyPath);
        }

        return null;
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        string? libraryPath = resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        if (libraryPath != null)
        {
            return LoadUnmanagedDllFromPath(libraryPath);
        }

        return IntPtr.Zero;
    }
}
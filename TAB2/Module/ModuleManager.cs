using System.Reflection;
using System.Text;
using log4net;
using TAB2.Api;
using TAB2.Api.Module;

namespace TAB2.Module;

public delegate void ModuleRunDelegate(Module module);
public delegate Task ModuleTaskDelegate(Module module);

public class ModuleManager
{
    private readonly ILog log;
    private readonly Dictionary<string, int> moduleIndices;
    private readonly List<Module> loadedModules;

    private readonly TAB2Loader loader;

    public ModuleManager(TAB2Loader loader)
    {
        this.loader = loader;
        
        log = LogManager.GetLogger("ModuleManager");
        moduleIndices = new Dictionary<string, int>();
        loadedModules = new List<Module>();
    }

    public void LoadModules(string directory, IBotInstance instance)
    {
        List<Module> modules = new List<Module>();
        
        DirectoryInfo directoryInfo = new DirectoryInfo(directory);

        if (!directoryInfo.Exists)
        {
            return;
        }
        
        IEnumerable<FileInfo> assemblyFiles = directoryInfo
            .EnumerateFiles()
            .Where(x => x.Extension.Equals(".dll", StringComparison.OrdinalIgnoreCase));

        List<Assembly> assemblies = new List<Assembly>();

        foreach (FileInfo assemblyFile in assemblyFiles)
        {
            if (ModuleLoader.TryLoadModule(assemblyFile.FullName, out Assembly? assembly))
            {
                assemblies.Add(assembly);
            }
            else
            {
                log.Warn($"Could not load module from file '{assemblyFile.FullName}'! (Invalid entrypoint)");
            }
        }

        foreach (Assembly assembly in assemblies)
        {
            ModuleLoader.ModuleToAttributes(assembly, out BaseModule entryPoint, out ModuleEntryAttribute attribute);
            modules.Add(new Module(entryPoint, attribute));
        }
        
        LogDiscoveredModules(modules);

        foreach (Module module in modules)
        {
            module.BaseModule.Initialize(instance);
            
            moduleIndices.Add(module.Attribute.Id, loadedModules.Count);
            loadedModules.Add(module);
        }
    }

    private void LogDiscoveredModules(ICollection<Module> modules)
    {
        StringBuilder text = new StringBuilder($"Discovered {modules.Count} module(s)\n");
        foreach (Module module in modules)
        {
            text.Append($"    - {module.Attribute.Name} (id: '{module.Attribute.Id}', version: {module.Attribute.Version})\n");
        }
        log.Info(text.ToString());
    }

    public void RunOnAllModules(ModuleRunDelegate moduleRunDelegate)
    {
        foreach (Module module in loadedModules)
        {
            moduleRunDelegate(module);
        }
    }
    
    // Hack
    public Task RunOnAllModulesAsync(ModuleTaskDelegate moduleTaskDelegate)
    {
        foreach (Module module in loadedModules)
        {
            loader.TaskScheduler.Run(() => moduleTaskDelegate(module));
        }
        
        return Task.CompletedTask;
    }

    public bool TryRunOnModule(string id, ModuleRunDelegate moduleRunDelegate)
    {
        if (!moduleIndices.TryGetValue(id, out int index))
        {
            return false;
        }

        moduleRunDelegate(loadedModules[index]);
        return true;
    }
    
    public Task<bool> TryRunOnModuleAsync(string id, ModuleTaskDelegate moduleTaskDelegate)
    {
        if (!moduleIndices.TryGetValue(id, out int index))
        {
            return Task.FromResult(false);
        }

        moduleTaskDelegate(loadedModules[index]);
        return Task.FromResult(true);
    }
}
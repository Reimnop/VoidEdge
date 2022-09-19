using System.IO;
using Cake.Common;
using Cake.Common.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Build;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Frosting;

public static class Program
{
    public static int Main(string[] args)
    {
        return new CakeHost()
            .UseContext<BuildContext>()
            .Run(args);
    }
}

public class BuildContext : FrostingContext
{
    public string MsBuildConfiguration { get; set; }
    
    public BuildContext(ICakeContext context)
        : base(context)
    {
        MsBuildConfiguration = context.Argument("configuration", "Debug");
    }
}

[TaskName("Clean Backend")]
public class CleanBackendTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Log.Information("Cleaning backend");
        context.CleanDirectory($"../TAB2/bin/{context.MsBuildConfiguration}");
    }
}

[TaskName("Build")]
public class BuildTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.DotNetBuild("../VoidEdge.sln", new DotNetBuildSettings
        {
            Configuration = context.MsBuildConfiguration
        });
    }
}

[TaskName("Copy Modules")]
public class CopyModulesTask : FrostingTask<BuildContext>
{
    private readonly string[] modules =
    {
        "VoidEdgeMain",
        "AntiPing"
    };
    
    public override void Run(BuildContext context)
    {
        string modulesDir = $"../TAB2/bin/{context.MsBuildConfiguration}/net6.0/Modules";
        
        Directory.CreateDirectory(modulesDir);
        
        foreach (string module in modules)
        {
            File.Copy(
                $"../{module}/bin/{context.MsBuildConfiguration}/net6.0/{module}.dll", 
                Path.Combine(modulesDir, $"{module}.dll"), true);
        }
    }
}

[TaskName("Default")]
[IsDependentOn(typeof(BuildTask))]
[IsDependentOn(typeof(CopyModulesTask))]
public class DefaultTask : FrostingTask<BuildContext>
{
}
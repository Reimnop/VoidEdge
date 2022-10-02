using System.Runtime.CompilerServices;
using log4net;
using log4net.Config;
using TAB2;

const string LogConfigFile = "log4net_cfg.xml";

XmlConfigurator.Configure(new FileInfo(LogConfigFile));

ILog log = LogManager.GetLogger("Entry");

string? token = args.Length > 0 ? args[0] : null;

if (token == null)
{
    log.Fatal("No bot token!");
    return;
}

#if !DEBUG
try
{
#endif
    log.Info("Starting TAB2 Loader");
    
    using TAB2Loader main = new TAB2Loader();
    Task runTask = main.Run(token);
    TaskAwaiter awaiter = runTask.GetAwaiter();
    awaiter.GetResult();
#if !DEBUG
}
catch (Exception e)
{
    log.Fatal("Crash!!", e);
}
#endif
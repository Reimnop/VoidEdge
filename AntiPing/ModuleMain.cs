using TAB2.Api;
using TAB2.Api.Command;
using TAB2.Api.Module;

namespace AntiPing;

[ModuleEntry(Name, Id, Version)]
public class ModuleMain : BaseModule
{
    public const string Name = "Anti Ping";
    public const string Id = "antiping";
    public const string Version = "1.0.0";

    private IBotInstance instance;
    
    public override void Initialize(IBotInstance instance)
    {
        this.instance = instance;
    }

    public override IEnumerator<DiscordCommandInfo> OnCommandRegister()
    {
        throw new NotImplementedException();
    }
}
using Cyotek.Data.Nbt;
using TAB2.Api.Data;

namespace AntiPing;

public class Config : IPersistentData
{
    private Dictionary<ulong, GuildConfig> guildConfigs = new Dictionary<ulong, GuildConfig>();

    public GuildConfig GetGuildConfig(ulong guildId)
    {
        if (guildConfigs.TryGetValue(guildId, out GuildConfig? config))
        {
            return config;
        }

        GuildConfig guildConfig = new GuildConfig();
        guildConfigs.Add(guildId, guildConfig);

        return guildConfig;
    }

    public void SetGuildConfig(ulong guildId, GuildConfig config)
    {
        guildConfigs[guildId] = config;
    }
    
    public void WriteData(TagCompound compound)
    {
        foreach (var (id, config) in guildConfigs)
        {
            TagCompound guildCompound = new TagCompound();
            config.WriteData(compound);

            compound.Value.Add(id.ToString(), guildCompound);
        }
    }

    public void ReadData(TagCompound compound)
    {
        guildConfigs.Clear();
        
        foreach (Tag tag in compound.Value)
        {
            string name = tag.Name;
            TagCompound guildCompound = (TagCompound) tag.GetValue();
            ulong id = ulong.Parse(name);
            GuildConfig config = new GuildConfig();
            config.ReadData(guildCompound);
            guildConfigs.Add(id, config);
        }
    }
}
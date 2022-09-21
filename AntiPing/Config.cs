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
            TagDictionary dictionary = new TagDictionary();
            config.WriteData(dictionary);

            compound.Value.Add(id.ToString(), dictionary);
        }
    }

    public void ReadData(TagCompound compound)
    {
        guildConfigs.Clear();
        
        foreach (Tag tag in compound.Value)
        {
            ulong id = ulong.Parse(tag.Name);
            TagDictionary dictionary = (TagDictionary) tag.GetValue();
            GuildConfig config = new GuildConfig();
            config.ReadData(dictionary);
            
            guildConfigs.Add(id, config);
        }
    }
}
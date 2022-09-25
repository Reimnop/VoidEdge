using Cyotek.Data.Nbt;
using TAB2.Api.Data;

namespace AntiPing;

public class StrikeData : IPersistentData
{
    private readonly Dictionary<ulong, GuildStrikeData> strikeDatas = new Dictionary<ulong, GuildStrikeData>();
    
    public GuildStrikeData GetGuildData(ulong guildId)
    {
        if (strikeDatas.TryGetValue(guildId, out GuildStrikeData? data))
        {
            return data;
        }

        GuildStrikeData guildData = new GuildStrikeData();
        strikeDatas.Add(guildId, guildData);

        return guildData;
    }

    public void SetGuildData(ulong guildId, GuildStrikeData data)
    {
        strikeDatas[guildId] = data;
    }

    public void WriteData(TagCompound compound)
    {
        foreach (var (id, data) in strikeDatas)
        {
            TagDictionary dictionary = new TagDictionary();
            data.WriteData(dictionary);

            compound.Value.Add(id.ToString(), dictionary);
        }
    }

    public void ReadData(TagCompound compound)
    {
        strikeDatas.Clear();
        
        foreach (Tag tag in compound.Value)
        {
            ulong id = ulong.Parse(tag.Name);
            TagDictionary dictionary = (TagDictionary) tag.GetValue();
            GuildStrikeData data = new GuildStrikeData();
            data.ReadData(dictionary);
            
            strikeDatas.Add(id, data);
        }
    }
}
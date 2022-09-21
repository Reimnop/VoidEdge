using Cyotek.Data.Nbt;
using TAB2.Api.Data;

namespace AntiPing;

public class GuildConfig
{
    public HashSet<ulong> PingRoleIds { get; } = new HashSet<ulong>();

    public void AddRoleId(ulong roleId)
    {
        if (!PingRoleIds.Contains(roleId))
        {
            PingRoleIds.Add(roleId);
        }
    }
    
    public void RemoveRoleId(ulong roleId)
    {
        if (PingRoleIds.Contains(roleId))
        {
            PingRoleIds.Remove(roleId);
        }
    }
    
    public void WriteData(TagDictionary dictionary)
    {
        string[] ids = PingRoleIds.Select(x => x.ToString()).ToArray();
        dictionary.Add("role_ids", ids);
    }

    public void ReadData(TagDictionary dictionary)
    {
        PingRoleIds.Clear();
        
        TagCollection tags = (TagCollection) dictionary["role_ids"].GetValue();
        foreach (Tag tag in tags)
        {
            PingRoleIds.Add(ulong.Parse((string) tag.GetValue()));
        }
    }
}
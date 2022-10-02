using Cyotek.Data.Nbt;
using TAB2.Api.Data;

namespace AntiPing;

public class GuildConfig
{
    private readonly HashSet<ulong> pingRoleIds = new HashSet<ulong>();

    public IReadOnlyCollection<ulong> GetPingRoles()
    {
        return pingRoleIds;
    }

    public bool ContainsRole(ulong roleId)
    {
        return pingRoleIds.Contains(roleId);
    }
    
    public void AddRoleId(ulong roleId)
    {
        if (!pingRoleIds.Contains(roleId))
        {
            pingRoleIds.Add(roleId);
        }
    }
    
    public void RemoveRoleId(ulong roleId)
    {
        if (pingRoleIds.Contains(roleId))
        {
            pingRoleIds.Remove(roleId);
        }
    }
    
    public void WriteData(TagDictionary dictionary)
    {
        string[] ids = pingRoleIds.Select(x => x.ToString()).ToArray();
        dictionary.Add("role_ids", ids);
    }

    public void ReadData(TagDictionary dictionary)
    {
        pingRoleIds.Clear();
        
        TagCollection tags = (TagCollection) dictionary["role_ids"].GetValue();
        foreach (Tag tag in tags)
        {
            pingRoleIds.Add(ulong.Parse((string) tag.GetValue()));
        }
    }
}
using Cyotek.Data.Nbt;
using TAB2.Api.Data;

namespace AntiPing;

public class GuildConfig : IPersistentData
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
    
    public void WriteData(TagCompound compound)
    {
        long[] ids = PingRoleIds.Select(x => unchecked((long) x)).ToArray();
        compound.Value.Add("role_ids", ids);
    }

    public void ReadData(TagCompound compound)
    {
        PingRoleIds.Clear();
        
        long[] ids = (long[]) compound["role_ids"].GetValue();
        foreach (long id in ids)
        {
            PingRoleIds.Add(unchecked((ulong) id));
        }
    }
}
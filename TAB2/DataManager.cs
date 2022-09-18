using Cyotek.Data.Nbt;
using TAB2.Api;
using TAB2.Api.Data;

namespace TAB2;

public class DataManager : IDataManager
{
    private readonly Dictionary<string, IPersistentData> datas = new Dictionary<string, IPersistentData>();

    public void RegisterData(string id, IPersistentData data)
    {
        datas.Add(id, data);

        string path = DataPath(id);
        if (File.Exists(path))
        {
            using FileStream stream = File.OpenRead(path);
            NbtDocument document = new NbtDocument();
            document.Load(stream);
            data.ReadData(document.DocumentRoot);
        }
    }

    public IPersistentData GetData(string id)
    {
        return datas[id];
    }

    public void SaveData(string id)
    {
        IPersistentData data = datas[id];

        Directory.CreateDirectory("Data");
        using FileStream stream = File.OpenWrite(DataPath(id));
        NbtDocument document = new NbtDocument();
        data.WriteData(document.DocumentRoot);
        document.Save(stream);
    }

    private string DataPath(string id)
    {
        return Path.Combine("Data", $"{id}.dat");
    }
}
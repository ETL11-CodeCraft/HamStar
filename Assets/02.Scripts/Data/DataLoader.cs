using System.IO;
using UnityEngine;

public class DataLoader
{
    public T Load<T>() where T : new()
    {
        T data = default;
        string dataPath = Application.persistentDataPath + typeof(T) + ".json";
        if (File.Exists(dataPath))
        {
            string loadData = File.ReadAllText(dataPath);
            data = JsonUtility.FromJson<T>(loadData);
        }
        else
        {
            data = new T();
            Save(data);
        }

        return data;
    }

    public void Save<T>(T data)
    {
        string dataPath = Application.persistentDataPath + typeof(T) + ".json";

        File.WriteAllText(dataPath, JsonUtility.ToJson(data));
    }
}

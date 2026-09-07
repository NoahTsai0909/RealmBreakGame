using UnityEngine;
using System.IO;

public static class SaveManager
{
    private static string GetFilePath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }

    public static void Save<T>(string fileName, T data)
    {
        string path = GetFilePath(fileName);

        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(path, json);

        Debug.Log($"[SaveManager] Successfully saved to {path}");
    }

    public static T Load<T>(string fileName) where T : new()
    {
        string path = GetFilePath(fileName);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            T loadedData = JsonUtility.FromJson<T>(json);

            Debug.Log($"[SaveManager] Successfully loaded from {path}");
            return loadedData;
        }
        else
        {
            Debug.LogWarning($"[SaveManager] Save file not found at {path}. Creating a new one.");
            return new T();
        }
    }
}

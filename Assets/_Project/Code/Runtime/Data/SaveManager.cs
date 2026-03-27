using System.IO;
using UnityEngine;

public static class SaveManager
{
    // persistentDataPath is perfect because it saves to a safe OS-level app data folder 
    // (e.g., AppData/LocalLow on Windows) meaning it survives app updates and doesn't need admin rights.
    private static string SavePath => Path.Combine(Application.persistentDataPath, "TaskManager_saveData.json");

    public static void Save(AppData data)
    {
        // Convert the data object into a JSON string. The "true" parameter makes it pretty-print 
        // (adds line breaks and indents) so it's readable if opened in Notepad.
        string json = JsonUtility.ToJson(data, true);
        
        File.WriteAllText(SavePath, json);
        Debug.Log($"[SaveManager] Data saved successfully to: {SavePath}");
    }

    public static AppData Load()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            AppData loadedData = JsonUtility.FromJson<AppData>(json);
            
            Debug.Log("[SaveManager] Save data loaded successfully.");
            return loadedData;
        }
        
        // If no save file exists (first time opening the app), return a fresh, empty AppData object
        Debug.Log("[SaveManager] No save file found. Creating fresh save data.");
        return new AppData();
    }
}
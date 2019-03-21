using UnityEngine;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public static class SaveManager
{
    private static readonly string path = Path.Combine(Application.persistentDataPath, "savegame.bin");
    private static BinaryFormatter formatter = new BinaryFormatter();

    public static void Save()
    {
        var data = new SaveData();

        using (var stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream, data);
        }

        Debug.Log("Data has been saved.");
    }

    public static SaveData Load()
    {
        if (File.Exists(path))
        {
            SaveData data = null;

            using (var stream = new FileStream(path, FileMode.Open))
            {
                data = formatter.Deserialize(stream) as SaveData;
            }

            Debug.Log("Data has been loaded.");
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

}


[System.Serializable]
public class SaveData
{
    public int souls;
    public int runSouls;
    public bool blacksmith;
    public bool alchemist;
    public bool inTavern;

    public SaveData()
    {
        souls = GameController.instance.lobbySouls;
        runSouls = GameController.instance.runSouls;
        blacksmith = GameController.instance.m_RescuedBlacksmith;
        alchemist = GameController.instance.m_RescuedAlchemist;

        // TODO: When tavern is a separate scene, change this to read inTavern of GameController.
        inTavern = false;
    }
}

#if UNITY_EDITOR
public class SaveTool
{
    [MenuItem("Window/Save Current")]
    public static void Save()
    {
        try
        {
            SaveManager.Save();
        }
        catch (Exception)
        {
            Debug.LogError("You cannot save the current state in edit-mode.");
        }
    }
}
#endif
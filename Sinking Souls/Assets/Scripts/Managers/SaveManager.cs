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
            SaveData data;

            using (var stream = new FileStream(path, FileMode.Open))
            {
                data = formatter.Deserialize(stream) as SaveData;
            }

            Debug.Log("Data has been loaded.");
            return data;
        }

        Debug.LogError("Save file not found in " + path);
        return null;
    }

    public static bool CheckFile()
    {
        return File.Exists(path);
    }

    public static void Reset()
    {
        File.Delete(path);
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
    public float maxHealth;

    public int upgradeCounts;
    public bool[] modifiersOwned;
    public bool[] abilitiesOwned;
    public bool[] conditionsTriggered;

    public int equippedModifier;
    public int equippedAbility;

    public bool visitedTavern;

    public SaveData()
    {
        souls = GameController.instance.lobbySouls;
        runSouls = GameController.instance.runSouls;
        blacksmith = GameController.instance.m_RescuedBlacksmith;
        alchemist = GameController.instance.m_RescuedAlchemist;
        maxHealth = GameController.instance.player.GetComponent<Player>().MaxHealth;
        upgradeCounts = GameController.instance.upgradeCounts;

        visitedTavern = GameController.instance.visitedTavern;

        var modifiers = GameController.instance.modifiers;
        modifiersOwned = new bool[modifiers.Length];
        for (int i = 0; i < modifiers.Length; i++)
        {
            modifiersOwned[i] = modifiers[i].owned;
        }


        var abilities = GameController.instance.abilities;
        abilitiesOwned = new bool[abilities.Length];
        for (int i = 0; i < abilities.Length; i++)
        {
            abilitiesOwned[i] = abilities[i].owned;
        }

        equippedModifier = -1; // If the player as no modifier equipped.
        var player = GameController.instance.player.GetComponent<Player>();
        for (int i = 0; i < modifiers.Length; i++)
        {
            if (modifiers[i] == player.Weapon.modifier)
            {
                equippedModifier = i;
                break;
            }
        }

        for (int i = 0; i < abilities.Length; i++)
        {
            if (abilities[i] == player.Abilities[0])
            {
                equippedAbility = i;
                break;
            }
        }

        var conditions = GameController.instance.conditions;
        conditionsTriggered = new bool[conditions.Length];
        for (int i = 0; i < conditions.Length; i++)
        {
            conditionsTriggered[i] = conditions[i].completed;
        }

        inTavern = false;
    }
}

#if UNITY_EDITOR
public class SaveTool
{
    [MenuItem("Window/Save Manager/Save current")]
    public static void Save()
    {
        try
        {
            SaveManager.Save();
        }
        catch (Exception exception)
        {
            Debug.LogError(exception.Message);
        }
    }

    [MenuItem("Window/Save Manager/Delete current")]
    public static void Delete()
    {
        try
        {
            SaveManager.Reset();
        }
        catch (Exception)
        {
            Debug.LogError("You cannot save the current state in edit-mode.");
        }
    }
}
#endif
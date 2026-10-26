using System;
using System.IO;
using UnityEngine;

// Static access point for all save data. Nothing else in the game
// needs to know the data is stored as JSON.
public static class SaveManager
{
    private const string FileName = "save.json";
    private static SaveData data;

    private static string FilePath => Path.Combine(Application.persistentDataPath, FileName);

    // Loads the file the first time anything asks for data
    private static SaveData Data
    {
        get
        {
            if (data == null) Load();
            return data;
        }
    }

    public static void Load()
    {
        data = null;
        try
        {
            if (File.Exists(FilePath))
                data = JsonUtility.FromJson<SaveData>(File.ReadAllText(FilePath));
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Could not read save file, starting fresh. {e.Message}");
        }

        if (data == null) data = new SaveData();
        if (data.unlockedLevel < 1) data.unlockedLevel = 1;
    }

    public static void Save()
    {
        try
        {
            // Write to a temp file first so a crash mid-write can't corrupt the real save
            string tempPath = FilePath + ".tmp";
            File.WriteAllText(tempPath, JsonUtility.ToJson(Data, true));
            if (File.Exists(FilePath)) File.Delete(FilePath);
            File.Move(tempPath, FilePath);
        }
        catch (Exception e)
        {
            Debug.LogError($"Could not save game: {e.Message}");
        }
    }

    // ---------- Levels ----------

    public static int UnlockedLevel => Data.unlockedLevel;

    public static bool IsUnlocked(int level) => level <= Data.unlockedLevel;

    // Returns null if the level has never been completed
    public static LevelRecord GetRecord(int level)
    {
        foreach (var r in Data.levels)
            if (r.level == level) return r;
        return null;
    }

    private static LevelRecord GetOrCreateRecord(int level)
    {
        var r = GetRecord(level);
        if (r == null)
        {
            r = new LevelRecord { level = level };
            Data.levels.Add(r);
        }
        return r;
    }

    // Marks a level complete, unlocks the next one, and keeps the best results.
    // Returns true if this run set a new best time.
    public static bool CompleteLevel(int level, float time, int coins)
    {
        var r = GetOrCreateRecord(level);
        r.completed = true;

        bool newBestTime = r.bestTime < 0f || time < r.bestTime;
        if (newBestTime) r.bestTime = time;
        if (coins > r.bestCoins) r.bestCoins = coins;

        // Only ever moves forward, so replaying an old level can't re-lock anything
        if (level + 1 > Data.unlockedLevel) Data.unlockedLevel = level + 1;

        Save();
        return newBestTime;
    }

    public static int TotalBestCoins
    {
        get
        {
            int total = 0;
            foreach (var r in Data.levels) total += r.bestCoins;
            return total;
        }
    }

    public static void ResetProgress()
    {
        data = new SaveData();
        Save();
    }

    public static void UnlockAll(int levelCount)
    {
        Data.unlockedLevel = Mathf.Max(Data.unlockedLevel, levelCount);
        Save();
    }

    // ---------- Editor-only shortcuts (Tools menu) ----------
#if UNITY_EDITOR
    [UnityEditor.MenuItem("Tools/Save/Reset Progress")]
    private static void EditorReset()
    {
        ResetProgress();
        Debug.Log("Save progress reset.");
    }

    [UnityEditor.MenuItem("Tools/Save/Unlock All Levels")]
    private static void EditorUnlockAll()
    {
        UnlockAll(LevelLoader.LevelCount);
        Debug.Log("All levels unlocked.");
    }

    [UnityEditor.MenuItem("Tools/Save/Open Save Folder")]
    private static void EditorOpenFolder()
    {
        UnityEditor.EditorUtility.RevealInFinder(Application.persistentDataPath);
    }
#endif
}

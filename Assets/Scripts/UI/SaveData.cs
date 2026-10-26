using System;
using System.Collections.Generic;

// Everything that gets written to the save file.
// JsonUtility can't save Dictionaries, so per-level data is a List.

[Serializable]
public class LevelRecord
{
    public int level;
    public bool completed;
    public float bestTime = -1f;   // -1 = no time recorded yet
    public int bestCoins;          // most coins collected in one run of this level
}

[Serializable]
public class SaveData
{
    public int version = 1;
    public int unlockedLevel = 1;  // highest level the player can play
    public List<LevelRecord> levels = new List<LevelRecord>();
}
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

// Loads scenes by name. Level scenes must be named Level1, Level2, Level3...
// and added to Build Profiles. Menu scenes can sit at any index.
public static class LevelLoader
{
    public const string LevelPrefix = "Level_";
    public const string LevelSelectScene = "LevelSelect";
    public const string MainMenuScene = "MainMenu";

    // Counts the "LevelN" scenes in Build Profiles, so adding a level
    // is just creating the scene and adding it to the build.
    public static int LevelCount
    {
        get
        {
            int count = 0;
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string name = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
                if (TryParseLevel(name, out _)) count++;
            }
            return count;
        }
    }

    // "Level7" -> 7. "LevelSelect" and other scenes return false.
    public static bool TryParseLevel(string sceneName, out int level)
    {
        level = 0;
        return sceneName.StartsWith(LevelPrefix)
            && int.TryParse(sceneName.Substring(LevelPrefix.Length), out level);
    }

    // 0 if the active scene isn't a level
    public static int CurrentLevel =>
        TryParseLevel(SceneManager.GetActiveScene().name, out int level) ? level : 0;

    public static bool HasNextLevel => CurrentLevel > 0 && CurrentLevel < LevelCount;

    public static void LoadLevel(int level)
    {
        Time.timeScale = 1f;   // in case the Level Complete panel paused the game
        SceneManager.LoadScene(LevelPrefix + level);
    }

    public static void LoadNextLevel()
    {
        if (HasNextLevel) LoadLevel(CurrentLevel + 1);
        else LoadLevelSelect();   // finished the last level
    }

    public static void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void LoadLevelSelect()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(LevelSelectScene);
    }

    public static void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MainMenuScene);
    }

    // For a "Continue" button: jumps to the furthest unlocked level
    public static void ContinueGame()
    {
        LoadLevel(Mathf.Clamp(SaveManager.UnlockedLevel, 1, LevelCount));
    }
}
using UnityEngine;

// Put in the LevelSelect scene. Builds one button per level automatically.
public class LevelSelectMenu : MonoBehaviour
{
    [SerializeField] private LevelButton buttonPrefab;
    [SerializeField] private Transform container;   // the object with the Grid Layout Group

    private void Start()
    {
        // Remove any placeholder buttons left in the scene while designing
        foreach (Transform child in container)
            Destroy(child.gameObject);

        int count = LevelLoader.LevelCount;
        for (int level = 1; level <= count; level++)
        {
            LevelButton button = Instantiate(buttonPrefab, container);
            button.Setup(level, SaveManager.IsUnlocked(level), SaveManager.GetRecord(level));
        }
    }

    // Hook to a Back button if you have one
    public void OnBack() => LevelLoader.LoadMainMenu();
}
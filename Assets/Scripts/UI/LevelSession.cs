using TMPro;
using UnityEngine;

// One per level scene. Tracks coins and time for the current run,
// updates the HUD, and saves results when the level is completed.
public class LevelSession : MonoBehaviour
{
    public static LevelSession Instance { get; private set; }

    [SerializeField] private LevelCompletePanel completePanel;

    [Header("HUD (optional)")]
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text timerText;

    public int Coins { get; private set; }
    public float ElapsedTime { get; private set; }
    public bool IsFinished { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void Start()
    {
        UpdateHud();
    }

    private void Update()
    {
        if (IsFinished) return;

        // Uses deltaTime, so the timer also stops while Time.timeScale is 0 (pause menu)
        ElapsedTime += Time.deltaTime;
        if (timerText) timerText.text = FormatTime(ElapsedTime);
    }

    // Call from your coin script when the player collects one
    public void AddCoin(int amount = 1)
    {
        if (IsFinished) return;
        Coins += amount;
        UpdateHud();
    }

    // Called by LevelGoal when the player reaches the end
    public void CompleteLevel()
    {
        if (IsFinished) return;
        IsFinished = true;

        int level = LevelLoader.CurrentLevel;
        bool newBest = SaveManager.CompleteLevel(level, ElapsedTime, Coins);
        float bestTime = SaveManager.GetRecord(level).bestTime;

        completePanel.Show(ElapsedTime, bestTime, newBest, Coins);
    }

    private void UpdateHud()
    {
        if (coinText) coinText.text = Coins.ToString();
        if (timerText) timerText.text = FormatTime(ElapsedTime);
    }

    // 83.456 -> "01:23.46"
    public static string FormatTime(float seconds)
    {
        int minutes = (int)(seconds / 60f);
        float rest = seconds % 60f;
        return $"{minutes:00}:{rest:00.00}";
    }
}
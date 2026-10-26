using TMPro;
using UnityEngine;

// Put this on the Canvas (or any always-active parent), NOT on the panel itself,
// and drag your existing Level Complete panel into the "panel" field.
public class LevelCompletePanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    [Header("Optional fields")]
    [SerializeField] private GameObject nextButton;     // hidden after the last level
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text bestTimeText;
    [SerializeField] private GameObject newBestLabel;   // e.g. "New Best!" text
    [SerializeField] private TMP_Text coinsText;

    [SerializeField] private bool pauseGame = true;

    private void Awake()
    {
        panel.SetActive(false);
    }

    public void Show(float time, float bestTime, bool newBest, int coins)
    {
        panel.SetActive(true);

        if (timeText) timeText.text = "Time: " + LevelSession.FormatTime(time);
        if (bestTimeText) bestTimeText.text = "Best: " + LevelSession.FormatTime(bestTime);
        if (newBestLabel) newBestLabel.SetActive(newBest);
        if (coinsText) coinsText.text = coins.ToString();
        if (nextButton) nextButton.SetActive(LevelLoader.HasNextLevel);

        if (pauseGame) Time.timeScale = 0f;   // LevelLoader resets this on scene load
    }

    // Hook these up in each button's On Click () list
    public void OnNextLevel() => LevelLoader.LoadNextLevel();
    public void OnRetry() => LevelLoader.RestartLevel();
    public void OnLevelSelect() => LevelLoader.LoadLevelSelect();
}
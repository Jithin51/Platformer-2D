using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Put on the level button prefab.
[RequireComponent(typeof(Button))]
public class LevelButton : MonoBehaviour
{
    [SerializeField] private TMP_Text numberText;
    [SerializeField] private GameObject lockIcon;

    [Header("Optional")]
    [SerializeField] private GameObject completedIcon;   // tick or star shown on finished levels
    [SerializeField] private TMP_Text bestTimeText;

    public void Setup(int level, bool unlocked, LevelRecord record)
    {
        var button = GetComponent<Button>();
        button.interactable = unlocked;
        button.onClick.AddListener(() => LevelLoader.LoadLevel(level));

        numberText.text = level.ToString();
        numberText.gameObject.SetActive(unlocked);
        if (lockIcon) lockIcon.SetActive(!unlocked);

        bool completed = record != null && record.completed;
        if (completedIcon) completedIcon.SetActive(completed);

        if (bestTimeText)
        {
            bestTimeText.gameObject.SetActive(completed);
            if (completed) bestTimeText.text = LevelSession.FormatTime(record.bestTime);
        }
    }
}
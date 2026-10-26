using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.Instance.PlayMusic("MainMusic", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

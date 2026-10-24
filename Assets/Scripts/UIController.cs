using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public void onRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void onNextLevel()
    {
        int next = SceneManager.GetActiveScene().buildIndex + 1;

        if (next < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(next);
        else
            OnAllLevelsComplete();
    }
    private void OnAllLevelsComplete()
    {
        // show a "You win" screen, load credits, go back to the menu, etc.
        SceneManager.LoadScene(0);
    }
}

using UnityEngine;

public class DoorController : MonoBehaviour
{

    public GameObject levelCompletePanel;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFX("Level");
            LevelSession.Instance.CompleteLevel();
            //levelCompletePanel.SetActive(true);
            Destroy(collision.gameObject);
        }
    }
}

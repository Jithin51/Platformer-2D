using UnityEngine;

public class Trap : MonoBehaviour
{

    [SerializeField]protected GameObject gameOverPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFX("Hurt");
            collision.GetComponent<PlayerController>().TakeDamage();
            gameOverPanel.SetActive(true);
            Destroy(collision.gameObject);
        }
    }
}

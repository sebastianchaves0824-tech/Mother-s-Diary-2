using UnityEngine;

public class WinZone : MonoBehaviour
{
    [SerializeField] private GameObject victoryCanvas;

    private void Awake()
    {
        if (victoryCanvas != null)
        {
            victoryCanvas.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Win();
        }
    }

    private void Win()
    {
        if (victoryCanvas != null)
        {
            victoryCanvas.SetActive(true);

            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}

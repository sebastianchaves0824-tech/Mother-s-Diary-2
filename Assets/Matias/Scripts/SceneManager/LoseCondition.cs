using UnityEngine;

public class LoseCondition : MonoBehaviour
{
    [SerializeField] private Cordura cordura;
    [SerializeField] private GameObject LoseCanvas;

    private bool isDead = false;

    private void Awake()
    {
        if (LoseCanvas != null)
        {
            LoseCanvas.SetActive(false);
        }
    }

    private void Update()
    {
        if (cordura != null && cordura.currentSanity <= 0 && !isDead)
        {
            Lose();
        }
    }

    private void Lose()
    {
        isDead = true;
        if (LoseCanvas != null)
        {
            LoseCanvas.SetActive(true);

            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}

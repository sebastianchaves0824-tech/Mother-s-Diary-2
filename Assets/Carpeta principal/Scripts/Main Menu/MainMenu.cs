using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsMenu;
    public GameObject mainMenu;
    public GameObject viewControllerWindow;
    public GameObject insanityBar;
    public GameObject diary;

    private bool gamePaused = false;

    void Update()
    {
        // No pausar si el diario está abierto
        if (diary != null && diary.activeInHierarchy) return;

        // Detectar la tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Si el viewer controller está abierto, volver a opciones
            if (viewControllerWindow != null && viewControllerWindow.activeInHierarchy)
            {
                viewControllerWindow.SetActive(false);
                optionsMenu.SetActive(true);
            }
            else if (gamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        // Mostrar el menu y detener el juego
        optionsMenu.SetActive(true);
        if (insanityBar != null) insanityBar.SetActive(false);

        Time.timeScale = 0f;
        gamePaused = true;

        // Mostrar el cursor y desbloquear una vez abierto el menu
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        if (mainMenu != null) mainMenu.SetActive(false);
        if (optionsMenu != null) optionsMenu.SetActive(false);
        if (insanityBar != null) insanityBar.SetActive(true);

        Time.timeScale = 1f;
        gamePaused = false;

        // Ocultar el cursor una vez cerrado el menu
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OpenOptionsPanel()
    {
        mainMenu.SetActive(false);
        viewControllerWindow.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void OpenMainMenuPanel()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    public void OpenViewController()
    {
        mainMenu.SetActive(false);
        viewControllerWindow.SetActive(true);
    }
}

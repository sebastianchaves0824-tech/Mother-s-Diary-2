using UnityEngine;

public class PauseManager : MonoBehaviour
{    
    public GameObject OptionsMenu;
    public GameObject MainMenu;    
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
            if (gamePaused)
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
        OptionsMenu.SetActive(true);
        insanityBar.SetActive(false);

        Time.timeScale = 0f;
        gamePaused = true;

        // Mostrar el cursor y desbloquear una vez abierto el menu
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        if (MainMenu != null) MainMenu.SetActive(false);
    if (OptionsMenu != null) OptionsMenu.SetActive(false);  
    if (insanityBar != null) insanityBar.SetActive(true);   
        
        Time.timeScale = 1f;
        gamePaused = false;

        // Ocultar el cursor una vez cerrado el menu
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    
}
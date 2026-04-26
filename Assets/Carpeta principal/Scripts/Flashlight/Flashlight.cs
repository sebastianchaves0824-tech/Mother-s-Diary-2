using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private Camera mainCamera;
    private Light flashlightLight;

    void Start()
    {
        // Obtener la cámara principal
        mainCamera = Camera.main;
        
        // Obtener el componente Light del flashlight
        flashlightLight = GetComponent<Light>();
        
        if (mainCamera == null)
            Debug.LogError("No se encontró la cámara principal");
        if (flashlightLight == null)
            Debug.LogError("No se encontró el componente Light en el flashlight");
    }

    void Update()
    {
        if (mainCamera == null)
            return;

        // Hacer que la linterna apunte en la dirección de la cámara (hacia el crosshair)
        transform.rotation = mainCamera.transform.rotation;
    }
}

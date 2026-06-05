using UnityEngine;
using UnityEngine.InputSystem; // Aseguramos que use el nuevo Input System

public class Flashlight : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private Light flashlightLight;

    void Start()
    {
        // Obtener la cámara principal
        mainCamera = Camera.main;

        // Obtener el componente Light del flashlight
        flashlightLight = GetComponentInChildren<Light>();

        if (mainCamera == null)
            Debug.LogError("No se encontró la cámara principal");
        if (flashlightLight == null)
            Debug.LogError("No se encontró el componente Light en el flashlight");
    }

    void Update()
    {
        if (mainCamera == null || flashlightLight == null)
            return;

        // Hacer que la linterna apunte en la dirección de la cámara (hacia el crosshair)
        transform.rotation = mainCamera.transform.rotation;

        // DETECTAR LA TECLA E PARA PRENDER / APAGAR
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            // Invierte el estado actual de la luz (si está encendida la apaga, y viceversa)
            flashlightLight.enabled = !flashlightLight.enabled;
        }
    }
}
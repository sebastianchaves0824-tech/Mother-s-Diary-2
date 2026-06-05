using UnityEngine;
using UnityEngine.InputSystem; // Aseguramos que use el nuevo Input System

public class Flashlight : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private Light flashlightLight;

    // Valores normales
    [SerializeField] private float rangoNormal = 10f;
    [SerializeField] private float intensidadNormal = 2f;
    [SerializeField] private float outerSpotNormal = 30f;

    // Valores al presionar click izquierdo
    [SerializeField] private float rangoAumentado = 20f;
    [SerializeField] private float intensidadAumentada = 5f;
    [SerializeField] private float outerSpotAumentado = 60f;
 
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

        if (flashlightLight != null)
        {
            flashlightLight.range = rangoNormal;
            flashlightLight.intensity = intensidadNormal;
            flashlightLight.spotAngle = outerSpotNormal;
        }
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

        // Si se mantiene presionado el click izquierdo, aumentamos el alcance e intensidad
        if (Input.GetMouseButton(0))
        {
            flashlightLight.range = rangoAumentado;
            flashlightLight.intensity = intensidadAumentada;
            flashlightLight.spotAngle = outerSpotAumentado;
        }
        else
        {
            flashlightLight.range = rangoNormal;
            flashlightLight.intensity = intensidadNormal;
            flashlightLight.spotAngle = outerSpotNormal;
        }
    }
}
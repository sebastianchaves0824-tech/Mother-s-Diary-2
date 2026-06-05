using UnityEngine;

public class FlashlightInteraction : MonoBehaviour
{
    [Header("References")]
    // Referencia a tu script original por si necesitas acceder a sus variables en el futuro
    public Flashlight flashlightScript;
    private Camera mainCamera;

    [Header("Interaction Settings")]
    public float interactRange = 10f; // Distancia máxima para iluminar las rocas

    void Start()
    {
        mainCamera = Camera.main;

        // Si no asignaste el script en el inspector, lo busca automáticamente en este mismo objeto
        if (flashlightScript == null)
        {
            flashlightScript = GetComponent<Flashlight>();
        }

        if (mainCamera == null)
            Debug.LogError("FlashlightInteractor: No se encontró la cámara principal");
    }

    void Update()
    {
        if (mainCamera == null)
            return;

        // Disparamos un rayo desde el centro de la cámara hacia donde estás mirando
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;

        // Si el rayo choca con algo dentro de nuestro rango
        if (Physics.Raycast(ray, out hit, interactRange))
        {
            // Intentamos ver si el objeto golpeado es un LightCube
            LightCube targetCube = hit.collider.GetComponent<LightCube>();

            // Si efectivamente es una de las rocas del puzzle, la iluminamos
            if (targetCube != null)
            {
                targetCube.Illuminate();
            }
        }
    }
}

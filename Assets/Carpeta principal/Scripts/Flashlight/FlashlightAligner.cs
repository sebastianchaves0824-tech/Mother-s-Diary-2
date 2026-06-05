using UnityEngine;

public class FlashlightAligner : MonoBehaviour
{
    private Camera mainCamera;
    
    [Header("Configuración")]
    [Tooltip("Distancia máxima a la que la linterna intentará corregir su enfoque")]
    public float maxFocusDistance = 20f;
    
    [Tooltip("Velocidad de ajuste de la luz (valores altos = instantáneo)")]
    public float alignmentSpeed = 15f;

    private void Start()
    {
        // Buscamos la cámara principal del jugador
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (mainCamera == null) return;

        // Creamos un rayo desde el centro exacto de la pantalla (la mira)
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        Vector3 targetPoint;

        // Si el jugador está mirando una pared o estructura, apuntamos ahí
        if (Physics.Raycast(ray, out hit, maxFocusDistance))
        {
            targetPoint = hit.point;
        }
        else
        {
            // Si mira al vacío infinito, proyectamos un punto hacia adelante
            targetPoint = ray.GetPoint(maxFocusDistance);
        }

        // Calculamos la rotación necesaria para que la linterna mire a ese punto objetivo
        Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
        
        // Suavizamos el movimiento para que no se vea robótico o rígido
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, alignmentSpeed * Time.deltaTime);
    }
}
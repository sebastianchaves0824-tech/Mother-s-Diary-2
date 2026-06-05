using UnityEngine;

public class SecretDoor : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [Tooltip("Hacia dónde se moverá la puerta al abrirse (relativo a sí misma)")]
    public Vector3 openOffset = new Vector3(3f, 0f, 0f); // Se desliza 3 unidades a la derecha por defecto
    public float speed = 2f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool shouldOpen = false;

    private void Start()
    {
        // Guardamos la posición inicial como la posición cerrada
        closedPosition = transform.position;
        // Calculamos la posición final sumando el offset
        openPosition = closedPosition + transform.TransformDirection(openOffset);
    }

    private void Update()
    {
        // Si se activó la apertura, movemos la puerta suavemente hacia la posición abierta
        if (shouldOpen)
        {
            transform.position = Vector3.MoveTowards(transform.position, openPosition, speed * Time.deltaTime);
        }
    }

    // Este método será llamado por el libro interactuable
    public void OpenDoor()
    {
        shouldOpen = true;
        Debug.Log("La puerta secreta se está abriendo...");
    }
}
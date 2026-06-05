using UnityEngine;

public class NewDoor : MonoBehaviour, IInteractable
{
    [Header("Ajustes de Seguridad")]
    [Tooltip("¿Esta puerta requiere una llave para abrirse?")]
    [SerializeField] private bool needsKey = true;

    [Tooltip("Escribí el nombre exacto de la llave que abre ESTA puerta.")]
    [SerializeField] private string requiredKeyID = "Llave_Sotano";

    private bool isOpen = false;
    private Quaternion closedRotation; // Para poder cerrarla

    void Start()
    {
        // Guardamos la rotación del PADRE (el pivote), no la del hijo
        if (transform.parent != null)
        {
            closedRotation = transform.parent.rotation;
        }
        else
        {
            // Fallback por si alguna puerta en tu juego no tiene pivote
            closedRotation = transform.rotation;
        }
    }

    public void Interact(PlayerInteraction player)
    {
        // Si necesita llave y no tenés:
        if (needsKey && !player.HasKey(requiredKeyID))
        {
            return;
        }

        // Si la puerta no necesita llave (o ya la abriste antes)
        ToggleDoor();
    }

    private void ToggleDoor()
    {
        // Determinamos qué objeto vamos a rotar (el padre si existe, si no, el hijo)
        Transform targetToRotate = transform.parent != null ? transform.parent : transform;

        if (!isOpen)
        {
            // Abrir: Rotamos el pivote
            targetToRotate.Rotate(0, -90, 0);
            isOpen = true;
        }
        else
        {
            // Cerrar: El pivote vuelve a la rotación original guardada
            targetToRotate.rotation = closedRotation;
            isOpen = false;
        }
    }
}

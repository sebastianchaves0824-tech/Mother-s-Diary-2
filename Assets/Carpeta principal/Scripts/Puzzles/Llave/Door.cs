using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [Header("Ajustes de Seguridad")]
    [Tooltip("¿Esta puerta requiere una llave para abrirse?")]
    [SerializeField] private bool needsKey = true;

    [Tooltip("Escribí el nombre exacto de la llave que abre ESTA puerta.")]
    [SerializeField] private string requiredKeyID = "Llave_Sotano";

    private bool isOpen = false;
    private Quaternion closedRotation; //para poder cerralra

    void Start()
    {
        //Guardas la rotacion
        closedRotation = transform.rotation;
    }

    public void Interact(PlayerInteraction player)
    {
        // Si necesita llave y no tenes:
        if (needsKey && !player.HasKey(requiredKeyID))
        {
            return;
        }

        // Si la puerta no necesita llave (o ya la abriste antes)
        ToggleDoor();
    }

    private void ToggleDoor()
    {
        if (!isOpen)
        {
            //abrir
            transform.Rotate(0, 90, 0);
            isOpen = true;
        }
        else
        {
            //Cerrar (se vuelve al original)
            transform.rotation = closedRotation;
            isOpen = false;
        }
    }
}
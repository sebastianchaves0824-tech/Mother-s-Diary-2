using UnityEngine;

public class BookTrigger : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Arrastra aquí la pared/puerta secreta")]
    public SecretDoor secretDoor;

    [Header("Animación del Libro (Opcional)")]
    [Tooltip("El libro se moverá un poco hacia adentro para simular que fue presionado")]
    public Vector3 pressedOffset = new Vector3(0f, 0f, -0.1f);
    public float animationSpeed = 5f;

    private Vector3 targetPosition;
    private bool wasInteracted = false;

    private void Start()
    {
        targetPosition = transform.localPosition;
    }

    private void Update()
    {
        if (wasInteracted)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, animationSpeed * Time.deltaTime);
        }
    }

    // Este es el método que debes llamar desde tu sistema de interacción actual
    public void Interact()
    {
        if (wasInteracted) return; // Evita que se active múltiples veces

        wasInteracted = true;
        
        // Animamos el libro moviéndolo hacia su posición presionada
        targetPosition = transform.localPosition + pressedOffset;

        // Le avisamos a la puerta que se abra
        if (secretDoor != null)
        {
            secretDoor.OpenDoor();
        }
        else
        {
            Debug.LogWarning($"¡El libro {gameObject.name} no tiene una puerta asignada en el Inspector!");
        }
    }
}
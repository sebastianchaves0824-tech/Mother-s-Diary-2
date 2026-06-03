using UnityEngine;

public class ShatteredGlassPriority : MonoBehaviour
{
    [Header("Configuración del Enemigo")]
    [Tooltip("La etiqueta (Tag) que tiene asignada el GameObject del enemigo.")]
    [SerializeField] private string enemyTag = "Enemy";

    [Header("Ajustes del Vidrio")]
    [Tooltip("¿Se puede pisar SOLO una vez o es reutilizable?")]
    [SerializeField] private bool triggerOnce = true;

    private bool hasBeenSteppedOn = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (triggerOnce && hasBeenSteppedOn) return;

            AlertEnemy();
        }
    }

    private void AlertEnemy()
    {
        // Busca al enemigo por su tag
        GameObject enemy = GameObject.FindWithTag(enemyTag);

        if (enemy != null)
        {
            MotherMovement motherMovement = enemy.GetComponent<MotherMovement>();
            MotherGlassReaction motherReaction = enemy.GetComponent<MotherGlassReaction>();

            if (motherMovement != null && motherReaction != null)
            {
                // Prioridad: se distrae nomás si no está viendo al jugador
                if (motherMovement.raycast != null && !motherMovement.raycast.detectado)
                {
                    hasBeenSteppedOn = true;

                    // Le avisa a la madre que escuche el ruido y le pasa su posición
                    motherReaction.InvestigateNoise(transform.position);

                    // Poner efectos de sonido acá
                }
            }
        }
    }
}
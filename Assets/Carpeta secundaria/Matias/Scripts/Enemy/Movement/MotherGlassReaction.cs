using UnityEngine;
using UnityEngine.AI;

public class MotherGlassReaction : MonoBehaviour
{
    [Header("Ajustes de Distracción")]
    [Tooltip("Velocidad a la que va a ir la madre a investigar")]
    [SerializeField] private float alertSpeed = 6f;

    private NavMeshAgent motherAgent;
    private float originalSpeed;
    private bool isMonitoringSpeed = false;

    private void Awake()
    {
        // obtiene el NavMeshAgent automáticamente de este mismo objeto
        motherAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (isMonitoringSpeed && motherAgent != null)
        {
            // Comprueba si ya llego al destino
            if (!motherAgent.pathPending && motherAgent.remainingDistance <= motherAgent.stoppingDistance)
            {
                // Si llego, vuelve a los valores normales
                motherAgent.speed = originalSpeed;
                isMonitoringSpeed = false;
            }
        }
    }

    public void InvestigateNoise(Vector3 targetPosition)
    {
        if (motherAgent != null)
        {
            // Guarda la velocidad
            originalSpeed = motherAgent.speed;

            // Aumenta la velocidad y le pone el destino a la fuerza
            motherAgent.speed = alertSpeed;
            motherAgent.SetDestination(targetPosition);

            // El update empieza a vigilar
            isMonitoringSpeed = true;
        }
    }
}

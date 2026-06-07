using UnityEngine;
using UnityEngine.AI;

public class MotherGlassReaction : MonoBehaviour
{
    [Header("Ajustes de Distracci�n")]
    [Tooltip("Velocidad a la que va a ir la madre a investigar")]
    [SerializeField] private float alertSpeed = 6f;

    private NavMeshAgent motherAgent;
    private float originalSpeed;
    private bool isMonitoringSpeed = false;

    private void Awake()
    {
        // obtiene el NavMeshAgent autom�ticamente de este mismo objeto
        motherAgent = GetComponent<NavMeshAgent>();
        originalSpeed = motherAgent.speed;
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

            // Aumenta la velocidad y le pone el destino a la fuerza
            motherAgent.speed = alertSpeed;
            motherAgent.SetDestination(targetPosition);

            // El update empieza a vigilar
            isMonitoringSpeed = true;
        }
    }
}

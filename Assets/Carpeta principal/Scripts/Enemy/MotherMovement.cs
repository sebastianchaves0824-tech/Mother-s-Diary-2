using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MotherMovement : MonoBehaviour
{
    [SerializeField] private List<Transform> destinos;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private float velocidadMovimiento = 3.5f;
    public OtherMotherRaycast raycast;

    private int ultimoDestinoIndex = -1;
    private bool estabaDetectando = false;

    void Start()
    {
        agent.speed = velocidadMovimiento;
        ElegirNuevoDestino();
    }

    void Update()
    {
        agent.speed = velocidadMovimiento;
        if (animator != null)
        {
            animator.speed = Mathf.Clamp(velocidadMovimiento / 3.5f, 0.1f, 3f);
        }

        if (raycast.detectado)
        {
            agent.SetDestination(raycast.jugadorTransform.position);
        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    Debug.Log("Elegir nuevo destino");
                    ElegirNuevoDestino();
                }
            }
            // Si acaba de perder la detección, vuelve a la ruta
            else if (agent.hasPath && agent.velocity.sqrMagnitude > 0f)
            {
                // Verificar si el destino actual era la posición del jugador
                // Si es así, elegir un nuevo destino de la ruta
                if (Vector3.Distance(transform.position, agent.destination) > 5f)
                {
                    // Destino muy lejano, probablemente era la posición del jugador
                    ElegirNuevoDestino();
                }
            }
        }

        ActualizarAnimator();
    }

    private void ActualizarAnimator()
    {
        Vector3 localVel = transform.InverseTransformDirection(agent.velocity);
        animator.SetFloat("VelX", localVel.x);
        animator.SetFloat("VelY", localVel.z);
    }

    private void ElegirNuevoDestino()
    {
        int nuevoIndex;
        do
        {
            nuevoIndex = Random.Range(0, destinos.Count);
        }
        while (nuevoIndex == ultimoDestinoIndex);

        ultimoDestinoIndex = nuevoIndex;
        agent.SetDestination(destinos[nuevoIndex].position);
    }
}
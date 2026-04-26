using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MotherMovement : MonoBehaviour
{
    [SerializeField] private List<Transform> destinos;

    [SerializeField] private NavMeshAgent agent;
    public OtherMotherRaycast raycast;

    private int ultimoDestinoIndex = -1;

    void Start()
    {
        ElegirNuevoDestino();
    }

    void Update()
    {
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
                ElegirNuevoDestino();
            }
        }
        }
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

        Transform destino = destinos[nuevoIndex];
        agent.SetDestination(destino.position);
    }
}

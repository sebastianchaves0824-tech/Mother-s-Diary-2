using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MotherMovement : MonoBehaviour
{
    [SerializeField] private List<Transform> destinos;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private Collider motherCollider;
    public OtherMotherRaycast raycast;

    private int ultimoDestinoIndex = -1;

    void Start()
    {
        ElegirNuevoDestino();
    }

    void Update()
    {
        if (animator != null)
        {
            animator.speed = Mathf.Clamp(agent.speed / 3.5f, 0.1f, 3f);
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

    private void OnCollisionEnter(Collision other) {
        if (gameObject.CompareTag("rampa"))
        {
            motherCollider.isTrigger = true;
        }
        else
        {
            motherCollider.isTrigger = false;
        }
    }

    }
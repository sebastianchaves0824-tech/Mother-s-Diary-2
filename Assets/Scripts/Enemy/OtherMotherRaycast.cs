using UnityEngine;

public class OtherMotherRaycast : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float rangoVision = 20f;

    [Header("Referencias")]
    [SerializeField] public Transform jugadorTransform;

    public bool detectado = false;   // Está dentro del trigger
    public bool enVista = false;     // Lo estoy viendo con raycast

    void Start()
    {
        if (jugadorTransform == null)
        {
            GameObject jugadorObj = GameObject.FindWithTag("Player");
            if (jugadorObj != null) jugadorTransform = jugadorObj.transform;
        }
    }

    void Update()
    {
        ComprobarVision();
    }

    void ComprobarVision()
    {
        if (!detectado || jugadorTransform == null)
        {
            enVista = false;
            return;
        }

        Vector3 origenOjos = transform.position + Vector3.up * 1.5f;
        Vector3 direccion = (jugadorTransform.position - origenOjos).normalized;

        RaycastHit hit;

        if (Physics.Raycast(origenOjos, direccion, out hit, rangoVision))
        {
            if (hit.collider.CompareTag("Player"))
            {
                enVista = true;
            }
            else
            {
                enVista = false;
            }
        }
        else
        {
            enVista = false;
        }
    }

    // --- DETECCIÓN POR TRIGGER ---

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            detectado = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            detectado = false;
            enVista = false;
        }
    }
}


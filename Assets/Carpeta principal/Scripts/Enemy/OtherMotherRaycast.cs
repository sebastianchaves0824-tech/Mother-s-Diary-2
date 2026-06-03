using UnityEngine;

public class OtherMotherRaycast : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float rangoVision = 20f;

    [SerializeField] private LayerMask capaObstaculos;
   
    [Header("Referencias")]
    [SerializeField] public Transform jugadorTransform;

    public bool detectado = false; 
    public bool enVista = false;

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
        if (jugadorTransform != null)
        {
            Vector3 origenOjos = transform.position + Vector3.up * 1.5f;
            Vector3 direccionAljugador = (jugadorTransform.position - origenOjos).normalized;

            RaycastHit hit;

if (Physics.Raycast(origenOjos, direccionAljugador, out hit, rangoVision, Physics.DefaultRaycastLayers))            {
                if (hit.collider.CompareTag("Player"))
                {
                    detectado = true;
                }
                else
                {
                    detectado = false;
                }
            }
            else
            {
                detectado = false;
            }
        }
    }

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
        }
    }
}
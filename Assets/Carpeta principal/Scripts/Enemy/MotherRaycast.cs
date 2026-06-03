using UnityEngine;

public class MotherRaycast : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private float rangoVision = 20f;
    [Header("Filtros")]
    [SerializeField] private LayerMask capasobstaculo;
    [Header("Referencias")]
    [SerializeField] private Transform jugadorTransform;
    private Rigidbody rb;
    private bool detectado = false;

    void Start()
    {
        // 1. Obtenemos el Rigidbody del enemigo
        rb = GetComponent<Rigidbody>();

        // 2. Si no asignaste al jugador en el Inspector, lo buscamos por Tag
        if (jugadorTransform == null)
        {
            GameObject jugadorObj = GameObject.FindWithTag("Player");
            if (jugadorObj != null) jugadorTransform = jugadorObj.transform;
        }
    }

    void FixedUpdate()
    {
        // Solo intentamos perseguir si el jugador está en el area de la esfera
        if (detectado && jugadorTransform != null)
        {
            // se establece el origen del rayo como los ojos del enemigo
            Vector3 origenOjos = transform.position + Vector3.up * 1.5f;
            Vector3 direccionAlJugador = (jugadorTransform.position - origenOjos).normalized;

            RaycastHit hit;
            
            if (Physics.Raycast(origenOjos, direccionAlJugador, out hit, rangoVision))
            {
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
        else
        {
            DetenerEnemigo();
        }
    }

    private void DetenerEnemigo()
    {
        // Frenamos el movimiento en X y Z, pero dejamos que la gravedad actúe en Y
        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
    }

    // --- DETECCIÓN POR TRIGGER ---

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            detectado = true;
            Debug.Log("Jugador entró en el rango");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            detectado = false;
            Debug.Log("Jugador salió del rango");
        }
    }
}
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInteraction : MonoBehaviour
{
    //q tan lejos llega el raycast del puntero
    [SerializeField] private float interactDistance = 5f;
    //que solo choque con objetos con el layer interactable
    [SerializeField] private LayerMask interactLayer;
    //Lista para guardar los ID de las llaves que el jugador tiene;
    [SerializeField] GameObject nextbutton;
    [SerializeField] float sanityIncreaseAmount;
    private List<string> inventoryKeys = new List<string>();
    public diarycode pages;
    public Cordura cordura;    
    private GameObject notaActual;
    private int layerDefault;
    private int layerInteractable;
    private int layerOutline;

    void Start()
    {
        layerDefault = LayerMask.NameToLayer("Default");
    layerInteractable = LayerMask.NameToLayer("Interactable");
    
    // Buscamos la capa
    layerOutline = LayerMask.NameToLayer("OutlineSelection");

    // ¡SALVAVIDAS!: Si devuelve -1, detenemos el juego y te avisamos el porqué
    if (layerOutline == -1)
    {
        Debug.LogError("🚨 ERROR: No se encontró la capa llamada 'OutlineSelection'. ¡Asegúrate de crearla en Unity exactamente con ese nombre!");
    }
    }

    private void Update()
    {
        DetectInteractable();

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            CheckInteraction();
        }       
    }

    private void CheckInteraction()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        //Variable q guarda la info de con quie choco
        RaycastHit hit;

        //Dispara el rayou y si tiene el componente necesario lo presiona
        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            // si el objeto con el que choco tiene el tag de nota, se activa la primera pagina del diario que este desactivada y se borra el objeto con que choco.
            if (hit.collider.CompareTag("note") == true)
            {
                for (int i = 0; i < pages.pages.Count; i++)
                {
                    if (pages.pages[i].gameObject.activeSelf == false)
                    {
                        pages.pages[i].gameObject.SetActive(true);
                        nextbutton.SetActive(true);
                        break;
                    }
                    hit.collider.gameObject.SetActive(false);

                    if (notaActual == hit.collider.gameObject)
                  {
                    notaActual = null; 
                  }
                }
                    cordura.currentSanity += sanityIncreaseAmount;
            }

            if (interactable != null)
            {
               interactable.Interact(this);
            }
        }
    }

    private void DetectInteractable()
{
    // Lanzamos el rayo hacia adelante usando tus variables
    Ray ray = new Ray(transform.position, transform.forward);
    RaycastHit hit;

    // El Raycast filtra por 'interactLayer', que ya incluye 'Interactable' y 'OutlineSelection'
    if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
    {
        // ELIMINAMOS EL FILTRO DEL TAG: Ahora reacciona a cualquier objeto en esta capa
        GameObject objetoDetectado = hit.collider.gameObject;

        if (notaActual != objetoDetectado)
        {
            ApagarContorno(); // Limpiamos el objeto anterior si lo hubiera
            notaActual = objetoDetectado; // Usamos tu variable existente para guardar el objeto enfocado

            // Si la capa de contorno es válida, la activamos
            if (layerOutline != -1)
            {
                notaActual.layer = layerOutline;
            }
        }
        return; // Mantiene el contorno encendido mientras mantengas la mirada
    }

    // Si dejas de mirar el objeto o miras al vacío, se apaga
    ApagarContorno();
}

    private void ApagarContorno()
{
    // Verificamos que el objeto todavía exista en la escena antes de cambiar su capa
    if (notaActual != null)
    {
        if (layerInteractable != -1)
        {
            notaActual.layer = layerInteractable;
        }
        notaActual = null;
    }
}
    
    public void AddKey(string id) => inventoryKeys.Add(id);
    public bool HasKey(string id) => inventoryKeys.Contains(id);
}

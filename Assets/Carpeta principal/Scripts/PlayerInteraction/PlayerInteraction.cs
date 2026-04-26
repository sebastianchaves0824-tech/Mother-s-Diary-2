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

    private void Update()
    {
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
                }
                    cordura.currentSanity += sanityIncreaseAmount;
            }

            if (interactable != null)
            {
               interactable.Interact(this);
            }
        }
    }
    public void AddKey(string id) => inventoryKeys.Add(id);
    public bool HasKey(string id) => inventoryKeys.Contains(id);
}

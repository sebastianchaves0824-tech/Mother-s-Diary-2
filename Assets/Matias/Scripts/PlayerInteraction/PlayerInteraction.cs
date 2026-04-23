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

    //Lista para guardar los ID d las llaves
    private List<string> inventoryKeys = new List<string>();

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

            if (interactable != null)
            {
               interactable.Interact(this);
            }
        }
    }
    public void AddKey(string id) => inventoryKeys.Add(id);
    public bool HasKey(string id) => inventoryKeys.Contains(id);
}

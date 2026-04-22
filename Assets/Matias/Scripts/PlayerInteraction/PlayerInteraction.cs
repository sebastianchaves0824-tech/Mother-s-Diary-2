using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInteraction : MonoBehaviour
{
    //q tan lejos llega el raycast del puntero
    [SerializeField] private float interactDistance = 5f;
    //que solo choque con objetos con el layer interactable
    [SerializeField] private LayerMask interactLayer;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            CheckForButton();
        }
    }

    private void CheckForButton()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        //Variable q guarda la info de con quie choco
        RaycastHit hit;

        //Dispara el rayou y si tiene el componente necesario lo presiona
        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            PhysicalButton button = hit.collider.GetComponent<PhysicalButton>();

            if (button != null)
            {
                button.Press();
            }
        }
    }
}

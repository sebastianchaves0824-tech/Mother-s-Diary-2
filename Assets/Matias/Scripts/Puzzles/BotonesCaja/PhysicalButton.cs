using UnityEngine;

public class PhysicalButton : MonoBehaviour, IInteractable
{
    [SerializeField] private int buttonValue;
    //referencia al NumberInput
    [SerializeField] private NumberInput numberInput;

    public void Interact(PlayerInteraction player)
    {
        Press();
    }

    //la funcion es llamada x el raycast del player
    public void Press()
    {
        //se fija q este asignado y le manda el valor
        if (numberInput != null)
        {
            numberInput.AddDigit(buttonValue);
        }
    }
}

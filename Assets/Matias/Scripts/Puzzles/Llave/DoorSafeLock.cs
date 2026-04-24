using UnityEngine;

public class DoorSafeLock : MonoBehaviour
{
    [SerializeField] private Vector3 rotationAmount = new Vector3(0, -90f, 0); //q tanto rota
    private bool isOpen = false;

    public void OpenSafe()
    {
        if (!isOpen)
        {
            // Debug para ver la rotación actual antes de moverla
            Debug.Log("Rotación antes: " + transform.localEulerAngles);

            // Forzamos la rotación sumando el ángulo a la rotación actual
            transform.localEulerAngles += rotationAmount;

            isOpen = true;
            Debug.Log("Rotación después: " + transform.localEulerAngles);
        }
    }
}

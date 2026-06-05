using UnityEngine;

public class ZoneUnlockButton : MonoBehaviour, IInteractable
{
    [SerializeField] private ZoneUnlocker zoneUnlocker;

    public void Interact(PlayerInteraction player)
    {
        // 1. Ejecutamos el desbloqueo de la zona
        zoneUnlocker.UnlockThisSpecificZoneAndInvestigate();

        // 2. (Opcional) Desactivamos el collider del botón para que no se pueda volver a apretar
        GetComponent<Collider>().enabled = false;

        Debug.Log("¡Botón presionado! La zona se ha desbloqueado.");
    }
}

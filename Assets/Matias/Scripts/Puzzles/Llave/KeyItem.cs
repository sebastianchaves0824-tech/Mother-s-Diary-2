using UnityEngine;

public class KeyItem : MonoBehaviour, IInteractable
{
    [Header("Identificador de Llave")]
    [Tooltip("Este nombre debe coincidir con el 'Required Key ID' de la puerta.")]
    public string keyID = "Llave_Sotano";

    public void Interact(PlayerInteraction player)
    {
        player.AddKey(keyID);
        Destroy(gameObject);
    }
}

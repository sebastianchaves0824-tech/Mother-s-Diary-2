using UnityEngine;
using UnityEngine.AI;

public class ZoneUnlocker : MonoBehaviour
{
    [SerializeField] private NavMeshAgent enemyAgent;
   
    // El punto de interes especifico de ESTA zona
    [SerializeField] private Transform pointOfInterest;

    // El nombre del area tal cual lo escribiste en la pestana "Navigation > Areas"
    [SerializeField] private string zoneAreaName;

    // Llamas a este metodo cuando se desbloquee esta zona en particular
    public void UnlockThisSpecificZoneAndInvestigate()
    {
        // 1. Obtenemos el índice numérico (0-31) del área basándonos en su nombre
        int areaIndex = NavMesh.GetAreaFromName(zoneAreaName);

        // Verificamos que el nombre esté bien escrito y exista
        if (areaIndex != -1)
        {
            // 2. Operación Bitwise (OR): Suma esta nueva área a la máscara que ya tiene el enemigo
            // Así conserva el acceso a las zonas anteriores y suma esta nueva.
            enemyAgent.areaMask = enemyAgent.areaMask | (1 << areaIndex);

            // 3. Le ordenamos ir al punto de interés
            enemyAgent.SetDestination(pointOfInterest.position);
        }
        else
        {
            // Aviso por si escribiste mal el nombre del área en el Inspector
            Debug.LogError("No se encontró un área de NavMesh llamada: " + zoneAreaName);
        }
    }
}

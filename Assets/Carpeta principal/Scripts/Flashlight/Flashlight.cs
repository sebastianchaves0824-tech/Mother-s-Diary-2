using UnityEngine;

public class Flashlight : MonoBehaviour
{
    // Referencia a la luz (asegúrate de que sea tipo Spot Light)
    [SerializeField] private Light linterna;

    // Valores normales
    [SerializeField] private float rangoNormal = 10f;
    [SerializeField] private float intensidadNormal = 2f;
    [SerializeField] private float outerSpotNormal = 30f;

    // Valores al presionar click izquierdo
    [SerializeField] private float rangoAumentado = 20f;
    [SerializeField] private float intensidadAumentada = 5f;
    [SerializeField] private float outerSpotAumentado = 60f;
 
    void Start()
    {
        // Configurar valores iniciales
        if (linterna != null)
        {
            linterna.range = rangoNormal;
            linterna.intensity = intensidadNormal;
            linterna.spotAngle = outerSpotNormal;
        }
    }

    void Update()
    {
        if (linterna != null)
        {
            // Si se mantiene presionado el click izquierdo
            if (Input.GetMouseButton(0)) // 0 = click izquierdo
            {
                linterna.range = rangoAumentado;
                linterna.intensity = intensidadAumentada;
                linterna.spotAngle = outerSpotAumentado;
            }
            else
            {
                // Vuelve a los valores normales
                linterna.range = rangoNormal;
                linterna.intensity = intensidadNormal;
                linterna.spotAngle = outerSpotNormal;
            }
        }
    }
}

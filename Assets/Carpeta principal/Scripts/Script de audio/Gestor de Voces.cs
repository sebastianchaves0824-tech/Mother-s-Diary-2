using System.Collections;
using UnityEngine;

public class GestorVoces : MonoBehaviour
{
    [Header("Componentes de Audio")]
    [SerializeField] private AudioSource audioSourceVoz;
    [SerializeField] private AudioClip[] clipsDeVoz; // Array que almacena tus archivos de voz procesados

    [Header("Configuración del Bucle Aleatorio")]
    [SerializeField] private float intervaloChequeo = 10f; // Cada cuántos segundos intenta hablar
    [Range(0f, 100f)]
    [SerializeField] private float probabilidadDeHablar = 40f; // Porcentaje de éxito (ej: 40% de chances)

    private int ultimoIndice = -1;
    private bool estaActivo = true;

    private void Start()
    {
        // Iniciamos el bucle temporal en el Start para que corra de fondo de forma optimizada
        if (clipsDeVoz != null && clipsDeVoz.Length > 0)
        {
            StartCoroutine(BuclePensamientosAleatorios());
        }
    }

    private IEnumerator BuclePensamientosAleatorios()
    {
        // El bucle se ejecutará de forma segura mientras el juego esté corriendo
        while (estaActivo)
        {
            // Pausa la ejecución por el tiempo determinado sin sobrecargar el frame rate
            yield return new WaitForSeconds(intervaloChequeo);

            // Si el protagonista ya está hablando, esperamos al siguiente ciclo
            if (audioSourceVoz.isPlaying) continue;

            // Lanzamos el dado probabilístico
            float dado = Random.Range(0f, 100f);

            if (dado <= probabilidadDeHablar)
            {
                ReproducirFraseAleatoria();
            }
        }
    }

    private void ReproducirFraseAleatoria()
    {
        // Seleccionamos un índice al azar dentro del tamaño de nuestro array
        int indiceAleatorio = Random.Range(0, clipsDeVoz.Length);

        // Algoritmo de control: Evita que se repita la misma frase de forma consecutiva
        if (clipsDeVoz.Length > 1 && indiceAleatorio == ultimoIndice)
        {
            indiceAleatorio = (indiceAleatorio + 1) % clipsDeVoz.Length;
        }

        // Actualizamos el registro del último audio reproducido
        ultimoIndice = indiceAleatorio;

        // Asignamos el clip al AudioSource y lo reproducimos en un plano 2D
        audioSourceVoz.clip = clipsDeVoz[indiceAleatorio];
        audioSourceVoz.Play();
    }

    // Método público para pausar o reactivar el sistema desde otros scripts (Encapsulamiento)
    public void SetEstadoSistema(bool estado)
    {
        estaActivo = estado;
        if (estado == true)
        {
            StartCoroutine(BuclePensamientosAleatorios());
        }
    }
}
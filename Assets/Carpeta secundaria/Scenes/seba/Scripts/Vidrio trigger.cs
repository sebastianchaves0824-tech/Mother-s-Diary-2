using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GlassAudioTrigger : MonoBehaviour
{
    [Header("Ajustes de un solo uso")]
    [Tooltip("¿El sonido debe sonar solo una vez por partida?")]
    [SerializeField] private bool soundOnce = true;

    private AudioSource audioSource;
    private bool alreadyPlayed = false; // Tu propio candado independiente

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        
        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Si está configurado para sonar una vez y YA sonó, salimos.
            if (soundOnce && alreadyPlayed) return;

            // Reproducimos el sonido de inmediato
            if (audioSource != null)
            {
                audioSource.Play();
                alreadyPlayed = true; // Nos aseguramos de que no vuelva a sonar si era único
            }
        }
    }
}
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyFootsteps : MonoBehaviour
{
    [Header("Configuración de Sonidos")]
    [Tooltip("Arrastrá acá tus 8 variantes de pasos procesadas")]
    [SerializeField] private AudioClip[] footstepClips;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // Forzamos 100% espacialización 3D
    }

    // Esta función va a ser llamada de forma directa por los Animation Events
    public void PlayFootstep()
    {
        if (footstepClips == null || footstepClips.Length == 0 || audioSource == null) return;

        // Elige un índice al azar entre 0 y 7
        int randomIndex = Random.Range(0, footstepClips.Length);
        AudioClip clip = footstepClips[randomIndex];

        // PlayOneShot permite que los sonidos se solapen de forma natural si camina rápido
        audioSource.PlayOneShot(clip);
    }
}
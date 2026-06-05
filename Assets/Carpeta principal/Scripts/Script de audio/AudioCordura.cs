using UnityEngine;
using UnityEngine.Audio; // Requisito obligatorio para interactuar con las clases del Mixer

public class SanityAudioController : MonoBehaviour
{
    // Instancia Singleton para acceso global (Clase 1 - POO)
    public static SanityAudioController Instance { get; private set; }

    [Header("Configuración de Audio")]
    public AudioMixer AudioMixer; 
    public string parameterName = "MasterLowpassFreq";

    [Header("Referencias a los Snapshots (Música Adaptativa)")] //
    [SerializeField] private AudioMixerSnapshot exploracionSnapshot;
    [SerializeField] private AudioMixerSnapshot persecucionSnapshot;
    [SerializeField] private float tiempoTransicionMusica = 1.5f; 

    [Header("Referencias a los Audio Sources")]
    [SerializeField] private AudioSource fuenteExploracion; 
    [SerializeField] private AudioSource fuentePersecucion; 

    [Header("Referencia al Componente del Grupo")]
    public Cordura sistemaCordura; 

    private float maxFrequency = 22000f; //
    private float minFrequency = 150f;   //

    // Variable interna de control según tus nombres de variables
    private bool estabaPerseguido = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (sistemaCordura == null)
        {
            sistemaCordura = FindFirstObjectByType<Cordura>(); 
        }
    }

    void Update()
    {
        if (sistemaCordura == null || AudioMixer == null) return;

        // --- LÓGICA DE SORDERA (FILTRO) ---
        float targetFreq;

        if (sistemaCordura.currentSanity >= 100f)
        {
            targetFreq = maxFrequency; 
        }
        else
        {
            float lowSanityNormalized = sistemaCordura.currentSanity / 100f; 
            targetFreq = Mathf.Lerp(minFrequency, maxFrequency, lowSanityNormalized); //
        }

        AudioMixer.SetFloat(parameterName, targetFreq); //


        // ==========================================
        // LÓGICA DE SNAPSHOTS CON DIAGNÓSTICO EN CONSOLA
        // ==========================================
        bool estaDetectado = sistemaCordura.raycast.detectado;

        if (!estaDetectado)
        {
            // exploracion
            if (estabaPerseguido)
            {
                
                if (exploracionSnapshot != null)
                {
                    exploracionSnapshot.TransitionTo(tiempoTransicionMusica); //
                }
                
                if (fuentePersecucion != null)
                {
                    fuentePersecucion.Stop(); // Apagamos la música de tensión
                }

                estabaPerseguido = false;
            }
        }
        else
        {
            // persecucion
            if (!estabaPerseguido)
            {
             

                if (fuentePersecucion != null)
                {
                    fuentePersecucion.Stop(); // Resetea la pista al segundo cero
                    fuentePersecucion.Play(); // Inicia el track desde el principio
                }
                if (persecucionSnapshot != null)
                {
                    persecucionSnapshot.TransitionTo(tiempoTransicionMusica);
                }
                estabaPerseguido = true;
            }
        }
        // ==========================================
    }
}
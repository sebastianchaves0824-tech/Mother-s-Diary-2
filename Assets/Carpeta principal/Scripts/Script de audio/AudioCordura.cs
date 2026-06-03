using UnityEngine;
using UnityEngine.Audio; 

public class SanityAudioController : MonoBehaviour
{
    public static SanityAudioController Instance { get; private set; }
    [Header("Configuración de Audio")]
    public AudioMixer AudioMixer; 
    public string parameterName = "MasterLowpassFreq";
    //-------------------------------------------------------------------------------------------- 
    [Header("Referencias a los Snapshots (Música Adaptativa)")]
    [SerializeField] private AudioMixerSnapshot exploracionSnapshot;
    [SerializeField] private AudioMixerSnapshot persecucionSnapshot;
    [SerializeField] private float tiempoTransicionMusica = 1.5f;
    //--------------------------------------------------------------------------------------------

    [Header("Referencias a los Audio Sources")]
    [SerializeField] private AudioSource fuenteExploracion; // Arrastra Musica_Exploracion acá
    [SerializeField] private AudioSource fuentePersecucion; // Arrastra Musica_Persecucion acá
    
    //--------------------------------------------------------------------------------------------
    [Header("Referencia al Componente del Grupo")]
    public Cordura sistemaCordura; 

    private float maxFrequency = 22000f; 
    private float minFrequency = 50f;   

    //--------------------------------------------------------------------------------------------

 private bool estabaPerseguido = false;
 void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    //--------------------------------------------------------------------------------------------
    void Start()
    {
        if (sistemaCordura == null)
        {
            // Busca automaticamente el script Cordura en la escena 
            sistemaCordura = FindFirstObjectByType<Cordura>(); 
        }
    }

    void Update()
    {
        
        if (sistemaCordura == null|| AudioMixer == null) return;

        // sordera
        float currentPercent = sistemaCordura.currentSanity / sistemaCordura.maxSanity;
        float targetFreq;

        if (currentPercent >= 100f)
        {
            targetFreq = maxFrequency; 
        }
        else
        {
            float lowSanityNormalized = currentPercent / 100f; 
            targetFreq = Mathf.Lerp(minFrequency, maxFrequency, lowSanityNormalized); 
        }

        AudioMixer.SetFloat(parameterName, targetFreq); bool estaDetectado = sistemaCordura.raycast.detectado;

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
                    fuentePersecucion.Stop(); 
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
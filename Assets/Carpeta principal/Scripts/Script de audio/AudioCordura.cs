using UnityEngine;
using UnityEngine.Audio; 

public class SanityAudioController : MonoBehaviour
{
    [Header("Configuración de Audio")]
    public AudioMixer AudioMixer; 
    public string parameterName = "MasterLowpassFreq";

    [Header("Referencia al Componente del Grupo")]
    public Cordura sistemaCordura; 

    private float maxFrequency = 22000f; 
    private float minFrequency = 150f;   

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
        if (sistemaCordura == null) return;

        // Calculamos el porcentaje in-game de cordura
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

        AudioMixer.SetFloat(parameterName, targetFreq); 
    }
}
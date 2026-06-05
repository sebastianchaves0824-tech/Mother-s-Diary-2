using UnityEngine;
using System.Collections;

public class AdvancedAtmosphericLight : MonoBehaviour
{
    private Light targetLight;
    private float baseIntensity;

    // ==========================================
    // VARIABLES AGREGADAS PARA EL MATERIAL REALISTA
    // ==========================================
    [SerializeField] private Renderer lampRenderer;
private Material lampMaterial;
    // ==========================================

    [System.Serializable]
    public class BehaviorWeights
    {
        [Range(0, 1)] public float Cycle = 0.4f;
        [Range(0, 1)] public float Flicker = 0.3f;
        [Range(0, 1)] public float Throb = 0.3f;
    }

    [Header("Behavior Modes (Weights)")]
    public BehaviorWeights modeWeights;
    [Tooltip("Tiempo de transición suave al cambiar de modo")]
    public float modeTransitionTime = 1.5f;

    [System.Serializable]
    public class CycleConfig
    {
        public float StableOnTimeAvg = 8.0f;
        public float StableOffTimeAvg = 4.0f;
        [Range(0, 0.5f)] public float RandomizedMargin = 0.2f;
    }

    [Header("Cycle Configuration")]
    public CycleConfig cycleConfig;

    [System.Serializable]
    public class FlickerConfig
    {
        public float MinTime = 0.03f;
        public float MaxTime = 0.15f;
        [MinMaxRange(0f, 20f)] public Vector2 IntensityRange = new Vector2(3f, 18f);
    }

    [Header("Random Flicker Configuration")]
    public FlickerConfig flickerConfig;

    [System.Serializable]
    public class ThrobConfig
    {
        public float ThrobSpeed = 0.5f;
        [MinMaxRange(0f, 20f)] public Vector2 IntensityRange = new Vector2(8f, 15f);
    }

    [Header("Variable Throb Configuration")]
    public ThrobConfig throbConfig;

    private Coroutine activeBehaviorCoroutine;
    private bool isTransitioning = false;

    private void Start()
{
    targetLight = GetComponent<Light>();
    if (targetLight == null)
    {
        Debug.LogError($"No Light component found on {gameObject.name}");
        return;
    }
    baseIntensity = targetLight.intensity;

    // Si no arrastraste manualmente el Renderer en el inspector, lo busca automáticamente
    if (lampRenderer == null)
    {
        lampRenderer = GetComponentInChildren<Renderer>();
    }

    if (lampRenderer != null)
    {
        lampMaterial = lampRenderer.material; // Crea la copia única del material
    }
    else
    {
        Debug.LogError($"No se encontró ningún Renderer en los hijos de {gameObject.name}. ¡Arrastra la esfera manualmente!");
    }

    StartBehaviorMaster();
}

    private void StartBehaviorMaster()
    {
        if (activeBehaviorCoroutine != null) StopCoroutine(activeBehaviorCoroutine);
        activeBehaviorCoroutine = StartCoroutine(BehaviorMasterRoutine());
    }

    private IEnumerator BehaviorMasterRoutine()
    {
        while (true)
        {
            float totalWeight = modeWeights.Cycle + modeWeights.Flicker + modeWeights.Throb;
            float randomValue = Random.Range(0, totalWeight);

            IEnumerator nextBehavior = null;

            if (randomValue < modeWeights.Cycle)
            {
                nextBehavior = CycleBehavior();
            }
            else if (randomValue < modeWeights.Cycle + modeWeights.Flicker)
            {
                nextBehavior = FlickerBehavior();
            }
            else
            {
                nextBehavior = ThrobBehavior();
            }

            yield return StartCoroutine(TransitionTo(nextBehavior));
            yield return StartCoroutine(nextBehavior);
        }
    }

    private IEnumerator TransitionTo(IEnumerator target)
    {
        isTransitioning = true;
        float elapsed = 0;
        float startInt = targetLight.intensity;
        float targetInt = baseIntensity;

        while (elapsed < modeTransitionTime)
        {
            targetLight.intensity = Mathf.Lerp(startInt, targetInt, elapsed / modeTransitionTime);
            
            // AGREGADO: Sincronizar emisión durante transiciones suaves
            ActualizarEmisionPorIntensidad(targetLight.intensity);

            elapsed += Time.deltaTime;
            yield return null;
        }
        targetLight.intensity = targetInt;
        ActualizarEmisionPorIntensidad(targetLight.intensity);
        isTransitioning = false;
    }

    private IEnumerator CycleBehavior()
    {
        targetLight.enabled = true;
        targetLight.intensity = baseIntensity;
        ToggleEmision(true); // AGREGADO: Encender vidrio

        yield return new WaitForSeconds(RandomizeTime(cycleConfig.StableOnTimeAvg, cycleConfig.RandomizedMargin));

        // Parpadeo de advertencia antes de apagar por completo
        for (int i = 0; i < 4; i++)
        {
            targetLight.enabled = !targetLight.enabled;
            ToggleEmision(targetLight.enabled); // AGREGADO: Parpadear vidrio
            yield return new WaitForSeconds(0.1f);
        }

        targetLight.enabled = false;
        ToggleEmision(false); // AGREGADO: Apagar vidrio por completo

        yield return new WaitForSeconds(RandomizeTime(cycleConfig.StableOffTimeAvg, cycleConfig.RandomizedMargin));
    }

    private IEnumerator FlickerBehavior()
    {
        targetLight.enabled = true;
        float flickerDuration = Random.Range(3f, 6f);
        float elapsed = 0;
        while (elapsed < flickerDuration)
        {
            targetLight.intensity = Random.Range(flickerConfig.IntensityRange.x, flickerConfig.IntensityRange.y);
            
            // AGREGADO: El brillo del cristal cambia dinámicamente según la intensidad aleatoria
            ActualizarEmisionPorIntensidad(targetLight.intensity);

            float wait = Random.Range(flickerConfig.MinTime, flickerConfig.MaxTime);
            elapsed += wait;
            yield return new WaitForSeconds(wait);
        }
    }

    private IEnumerator ThrobBehavior()
    {
        targetLight.enabled = true;
        float throbDuration = Random.Range(4f, 8f);
        float elapsed = 0;
        while (elapsed < throbDuration)
        {
            float val = Mathf.Sin(Time.time * throbConfig.ThrobSpeed) * 0.5f + 0.5f;
            targetLight.intensity = Mathf.Lerp(throbConfig.IntensityRange.x, throbConfig.IntensityRange.y, val);
            
            // AGREGADO: Sincronización del latido de intensidad con el cristal
            ActualizarEmisionPorIntensidad(targetLight.intensity);

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private float RandomizeTime(float avg, float margin)
    {
        return Random.Range(avg * (1 - margin), avg * (1 + margin));
    }

    // ==========================================
    // MÉTODOS DE CONTROL AUXILIARES AGREGADOS
    // ==========================================
    private void ToggleEmision(bool encendido)
    {
        if (lampMaterial == null) return;

        if (encendido)
        {
            lampMaterial.EnableKeyword("_EMISSION");
            // Al encender de golpe, le devolvemos el brillo proporcional a la intensidad base
            ActualizarEmisionPorIntensidad(baseIntensity);
        }
        else
        {
            lampMaterial.DisableKeyword("_EMISSION");
            lampMaterial.SetColor("_EmissionColor", Color.black);
        }
    }

    private void ActualizarEmisionPorIntensidad(float intensidadActual)
    {
        if (lampMaterial == null) return;

        // Si la luz baja de un umbral mínimo, apagamos la emisión visual por completo
        if (intensidadActual <= 0.1f)
        {
            lampMaterial.DisableKeyword("_EMISSION");
            lampMaterial.SetColor("_EmissionColor", Color.black);
        }
        else
        {
            lampMaterial.EnableKeyword("_EMISSION");

            // 1. Calculamos un porcentaje (de 0 a 1) de qué tan fuerte está la luz respecto a su base
            float porcentajeIntensidad = Mathf.Clamp01(intensidadActual / baseIntensity);

            // 2. Definimos el color base de tu lámpara (puedes ajustar el tono aquí mismo si lo deseas)
            Color colorBaseEmision = new Color(1f, 0.75f, 0.3f); // Un amarillo-naranja cálido estándar

            // 3. Multiplicamos el color por el porcentaje y por un factor HDR (el 2f final le da el "resplandor")
            Color colorFinalCalculado = colorBaseEmision * porcentajeIntensidad * 2f;

            // 4. Le aplicamos el color e intensidad exacta al mapa de emisión del Shader
            lampMaterial.SetColor("_EmissionColor", colorFinalCalculado);
        }
    }
    // ==========================================
}

// Atributo MinMaxRange (Se mantiene igual)
public class MinMaxRangeAttribute : PropertyAttribute
{
    public float Min { get; private set; }
    public float Max { get; private set; }

    public MinMaxRangeAttribute(float min, float max)
    {
        Min = min;
        Max = max;
    }
}

#if UNITY_EDITOR
[UnityEditor.CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
public class MinMaxRangeDrawer : UnityEditor.PropertyDrawer
{
    public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
    {
        MinMaxRangeAttribute attr = (MinMaxRangeAttribute)attribute;
        Vector2 range = property.vector2Value;
        float min = range.x;
        float max = range.y;

        UnityEditor.EditorGUI.BeginChangeCheck();
        UnityEditor.EditorGUI.MinMaxSlider(position, label, ref min, ref max, attr.Min, attr.Max);
        if (UnityEditor.EditorGUI.EndChangeCheck())
        {
            property.vector2Value = new Vector2(min, max);
        }
    }
}
#endif
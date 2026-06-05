using UnityEngine;

public class LightCube : MonoBehaviour, IInteractable
{
    [Header("Puzzle Settings")]
    public LightCubePuzzleManager puzzleManager;

    [Header("Visuals")]
    [ColorUsage(true, true)]
    public Color glowColor = Color.white;

    [Header("Flashlight Settings")]
    public float timeToLightUp = 2f; // Segundos de luz directa para prenderlo al 100%
    public float cooldownRate = 1f;  // Velocidad a la que se apaga si le dejas de apuntar

    private Material cubeMaterial;
    private bool isLit = false;

    // Control de carga gradual
    private float currentCharge = 0f;
    private bool isBeingIlluminated = false;

    private void Start()
    {
        cubeMaterial = GetComponent<Renderer>().material;
        // Habilitamos la emisión desde el inicio para modificar el color en Update
        cubeMaterial.EnableKeyword("_EMISSION");
        TurnOff();
    }

    private void Update()
    {
        if (isLit || puzzleManager.isSolved)
            return;

        if (isBeingIlluminated)
        {
            currentCharge += Time.deltaTime;

            if (currentCharge >= timeToLightUp)
            {
                currentCharge = timeToLightUp;
                CompleteLighting();
            }
        }
        else
        {
            if (currentCharge > 0)
            {
                currentCharge -= Time.deltaTime * cooldownRate;
                if (currentCharge < 0) currentCharge = 0;
            }
        }

        // Interpolar el color gradualmente de negro a tu glowColor según la carga
        float chargePercent = currentCharge / timeToLightUp;
        Color currentColor = Color.Lerp(Color.black, glowColor, chargePercent);
        cubeMaterial.SetColor("_EmissionColor", currentColor);

        // Se resetea cada frame. Si la linterna sigue apuntando, lo volverá a poner en true
        isBeingIlluminated = false;
    }

    // Esta es la función clave que llama tu nuevo script FlashlightInteractor
    public void Illuminate()
    {
        if (isLit || puzzleManager.isSolved) return;
        isBeingIlluminated = true;
    }

    // Mantenemos esto por si sigues queriendo que se pueda interactuar con la 'F'
    public void Interact(PlayerInteraction player)
    {
        if (isLit || puzzleManager.isSolved) return;
        CompleteLighting();
    }

    private void CompleteLighting()
    {
        TurnOn();
        puzzleManager.OnCubeInteracted(this);
    }

    public void TurnOn()
    {
        isLit = true;
        currentCharge = timeToLightUp;
        cubeMaterial.SetColor("_EmissionColor", glowColor);
    }

    public void TurnOff()
    {
        isLit = false;
        currentCharge = 0f;
        cubeMaterial.SetColor("_EmissionColor", Color.black);
    }
}
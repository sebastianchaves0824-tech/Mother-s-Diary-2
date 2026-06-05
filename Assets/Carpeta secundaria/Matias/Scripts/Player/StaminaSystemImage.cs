using System.Collections;
using System.Reflection; // Importante para leer variables privadas de otro script
using UnityEngine;
using UnityEngine.UI;

public class StaminaSystem : MonoBehaviour
{
    [Header("Stamina Settings")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDrainPerSecond = 20f;
    [SerializeField] private float staminaRegenPerSecond = 15f;

    [Header("UI (Image with Fill)")]
    [SerializeField] private Image staminaFillImage;
    [SerializeField] private CanvasGroup staminaCanvas;

    [Header("Fade Settings")]
    [SerializeField] private float fadeSpeed = 4f;
    [SerializeField] private float hideDelay = 1.2f;

    [Header("Colors")]
    [SerializeField] private Color normalColor = Color.white; // Lo cambié a blanco por defecto para ti
    [SerializeField] private Color exhaustedColor = Color.red;

    [Header("References")]
    [SerializeField] private NewMonoBehaviourScript playerMovement;

    private float currentStamina;
    private bool isExhausted = false;
    private bool isBlinking = false;

    private Coroutine fadeRoutine;
    private Coroutine blinkCoroutine; // <-- Añadido para controlar la corrutina
    private float hideTimer;
    private bool shouldBeVisible;

    // Variables para leer tu script original con Reflection
    private FieldInfo isRunningField;
    private FieldInfo moveField;

    private void Start()
    {
        currentStamina = maxStamina;

        if (staminaFillImage != null)
        {
            staminaFillImage.fillAmount = 1f;
            staminaFillImage.color = normalColor;
        }

        if (staminaCanvas != null)
        {
            staminaCanvas.alpha = 0f; // Empieza invisible (afecta a barra y fondo)
        }

        // MAGIA: Buscamos las variables privadas en tu script original
        isRunningField = typeof(NewMonoBehaviourScript).GetField("isRunning", BindingFlags.NonPublic | BindingFlags.Instance);
        moveField = typeof(NewMonoBehaviourScript).GetField("move", BindingFlags.NonPublic | BindingFlags.Instance);
    }

    private void Update()
    {
        HandleStamina();
        UpdateUI();
        UpdateVisibilityLogic();
        HandleFade();
    }

    private void HandleStamina()
    {
        bool isPlayerRunning = isRunningField != null && (bool)isRunningField.GetValue(playerMovement);
        Vector2 playerMoveInput = moveField != null ? (Vector2)moveField.GetValue(playerMovement) : Vector2.zero;

        bool isMoving = playerMoveInput.magnitude > 0.1f;
        bool wantsToSprint = isPlayerRunning && isMoving;

        //  EXHAUSTO
        if (isExhausted)
        {
            playerMovement.StopRunning();
            currentStamina += staminaRegenPerSecond * Time.deltaTime;

            if (currentStamina >= maxStamina)
            {
                isExhausted = false;
                currentStamina = maxStamina;

                StopBlink(); // Ahora sí lo frena en seco

                if (staminaFillImage != null)
                    staminaFillImage.color = normalColor;
            }
            return;
        }

        //  CORRIENDO
        if (wantsToSprint)
        {
            currentStamina -= staminaDrainPerSecond * Time.deltaTime;

            if (currentStamina <= 0)
            {
                currentStamina = 0;
                isExhausted = true;

                StartBlink();
            }
        }
        else //  CAMINANDO O DETENIDO
        {
            currentStamina += staminaRegenPerSecond * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
    }

    // ------------------------------------
    //        LÓGICA DE CUÁNDO MOSTRAR
    // ------------------------------------
    private void UpdateVisibilityLogic()
    {
        bool isFull = currentStamina >= maxStamina - 0.01f;

        bool isPlayerRunning = isRunningField != null && (bool)isRunningField.GetValue(playerMovement);
        Vector2 playerMoveInput = moveField != null ? (Vector2)moveField.GetValue(playerMovement) : Vector2.zero;

        bool wantsToSprint = isPlayerRunning && (playerMoveInput.magnitude > 0.1f);

        shouldBeVisible = isExhausted || wantsToSprint || !isFull;

        if (shouldBeVisible)
        {
            hideTimer = 0f;
        }
        else
        {
            hideTimer += Time.deltaTime;
        }
    }

    // ------------------------------------
    //               FADES 
    // ------------------------------------
    private void HandleFade()
    {
        if (staminaCanvas == null) return;

        if (shouldBeVisible)
        {
            FadeTo(1f);
        }
        else
        {
            if (hideTimer >= hideDelay)
            {
                FadeTo(0f);
            }
        }
    }

    private void FadeTo(float targetAlpha)
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = StartCoroutine(FadeRoutine(targetAlpha));
    }

    private IEnumerator FadeRoutine(float target)
    {
        while (Mathf.Abs(staminaCanvas.alpha - target) > 0.01f)
        {
            staminaCanvas.alpha = Mathf.MoveTowards(staminaCanvas.alpha, target, Time.deltaTime * fadeSpeed);
            yield return null;
        }

        staminaCanvas.alpha = target;
    }

    private void UpdateUI()
    {
        if (staminaFillImage != null)
        {
            staminaFillImage.fillAmount = currentStamina / maxStamina;
        }
    }

    // ------------------------------------
    //     PARPADEO (SOLUCIONADO)
    // ------------------------------------
    private void StartBlink()
    {
        if (!isBlinking && staminaFillImage != null)
        {
            // Detenemos cualquier parpadeo previo por seguridad
            if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);

            blinkCoroutine = StartCoroutine(BlinkRoutine());
        }
    }

    private void StopBlink()
    {
        isBlinking = false;

        // Asesinamos la corrutina en seco para evitar que vuelva a pintar rojo
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
    }

    private IEnumerator BlinkRoutine()
    {
        isBlinking = true;

        while (isBlinking)
        {
            Color blinkColor = exhaustedColor;
            blinkColor.a = 0.2f;
            staminaFillImage.color = blinkColor;

            yield return new WaitForSeconds(0.18f);

            blinkColor.a = 1f;
            staminaFillImage.color = blinkColor;

            yield return new WaitForSeconds(0.18f);
        }
    }
}
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PuzzleQTE : MonoBehaviour
{
    [SerializeField] private NewMonoBehaviourScript movimientoPJ;
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private Image radialCircle;
    [SerializeField] private Image radialCircleShadow;
    [SerializeField] private Cordura cordura;

    [Header("Configuración de Audio")]
    [SerializeField] private AudioSource soundTransmitter;
    [SerializeField] private AudioClip failSound;

    [Header("Configuraión")]
    [SerializeField] private float timeToPress = 1.5f; //Sefundos para presionar
    [SerializeField] private float failPenaltyPercent = 0.025f; // Penalización ajustable (2.5%)

    private string[] keys = { "W", "A", "S", "D" };
    private string currentKey;
    private int successes = 0;
    private bool isPuzzleActive = false;

    private void Start()
    {
        textUI.gameObject.SetActive(false);
        radialCircle.gameObject.SetActive(false);
        radialCircleShadow.gameObject.SetActive(false);
    }

    public void StartPuzzle()
    {
        if (isPuzzleActive) return;

        isPuzzleActive = true;
        successes = 0;
        movimientoPJ.enabled = false; // Bloquea el movimiento
        StartCoroutine(PuzzleRoutine());
    }

    private IEnumerator PuzzleRoutine()
    {
        while (successes < 3)
        {
            currentKey = keys[Random.Range(0, keys.Length)];
            textUI.text = currentKey;
            radialCircle.fillAmount = 1f;

            textUI.gameObject.SetActive(true);
            radialCircle.gameObject.SetActive(true);
            radialCircleShadow.gameObject.SetActive(true);

            float timer = timeToPress;
            bool pressedCorrectly = false;

            while (timer > 0)
            {
                timer -= Time.deltaTime;
                //Se va vaciando con el pasar del tiempo
                radialCircle.fillAmount = timer / timeToPress;
                // Detectamos la tecla
                if (Input.GetKeyDown(currentKey.ToLower()))
                {
                    pressedCorrectly = true;
                    break;
                }

                // Si presiona la tecla equivocada, reinicia el contador de exitos
                if (Input.anyKeyDown && !Input.GetKeyDown(currentKey.ToLower()) && !Input.GetKeyDown(KeyCode.P))
                {
                    pressedCorrectly = false;
                    break;
                }
                yield return null;
            }

            radialCircle.gameObject.SetActive(false);
            radialCircleShadow.gameObject.SetActive(false);
            textUI.gameObject.SetActive(false);

            if (pressedCorrectly)
            {
                successes++;
                //para ponerlo verde antes de pasar
                yield return new WaitForSeconds(0.2f);
            }
            else
            {
                // Aplica el castigo por errar
                if (cordura != null)
                {
                    float penalty = cordura.maxSanity * failPenaltyPercent;
                    cordura.currentSanity -= penalty;
                    if (cordura.currentSanity < 0) cordura.currentSanity = 0;
                }

                if (soundTransmitter != null && failSound != null)
                {
                    soundTransmitter.PlayOneShot(failSound);
                }
                successes = 0; // Si fallas, volves a empezar
                yield return new WaitForSeconds(0.8f);
            }

        }

        // Al terminar
        yield return new WaitForSeconds(0.5f); // Pausa breve entre teclas
        movimientoPJ.enabled = true;
        isPuzzleActive = false;
    }
}
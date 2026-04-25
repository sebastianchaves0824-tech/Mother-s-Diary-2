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

    [Header("Configuraión")]
    [SerializeField] private float timeToPress = 1.5f; //Sefundos para presionar
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
                if (Input.anyKeyDown && !Input.GetKeyDown(currentKey.ToLower()))
                {
                    successes = 0;
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
                successes = 0; // Si fallás, volvés a empezar
                yield return new WaitForSeconds(0.8f);
            }

        }

        // Al terminar
        yield return new WaitForSeconds(0.5f); // Pausa breve entre teclas
        movimientoPJ.enabled = true;
        isPuzzleActive = false;
    }


    //POR AHORA INICIA EL QTE CON "P"
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !isPuzzleActive)
        {
            StartPuzzle();
        }
    }

}

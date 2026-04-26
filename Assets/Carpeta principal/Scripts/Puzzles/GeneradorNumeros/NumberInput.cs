using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class NumberInput : MonoBehaviour
{
    [SerializeField] private NumberGenerator generator;
    [SerializeField] private TextMeshProUGUI screenText;

    [Header("Ajustes de Recompensa")] //q objeto y donde va a aparecer
    [SerializeField] private GameObject keyPrefab; 
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private DoorSafeLock doorSafeLock;

    //guarda los numeros como strings
    private string currentInput = "";
    private bool isLocked = false;
    private bool rewardSpawned = false;

    private void Start()
    {
        //Se ponen rayitas al empezar
        if (screenText != null) screenText.text = "----";
    }
    public void AddDigit(int digit)
    {
        if (isLocked) return;
        //agrega un nuevo numero si no llegamos a los 4, si llegamos, valida el codigo
        if (currentInput.Length < 4)
        {
            currentInput += digit.ToString();
            UpdateScreen();
        }
        if (currentInput.Length == 4)
        {
            ValidateGuess();
        }
    }

    private void UpdateScreen()
    {
        if (screenText != null) screenText.text = currentInput;
    }

    private void ValidateGuess()
    {
        //Pasa el texto a num entero. Si es correcto, bloquea el pad, si es incorrecto, lo devuelve a 0
        if (int.TryParse(currentInput, out int finalGuess))
        {
            if (generator.CheckCode(finalGuess))
            {
                isLocked = true;
                if (screenText != null) screenText.text = "Ok";
                if (doorSafeLock != null) doorSafeLock.OpenSafe();
                SpawnKey();
            }
            else
            {
                if (screenText != null) screenText.text = "Wrong";
                Invoke("ResetScreen", 1f);
                
            }
        }
    }

    private void SpawnKey()
    {
        if (rewardSpawned) return;
        if (keyPrefab != null && spawnPoint != null)
        {
            Instantiate(keyPrefab, spawnPoint.position, spawnPoint.rotation);
            rewardSpawned = true;
        }
    }
    private void ResetScreen()
    {
        currentInput = "";
        if (screenText != null) screenText.text = "----";
    }

}

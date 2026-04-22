using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class NumberInput : MonoBehaviour
{
    [SerializeField] private NumberGenerator generator;
    [SerializeField] private TextMeshProUGUI screenText;
    //guarda los numeros como strings
    private string currentInput = "";
    private bool isLocked = false;

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
            }
            else
            {
                if (screenText != null) screenText.text = "Wrong";
                Invoke("ResetScreen", 1f);
                
            }
        }
    }

    private void ResetScreen()
    {
        currentInput = "";
        if (screenText != null) screenText.text = "----";
    }
}

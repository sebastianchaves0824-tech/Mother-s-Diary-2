using UnityEngine;
using TMPro;

public class scriptcodigo : MonoBehaviour
{
    public NumberGenerator Numbers;
    [SerializeField] TextMeshProUGUI displayText;
    [SerializeField] int currentDigitIndex = 0;

    [HideInInspector] public int digito1;
    [HideInInspector] public int digito2;
    [HideInInspector] public int digito3;
    [HideInInspector] public int digito4;

    void Start()
    {
        SepararDigitos(Numbers.secretNumber);
    }
    void Update()
    {
        actualizarDisplay();
    }
    public void SepararDigitos(int numero)
    {
        string texto = numero.ToString().PadLeft(4, '0');

        digito1 = texto[0] - '0';
        digito2 = texto[1] - '0';
        digito3 = texto[2] - '0';
        digito4 = texto[3] - '0';
    }

    public void actualizarDisplay()
    {
       if(currentDigitIndex == 0)
        {
            displayText.text = $"{digito1}";
        }
       else if(currentDigitIndex == 1)
        {
            displayText.text = $"{digito2}";
        }
       else if(currentDigitIndex == 2)
        {
            displayText.text = $"{digito3}";
        }
       else if(currentDigitIndex == 3)
        {
            displayText.text = $"{digito4}";
        }
    }
}

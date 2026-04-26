using UnityEngine;

public class NumberGenerator : MonoBehaviour
{
    public int secretNumber;
    void Start()
    {
        GenerateCode();
    }

    //Genero el numero de 4 digitos
    void GenerateCode()
    {
        secretNumber = Random.Range(1000, 10000);
        Debug.Log(secretNumber);
    }

    //Recibe el numero y devuelve true o falkse
    public bool CheckCode(int input)
    {
        return input == secretNumber;
    }

}

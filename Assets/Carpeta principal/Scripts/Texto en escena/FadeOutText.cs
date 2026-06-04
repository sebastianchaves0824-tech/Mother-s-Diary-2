using UnityEngine;
using TMPro;
using System.Collections;

public class FadeOutText : MonoBehaviour
{
    [Header("Configuración de Tiempo")]
    [SerializeField] private float waitingTime = 3f;
    [SerializeField] private float fadeDuration = 1.5f;

    private TextMeshPro textMeshPro;
    void Start()
    {
        textMeshPro = GetComponent<TextMeshPro>();

        if (textMeshPro != null)
        {
            StartCoroutine(InitiateFadeOut());
        }
        else
        {
            Debug.LogWarning($"No se encontro TextMeshPro en el objeto {gameObject.name}");
        }
    }

    private IEnumerator InitiateFadeOut()
    {
        yield return new WaitForSeconds(waitingTime);

        Color colorOriginal = textMeshPro.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alfa = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

            textMeshPro.color = new Color(colorOriginal.r, colorOriginal.g, colorOriginal.b, alfa);

            yield return null;
        }

        gameObject.SetActive(false);
    }    
}

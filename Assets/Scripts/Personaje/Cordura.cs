using UnityEngine;
using UnityEngine.UI;

public class Cordura : MonoBehaviour
{
[SerializeField] Image sanityBar;

public float currentSanity;
public float maxSanity;
private float sanityDecreaseRate = 1f;
    void Start()
    {
        currentSanity = maxSanity;
    }
    void Update()
    {
       if (currentSanity > 0)
        {
            currentSanity -= sanityDecreaseRate * Time.deltaTime;
            sanityBar.fillAmount = currentSanity / maxSanity;
        }
        Debug.Log(Mathf.FloorToInt(currentSanity));
    }
}

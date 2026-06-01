using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Cordura : MonoBehaviour
{
[SerializeField] Image sanityBar;

public float currentSanity;
public float maxSanity;
public OtherMotherRaycast raycast;
private float sanityDecreaseRate = 1f;
    void Start()
    {
        currentSanity = maxSanity;
    }
    void Update()
    {
       if (currentSanity > 0)
        {
            if (!raycast.detectado)
            {
                currentSanity -= sanityDecreaseRate * Time.deltaTime;
            }
            else
            {
                currentSanity -= sanityDecreaseRate * 3 * Time.deltaTime;
            }
            sanityBar.fillAmount = currentSanity / maxSanity;
            Debug.Log("Sanity: " + currentSanity);
        }

    }
}

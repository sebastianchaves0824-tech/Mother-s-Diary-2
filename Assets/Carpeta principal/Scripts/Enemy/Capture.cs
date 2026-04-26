using UnityEngine;

public class Capture : MonoBehaviour
{
    [Header("Configuración de Daño (%)")]
    [Range(0f, 1f)][SerializeField] private float damage1 = 0.15f;
    [Range(0f, 1f)][SerializeField] private float damage2 = 0.50f;

    [Header("Referencias")]
    [SerializeField] private Transform teleportPoint;
    [SerializeField] private PuzzleQTE puzzleQTE;

    private int timesCaught = 0;
    private Cordura cordura;

    //para el cooldown
    private float timeSinceCaptured;
    private float cooldown = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (Time.time > timeSinceCaptured + cooldown)
        {
            if (other.CompareTag("Player"))
            {
                cordura = other.GetComponent<Cordura>();
                if (cordura != null)
                {
                    timeSinceCaptured = Time.time;
                    Catch(other.gameObject);
                }
            }
        }
    }

    private void Catch(GameObject player)
    {
        timesCaught++;

        float damage = 0;

        if (timesCaught == 1)
        {
            damage = cordura.maxSanity * damage1;
        }
        else if (timesCaught == 2)
        {
            damage = cordura.maxSanity * damage2;
        }
        else if (timesCaught >= 3)
        {
            damage = cordura.maxSanity;
        }

        //aplica el danio
        cordura.currentSanity -= damage;

        if (cordura.currentSanity < 0)
            cordura.currentSanity = 0;

        Teleport(player);

        if (puzzleQTE != null)
        {
            puzzleQTE.StartPuzzle();
        }
    }

    private void Teleport(GameObject player)
    {
        //Desactiva el character controller 1 segundito para q no sobreescriba nd
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;
        {
            player.transform.position = teleportPoint.position;
            player.transform.rotation = teleportPoint.rotation;

            if (cc != null) cc.enabled = true;
        }
    }
}